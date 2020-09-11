namespace ChroniCalc
{
    partial class SkillSelectButton
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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(0, 0);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // SkillSelectButton
            // 
            this.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.FlatAppearance.BorderSize = 2;
            this.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Click += new System.EventHandler(this.SkillSelectButton_Click);
            this.MouseLeave += new System.EventHandler(this.SkillSelectButton_MouseLeave);
            this.MouseHover += new System.EventHandler(this.SkillSelectButton_MouseHover);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
    }
}
