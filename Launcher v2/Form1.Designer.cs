using System;
using System.Drawing;

namespace Launcher_v2
{
    public partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.strtGameBtn = new System.Windows.Forms.Button();
            this.downloadLbl = new System.Windows.Forms.Label();
            this.minimizeBtn = new System.Windows.Forms.Button();
            this.closeBtn = new System.Windows.Forms.Button();
            this.patchNotes = new System.Windows.Forms.WebBrowser();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // strtGameBtn
            // 
            this.strtGameBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(21)))));
            this.strtGameBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(121)))), ((int)(((byte)(203)))));
            this.strtGameBtn.FlatAppearance.BorderSize = 2;
            this.strtGameBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.strtGameBtn.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.strtGameBtn.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(121)))), ((int)(((byte)(203)))));
            this.strtGameBtn.Location = new System.Drawing.Point(736, 379);
            this.strtGameBtn.Name = "strtGameBtn";
            this.strtGameBtn.Size = new System.Drawing.Size(150, 56);
            this.strtGameBtn.TabIndex = 1;
            this.strtGameBtn.Text = "START GAME";
            this.strtGameBtn.UseVisualStyleBackColor = false;
            this.strtGameBtn.Click += new System.EventHandler(this.strtGameBtn_Click);
            // 
            // downloadLbl
            // 
            this.downloadLbl.AutoSize = true;
            this.downloadLbl.BackColor = System.Drawing.Color.Transparent;
            this.downloadLbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(105)))), ((int)(((byte)(136)))));
            this.downloadLbl.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.downloadLbl.Location = new System.Drawing.Point(14, 380);
            this.downloadLbl.Name = "downloadLbl";
            this.downloadLbl.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.downloadLbl.Size = new System.Drawing.Size(0, 13);
            this.downloadLbl.TabIndex = 2;
            // 
            // minimizeBtn
            // 
            this.minimizeBtn.BackColor = System.Drawing.Color.Transparent;
            this.minimizeBtn.BackgroundImage = global::Launcher_v2.Properties.Resources.minimize2;
            this.minimizeBtn.FlatAppearance.BorderSize = 0;
            this.minimizeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.minimizeBtn.Location = new System.Drawing.Point(832, 0);
            this.minimizeBtn.Name = "minimizeBtn";
            this.minimizeBtn.Size = new System.Drawing.Size(34, 26);
            this.minimizeBtn.TabIndex = 4;
            this.minimizeBtn.UseVisualStyleBackColor = false;
            this.minimizeBtn.Click += new System.EventHandler(this.minimizeBtn_Click);
            this.minimizeBtn.MouseEnter += new System.EventHandler(this.minimizeBtn_MouseEnter);
            this.minimizeBtn.MouseLeave += new System.EventHandler(this.minimizeBtn_MouseLeave);
            // 
            // closeBtn
            // 
            this.closeBtn.BackColor = System.Drawing.Color.Transparent;
            this.closeBtn.BackgroundImage = global::Launcher_v2.Properties.Resources.close2;
            this.closeBtn.FlatAppearance.BorderSize = 0;
            this.closeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.closeBtn.Location = new System.Drawing.Point(866, 0);
            this.closeBtn.Name = "closeBtn";
            this.closeBtn.Size = new System.Drawing.Size(34, 26);
            this.closeBtn.TabIndex = 3;
            this.closeBtn.UseVisualStyleBackColor = false;
            this.closeBtn.Click += new System.EventHandler(this.closeBtn_Click);
            this.closeBtn.MouseEnter += new System.EventHandler(this.closeBtn_MouseEnter);
            this.closeBtn.MouseLeave += new System.EventHandler(this.closeBtn_MouseLeave);
            // 
            // patchNotes
            // 
            this.patchNotes.AllowNavigation = false;
            this.patchNotes.AllowWebBrowserDrop = false;
            this.patchNotes.CausesValidation = false;
            this.patchNotes.Location = new System.Drawing.Point(0, 0);
            this.patchNotes.MinimumSize = new System.Drawing.Size(20, 20);
            this.patchNotes.Name = "patchNotes";
            this.patchNotes.ScrollBarsEnabled = false;
            this.patchNotes.Size = new System.Drawing.Size(900, 450);
            this.patchNotes.TabIndex = 5;
            this.patchNotes.TabStop = false;
            this.patchNotes.Url = new System.Uri("http://mods.australianarmedforces.org/html/", System.UriKind.Absolute);
            this.patchNotes.WebBrowserShortcutsEnabled = false;
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::Launcher_v2.Properties.Resources.bg;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(832, 26);
            this.panel1.TabIndex = 6;
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.titleBarPanel_Mousedown);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.DodgerBlue;
            this.pictureBox1.Location = new System.Drawing.Point(14, 404);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(710, 31);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(0, 31);
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(9)))), ((int)(((byte)(15)))), ((int)(((byte)(32)))));
            this.pictureBox2.Location = new System.Drawing.Point(14, 404);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(0);
            this.pictureBox2.MaximumSize = new System.Drawing.Size(710, 31);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(710, 31);
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(13)))), ((int)(((byte)(21)))));
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(900, 450);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.minimizeBtn);
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.downloadLbl);
            this.Controls.Add(this.strtGameBtn);
            this.Controls.Add(this.patchNotes);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AAF Updater";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button strtGameBtn;
        private System.Windows.Forms.Label downloadLbl;
        private System.Windows.Forms.Button closeBtn;
        private System.Windows.Forms.Button minimizeBtn;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.WebBrowser patchNotes;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}

