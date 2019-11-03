namespace ChroniCalc
{
    partial class PastebinPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlPastebin = new System.Windows.Forms.Panel();
            this.lblHorizontalDivider = new System.Windows.Forms.Label();
            this.txtPastebinLoad = new System.Windows.Forms.TextBox();
            this.txtPastebinShare = new System.Windows.Forms.TextBox();
            this.lblPastebinHeader = new System.Windows.Forms.Label();
            this.lblPastebinShare = new System.Windows.Forms.Label();
            this.lblPastebinLoad = new System.Windows.Forms.Label();
            this.btnPastebinShare = new System.Windows.Forms.Button();
            this.btnPastebinLoad = new System.Windows.Forms.Button();
            this.pnlPastebin.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlPastebin
            // 
            this.pnlPastebin.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnlPastebin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlPastebin.Controls.Add(this.lblHorizontalDivider);
            this.pnlPastebin.Controls.Add(this.txtPastebinLoad);
            this.pnlPastebin.Controls.Add(this.txtPastebinShare);
            this.pnlPastebin.Controls.Add(this.lblPastebinHeader);
            this.pnlPastebin.Controls.Add(this.lblPastebinShare);
            this.pnlPastebin.Controls.Add(this.lblPastebinLoad);
            this.pnlPastebin.Controls.Add(this.btnPastebinShare);
            this.pnlPastebin.Controls.Add(this.btnPastebinLoad);
            this.pnlPastebin.Location = new System.Drawing.Point(0, 0);
            this.pnlPastebin.Name = "pnlPastebin";
            this.pnlPastebin.Size = new System.Drawing.Size(325, 150);
            this.pnlPastebin.TabIndex = 0;
            // 
            // lblHorizontalDivider
            // 
            this.lblHorizontalDivider.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHorizontalDivider.Location = new System.Drawing.Point(0, 20);
            this.lblHorizontalDivider.Name = "lblHorizontalDivider";
            this.lblHorizontalDivider.Size = new System.Drawing.Size(325, 2);
            this.lblHorizontalDivider.TabIndex = 8;
            this.lblHorizontalDivider.Text = "label1";
            // 
            // txtPastebinLoad
            // 
            this.txtPastebinLoad.BackColor = System.Drawing.Color.White;
            this.txtPastebinLoad.Location = new System.Drawing.Point(6, 105);
            this.txtPastebinLoad.Name = "txtPastebinLoad";
            this.txtPastebinLoad.Size = new System.Drawing.Size(172, 20);
            this.txtPastebinLoad.TabIndex = 6;
            // 
            // txtPastebinShare
            // 
            this.txtPastebinShare.BackColor = System.Drawing.Color.White;
            this.txtPastebinShare.Location = new System.Drawing.Point(6, 51);
            this.txtPastebinShare.Name = "txtPastebinShare";
            this.txtPastebinShare.Size = new System.Drawing.Size(172, 20);
            this.txtPastebinShare.TabIndex = 5;
            // 
            // lblPastebinHeader
            // 
            this.lblPastebinHeader.AutoSize = true;
            this.lblPastebinHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblPastebinHeader.Font = new System.Drawing.Font("Comic Sans MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPastebinHeader.ForeColor = System.Drawing.Color.White;
            this.lblPastebinHeader.Location = new System.Drawing.Point(3, 0);
            this.lblPastebinHeader.Name = "lblPastebinHeader";
            this.lblPastebinHeader.Size = new System.Drawing.Size(90, 18);
            this.lblPastebinHeader.TabIndex = 4;
            this.lblPastebinHeader.Text = "Build Sharing";
            // 
            // lblPastebinShare
            // 
            this.lblPastebinShare.AutoSize = true;
            this.lblPastebinShare.BackColor = System.Drawing.Color.Transparent;
            this.lblPastebinShare.ForeColor = System.Drawing.Color.White;
            this.lblPastebinShare.Location = new System.Drawing.Point(3, 32);
            this.lblPastebinShare.Name = "lblPastebinShare";
            this.lblPastebinShare.Size = new System.Drawing.Size(303, 13);
            this.lblPastebinShare.TabIndex = 3;
            this.lblPastebinShare.Text = "Copy and share the generated link to send your Build to others:";
            // 
            // lblPastebinLoad
            // 
            this.lblPastebinLoad.AutoSize = true;
            this.lblPastebinLoad.BackColor = System.Drawing.Color.Transparent;
            this.lblPastebinLoad.ForeColor = System.Drawing.Color.White;
            this.lblPastebinLoad.Location = new System.Drawing.Point(3, 86);
            this.lblPastebinLoad.Name = "lblPastebinLoad";
            this.lblPastebinLoad.Size = new System.Drawing.Size(193, 13);
            this.lblPastebinLoad.TabIndex = 2;
            this.lblPastebinLoad.Text = "To Load a Build, enter a Pastebin URL:";
            // 
            // btnPastebinShare
            // 
            this.btnPastebinShare.Location = new System.Drawing.Point(184, 48);
            this.btnPastebinShare.Name = "btnPastebinShare";
            this.btnPastebinShare.Size = new System.Drawing.Size(109, 23);
            this.btnPastebinShare.TabIndex = 1;
            this.btnPastebinShare.Text = "Share with Pastebin";
            this.btnPastebinShare.UseVisualStyleBackColor = true;
            this.btnPastebinShare.Click += new System.EventHandler(this.BtnPastebinShare_Click);
            // 
            // btnPastebinLoad
            // 
            this.btnPastebinLoad.Location = new System.Drawing.Point(184, 102);
            this.btnPastebinLoad.Name = "btnPastebinLoad";
            this.btnPastebinLoad.Size = new System.Drawing.Size(109, 23);
            this.btnPastebinLoad.TabIndex = 0;
            this.btnPastebinLoad.Text = "Load from Pastebin";
            this.btnPastebinLoad.UseVisualStyleBackColor = true;
            this.btnPastebinLoad.Click += new System.EventHandler(this.BtnPastebinLoad_Click);
            // 
            // PastebinPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlPastebin);
            this.Name = "PastebinPanel";
            this.Size = new System.Drawing.Size(325, 150);
            this.pnlPastebin.ResumeLayout(false);
            this.pnlPastebin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPastebin;
        private System.Windows.Forms.Button btnPastebinShare;
        private System.Windows.Forms.Button btnPastebinLoad;
        private System.Windows.Forms.TextBox txtPastebinShare;
        private System.Windows.Forms.Label lblPastebinHeader;
        private System.Windows.Forms.Label lblPastebinShare;
        private System.Windows.Forms.Label lblPastebinLoad;
        private System.Windows.Forms.TextBox txtPastebinLoad;
        private System.Windows.Forms.Label lblHorizontalDivider;
    }
}
