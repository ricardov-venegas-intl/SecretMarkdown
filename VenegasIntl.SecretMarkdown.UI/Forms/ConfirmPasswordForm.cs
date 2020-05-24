// <copyright file="ConfirmPasswordForm.cs" company="Ricardo Venegas / Venegas International, LLC">
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
    /// Confirm password Form.
    /// </summary>
    public partial class ConfirmPasswordForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfirmPasswordForm"/> class.
        /// </summary>
        public ConfirmPasswordForm()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Gets or sets confirmend password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Enables OK when both password are the same and not empty.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void Password_TextChanged(object sender, EventArgs e)
        {
            this.okButton.Enabled = this.passwordTextBox.Text.Length > 0
                && this.passwordTextBox.Text == this.confirmPasswordTextBox.Text;
        }

        /// <summary>
        /// OK button pressend.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">EventArgs.</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Password = this.passwordTextBox.Text;
            this.Close();
        }
    }
}
