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
            lblMaxRankDescription.MaximumSize = new Size(lblMaxRankDescription.Parent.Width - (lblMaxRankDescription.Left * 2), this.lblMaxRankDescription.Font.Height * 7);

            this.Parent = form.Controls.Find("pnlTrees", true).First();
            this.Visible = false;

            //Fill out the controls on this custom SkillTooltipPanel

            this.lblElement.Text = this.skill.element;
            this.lblElement.ForeColor = this.element.color;


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

            //Dynamically adjust the size of the tooltip based on the heght of its label controls now that they're populated
            //UpdateHeightAndControlPositions();
        }

        public void PopulateDescription(bool maxRank)
        {
            string replaceValue;
            string replaceWord;
            string description;
            IEnumerable<string> capitalWords;
            List<string> replaceWords = new List<string>();

            description = this.skill.description;
            
            //Capture all uppercase words in the description that should be replaced with data from the skill itself (ie. DAMAGE, DURATION, RANGE, etc)
            capitalWords = description.Split(' ').Where(x => x == x.ToUpper()); //TODOSSG change this to only look at words >= 2 characters, or build a stored list of replaceWords so there's no confusion

            //Loop through all words to be replaced with values
            foreach (string capitalWord in capitalWords)
            {
                //Remove any non-alpha characters (ie. ".", "%", etc)
                replaceWord = Regex.Replace(capitalWord, "[^A-Z0-9]", string.Empty, RegexOptions.Compiled);
                replaceWords.Add(replaceWord);

                //Pull the value from the xml for the current word to be replaced with a value
                replaceValue = GetReplacementValue(replaceWord);

                //Once you have the value, do a StringReplace(replaceWord, replaceValue);
                description = description.Replace(replaceWord, replaceValue);
            }

            //Set the updated description onto the Skill and also on the label that shows it
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
                //No level is yet assigned so show the max_rank value
                index = skill.max_rank - 1;
            }

            //Define a mapping for which replaceWord should pull which skill.property
            switch (replaceWord)
            {
                case "EFFECT":
                    replaceValue = skill.effect[index];
                    break;
                case "DURATION":
                    replaceValue = skill.duration[index].ToString();
                    break;
                case "REQUIRED":
                    Control[] preReqControls;
                    int preReqId;
                    SkillButton preReqSkillButton;

                    if (skill.skill_requirement.Length > 1)
                    {
                        //The skills prereq is a multiselect skill so find which one the user chose
                        //TODOSSG i have this logic somewhere i can reuse
                        foreach (int id in skill.skill_requirement)
                        {
                            preReqControls = treeTableLayoutPanel.Controls.Find(id.ToString(), false);

                            if (preReqControls.Length < 1)
                            {
                                if (Array.IndexOf(skill.skill_requirement, id) == skill.skill_requirement.Length - 1)
                                {
                                    //PreReq control not found; this is because no Skill has yet been selected from the positions SkillSelect panel
                                    //TODOSSG test this and see how to handle it
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
                //case "":
                //    replaceValue = skill.x.ToString();
                    //break;
                case "DAMAGE":
                    replaceValue = skill.damage[index];
                    break;
                case "RANGE2":
                    replaceValue = skill.range2[index].ToString();
                    break;
                //case "":
                //    replaceValue = skill.family;
                //    break;
                //case "":
                //    replaceValue = skill.min_level;
                //    break;
                //case "":
                //    replaceValue = skill.id;
                //    break;
                case "RANGE":
                    replaceValue = skill.range[index].ToString();
                    break;
                //case "":
                //    replaceValue = skill.element;
                //    break;
                case "VALUE":
                    replaceValue = skill.value[index].ToString();
                    break;
                case "PROC":
                    replaceValue = skill.proc[index].ToString();
                    break;
                //case "":
                //    replaceValue = skill.type;
                //    break;
                //case "":
                //    replaceValue = skill.description_next;
                //    break;
                //case "":
                //    replaceValue = skill.description;
                //    break;
                //case "":
                //    replaceValue = skill.max_rank;
                //    break;
                //case "":
                //    replaceValue = skill.y;
                //    break;
                //case "":
                //    replaceValue = skill.name;
                //    break;
                default:
                    break; //throw error that mapping was not found for replaceWord and needs to be added
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
            // Set a new Height and Top for pnlMaxRank since the lblDescription control above it may have changed in size once its text was set
            pnlMaxRank.Height = lblMaxRankCaption.Height + lblMaxRankDescription.Height + PADDING_VERTICAL;
            pnlMaxRank.Top = lblDescription.Location.Y + lblDescription.Height + MARGIN_VERTICAL;

            // Set the height of the tooltip now that all controls are sized and positioned within it based on their content
            // NOTE: Need to set the SkillTooltipPanel control Height AND the pnlTooltip Height because the main control (SkillTooltipPanel) is not set to Autosize as pnlTooltip grows
            this.Height = pnlMaxRank.Location.Y + pnlMaxRank.Height + (MARGIN_VERTICAL * 2);
            this.pnlTooltip.Height = pnlMaxRank.Location.Y + pnlMaxRank.Height + (MARGIN_VERTICAL * 2);
        }
    }
}
