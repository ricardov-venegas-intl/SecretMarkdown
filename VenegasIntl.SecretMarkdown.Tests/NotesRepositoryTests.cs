// <copyright file="NotesRepositoryTests.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VenegasIntl.SecretMarkdown.Backend.Repositories;

namespace VenegasIntl.SecretMarkdown.Tests
{
    /// <summary>
    /// Notes Repository Tests.
    /// </summary>
    [TestClass]
    public class NotesRepositoryTests
    {
        /// <summary>
        /// Test round trip using all methods in the interface.
        /// </summary>
        [TestMethod]
        public void TestRoundTrip()
        {
            var filename = $"TestFile {DateTime.UtcNow.ToString("o").Replace(":", string.Empty).Replace("-", string.Empty)}.test";

            var content = Encoding.UTF8.GetBytes("Hello world");

            NotesRepository repository = new NotesRepository(filename);

            try
            {
                Assert.IsTrue(repository is INotesRepository);

                Assert.IsFalse(repository.AreNotesPresent());
                Assert.IsFalse(File.Exists(repository.RepositoryFilePath));

                repository.SaveNotes(content);

                Assert.IsTrue(repository.AreNotesPresent());
                Assert.IsTrue(File.Exists(repository.RepositoryFilePath));

                var readContent = repository.LoadNotes();
                var text = Encoding.UTF8.GetString(readContent);
                Assert.AreEqual("Hello world", text);
            }
            finally
            {
                if (File.Exists(repository.RepositoryFilePath) == true)
                {
                    File.Delete(repository.RepositoryFilePath);
                }
            }
        }

        /// <summary>
        /// Test validations.
        /// </summary>
        [TestMethod]
        public void Validations()
        {
            var filename = $"TestFile {DateTime.UtcNow.ToString("o").Replace(":", string.Empty).Replace("-", string.Empty)}.test";
            NotesRepository repository = new NotesRepository(filename);
            Assert.ThrowsException<InvalidOperationException>(() => _ = repository.LoadNotes());
        }
    }
}
