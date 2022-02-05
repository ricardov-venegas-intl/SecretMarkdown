namespace VenegasIntl.SecretMarkdown.UI.Forms
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();

                // Dispose fonts
                this.header1Font.Dispose();
                this.header2Font.Dispose();
                this.header3Font.Dispose();
                this.listFont.Dispose();
                this.normalFont.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainView = new System.Windows.Forms.RichTextBox();
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // mainView
            // 
            this.mainView.AcceptsTab = true;
            this.mainView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.mainView.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.mainView.ForeColor = System.Drawing.Color.LightGray;
            this.mainView.Location = new System.Drawing.Point(1, 4);
            this.mainView.Name = "mainView";
            this.mainView.Size = new System.Drawing.Size(798, 443);
            this.mainView.TabIndex = 0;
            this.mainView.Text = "";
            this.mainView.TextChanged += new System.EventHandler(this.MainView_TextChanged);
            this.mainView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainView_KeyDown);
            // 
            // refreshTimer
            // 
            this.refreshTimer.Interval = 250;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.mainView);
            this.Name = "MainForm";
            this.Text = "Markdown Secret Notes";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox mainView;
        private System.Windows.Forms.Timer refreshTimer;
    }
}

