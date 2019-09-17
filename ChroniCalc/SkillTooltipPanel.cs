using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChroniCalc
{
    public partial class SkillTooltipPanel : UserControl
    {
        private MainForm form;
        public Skill skill;
        public TreeTableLayoutPanel treeTableLayoutPanel;

        public SkillTooltipPanel(Skill inSkill, MainForm inForm)
        {
            InitializeComponent();

            this.form = inForm;
            this.skill = inSkill;

            //Specify defaults for this custom control
            this.Parent = form.Controls.Find("pnlTrees", true).First();
            this.Visible = false;

            //Fill out the controls on this custom SkillTooltipPanel
            this.lblType.Text = this.skill.type;
            this.lblDescription.Text = PopulateDescription(false);
            this.lblElement.Text = this.skill.element;
            this.lblMaxRankDescription.Text = PopulateDescription(true);
            this.lblName.Text = this.skill.name;
            this.lblPointsRequirement.Text = GetPointsRequirement().ToString();
            this.lblRank.Text = GetCurrentRank().ToString();            
        }

        private string PopulateDescription(bool maxRank)
        {
            string description = this.skill.description;

            //Fill in the description with data (ie. damage and modifiers)
            //TODO

            return description;
        }

        private int GetPointsRequirement()
        {
            int requiredPoints = 0;

            //Determine how many points are needed to be spent before this skill can be allocated
            /*
             * TODO: will need a .level property on the TreeTableLayoutPanel itself
             * #PointsInTree = tree.skillPointsAllocated;
             * this.skill.x = Find the X position in the tree
             * this.skill.x * 5 = # of points (to get to each X requires 5 points)
             * (this.skill.x * 5) - #PointsInTree --> Positive means it can be allocated, negative means display msg in this.lblPointsRequirement.Text
             */
            //TODO

            return requiredPoints;
        }

        private int GetCurrentRank()
        {
            int rank = 0;

            //Get the current level of this skill
            //TODO

            return rank;
        }
    }
}
