using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChroniCalc
{
    //A custom button that has the ability to hold a Skill object
    public partial class SkillButton : Button
    {
        private MainForm form;
        public int level;
        readonly ResourceManager ResourceManagerImageSkill;
        public Skill skill;
        public SkillTooltipPanel skillTooltipPanel;
        //TODO public Tree tree <-- would it be helpful to have?

        public SkillButton(Skill inSkill, TreeTableLayoutPanel parentControl, SkillTooltipPanel skillTooltip, MainForm inForm)
        {
            InitializeComponent();

            this.Parent = parentControl;
            this.form = inForm;
            this.level = 0;
            this.skillTooltipPanel = skillTooltip;

            //Set the Skill
            skill = inSkill;

            //Set the .Name property based on the Skill's Name
            this.Name = skill.id.ToString();

            //Specify defaults for this custom control

            //Background Image
            ResourceManagerImageSkill = new ResourceManager("ChroniCalc.ResourceImageSkill", Assembly.GetExecutingAssembly());

            if (!((Image)ResourceManagerImageSkill.GetObject(MainForm.IMAGE_FILENAME_PREFIX + skill.id.ToString()) is null))
            {                
                this.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject(MainForm.IMAGE_FILENAME_PREFIX + skill.id.ToString());
            }
            else
            {
                //Use an ImageNotFound image as a placeholder until the skill's image is created and added to the Resource Manager
                this.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject("ImageNotFound");
            }

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void SkillButton_MouseUp(object sender, MouseEventArgs e)
        {
            //START Debug Info
            //string debugMessage;

            //debugMessage = "Skill: " + this.skill.name + "\n" +
            //                "XPos: " + this.skill.x + "\n" +
            //                "YPos:" + this.skill.y + "\n" +
            //                "id:" + this.skill.id + "\n" +
            //                "Width:" + this.Width + "\n" +
            //                "height:" + this.Height;

            //MessageBox.Show(debugMessage);
            //END Debug Info

            //Adjust the level of the skill, ensuring we're not beyond the min/max allowed
            if (e.Button == MouseButtons.Left && this.level < this.skill.max_rank && HavePrereqs() && (form.SkillPointsUsed < MainForm.SKILL_POINTS_MAX))
            {
                //Increase the level
                this.level++;

                UpdateSkillPointAndLevelCounter(1);
            }
            else if (e.Button == MouseButtons.Right && this.level > 0)
            {
                //TODO don't let user de-level if it's going to set level to 0 and this skill is a needed prereq
                //Decrease the level
                this.level--;

                UpdateSkillPointAndLevelCounter(-1);
            }

            //Update the displayed level on the skill button
            if (this.level == 0)
            {
                this.Text = "";
            }
            else
            {
                this.Text = this.level.ToString();
            }

            //TODO update stats (damage, health, mana, etc)
        }

        private bool HavePrereqs()
        {
            bool result = true;
            Control[] preReqControls;
            SkillButton preReqSkillButton;

            //Get the Tree this Skill belong to
            TreeTableLayoutPanel ttlp = (TreeTableLayoutPanel)this.Parent;

            //Compile list of prereq skill ids from the current skill's skill_requirement property
            // NOTE: There's always only 1 prereq skill but we need this as a list to handle when a MultiSelectSkill is a prereq (ie. one OR another)
            int[] prereqs = this.skill.skill_requirement;

            //Analyze each prereq skill id to see if it's leveled (we loop through multiple for cases where a skill is prereq'd by a MultiSelectSkill)
            foreach (int prereqSkillId in prereqs)
            {
                //Get the Skill button from the Tree by SkillID
                preReqControls = ttlp.Controls.Find(prereqSkillId.ToString(), false);

                //See if there is no prereq control found
                if (preReqControls.Length < 1)
                {
                    if (Array.IndexOf(prereqs, prereqSkillId) == prereqs.Length - 1)
                    {
                        //PreReq control not found; this is because no Skill has yet been selected from the positions SkillSelect panel
                        result = false;
                        break;
                    }

                    //There are more PreReq IDs to search through, so move onto the next iteration
                    continue;
                }

                preReqSkillButton = (SkillButton)preReqControls.First();

                //Verify the PreReq skill is leveled
                if (preReqSkillButton.level < 1)
                {
                    result = false;
                    break;
                }
                else
                {
                    //We found a leveled PreReq, there is no need to search the others so break out of the loop
                    result = true;
                    break;
                }
            }

            return result;
        }

        private void UpdateSkillPointAndLevelCounter(int levelAdjust)
        {
            int skillPointsUsed;
            TreeTableLayoutPanel ttlpTree = (TreeTableLayoutPanel)this.Parent;

            //Adjust the total spent skill points counter
            form.SkillPointsUsed += levelAdjust;
            skillPointsUsed = form.SkillPointsUsed;

            //Update the total spent skill points on this particular Tree for the passive bonus stats
            ttlpTree.skillPointsAllocated = ttlpTree.skillPointsAllocated + levelAdjust;

            SkillButton passiveBonusBtn = (SkillButton)ttlpTree.Controls.Find(ttlpTree.passiveSkillId.ToString(), true).First();
            passiveBonusBtn.level = ttlpTree.skillPointsAllocated;
            passiveBonusBtn.Text = passiveBonusBtn.level.ToString();

            //Update the label that shows remaining points that can be spent
           (form.Controls.Find("lblSkillPointsRemaining", true).First() as Label).Text = (MainForm.SKILL_POINTS_MAX - skillPointsUsed).ToString();

            //Update the current level of the character based on how many skill points have been spent
            (form.Controls.Find("lblLevel", true).First() as Label).Text = skillPointsUsed.ToString();
        }

        private void SkillButton_MouseHover(object sender, EventArgs e)
        {
            //Display the Skill Tooltip

        }
    }
}
