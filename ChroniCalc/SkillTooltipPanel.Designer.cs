﻿namespace ChroniCalc
{
    partial class SkillTooltipPanel
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
            this.pnlTooltip = new System.Windows.Forms.Panel();
            this.pnlMaxRank = new System.Windows.Forms.Panel();
            this.lblMaxRankCaption = new System.Windows.Forms.Label();
            this.lblMaxRankDescription = new System.Windows.Forms.Label();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblRank = new System.Windows.Forms.Label();
            this.lblType = new System.Windows.Forms.Label();
            this.lblElement = new System.Windows.Forms.Label();
            this.lblPointsRequirement = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.pbIcon = new System.Windows.Forms.PictureBox();
            this.pnlTooltip.SuspendLayout();
            this.pnlMaxRank.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTooltip
            // 
            this.pnlTooltip.Controls.Add(this.pnlMaxRank);
            this.pnlTooltip.Controls.Add(this.lblDescription);
            this.pnlTooltip.Controls.Add(this.lblRank);
            this.pnlTooltip.Controls.Add(this.lblType);
            this.pnlTooltip.Controls.Add(this.lblElement);
            this.pnlTooltip.Controls.Add(this.lblPointsRequirement);
            this.pnlTooltip.Controls.Add(this.lblName);
            this.pnlTooltip.Controls.Add(this.pbIcon);
            this.pnlTooltip.Location = new System.Drawing.Point(0, 0);
            this.pnlTooltip.Name = "pnlTooltip";
            this.pnlTooltip.Size = new System.Drawing.Size(215, 215);
            this.pnlTooltip.TabIndex = 0;
            // 
            // pnlMaxRank
            // 
            this.pnlMaxRank.Controls.Add(this.lblMaxRankCaption);
            this.pnlMaxRank.Controls.Add(this.lblMaxRankDescription);
            this.pnlMaxRank.Location = new System.Drawing.Point(10, 108);
            this.pnlMaxRank.Name = "pnlMaxRank";
            this.pnlMaxRank.Size = new System.Drawing.Size(200, 100);
            this.pnlMaxRank.TabIndex = 9;
            // 
            // lblMaxRankCaption
            // 
            this.lblMaxRankCaption.AutoSize = true;
            this.lblMaxRankCaption.Location = new System.Drawing.Point(3, 3);
            this.lblMaxRankCaption.Name = "lblMaxRankCaption";
            this.lblMaxRankCaption.Size = new System.Drawing.Size(72, 13);
            this.lblMaxRankCaption.TabIndex = 7;
            this.lblMaxRankCaption.Text = "At Max Rank:";
            // 
            // lblMaxRankDescription
            // 
            this.lblMaxRankDescription.AutoSize = true;
            this.lblMaxRankDescription.Location = new System.Drawing.Point(3, 16);
            this.lblMaxRankDescription.Name = "lblMaxRankDescription";
            this.lblMaxRankDescription.Size = new System.Drawing.Size(105, 13);
            this.lblMaxRankDescription.TabIndex = 8;
            this.lblMaxRankDescription.Text = "maxRankDescription";
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(10, 75);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(58, 13);
            this.lblDescription.TabIndex = 6;
            this.lblDescription.Text = "description";
            // 
            // lblRank
            // 
            this.lblRank.AutoSize = true;
            this.lblRank.Location = new System.Drawing.Point(10, 62);
            this.lblRank.Name = "lblRank";
            this.lblRank.Size = new System.Drawing.Size(28, 13);
            this.lblRank.TabIndex = 5;
            this.lblRank.Text = "rank";
            // 
            // lblType
            // 
            this.lblType.AutoSize = true;
            this.lblType.Location = new System.Drawing.Point(10, 49);
            this.lblType.Name = "lblType";
            this.lblType.Size = new System.Drawing.Size(85, 13);
            this.lblType.TabIndex = 4;
            this.lblType.Text = "ActiveOrPassive";
            // 
            // lblElement
            // 
            this.lblElement.AutoSize = true;
            this.lblElement.Location = new System.Drawing.Point(43, 36);
            this.lblElement.Name = "lblElement";
            this.lblElement.Size = new System.Drawing.Size(44, 13);
            this.lblElement.TabIndex = 3;
            this.lblElement.Text = "element";
            // 
            // lblPointsRequirement
            // 
            this.lblPointsRequirement.AutoSize = true;
            this.lblPointsRequirement.Location = new System.Drawing.Point(43, 23);
            this.lblPointsRequirement.Name = "lblPointsRequirement";
            this.lblPointsRequirement.Size = new System.Drawing.Size(96, 13);
            this.lblPointsRequirement.TabIndex = 2;
            this.lblPointsRequirement.Text = "points_requirement";
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(43, 10);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(35, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name";
            // 
            // pbIcon
            // 
            this.pbIcon.Location = new System.Drawing.Point(10, 10);
            this.pbIcon.Name = "pbIcon";
            this.pbIcon.Size = new System.Drawing.Size(30, 30);
            this.pbIcon.TabIndex = 0;
            this.pbIcon.TabStop = false;
            // 
            // SkillTooltipPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnlTooltip);
            this.Name = "SkillTooltipPanel";
            this.Size = new System.Drawing.Size(215, 215);
            this.pnlTooltip.ResumeLayout(false);
            this.pnlTooltip.PerformLayout();
            this.pnlMaxRank.ResumeLayout(false);
            this.pnlMaxRank.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTooltip;
        private System.Windows.Forms.Panel pnlMaxRank;
        private System.Windows.Forms.Label lblMaxRankCaption;
        private System.Windows.Forms.Label lblMaxRankDescription;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblRank;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.Label lblElement;
        private System.Windows.Forms.Label lblPointsRequirement;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.PictureBox pbIcon;
    }
}
