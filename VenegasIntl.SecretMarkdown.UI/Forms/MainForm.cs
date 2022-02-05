// <copyright file="MainForm.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using VenegasIntl.SecretMarkdown.Backend.ColorParser;
using VenegasIntl.SecretMarkdown.Backend.Encryptor;
using VenegasIntl.SecretMarkdown.Backend.Repositories;

namespace VenegasIntl.SecretMarkdown.UI.Forms
{
    /// <summary>
    /// MainForm.
    /// </summary>
    public partial class MainForm : Form
    {
        // File Header
        private const string FileHeader = "VenegasIntl.SecretMarkdow";

        private List<StyleRegion> currentRegionStyles = new List<StyleRegion>();

        // Dependant services
        private IColorParser colorParser;
        private ITextEncryptor textEncryptor;
        private INotesRepository notesRepository;

        // Style Fonts
        private Font header1Font;
        private Font header2Font;
        private Font header3Font;
        private Font listFont;
        private Font normalFont;

        // Password
        private string password;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainForm"/> class.
        /// </summary>
        /// <param name="colorParser">Color Parser.</param>
        /// <param name="textEncryptor">Text Encryptor.</param>
        /// <param name="notesRepository">Notes Repository.</param>
        public MainForm(IColorParser colorParser, ITextEncryptor textEncryptor, INotesRepository notesRepository)
        {
            this.InitializeComponent();

            this.Icon = Properties.Resources.SecretMarkdown;

            // Services
            this.colorParser = colorParser;
            this.textEncryptor = textEncryptor;
            this.notesRepository = notesRepository;

            // Create fonts
            var fontFamily = this.mainView.Font.FontFamily;
            this.header1Font = new Font(fontFamily, 16.0f, FontStyle.Bold);
            this.header2Font = new Font(fontFamily, 14.0f, FontStyle.Bold);
            this.header3Font = new Font(fontFamily, 13.0f, FontStyle.Bold);
            this.listFont = new Font(fontFamily, 12.0f, FontStyle.Bold);
            this.normalFont = this.mainView.Font;

            // No password yet
            this.password = null;

            // Refresh Timer
            this.refreshTimer.Tick += this.RefreshTimer_Tick;
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            this.refreshTimer.Enabled = false;
            this.refreshTimer.Stop();
            this.mainView.Refresh();
        }

        /// <summary>
        /// Color text when the text changes.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void MainView_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                NativeMethods.LockWindowUpdate(this.mainView.Handle);
                this.ColorMarkDownText();
            }
            finally
            {
                NativeMethods.LockWindowUpdate(IntPtr.Zero);

                // Reset timer
                this.refreshTimer.Enabled = true;
                this.refreshTimer.Stop();
                this.refreshTimer.Start();
            }
        }

        /// <summary>
        /// Saves the file when ctrl+s is press.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">KeyEventArgs.</param>
        private void MainView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.S && e.Modifiers == Keys.Control)
            {
                this.SaveNotes();
            }
        }

        /// <summary>
        /// Loads the text if present when the forms loads.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Args.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            if (this.notesRepository.AreNotesPresent())
            {
                var encryptedText = this.notesRepository.LoadNotes();
                bool loadSuccessFull = false;
                while (loadSuccessFull == false)
                {
                    using (var requestPasswordForm = new RequestPasswordForm())
                    {
                        var dialogResult = requestPasswordForm.ShowDialog();
                        if (dialogResult == DialogResult.OK)
                        {
                            string decryptedText;
                            try
                            {
                                decryptedText = this.textEncryptor.DecryptText(encryptedText, requestPasswordForm.Password);
                                if (decryptedText.StartsWith(FileHeader) == true)
                                {
                                    this.password = requestPasswordForm.Password;
                                    this.mainView.Text = decryptedText.Substring(FileHeader.Length);
                                    loadSuccessFull = true;
                                }
                                else
                                {
                                    MessageBox.Show("Invalid Password", "Invalid Password", MessageBoxButtons.OK);
                                }
                            }
                            catch (CryptographicException)
                            {
                                MessageBox.Show("Invalid Password", "Invalid Password", MessageBoxButtons.OK);
                            }
                        }
                        else
                        {
                            this.Close();
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Colors the text of the main view.
        /// </summary>
        private void ColorMarkDownText()
        {
            int currentSelectionStart = this.mainView.SelectionStart;
            int currentSelectionLength = this.mainView.SelectionLength;

            var colorRegions = this.colorParser.Parse(this.mainView.Text).ToList();
            var stillMatching = true;
            var currentRegionIndex = 0;
            foreach (var region in colorRegions)
            {
                if (stillMatching == true
                    && currentRegionIndex < this.currentRegionStyles.Count
                    && this.currentRegionStyles[currentRegionIndex].Equals(region))
                {
                    // skip matching regions
                    currentRegionIndex++;
                }
                else
                {
                    var font = (region.ColorStyle == ColorStyle.Header1) ? this.header1Font :
                        (region.ColorStyle == ColorStyle.Header2) ? this.header2Font :
                        (region.ColorStyle == ColorStyle.Header3) ? this.header3Font :
                        (region.ColorStyle == ColorStyle.ListPrefix) ? this.listFont :
                        this.normalFont;

                    var color = (region.ColorStyle == ColorStyle.Header1) ? Color.White :
                        (region.ColorStyle == ColorStyle.Header2) ? Color.LightBlue :
                        (region.ColorStyle == ColorStyle.Header3) ? Color.LightCyan :
                        (region.ColorStyle == ColorStyle.ListPrefix) ? Color.LightSkyBlue :
                        Color.LightGray;

                    this.mainView.Select(region.StartPosition, region.Length);
                    this.mainView.SelectionFont = font;
                    this.mainView.SelectionColor = color;
                    stillMatching = false;
                }
            }

            // Restore Cursor
            if (currentSelectionStart >= 0)
            {
                this.mainView.Select(currentSelectionStart, currentSelectionLength);
            }
        }

        /// <summary>
        /// Saves the notes.
        /// </summary>
        private void SaveNotes()
        {
            // Get the password if there is not password yet.
            if (this.password == null)
            {
                using (var confirmPasswordForm = new ConfirmPasswordForm())
                {
                    var dialogResult = confirmPasswordForm.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        this.password = confirmPasswordForm.Password;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            var textToSave = FileHeader + this.mainView.Text;
            var encrypted = this.textEncryptor.EncryptText(textToSave, this.password);
            this.notesRepository.SaveNotes(encrypted);
        }
    }
}
