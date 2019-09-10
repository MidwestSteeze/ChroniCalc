namespace ChroniCalc
{
    partial class MainForm
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
            this.pnlClass = new System.Windows.Forms.Panel();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbClass = new System.Windows.Forms.PictureBox();
            this.pnlClassData = new System.Windows.Forms.Panel();
            this.lblSkillPoints = new System.Windows.Forms.Label();
            this.lblSkillPointsCaption = new System.Windows.Forms.Label();
            this.lblTree = new System.Windows.Forms.Label();
            this.pnlTrees = new System.Windows.Forms.Panel();
            this.btnTree1 = new System.Windows.Forms.Button();
            this.btnTree2 = new System.Windows.Forms.Button();
            this.btnTree3 = new System.Windows.Forms.Button();
            this.btnTree4 = new System.Windows.Forms.Button();
            this.btnTreeMastery = new System.Windows.Forms.Button();
            this.pnlClass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClass)).BeginInit();
            this.pnlClassData.SuspendLayout();
            this.pnlTrees.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlClass
            // 
            this.pnlClass.Controls.Add(this.pbClass);
            this.pnlClass.Controls.Add(this.cboClass);
            this.pnlClass.Location = new System.Drawing.Point(3, 39);
            this.pnlClass.Name = "pnlClass";
            this.pnlClass.Size = new System.Drawing.Size(210, 381);
            this.pnlClass.TabIndex = 7;
            // 
            // cboClass
            // 
            this.cboClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClass.FormattingEnabled = true;
            this.cboClass.Items.AddRange(new object[] {
            "Berserker",
            "Templar",
            "Warden",
            "Warlock"});
            this.cboClass.Location = new System.Drawing.Point(3, 3);
            this.cboClass.Name = "cboClass";
            this.cboClass.Size = new System.Drawing.Size(203, 21);
            this.cboClass.TabIndex = 0;
            this.cboClass.SelectedIndexChanged += new System.EventHandler(this.CboClass_SelectedIndexChanged);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Rockwell", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(1, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(128, 26);
            this.lblTitle.TabIndex = 8;
            this.lblTitle.Text = "ChroniCalc";
            // 
            // pbClass
            // 
            this.pbClass.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pbClass.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pbClass.Location = new System.Drawing.Point(3, 28);
            this.pbClass.Name = "pbClass";
            this.pbClass.Size = new System.Drawing.Size(200, 350);
            this.pbClass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbClass.TabIndex = 1;
            this.pbClass.TabStop = false;
            // 
            // pnlClassData
            // 
            this.pnlClassData.BackgroundImage = global::ChroniCalc.Properties.Resources.background;
            this.pnlClassData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlClassData.Controls.Add(this.lblSkillPoints);
            this.pnlClassData.Controls.Add(this.lblSkillPointsCaption);
            this.pnlClassData.Controls.Add(this.lblTree);
            this.pnlClassData.Controls.Add(this.pnlTrees);
            this.pnlClassData.Location = new System.Drawing.Point(220, 0);
            this.pnlClassData.Name = "pnlClassData";
            this.pnlClassData.Size = new System.Drawing.Size(560, 420);
            this.pnlClassData.TabIndex = 7;
            // 
            // lblSkillPoints
            // 
            this.lblSkillPoints.AutoSize = true;
            this.lblSkillPoints.BackColor = System.Drawing.Color.Transparent;
            this.lblSkillPoints.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblSkillPoints.ForeColor = System.Drawing.Color.White;
            this.lblSkillPoints.Location = new System.Drawing.Point(127, 39);
            this.lblSkillPoints.Name = "lblSkillPoints";
            this.lblSkillPoints.Size = new System.Drawing.Size(18, 20);
            this.lblSkillPoints.TabIndex = 9;
            this.lblSkillPoints.Text = "0";
            // 
            // lblSkillPointsCaption
            // 
            this.lblSkillPointsCaption.AutoSize = true;
            this.lblSkillPointsCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblSkillPointsCaption.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblSkillPointsCaption.ForeColor = System.Drawing.Color.White;
            this.lblSkillPointsCaption.Location = new System.Drawing.Point(39, 39);
            this.lblSkillPointsCaption.Name = "lblSkillPointsCaption";
            this.lblSkillPointsCaption.Size = new System.Drawing.Size(90, 20);
            this.lblSkillPointsCaption.TabIndex = 8;
            this.lblSkillPointsCaption.Text = "Skill Points:";
            // 
            // lblTree
            // 
            this.lblTree.AutoSize = true;
            this.lblTree.BackColor = System.Drawing.Color.Transparent;
            this.lblTree.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblTree.ForeColor = System.Drawing.Color.White;
            this.lblTree.Location = new System.Drawing.Point(18, 60);
            this.lblTree.Name = "lblTree";
            this.lblTree.Size = new System.Drawing.Size(0, 20);
            this.lblTree.TabIndex = 7;
            // 
            // pnlTrees
            // 
            this.pnlTrees.BackColor = System.Drawing.Color.Transparent;
            this.pnlTrees.Controls.Add(this.btnTree1);
            this.pnlTrees.Controls.Add(this.btnTree2);
            this.pnlTrees.Controls.Add(this.btnTree3);
            this.pnlTrees.Controls.Add(this.btnTree4);
            this.pnlTrees.Controls.Add(this.btnTreeMastery);
            this.pnlTrees.Location = new System.Drawing.Point(0, 83);
            this.pnlTrees.Name = "pnlTrees";
            this.pnlTrees.Size = new System.Drawing.Size(540, 317);
            this.pnlTrees.TabIndex = 6;
            // 
            // btnTree1
            // 
            this.btnTree1.BackColor = System.Drawing.Color.Transparent;
            this.btnTree1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTree1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTree1.Location = new System.Drawing.Point(24, 3);
            this.btnTree1.Name = "btnTree1";
            this.btnTree1.Size = new System.Drawing.Size(30, 30);
            this.btnTree1.TabIndex = 1;
            this.btnTree1.UseVisualStyleBackColor = false;
            this.btnTree1.Click += new System.EventHandler(this.ShowTree);
            // 
            // btnTree2
            // 
            this.btnTree2.BackColor = System.Drawing.Color.Transparent;
            this.btnTree2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTree2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTree2.Location = new System.Drawing.Point(67, 3);
            this.btnTree2.Name = "btnTree2";
            this.btnTree2.Size = new System.Drawing.Size(30, 30);
            this.btnTree2.TabIndex = 2;
            this.btnTree2.UseVisualStyleBackColor = false;
            this.btnTree2.Click += new System.EventHandler(this.ShowTree);
            // 
            // btnTree3
            // 
            this.btnTree3.BackColor = System.Drawing.Color.Transparent;
            this.btnTree3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTree3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTree3.Location = new System.Drawing.Point(111, 3);
            this.btnTree3.Name = "btnTree3";
            this.btnTree3.Size = new System.Drawing.Size(30, 30);
            this.btnTree3.TabIndex = 3;
            this.btnTree3.UseVisualStyleBackColor = false;
            this.btnTree3.Click += new System.EventHandler(this.ShowTree);
            // 
            // btnTree4
            // 
            this.btnTree4.BackColor = System.Drawing.Color.Transparent;
            this.btnTree4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTree4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTree4.Location = new System.Drawing.Point(157, 3);
            this.btnTree4.Name = "btnTree4";
            this.btnTree4.Size = new System.Drawing.Size(30, 30);
            this.btnTree4.TabIndex = 4;
            this.btnTree4.UseVisualStyleBackColor = false;
            this.btnTree4.Click += new System.EventHandler(this.ShowTree);
            // 
            // btnTreeMastery
            // 
            this.btnTreeMastery.BackColor = System.Drawing.Color.Transparent;
            this.btnTreeMastery.BackgroundImage = global::ChroniCalc.ResourceImageTree.IconMastery;
            this.btnTreeMastery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTreeMastery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTreeMastery.Location = new System.Drawing.Point(201, 3);
            this.btnTreeMastery.Name = "btnTreeMastery";
            this.btnTreeMastery.Size = new System.Drawing.Size(30, 30);
            this.btnTreeMastery.TabIndex = 5;
            this.btnTreeMastery.UseVisualStyleBackColor = false;
            this.btnTreeMastery.Click += new System.EventHandler(this.ShowTree);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(784, 427);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlClass);
            this.Controls.Add(this.pnlClassData);
            this.DoubleBuffered = true;
            this.Name = "MainForm";
            this.Text = "ChroniCalc";
            this.pnlClass.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbClass)).EndInit();
            this.pnlClassData.ResumeLayout(false);
            this.pnlClassData.PerformLayout();
            this.pnlTrees.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnTree1;
        private System.Windows.Forms.Button btnTree2;
        private System.Windows.Forms.Button btnTree3;
        private System.Windows.Forms.Button btnTree4;
        private System.Windows.Forms.Button btnTreeMastery;
        private System.Windows.Forms.Panel pnlTrees;
        private System.Windows.Forms.Panel pnlClassData;
        private System.Windows.Forms.Panel pnlClass;
        private System.Windows.Forms.ComboBox cboClass;
        private System.Windows.Forms.PictureBox pbClass;
        private System.Windows.Forms.Label lblTree;
        private System.Windows.Forms.Label lblSkillPointsCaption;
        private System.Windows.Forms.Label lblSkillPoints;
        private System.Windows.Forms.Label lblTitle;
    }
}

