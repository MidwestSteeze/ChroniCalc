namespace ChroniCalc
{
    partial class BuildShareForm
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
            this.pastebinPanel1 = new ChroniCalc.PastebinPanel();
            this.SuspendLayout();
            // 
            // pastebinPanel1
            // 
            this.pastebinPanel1.Location = new System.Drawing.Point(0, 0);
            this.pastebinPanel1.Name = "pastebinPanel1";
            this.pastebinPanel1.Size = new System.Drawing.Size(325, 150);
            this.pastebinPanel1.TabIndex = 0;
            // 
            // BuildShareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(324, 150);
            this.Controls.Add(this.pastebinPanel1);
            this.Location = new System.Drawing.Point(111, 73);
            this.Name = "BuildShareForm";
            this.ResumeLayout(false);

        }

        #endregion

        private PastebinPanel pastebinPanel1;
    }
}