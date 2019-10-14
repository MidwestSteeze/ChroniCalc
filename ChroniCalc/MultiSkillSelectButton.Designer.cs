namespace ChroniCalc
{
    partial class MultiSkillSelectButton
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
            this.pnlMultiSkillSelectButton = new System.Windows.Forms.Panel();
            this.lblMultiSkillSelectLevel = new System.Windows.Forms.Label();
            this.pbMultiSkillSelectIcon = new System.Windows.Forms.PictureBox();
            this.pnlMultiSkillSelectButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMultiSkillSelectIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlMultiSkillSelectButton
            // 
            this.pnlMultiSkillSelectButton.AutoSize = true;
            this.pnlMultiSkillSelectButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pnlMultiSkillSelectButton.Controls.Add(this.lblMultiSkillSelectLevel);
            this.pnlMultiSkillSelectButton.Controls.Add(this.pbMultiSkillSelectIcon);
            this.pnlMultiSkillSelectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMultiSkillSelectButton.Location = new System.Drawing.Point(0, 0);
            this.pnlMultiSkillSelectButton.Margin = new System.Windows.Forms.Padding(0);
            this.pnlMultiSkillSelectButton.Name = "pnlMultiSkillSelectButton";
            this.pnlMultiSkillSelectButton.Size = new System.Drawing.Size(56, 54);
            this.pnlMultiSkillSelectButton.TabIndex = 0;
            // 
            // lblMultiSkillSelectLevel
            // 
            this.lblMultiSkillSelectLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMultiSkillSelectLevel.AutoSize = true;
            this.lblMultiSkillSelectLevel.BackColor = System.Drawing.Color.Black;
            this.lblMultiSkillSelectLevel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMultiSkillSelectLevel.Font = new System.Drawing.Font("TechnicBold", 9.749999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.lblMultiSkillSelectLevel.ForeColor = System.Drawing.Color.White;
            this.lblMultiSkillSelectLevel.Location = new System.Drawing.Point(27, 0);
            this.lblMultiSkillSelectLevel.Margin = new System.Windows.Forms.Padding(0);
            this.lblMultiSkillSelectLevel.Name = "lblMultiSkillSelectLevel";
            this.lblMultiSkillSelectLevel.Size = new System.Drawing.Size(17, 16);
            this.lblMultiSkillSelectLevel.TabIndex = 1;
            this.lblMultiSkillSelectLevel.Text = "0";
            this.lblMultiSkillSelectLevel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbMultiSkillSelectIcon
            // 
            this.pbMultiSkillSelectIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMultiSkillSelectIcon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pbMultiSkillSelectIcon.Location = new System.Drawing.Point(4, 6);
            this.pbMultiSkillSelectIcon.Margin = new System.Windows.Forms.Padding(0);
            this.pbMultiSkillSelectIcon.Name = "pbMultiSkillSelectIcon";
            this.pbMultiSkillSelectIcon.Size = new System.Drawing.Size(44, 47);
            this.pbMultiSkillSelectIcon.TabIndex = 0;
            this.pbMultiSkillSelectIcon.TabStop = false;
            this.pbMultiSkillSelectIcon.Click += new System.EventHandler(this.SkillSelectButton_Click);
            // 
            // MultiSkillSelectButton
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.pnlMultiSkillSelectButton);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "MultiSkillSelectButton";
            this.Size = new System.Drawing.Size(56, 54);
            this.Click += new System.EventHandler(this.SkillSelectButton_Click);
            this.pnlMultiSkillSelectButton.ResumeLayout(false);
            this.pnlMultiSkillSelectButton.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMultiSkillSelectIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlMultiSkillSelectButton;
        private System.Windows.Forms.PictureBox pbMultiSkillSelectIcon;
        private System.Windows.Forms.Label lblMultiSkillSelectLevel;
    }
}
