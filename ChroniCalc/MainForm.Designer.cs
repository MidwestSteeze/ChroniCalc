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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlClass = new System.Windows.Forms.Panel();
            this.pbClass = new System.Windows.Forms.PictureBox();
            this.cboClass = new System.Windows.Forms.ComboBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.toolTipInfo = new System.Windows.Forms.ToolTip(this.components);
            this.btnNavSaveAs = new System.Windows.Forms.Button();
            this.btnNavSave = new System.Windows.Forms.Button();
            this.btnNavBuilds = new System.Windows.Forms.Button();
            this.btnNavTrees = new System.Windows.Forms.Button();
            this.btnNavInventory = new System.Windows.Forms.Button();
            this.btnResetTree = new System.Windows.Forms.Button();
            this.btnBuildSharing = new System.Windows.Forms.Button();
            this.pnlClassData = new System.Windows.Forms.Panel();
            this.pnlBuilds = new System.Windows.Forms.Panel();
            this.pbpBuildShare = new ChroniCalc.PastebinPanel();
            this.btnBuildDelete = new System.Windows.Forms.Button();
            this.btnBuildOpen = new System.Windows.Forms.Button();
            this.dgvBuilds = new System.Windows.Forms.DataGridView();
            this.BuildName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stats = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblTree = new System.Windows.Forms.Label();
            this.pnlInventory = new System.Windows.Forms.Panel();
            this.pnlInventoryGear = new System.Windows.Forms.Panel();
            this.pnlTrees = new System.Windows.Forms.Panel();
            this.btnTree1 = new System.Windows.Forms.Button();
            this.btnTree2 = new System.Windows.Forms.Button();
            this.btnTree3 = new System.Windows.Forms.Button();
            this.btnTree4 = new System.Windows.Forms.Button();
            this.btnTreeMastery = new System.Windows.Forms.Button();
            this.pnlClassCaptions = new System.Windows.Forms.Panel();
            this.lblBuildName = new System.Windows.Forms.Label();
            this.lblMastery = new System.Windows.Forms.Label();
            this.lblMasteryCaption = new System.Windows.Forms.Label();
            this.lblLevel = new System.Windows.Forms.Label();
            this.lblLevelCaption = new System.Windows.Forms.Label();
            this.lblSkillPointsRemaining = new System.Windows.Forms.Label();
            this.lblSkillPointsRemainingCaption = new System.Windows.Forms.Label();
            this.pnlClass.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClass)).BeginInit();
            this.pnlClassData.SuspendLayout();
            this.pnlBuilds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuilds)).BeginInit();
            this.pnlInventory.SuspendLayout();
            this.pnlTrees.SuspendLayout();
            this.pnlClassCaptions.SuspendLayout();
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
            // btnNavSaveAs
            // 
            this.btnNavSaveAs.BackColor = System.Drawing.Color.Transparent;
            this.btnNavSaveAs.BackgroundImage = global::ChroniCalc.ResourceImageUI.IconSaveAs;
            this.btnNavSaveAs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNavSaveAs.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNavSaveAs.FlatAppearance.BorderSize = 0;
            this.btnNavSaveAs.Location = new System.Drawing.Point(511, 15);
            this.btnNavSaveAs.Name = "btnNavSaveAs";
            this.btnNavSaveAs.Size = new System.Drawing.Size(31, 31);
            this.btnNavSaveAs.TabIndex = 19;
            this.toolTipInfo.SetToolTip(this.btnNavSaveAs, "Save As...");
            this.btnNavSaveAs.UseVisualStyleBackColor = false;
            this.btnNavSaveAs.Click += new System.EventHandler(this.BtnNavSaveAs_Click);
            // 
            // btnNavSave
            // 
            this.btnNavSave.BackColor = System.Drawing.Color.Transparent;
            this.btnNavSave.BackgroundImage = global::ChroniCalc.ResourceImageUI.IconSave;
            this.btnNavSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNavSave.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNavSave.FlatAppearance.BorderSize = 0;
            this.btnNavSave.Location = new System.Drawing.Point(474, 15);
            this.btnNavSave.Name = "btnNavSave";
            this.btnNavSave.Size = new System.Drawing.Size(31, 31);
            this.btnNavSave.TabIndex = 18;
            this.toolTipInfo.SetToolTip(this.btnNavSave, "Save");
            this.btnNavSave.UseVisualStyleBackColor = false;
            this.btnNavSave.Click += new System.EventHandler(this.BtnNavSave_Click);
            // 
            // btnNavBuilds
            // 
            this.btnNavBuilds.BackColor = System.Drawing.Color.Transparent;
            this.btnNavBuilds.BackgroundImage = global::ChroniCalc.ResourceImageUI.IconBuilds;
            this.btnNavBuilds.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNavBuilds.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNavBuilds.FlatAppearance.BorderSize = 0;
            this.btnNavBuilds.Location = new System.Drawing.Point(437, 15);
            this.btnNavBuilds.Name = "btnNavBuilds";
            this.btnNavBuilds.Size = new System.Drawing.Size(31, 31);
            this.btnNavBuilds.TabIndex = 17;
            this.toolTipInfo.SetToolTip(this.btnNavBuilds, "Builds");
            this.btnNavBuilds.UseVisualStyleBackColor = false;
            this.btnNavBuilds.Click += new System.EventHandler(this.BtnNavBuilds_Click);
            // 
            // btnNavTrees
            // 
            this.btnNavTrees.BackColor = System.Drawing.Color.Transparent;
            this.btnNavTrees.BackgroundImage = global::ChroniCalc.ResourceImageUI.IconTrees;
            this.btnNavTrees.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNavTrees.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNavTrees.FlatAppearance.BorderSize = 0;
            this.btnNavTrees.Location = new System.Drawing.Point(400, 15);
            this.btnNavTrees.Name = "btnNavTrees";
            this.btnNavTrees.Size = new System.Drawing.Size(31, 31);
            this.btnNavTrees.TabIndex = 16;
            this.toolTipInfo.SetToolTip(this.btnNavTrees, "Trees");
            this.btnNavTrees.UseVisualStyleBackColor = false;
            this.btnNavTrees.Click += new System.EventHandler(this.BtnNavTrees_Click);
            // 
            // btnNavInventory
            // 
            this.btnNavInventory.BackColor = System.Drawing.Color.Transparent;
            this.btnNavInventory.BackgroundImage = global::ChroniCalc.ResourceImageUI.IconInventory;
            this.btnNavInventory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNavInventory.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNavInventory.FlatAppearance.BorderSize = 0;
            this.btnNavInventory.Location = new System.Drawing.Point(363, 15);
            this.btnNavInventory.Name = "btnNavInventory";
            this.btnNavInventory.Size = new System.Drawing.Size(31, 31);
            this.btnNavInventory.TabIndex = 15;
            this.toolTipInfo.SetToolTip(this.btnNavInventory, "Inventory");
            this.btnNavInventory.UseVisualStyleBackColor = false;
            this.btnNavInventory.Click += new System.EventHandler(this.BtnNavInventory_Click);
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
            this.toolTipInfo.SetToolTip(this.btnResetTree, "Reset Tree");
            this.btnResetTree.UseVisualStyleBackColor = false;
            this.btnResetTree.Click += new System.EventHandler(this.BtnResetTree_Click);
            // 
            // btnBuildSharing
            // 
            this.btnBuildSharing.Location = new System.Drawing.Point(429, 10);
            this.btnBuildSharing.Name = "btnBuildSharing";
            this.btnBuildSharing.Size = new System.Drawing.Size(88, 23);
            this.btnBuildSharing.TabIndex = 7;
            this.btnBuildSharing.Text = "Build Sharing...";
            this.toolTipInfo.SetToolTip(this.btnBuildSharing, "Opens the Build Sharing panel for Loading or Sharing Builds");
            this.btnBuildSharing.UseVisualStyleBackColor = true;
            this.btnBuildSharing.Click += new System.EventHandler(this.btnBuildSharing_Click);
            // 
            // pnlClassData
            // 
            this.pnlClassData.BackgroundImage = global::ChroniCalc.Properties.Resources.background;
            this.pnlClassData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnlClassData.Controls.Add(this.pnlBuilds);
            this.pnlClassData.Controls.Add(this.btnNavSaveAs);
            this.pnlClassData.Controls.Add(this.btnNavSave);
            this.pnlClassData.Controls.Add(this.btnNavBuilds);
            this.pnlClassData.Controls.Add(this.btnNavTrees);
            this.pnlClassData.Controls.Add(this.btnNavInventory);
            this.pnlClassData.Controls.Add(this.lblTree);
            this.pnlClassData.Controls.Add(this.pnlInventory);
            this.pnlClassData.Controls.Add(this.pnlTrees);
            this.pnlClassData.Controls.Add(this.pnlClassCaptions);
            this.pnlClassData.Location = new System.Drawing.Point(220, 0);
            this.pnlClassData.Name = "pnlClassData";
            this.pnlClassData.Size = new System.Drawing.Size(560, 420);
            this.pnlClassData.TabIndex = 7;
            // 
            // pnlBuilds
            // 
            this.pnlBuilds.BackColor = System.Drawing.Color.Transparent;
            this.pnlBuilds.Controls.Add(this.pbpBuildShare);
            this.pnlBuilds.Controls.Add(this.btnBuildSharing);
            this.pnlBuilds.Controls.Add(this.btnBuildDelete);
            this.pnlBuilds.Controls.Add(this.btnBuildOpen);
            this.pnlBuilds.Controls.Add(this.dgvBuilds);
            this.pnlBuilds.Location = new System.Drawing.Point(0, 83);
            this.pnlBuilds.Name = "pnlBuilds";
            this.pnlBuilds.Size = new System.Drawing.Size(540, 317);
            this.pnlBuilds.TabIndex = 14;
            // 
            // pbpBuildShare
            // 
            this.pbpBuildShare.Location = new System.Drawing.Point(111, 53);
            this.pbpBuildShare.Name = "pbpBuildShare";
            this.pbpBuildShare.Size = new System.Drawing.Size(325, 150);
            this.pbpBuildShare.TabIndex = 8;
            this.pbpBuildShare.Visible = false;
            // 
            // btnBuildDelete
            // 
            this.btnBuildDelete.Location = new System.Drawing.Point(115, 10);
            this.btnBuildDelete.Name = "btnBuildDelete";
            this.btnBuildDelete.Size = new System.Drawing.Size(75, 23);
            this.btnBuildDelete.TabIndex = 2;
            this.btnBuildDelete.Text = "Delete";
            this.btnBuildDelete.UseVisualStyleBackColor = true;
            this.btnBuildDelete.Click += new System.EventHandler(this.BtnBuildDelete_Click);
            // 
            // btnBuildOpen
            // 
            this.btnBuildOpen.Location = new System.Drawing.Point(34, 10);
            this.btnBuildOpen.Name = "btnBuildOpen";
            this.btnBuildOpen.Size = new System.Drawing.Size(75, 23);
            this.btnBuildOpen.TabIndex = 1;
            this.btnBuildOpen.Text = "Open";
            this.btnBuildOpen.UseVisualStyleBackColor = true;
            this.btnBuildOpen.Click += new System.EventHandler(this.BtnBuildOpen_Click);
            // 
            // dgvBuilds
            // 
            this.dgvBuilds.AllowUserToAddRows = false;
            this.dgvBuilds.AllowUserToDeleteRows = false;
            this.dgvBuilds.AllowUserToResizeColumns = false;
            this.dgvBuilds.AllowUserToResizeRows = false;
            this.dgvBuilds.BackgroundColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBuilds.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBuilds.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBuilds.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BuildName,
            this.Stats});
            this.dgvBuilds.Location = new System.Drawing.Point(34, 42);
            this.dgvBuilds.MultiSelect = false;
            this.dgvBuilds.Name = "dgvBuilds";
            this.dgvBuilds.ReadOnly = true;
            this.dgvBuilds.RowHeadersVisible = false;
            this.dgvBuilds.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvBuilds.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBuilds.Size = new System.Drawing.Size(483, 272);
            this.dgvBuilds.TabIndex = 0;
            this.dgvBuilds.DoubleClick += new System.EventHandler(this.BtnBuildOpen_Click);
            // 
            // BuildName
            // 
            this.BuildName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            this.BuildName.DefaultCellStyle = dataGridViewCellStyle2;
            this.BuildName.HeaderText = "Name";
            this.BuildName.Name = "BuildName";
            this.BuildName.ReadOnly = true;
            this.BuildName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // Stats
            // 
            this.Stats.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.Stats.DefaultCellStyle = dataGridViewCellStyle3;
            this.Stats.HeaderText = "Stats";
            this.Stats.Name = "Stats";
            this.Stats.ReadOnly = true;
            this.Stats.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Stats.Width = 135;
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
            // pnlInventory
            // 
            this.pnlInventory.BackColor = System.Drawing.Color.Transparent;
            this.pnlInventory.Controls.Add(this.pnlInventoryGear);
            this.pnlInventory.Location = new System.Drawing.Point(10, 83);
            this.pnlInventory.Name = "pnlInventory";
            this.pnlInventory.Size = new System.Drawing.Size(540, 317);
            this.pnlInventory.TabIndex = 20;
            // 
            // pnlInventoryGear
            // 
            this.pnlInventoryGear.BackgroundImage = global::ChroniCalc.ResourceImageInventory.Inventory;
            this.pnlInventoryGear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pnlInventoryGear.Location = new System.Drawing.Point(10, 44);
            this.pnlInventoryGear.Name = "pnlInventoryGear";
            this.pnlInventoryGear.Size = new System.Drawing.Size(520, 222);
            this.pnlInventoryGear.TabIndex = 0;
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
            // btnTree1
            // 
            this.btnTree1.BackColor = System.Drawing.Color.Transparent;
            this.btnTree1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTree1.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnTree1.FlatAppearance.BorderSize = 0;
            this.btnTree1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTree1.Location = new System.Drawing.Point(24, 1);
            this.btnTree1.Name = "btnTree1";
            this.btnTree1.Size = new System.Drawing.Size(35, 35);
            this.btnTree1.TabIndex = 1;
            this.btnTree1.UseVisualStyleBackColor = false;
            this.btnTree1.Click += new System.EventHandler(this.ShowTree);
            // 
            // btnTree2
            // 
            this.btnTree2.BackColor = System.Drawing.Color.Transparent;
            this.btnTree2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTree2.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnTree2.FlatAppearance.BorderSize = 0;
            this.btnTree2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTree2.Location = new System.Drawing.Point(67, 1);
            this.btnTree2.Name = "btnTree2";
            this.btnTree2.Size = new System.Drawing.Size(35, 35);
            this.btnTree2.TabIndex = 2;
            this.btnTree2.UseVisualStyleBackColor = false;
            this.btnTree2.Click += new System.EventHandler(this.ShowTree);
            // 
            // btnTree3
            // 
            this.btnTree3.BackColor = System.Drawing.Color.Transparent;
            this.btnTree3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTree3.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnTree3.FlatAppearance.BorderSize = 0;
            this.btnTree3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTree3.Location = new System.Drawing.Point(110, 1);
            this.btnTree3.Name = "btnTree3";
            this.btnTree3.Size = new System.Drawing.Size(35, 35);
            this.btnTree3.TabIndex = 3;
            this.btnTree3.UseVisualStyleBackColor = false;
            this.btnTree3.Click += new System.EventHandler(this.ShowTree);
            // 
            // btnTree4
            // 
            this.btnTree4.BackColor = System.Drawing.Color.Transparent;
            this.btnTree4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTree4.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnTree4.FlatAppearance.BorderSize = 0;
            this.btnTree4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTree4.Location = new System.Drawing.Point(153, 1);
            this.btnTree4.Name = "btnTree4";
            this.btnTree4.Size = new System.Drawing.Size(35, 35);
            this.btnTree4.TabIndex = 4;
            this.btnTree4.UseVisualStyleBackColor = false;
            this.btnTree4.Click += new System.EventHandler(this.ShowTree);
            // 
            // btnTreeMastery
            // 
            this.btnTreeMastery.BackColor = System.Drawing.Color.Transparent;
            this.btnTreeMastery.BackgroundImage = global::ChroniCalc.ResourceImageTree.IconMastery;
            this.btnTreeMastery.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTreeMastery.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnTreeMastery.FlatAppearance.BorderSize = 0;
            this.btnTreeMastery.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTreeMastery.Location = new System.Drawing.Point(196, 1);
            this.btnTreeMastery.Name = "btnTreeMastery";
            this.btnTreeMastery.Size = new System.Drawing.Size(35, 35);
            this.btnTreeMastery.TabIndex = 5;
            this.btnTreeMastery.UseVisualStyleBackColor = false;
            this.btnTreeMastery.Click += new System.EventHandler(this.ShowTree);
            // 
            // pnlClassCaptions
            // 
            this.pnlClassCaptions.BackColor = System.Drawing.Color.Transparent;
            this.pnlClassCaptions.Controls.Add(this.lblBuildName);
            this.pnlClassCaptions.Controls.Add(this.lblMastery);
            this.pnlClassCaptions.Controls.Add(this.lblMasteryCaption);
            this.pnlClassCaptions.Controls.Add(this.lblLevel);
            this.pnlClassCaptions.Controls.Add(this.lblLevelCaption);
            this.pnlClassCaptions.Controls.Add(this.lblSkillPointsRemaining);
            this.pnlClassCaptions.Controls.Add(this.lblSkillPointsRemainingCaption);
            this.pnlClassCaptions.Location = new System.Drawing.Point(39, 15);
            this.pnlClassCaptions.Margin = new System.Windows.Forms.Padding(0);
            this.pnlClassCaptions.Name = "pnlClassCaptions";
            this.pnlClassCaptions.Size = new System.Drawing.Size(300, 45);
            this.pnlClassCaptions.TabIndex = 20;
            // 
            // lblBuildName
            // 
            this.lblBuildName.AutoSize = true;
            this.lblBuildName.BackColor = System.Drawing.Color.Transparent;
            this.lblBuildName.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBuildName.ForeColor = System.Drawing.Color.White;
            this.lblBuildName.Location = new System.Drawing.Point(0, 0);
            this.lblBuildName.Margin = new System.Windows.Forms.Padding(0);
            this.lblBuildName.Name = "lblBuildName";
            this.lblBuildName.Size = new System.Drawing.Size(0, 15);
            this.lblBuildName.TabIndex = 14;
            // 
            // lblMastery
            // 
            this.lblMastery.AutoSize = true;
            this.lblMastery.BackColor = System.Drawing.Color.Transparent;
            this.lblMastery.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMastery.ForeColor = System.Drawing.Color.White;
            this.lblMastery.Location = new System.Drawing.Point(80, 15);
            this.lblMastery.Margin = new System.Windows.Forms.Padding(0);
            this.lblMastery.Name = "lblMastery";
            this.lblMastery.Size = new System.Drawing.Size(14, 15);
            this.lblMastery.TabIndex = 13;
            this.lblMastery.Text = "0";
            // 
            // lblMasteryCaption
            // 
            this.lblMasteryCaption.AutoSize = true;
            this.lblMasteryCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblMasteryCaption.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMasteryCaption.ForeColor = System.Drawing.Color.White;
            this.lblMasteryCaption.Location = new System.Drawing.Point(60, 15);
            this.lblMasteryCaption.Margin = new System.Windows.Forms.Padding(0);
            this.lblMasteryCaption.Name = "lblMasteryCaption";
            this.lblMasteryCaption.Size = new System.Drawing.Size(17, 15);
            this.lblMasteryCaption.TabIndex = 12;
            this.lblMasteryCaption.Text = "M";
            // 
            // lblLevel
            // 
            this.lblLevel.AutoSize = true;
            this.lblLevel.BackColor = System.Drawing.Color.Transparent;
            this.lblLevel.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevel.ForeColor = System.Drawing.Color.White;
            this.lblLevel.Location = new System.Drawing.Point(25, 15);
            this.lblLevel.Margin = new System.Windows.Forms.Padding(0);
            this.lblLevel.Name = "lblLevel";
            this.lblLevel.Size = new System.Drawing.Size(14, 15);
            this.lblLevel.TabIndex = 11;
            this.lblLevel.Text = "0";
            // 
            // lblLevelCaption
            // 
            this.lblLevelCaption.AutoSize = true;
            this.lblLevelCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblLevelCaption.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLevelCaption.ForeColor = System.Drawing.Color.White;
            this.lblLevelCaption.Location = new System.Drawing.Point(0, 15);
            this.lblLevelCaption.Margin = new System.Windows.Forms.Padding(0);
            this.lblLevelCaption.Name = "lblLevelCaption";
            this.lblLevelCaption.Size = new System.Drawing.Size(22, 15);
            this.lblLevelCaption.TabIndex = 10;
            this.lblLevelCaption.Text = "Lv.";
            // 
            // lblSkillPointsRemaining
            // 
            this.lblSkillPointsRemaining.AutoSize = true;
            this.lblSkillPointsRemaining.BackColor = System.Drawing.Color.Transparent;
            this.lblSkillPointsRemaining.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSkillPointsRemaining.ForeColor = System.Drawing.Color.White;
            this.lblSkillPointsRemaining.Location = new System.Drawing.Point(71, 30);
            this.lblSkillPointsRemaining.Margin = new System.Windows.Forms.Padding(0);
            this.lblSkillPointsRemaining.Name = "lblSkillPointsRemaining";
            this.lblSkillPointsRemaining.Size = new System.Drawing.Size(14, 15);
            this.lblSkillPointsRemaining.TabIndex = 9;
            this.lblSkillPointsRemaining.Text = "0";
            // 
            // lblSkillPointsRemainingCaption
            // 
            this.lblSkillPointsRemainingCaption.AutoSize = true;
            this.lblSkillPointsRemainingCaption.BackColor = System.Drawing.Color.Transparent;
            this.lblSkillPointsRemainingCaption.Font = new System.Drawing.Font("Comic Sans MS", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSkillPointsRemainingCaption.ForeColor = System.Drawing.Color.White;
            this.lblSkillPointsRemainingCaption.Location = new System.Drawing.Point(0, 30);
            this.lblSkillPointsRemainingCaption.Margin = new System.Windows.Forms.Padding(0);
            this.lblSkillPointsRemainingCaption.Name = "lblSkillPointsRemainingCaption";
            this.lblSkillPointsRemainingCaption.Size = new System.Drawing.Size(68, 15);
            this.lblSkillPointsRemainingCaption.TabIndex = 8;
            this.lblSkillPointsRemainingCaption.Text = "Skill Points:";
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.pnlClass.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbClass)).EndInit();
            this.pnlClassData.ResumeLayout(false);
            this.pnlClassData.PerformLayout();
            this.pnlBuilds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBuilds)).EndInit();
            this.pnlInventory.ResumeLayout(false);
            this.pnlTrees.ResumeLayout(false);
            this.pnlTrees.PerformLayout();
            this.pnlClassCaptions.ResumeLayout(false);
            this.pnlClassCaptions.PerformLayout();
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
        private System.Windows.Forms.Panel pnlInventory;
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
        private System.Windows.Forms.Panel pnlBuilds;
        private System.Windows.Forms.Button btnNavBuilds;
        private System.Windows.Forms.Button btnNavTrees;
        private System.Windows.Forms.Button btnNavInventory;
        private System.Windows.Forms.DataGridView dgvBuilds;
        private System.Windows.Forms.Button btnBuildDelete;
        private System.Windows.Forms.Button btnBuildOpen;
        private System.Windows.Forms.Button btnNavSaveAs;
        private System.Windows.Forms.Button btnNavSave;
        private System.Windows.Forms.ToolTip toolTipInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn BuildName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stats;
        private System.Windows.Forms.Panel pnlClassCaptions;
        private System.Windows.Forms.Label lblBuildName;
        private System.Windows.Forms.Button btnBuildSharing;
        private PastebinPanel pbpBuildShare;
        private System.Windows.Forms.Panel pnlInventoryGear;
    }
}

