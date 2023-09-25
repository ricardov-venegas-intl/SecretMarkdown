// <copyright file="AesTextEncryptor.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace VenegasIntl.SecretMarkdown.Backend.Encryptor
{
    /// <summary>
    /// Encrypts a text using AES in CBC mode.
    /// </summary>
    public class AesTextEncryptor : ITextEncryptor
    {
        /// <summary>
        /// Descrypts a text with a password.
        /// </summary>
        /// <param name="encryptedData">A byte array with the encrypted text.</param>
        /// <param name="password">Password to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        public string DecryptText(byte[] encryptedData, string password)
        {
            if (encryptedData == null)
            {
                throw new ArgumentNullException(nameof(encryptedData));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (password.Length == 0)
            {
                throw new ArgumentException("Invalid password");
            }

            if (encryptedData.Length < 512)
            {
                throw new ArgumentException("Encrypted data is too small");
            }

            // Separate salt from the encrypted content
            var salt = encryptedData.Take(256).ToArray();
            var encryptedContent = encryptedData.Skip(256).ToArray();

            // Generate the aes keys from the password, iteration count is larger for smaller passwords
            int iterationCount = 1023 + (Math.Max(63, 127 - password.Length) * 31);
            using (var pdb = new Rfc2898DeriveBytes(password, salt, iterationCount, HashAlgorithmName.SHA512))
            {
                using (var ms = new MemoryStream(encryptedContent))
                {
                    using (Aes aes = Aes.Create())
                    {
                        aes.Mode = CipherMode.CBC;
                        var key = pdb.GetBytes(aes.KeySize / 8);
                        var iv = pdb.GetBytes(aes.BlockSize / 8);
                        aes.Key = key;
                        aes.IV = iv;
                        CryptoStream cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
                        var decryptedData = new byte[encryptedContent.Length];
                        var bytesDecrypted = cs.Read(decryptedData, 0, decryptedData.Length);
                        cs.Close();
                        var contentWithoutFiller = decryptedData.Skip(256).Take(bytesDecrypted).ToArray();
                        return Encoding.UTF8.GetString(contentWithoutFiller).TrimEnd('\0');
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts a text with a password.
        /// </summary>
        /// <param name="content">Content to encrypt.</param>
        /// <param name="password">Password to encrypt.</param>
        /// <returns>A byte array with the encrypted text.</returns>
        public byte[] EncryptText(string content, string password)
        {
            if (content == null)
            {
                throw new ArgumentNullException(nameof(content));
            }

            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (password.Length == 0)
            {
                throw new ArgumentException("Invalid password");
            }

            // Generate ramdom salt
            var salt = new byte[256];
            RandomNumberGenerator.Fill(salt);

            // Create a random prefix for the content
            var filler = new byte[256];
            RandomNumberGenerator.Fill(filler);

            // Convert the text to bytes
            var clearBytes = Encoding.UTF8.GetBytes(content);

            // Generate the aes keys from the password, iteration count is larger for smaller passwords
            int iterationCount = 1023 + (Math.Max(63, 127 - password.Length) * 31);
            using (var pdb = new Rfc2898DeriveBytes(password, salt, iterationCount, HashAlgorithmName.SHA512))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ms.Write(salt, 0, salt.Length);
                    using (Aes aes = Aes.Create())
                    {
                        aes.Mode = CipherMode.CBC;
                        var key = pdb.GetBytes(aes.KeySize / 8);
                        var iv = pdb.GetBytes(aes.BlockSize / 8);
                        aes.Key = key;
                        aes.IV = iv;

                        // Encrypt  random filler + content
                        CryptoStream cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
                        cs.Write(filler, 0, filler.Length);
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                        ms.Flush();
                        return ms.ToArray();
                    }
                }
            }
        }
    }
}
