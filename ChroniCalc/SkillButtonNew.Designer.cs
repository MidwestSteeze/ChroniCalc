namespace ChroniCalc
{
    partial class SkillButtonNew
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
            this.pnlSkillButton = new System.Windows.Forms.Panel();
            this.lblSkillLevel = new System.Windows.Forms.Label();
            this.pbSkillIcon = new System.Windows.Forms.PictureBox();
            this.pnlSkillButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSkillButton
            // 
            this.pnlSkillButton.AutoSize = true;
            this.pnlSkillButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlSkillButton.BackColor = System.Drawing.Color.Transparent;
            this.pnlSkillButton.Controls.Add(this.lblSkillLevel);
            this.pnlSkillButton.Controls.Add(this.pbSkillIcon);
            this.pnlSkillButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSkillButton.Location = new System.Drawing.Point(0, 0);
            this.pnlSkillButton.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSkillButton.Name = "pnlSkillButton";
            this.pnlSkillButton.Size = new System.Drawing.Size(58, 56);
            this.pnlSkillButton.TabIndex = 0;
            // 
            // lblSkillLevel
            // 
            this.lblSkillLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSkillLevel.AutoSize = true;
            this.lblSkillLevel.BackColor = System.Drawing.Color.Black;
            this.lblSkillLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSkillLevel.Font = new System.Drawing.Font("TechnicBold", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lblSkillLevel.ForeColor = System.Drawing.Color.White;
            this.lblSkillLevel.Location = new System.Drawing.Point(25, 0);
            this.lblSkillLevel.Margin = new System.Windows.Forms.Padding(0);
            this.lblSkillLevel.Name = "lblSkillLevel";
            this.lblSkillLevel.Size = new System.Drawing.Size(17, 16);
            this.lblSkillLevel.TabIndex = 1;
            this.lblSkillLevel.Text = "0";
            this.lblSkillLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbSkillIcon
            // 
            this.pbSkillIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbSkillIcon.BackColor = System.Drawing.Color.Transparent;
            this.pbSkillIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbSkillIcon.Location = new System.Drawing.Point(9, 8);
            this.pbSkillIcon.Margin = new System.Windows.Forms.Padding(0);
            this.pbSkillIcon.Name = "pbSkillIcon";
            this.pbSkillIcon.Size = new System.Drawing.Size(40, 40);
            this.pbSkillIcon.TabIndex = 0;
            this.pbSkillIcon.TabStop = false;
            this.pbSkillIcon.MouseHover += new System.EventHandler(this.SkillButtonNew_MouseHover);
            this.pbSkillIcon.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SkillButtonNew_MouseUp);
            // 
            // SkillButtonNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlSkillButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SkillButtonNew";
            this.Size = new System.Drawing.Size(58, 56);
            this.pnlSkillButton.ResumeLayout(false);
            this.pnlSkillButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSkillIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlSkillButton;
        private System.Windows.Forms.PictureBox pbSkillIcon;
        private System.Windows.Forms.Label lblSkillLevel;
    }
}
