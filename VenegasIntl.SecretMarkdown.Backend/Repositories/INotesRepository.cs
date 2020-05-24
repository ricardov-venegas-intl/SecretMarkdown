// <copyright file="INotesRepository.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Text;

namespace VenegasIntl.SecretMarkdown.Backend.Repositories
{
    /// <summary>
    /// Notes repository interface.
    /// </summary>
    public interface INotesRepository
    {
        /// <summary>
        /// Indicates if there is aleady notes available.
        /// </summary>
        /// <returns>True if notes are available.</returns>
        bool AreNotesPresent();

        /// <summary>
        /// Saves the notes.
        /// </summary>
        /// <param name="encryptedNotes">Notes already encrypted.</param>
        void SaveNotes(byte[] encryptedNotes);

        /// <summary>
        /// Loads the notes.
        /// </summary>
        /// <returns>Encryped notes.</returns>
        byte[] LoadNotes();
    }
}
