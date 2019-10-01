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
            this.lblVersion = new System.Windows.Forms.Label();
            this.pbClass = new System.Windows.Forms.PictureBox();
            this.pnlClassData = new System.Windows.Forms.Panel();
            this.lblMastery = new System.Windows.Forms.Label();
            this.lblMasteryCaption = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblLevelCaption = new System.Windows.Forms.Label();
            this.lblSkillPointsRemaining = new System.Windows.Forms.Label();
            this.lblSkillPointsRemainingCaption = new System.Windows.Forms.Label();
            this.lblTree = new System.Windows.Forms.Label();
            this.pnlTrees = new System.Windows.Forms.Panel();
            this.btnResetTree = new System.Windows.Forms.Button();
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
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Rockwell", 15.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(130, 9);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(23, 26);
            this.lblVersion.TabIndex = 9;
            this.lblVersion.Text = "v";
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
            this.pnlClassData.Controls.Add(this.lblMastery);
            this.pnlClassData.Controls.Add(this.lblMasteryCaption);
            this.pnlClassData.Controls.Add(this.lblLevel);
            this.pnlClassData.Controls.Add(this.lblLevelCaption);
            this.pnlClassData.Controls.Add(this.lblSkillPointsRemaining);
            this.pnlClassData.Controls.Add(this.lblSkillPointsRemainingCaption);
            this.pnlClassData.Controls.Add(this.lblTree);
            this.pnlClassData.Controls.Add(this.pnlTrees);
            this.pnlClassData.Location = new System.Drawing.Point(220, 0);
            this.pnlClassData.Name = "pnlClassData";
            this.pnlClassData.Size = new System.Drawing.Size(560, 420);
            this.pnlClassData.TabIndex = 7;
            // 
            // lblMastery
            // 
            this.lblMastery.AutoSize = true;
            this.lblMastery.BackColor = System.Drawing.Color.Transparent;
            this.lblMastery.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblMastery.ForeColor = System.Drawing.Color.White;
            this.lblMastery.Location = new System.Drawing.Point(125, 15);
            this.lblMastery.Name = "lblMastery";
            this.lblMastery.Size = new System.Drawing.Size(18, 20);
            this.lblMastery.TabIndex = 13;
            this.lblMastery.Text = "0";
            // 
            // lblMasteryCaption
            // 
            this.lblMasteryCaption.AutoSize = true;
            this.lblMasteryCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblMasteryCaption.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblMasteryCaption.ForeColor = System.Drawing.Color.White;
            this.lblMasteryCaption.Location = new System.Drawing.Point(97, 15);
            this.lblMasteryCaption.Name = "lblMasteryCaption";
            this.lblMasteryCaption.Size = new System.Drawing.Size(22, 20);
            this.lblMasteryCaption.TabIndex = 12;
            this.lblMasteryCaption.Text = "M";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.BackColor = System.Drawing.Color.Transparent;
            this.lblLevel.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblLevel.ForeColor = System.Drawing.Color.White;
            this.lblLevel.Location = new System.Drawing.Point(72, 15);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(18, 20);
            this.lblLevel.TabIndex = 11;
            this.lblLevel.Text = "0";
            // 
            // lblLevelCaption
            // 
            this.lblLevelCaption.AutoSize = true;
            this.lblLevelCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblLevelCaption.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblLevelCaption.ForeColor = System.Drawing.Color.White;
            this.lblLevelCaption.Location = new System.Drawing.Point(39, 15);
            this.lblLevelCaption.Name = "lblLevelCaption";
            this.lblLevelCaption.Size = new System.Drawing.Size(27, 20);
            this.lblLevelCaption.TabIndex = 10;
            this.lblLevelCaption.Text = "Lv.";
            // 
            // lblSkillPointsRemaining
            // 
            this.lblSkillPointsRemaining.AutoSize = true;
            this.lblSkillPointsRemaining.BackColor = System.Drawing.Color.Transparent;
            this.lblSkillPointsRemaining.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblSkillPointsRemaining.ForeColor = System.Drawing.Color.White;
            this.lblSkillPointsRemaining.Location = new System.Drawing.Point(127, 35);
            this.lblSkillPointsRemaining.Name = "lblSkillPointsRemaining";
            this.lblSkillPointsRemaining.Size = new System.Drawing.Size(18, 20);
            this.lblSkillPointsRemaining.TabIndex = 9;
            this.lblSkillPointsRemaining.Text = "0";
            // 
            // lblSkillPointsRemainingCaption
            // 
            this.lblSkillPointsRemainingCaption.AutoSize = true;
            this.lblSkillPointsRemainingCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblSkillPointsRemainingCaption.Font = new System.Drawing.Font("Comic Sans MS", 11F);
            this.lblSkillPointsRemainingCaption.ForeColor = System.Drawing.Color.White;
            this.lblSkillPointsRemainingCaption.Location = new System.Drawing.Point(39, 35);
            this.lblSkillPointsRemainingCaption.Name = "lblSkillPointsRemainingCaption";
            this.lblSkillPointsRemainingCaption.Size = new System.Drawing.Size(90, 20);
            this.lblSkillPointsRemainingCaption.TabIndex = 8;
            this.lblSkillPointsRemainingCaption.Text = "Skill Points:";
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
            this.pnlTrees.Controls.Add(this.btnResetTree);
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
            // btnResetTree
            // 
            this.btnResetTree.AutoSize = true;
            this.btnResetTree.BackColor = System.Drawing.Color.Transparent;
            this.btnResetTree.BackgroundImage = global::ChroniCalc.ResourceImageUI.ResetTree;
            this.btnResetTree.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnResetTree.FlatAppearance.BorderSize = 0;
            this.btnResetTree.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnResetTree.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnResetTree.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnResetTree.ForeColor = System.Drawing.Color.White;
            this.btnResetTree.Location = new System.Drawing.Point(507, 6);
            this.btnResetTree.Name = "btnResetTree";
            this.btnResetTree.Size = new System.Drawing.Size(30, 27);
            this.btnResetTree.TabIndex = 6;
            this.btnResetTree.UseVisualStyleBackColor = false;
            this.btnResetTree.Click += new System.EventHandler(this.BtnResetTree_Click);
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
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlClass);
            this.Controls.Add(this.pnlClassData);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "ChroniCalc";
            this.pnlClass.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbClass)).EndInit();
            this.pnlClassData.ResumeLayout(false);
            this.pnlClassData.PerformLayout();
            this.pnlTrees.ResumeLayout(false);
            this.pnlTrees.PerformLayout();
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
        private System.Windows.Forms.Label lblSkillPointsRemainingCaption;
        private System.Windows.Forms.Label lblSkillPointsRemaining;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblLevel;
        private System.Windows.Forms.Label lblLevelCaption;
        private System.Windows.Forms.Label lblMastery;
        private System.Windows.Forms.Label lblMasteryCaption;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Button btnResetTree;
    }
}

