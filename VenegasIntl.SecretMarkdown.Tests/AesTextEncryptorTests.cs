// <copyright file="AesTextEncryptorTests.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenegasIntl.SecretMarkdown.Backend.Encryptor;

namespace VenegasIntl.SecretMarkdown.Tests
{
    /// <summary>
    /// Aes Text Encryptor Tests.
    /// </summary>
    [TestClass]
    public class AesTextEncryptorTests
    {
        /// <summary>
        /// Simple Round Trip test.
        /// </summary>
        [TestMethod]
        public void SimpleRoundTrip()
        {
            // Long test
            var secretText = string.Concat(Enumerable.Repeat("Hello World ", 1023 * 1023));
            var password = "MySecretPassword";
            ITextEncryptor textEncryptor = new AesTextEncryptor();
            var encryptedText = textEncryptor.EncryptText(secretText, password);
            Assert.IsTrue(encryptedText.Length > 512);
            var decriptedText = textEncryptor.DecryptText(encryptedText, password);
            Assert.AreEqual(secretText, decriptedText);
        }

        /// <summary>
        /// Simple Round Trip test.
        /// </summary>
        [TestMethod]
        public void EmptyContent()
        {
            var secretText = string.Empty;
            var password = "MySecretPassword";
            ITextEncryptor textEncryptor = new AesTextEncryptor();
            var encryptedText = textEncryptor.EncryptText(secretText, password);
            Assert.IsTrue(encryptedText.Length > 512);
            var decriptedText = textEncryptor.DecryptText(encryptedText, password);
            Assert.AreEqual(secretText, decriptedText);
        }

        /// <summary>
        /// Very long password test.
        /// </summary>
        [TestMethod]
        public void VeryLongPassword()
        {
            var secretText = @"Contrary to popular belief, Lorem Ipsum is not simply random text. 
It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. 
Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, 
looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, 
and going through the cites of the word in classical literature, 
discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of ""de Finibus Bonorum et Malorum""
(The Extremes of Good and Evil) by Cicero, written in 45 BC. 
This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, 
""Lorem ipsum dolor sit amet.."", comes from a line in section 1.10.32.";
            var password = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec non faucibus tortor, vel placerat risus. Etiam a feugiat elit. Donec convallis dui ut dignissim dignissim. Nam quis efficitur sem. Morbi ipsum ex, ultrices eu turpis sed, elementum semper nisi.";
            ITextEncryptor textEncryptor = new AesTextEncryptor();
            var encryptedText = textEncryptor.EncryptText(secretText, password);
            Assert.IsTrue(encryptedText.Length > 512);
            var decriptedText = textEncryptor.DecryptText(encryptedText, password);
            Assert.AreEqual(secretText, decriptedText);
        }

        /// <summary>
        /// Very long password test.
        /// </summary>
        [TestMethod]
        public void TestValidartions()
        {
            var secretText = "Hello World";
            var password = "MySecretPassword";
            ITextEncryptor textEncryptor = new AesTextEncryptor();
            var encryptedText = textEncryptor.EncryptText(secretText, password);

            // Test EncryptText validations
            Assert.ThrowsException<ArgumentNullException>(() => { _ = textEncryptor.EncryptText(null, password); });
            Assert.ThrowsException<ArgumentNullException>(() => { _ = textEncryptor.EncryptText(secretText, null); });
            Assert.ThrowsException<ArgumentNullException>(() => { _ = textEncryptor.EncryptText(null, null); });
            Assert.ThrowsException<ArgumentException>(() => { _ = textEncryptor.EncryptText(secretText, string.Empty); });

            // Test DecryptText validarions
            Assert.ThrowsException<ArgumentNullException>(() => { _ = textEncryptor.DecryptText(null, password); });
            Assert.ThrowsException<ArgumentNullException>(() => { _ = textEncryptor.DecryptText(encryptedText, null); });
            Assert.ThrowsException<ArgumentNullException>(() => { _ = textEncryptor.DecryptText(null, null); });
            Assert.ThrowsException<ArgumentException>(() => { _ = textEncryptor.DecryptText(encryptedText, string.Empty); });
            Assert.ThrowsException<ArgumentException>(() => { _ = textEncryptor.DecryptText(encryptedText.Take(511).ToArray(), password); });
        }
    }
}
