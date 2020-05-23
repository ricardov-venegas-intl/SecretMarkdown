// <copyright file="ITextEncryptor.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

namespace VenegasIntl.SecretMarkdown.Backend.Encryptor
{
    /// <summary>
    /// Text Encryptor Interface.
    /// </summary>
    public interface ITextEncryptor
    {
        /// <summary>
        /// Encrypts a text with a password.
        /// </summary>
        /// <param name="content">Content to encrypt.</param>
        /// <param name="password">Password to encrypt.</param>
        /// <returns>A byte array with the encrypted text.</returns>
        byte[] EncryptText(string content, string password);

        /// <summary>
        /// Descrypts a text with a password.
        /// </summary>
        /// <param name="encryptedData">A byte array with the encrypted text.</param>
        /// <param name="password">Password to decrypt.</param>
        /// <returns>The decrypted text.</returns>
        string DecryptText(byte[] encryptedData, string password);
    }
}
