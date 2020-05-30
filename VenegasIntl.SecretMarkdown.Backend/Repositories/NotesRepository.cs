// <copyright file="NotesRepository.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace VenegasIntl.SecretMarkdown.Backend.Repositories
{
    /// <summary>
    /// Notes repository.
    /// Stores notes in OneDrive if present otherwise in AppData.
    /// </summary>
    public class NotesRepository : INotesRepository
    {
        private string repositoryFolder;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesRepository"/> class.
        /// </summary>
        /// <param name="configuration">Configuration.</param>
        public NotesRepository(IConfiguration configuration)
            : this((configuration ?? throw new ArgumentNullException(nameof(configuration)))["NotesFileName"])
        {
            return;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NotesRepository"/> class.
        /// </summary>
        /// <param name="fileName">File name of the notes file.</param>
        public NotesRepository(string fileName)
        {
            if (Environment.GetEnvironmentVariable("OneDrive") != null)
            {
                this.repositoryFolder = Path.Combine(Environment.GetEnvironmentVariable("OneDrive"), "SecretMarkdown");
            }
            else
            {
                this.repositoryFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SecretMarkdown");
            }

            this.RepositoryFilePath = Path.Combine(this.repositoryFolder, fileName);
        }

        /// <summary>
        /// Gets or sets the RepositoryFilePath (used to test implementation).
        /// </summary>
        public string RepositoryFilePath { get; set; }

        /// <summary>
        /// Indicates if there is aleady notes available.
        /// </summary>
        /// <returns>True if notes are available.</returns>
        public bool AreNotesPresent()
        {
            return File.Exists(this.RepositoryFilePath);
        }

        /// <summary>
        /// Saves the notes.
        /// </summary>
        /// <param name="encryptedNotes">Notes already encrypted.</param>
        public void SaveNotes(byte[] encryptedNotes)
        {
            if (encryptedNotes == null)
            {
                throw new ArgumentNullException(nameof(encryptedNotes));
            }

            if (Directory.Exists(this.repositoryFolder) == false)
            {
                Directory.CreateDirectory(this.repositoryFolder);
            }

            File.WriteAllBytes(this.RepositoryFilePath, encryptedNotes);
        }

        /// <summary>
        /// Loads the notes.
        /// </summary>
        /// <returns>Encryped notes.</returns>
        public byte[] LoadNotes()
        {
            if (File.Exists(this.RepositoryFilePath) == false)
            {
                throw new InvalidOperationException("Notes doesn't exist");
            }

            return File.ReadAllBytes(this.RepositoryFilePath);
        }
    }
}
