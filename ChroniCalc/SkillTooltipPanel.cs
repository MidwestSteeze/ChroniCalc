using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChroniCalc
{
    public partial class SkillTooltipPanel : UserControl
    {
        private const int MARGIN_VERTICAL = 3;
        private const int PADDING_VERTICAL = 3;

        private Element element;
        private MainForm form;
        private int level;
        private List<string> replaceWords;

        readonly ResourceManager ResourceManagerImageSkill;
        public Skill skill;
        public TreeTableLayoutPanel treeTableLayoutPanel;

        public SkillTooltipPanel(Skill inSkill, TreeTableLayoutPanel tlpTree, MainForm inForm)
        {
            InitializeComponent();

            this.form = inForm;
            this.skill = inSkill;
            this.treeTableLayoutPanel = tlpTree;
            this.element = new Element(this.skill.element);

            //Specify defaults for this custom control
            //Max width and height of labels (to enable word-wrapping)
            lblDescription.MaximumSize = new Size(lblDescription.Parent.Width - (lblDescription.Left * 2), this.lblDescription.Font.Height * 7);

            this.Parent = form.Controls.Find("pnlTrees", true).First();
            this.Visible = false;

            //Fill out the controls on this custom SkillTooltipPanel

            this.lblElement.Text = this.skill.element;
            this.lblElement.ForeColor = this.element.color;


            this.lblName.Text = this.skill.name;
            UpdateRankText(skill.level);
            this.lblTypeAndFamily.Text = this.skill.type + (skill.family != "None" ? (", " + skill.family) : "");

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

            //Dynamically adjust the size of the tooltip based on the heght of its label controls now that they're populated
            //UpdateHeightAndControlPositions();

            this.replaceWords = BuildReplaceWordsList();
        }

        private List<string> BuildReplaceWordsList()
        {
			//A list of capitalized words found in a skill's description that are placeholders to be replaced
			// with actual values based on the skill's level
            List<string> capitalWords = new List<string>
            {
                "DAMAGE",
                "DURATION",
                "EFFECT",
                "PROC",
                "RANGE",
                "RANGE2",
                "REQUIRED",
                "VALUE"
            };

            return capitalWords;
        }

        public void PopulateDescription()
        {
            string replaceValue;
            string description;

            //Get the skill's base description with words that need to be replaced
            description = this.skill.description;

            //Loop through all words to be replaced with values
            foreach (string replaceWord in this.replaceWords)
            {
                //See if the current replaceWord exists in the skill's description
                if (description.Contains(replaceWord))
                {
                    //Pull the value from the xml for the current word to be replaced with a value
                    replaceValue = GetReplacementValue(replaceWord);

                    //Replace the placeholder word with the found value into the description
                    description = description.Replace(replaceWord, replaceValue);
                }
            }

            //Update the description label text
            this.lblDescription.Text = description;

            //Dynamically adjust the size of the tooltip based on the heght of its label controls now that they're populated
            UpdateHeightAndControlPositions();
        }

        private string GetReplacementValue(string replaceWord)
        {
            int index = level;
            string replaceValue = string.Empty;

            //Adjust the lookup index depending on the current level of the skill
            if (level > 0)
            {
                //Adjust index beacuse it's a 0-based lookup
                index -= 1;
            }
            else
            {
                //No level is yet assigned so show the level 1 value
                index = 0;
            }

            //Define a mapping for which replaceWord should pull which skill.property
            switch (replaceWord)
            {
                case "DAMAGE":
                    //DAMAGE is stored with a percent sign, trim it off the end
                    replaceValue = skill.damage[index].Substring(0, skill.damage[index].IndexOf("%"));
                    break;
                case "DURATION":
                    replaceValue = skill.duration[index].ToString();
                    break;
                case "EFFECT":
                    //EFFECT is stored with a percent sign, trim it off the end
                    replaceValue = skill.effect[index].Substring(0, skill.effect[index].IndexOf("%"));
                    break;
                case "PROC":
                    //PROC chance is stored in hundreds; divide by 100 to get the percentage to display
                    replaceValue = (skill.proc[index] / 100).ToString();
                    break;
                case "RANGE":
                    replaceValue = skill.range[index].ToString();
                    break;
                case "RANGE2":
                    replaceValue = skill.range2[index].ToString();
                    break;
                case "REQUIRED":
                    Control[] preReqControls;
                    int preReqId;
                    SkillButton preReqSkillButton;

                    if (skill.skill_requirement.Length > 1)
                    {
                        //The skills prereq is a multiselect skill so find which one the user chose
                        foreach (int id in skill.skill_requirement)
                        {
                            preReqControls = treeTableLayoutPanel.Controls.Find(id.ToString(), false);

                            if (preReqControls.Length < 1)
                            {
                                if (Array.IndexOf(skill.skill_requirement, id) == skill.skill_requirement.Length - 1)
                                {
                                    //PreReq control not found; this is because no Skill has yet been selected from the positions SkillSelect panel
                                    // so use some generic placeholder verbiage
                                    replaceValue = "Previous Skill";
                                    break;
                                }

                                //There are more PreReq IDs to search through, so move onto the next iteration
                                continue;
                            }

                            //We found the preReqId that the user chose to use on the tree
                            preReqId = id;
                            preReqSkillButton = (SkillButton)preReqControls.First();
                            //Get the name of the prereq skill by finding the control with the skill id and pulling skill.name
                            replaceValue = preReqSkillButton.skill.name;

                            break;
                        }
                    }
                    else
                    {
                        //Assume there is only 1 prereq skill id (and not that skill_requirement=none, 
                        // otherwise we wouldn't be looking for a replacement value for REQURIED in the description)
                        preReqId = skill.skill_requirement[0];
                        preReqControls = treeTableLayoutPanel.Controls.Find(preReqId.ToString(), false);
                        preReqSkillButton = (SkillButton)preReqControls.First();

                        //Get the name of the prereq skill by finding the control with the skill id and pulling skill.name
                        replaceValue = preReqSkillButton.skill.name;
                    }
                    break;
                case "VALUE":
                    replaceValue = skill.value[index].ToString();
                    break;
                default:
                    MessageBox.Show("SkillTooltipPanel.GetReplacementValue(): No mapping found for " + replaceWord + ".");
                    break;
            }

            return replaceValue;
        }

        public void UpdateRankText(int inLevel)
        {
            //Set the current level of this skill
            level = inLevel;
            lblRank.Text = "Rank " + level.ToString() + "/" + this.skill.max_rank.ToString();
        }

        public void UpdateHeightAndControlPositions()
        {
            //Adjust position and size of controls that may have been imapcted by dynamically-sized controls above it

            // Set the height of the tooltip now that all controls are sized and positioned within it based on their content
            // NOTE: Need to set the SkillTooltipPanel control Height AND the pnlTooltip Height because the main control (SkillTooltipPanel) is not set to Autosize as pnlTooltip grows
            this.Height = lblDescription.Location.Y + lblDescription.Height + (MARGIN_VERTICAL * 2);
            this.pnlTooltip.Height = lblDescription.Location.Y + lblDescription.Height + (MARGIN_VERTICAL * 2);
        }
    }
}
