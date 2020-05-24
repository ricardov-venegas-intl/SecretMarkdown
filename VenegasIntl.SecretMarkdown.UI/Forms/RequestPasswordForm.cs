// <copyright file="RequestPasswordForm.cs" company="Ricardo Venegas / Venegas International, LLC">
// Copyright (c) Ricardo Venegas / Venegas International, LLC. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace VenegasIntl.SecretMarkdown.UI.Forms
{
    /// <summary>
    /// Request Password Form.
    /// </summary>
    public partial class RequestPasswordForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestPasswordForm"/> class.
        /// </summary>
        public RequestPasswordForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Enable the OK button when there is content in the password.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Args.</param>
        private void PasswordTextBox_TextChanged(object sender, EventArgs e)
        {
            this.okButton.Enabled = this.passwordTextBox.Text.Length > 0;
        }

        /// <summary>
        /// OK button was pressed.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            this.Password = this.passwordTextBox.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
