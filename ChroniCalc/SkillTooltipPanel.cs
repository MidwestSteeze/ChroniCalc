using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChroniCalc
{
    public partial class SkillTooltipPanel : UserControl
    {
        private Element element;
        private MainForm form;
        readonly ResourceManager ResourceManagerImageSkill;
        public Skill skill;
        public TreeTableLayoutPanel treeTableLayoutPanel;

        public SkillTooltipPanel(Skill inSkill, MainForm inForm)
        {
            InitializeComponent();

            this.form = inForm;
            this.skill = inSkill;
            this.element = new Element(this.skill.element);

            //Specify defaults for this custom control
            //Max width and height of labels (to enable word-wrapping)
            lblDescription.MaximumSize = new Size(lblDescription.Parent.Width - (lblDescription.Left * 2), this.lblDescription.Font.Height * 3);
            lblMaxRankDescription.MaximumSize = new Size(lblMaxRankDescription.Parent.Width - (lblMaxRankDescription.Left * 2), this.lblMaxRankDescription.Font.Height * 3);

            this.Parent = form.Controls.Find("pnlTrees", true).First();
            this.Visible = false;

            //Fill out the controls on this custom SkillTooltipPanel
            this.lblDescription.Text = PopulateDescription(false);

            this.lblElement.Text = this.skill.element;
            this.lblElement.ForeColor = this.element.color;

            this.lblMaxRankDescription.Text = PopulateDescription(true);
            this.lblName.Text = this.skill.name;
            UpdateRankText(0); //this updates lblRank.Text
            this.lblType.Text = this.skill.type;

            //Background Image
            ResourceManagerImageSkill = new ResourceManager("ChroniCalc.ResourceImageSkill", Assembly.GetExecutingAssembly());

            if (!((Image)ResourceManagerImageSkill.GetObject(MainForm.IMAGE_FILENAME_PREFIX + skill.id.ToString()) is null))
            {
                this.pbIcon.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject(MainForm.IMAGE_FILENAME_PREFIX + skill.id.ToString());
            }
            else
            {
                //Use an ImageNotFound image as a placeholder until the skill's image is created and added to the Resource Manager
                this.pbIcon.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject("ImageNotFound");
            }
        }

        private string PopulateDescription(bool maxRank)
        {
            string description = this.skill.description;

            //Fill in the description with data (ie. damage and modifiers)
            //TODO

            return description;
        }

        public void UpdateRankText(int level)
        {
            //Set the current level of this skill
            lblRank.Text = "Rank " + level.ToString() + "/" + this.skill.max_rank.ToString();
        }
    }
}
