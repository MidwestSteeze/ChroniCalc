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
    public partial class SkillButton : UserControl
    {
        private MainForm form;
        readonly ResourceManager ResourceManagerImageSkill;
        public bool isPassiveBonusButton;
        public Skill skill;
        public SkillSelectPanel skillSelectPanel;
        public SkillTooltipPanel skillTooltipPanel;
        //TODO public Tree tree <-- would it be helpful to have?

        public SkillButton(Skill inSkill, TreeTableLayoutPanel parentControl, SkillTooltipPanel skillTooltip, MainForm inForm)
        {
            InitializeComponent();

            //Set the Skill
            skill = inSkill;

            this.Parent = parentControl;
            this.form = inForm;
            this.lblSkillLevel.Text = this.skill.level.ToString();
            this.skillTooltipPanel = skillTooltip;

            //Set the .Name property based on the Skill's Name
            this.Name = skill.id.ToString();

            //Specify defaults for this custom control
            this.pnlSkillButton.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);
            this.pnlSkillButton.Dock = DockStyle.Fill;

            //Background Image
            ResourceManagerImageSkill = new ResourceManager("ChroniCalc.ResourceImageSkill", Assembly.GetExecutingAssembly());

            if (!((Image)ResourceManagerImageSkill.GetObject(MainForm.IMAGE_FILENAME_PREFIX + skill.id.ToString()) is null))
            {                
                this.pbSkillIcon.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject(MainForm.IMAGE_FILENAME_PREFIX + skill.id.ToString());
            }
            else
            {
                //Use an ImageNotFound image as a placeholder until the skill's image is created and added to the Resource Manager
                this.pbSkillIcon.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject("ImageNotFound");
            }

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public void SkillButton_MouseUp(object sender, MouseEventArgs e)
        {
            //START Debug Info
            //string debugMessage = "";

            //debugMessage += "Skill: " + this.skill.name + "\n" +
            //                "XPos: " + this.skill.x + "\n" +
            //                "YPos:" + this.skill.y + "\n" +
            //                "id:" + this.skill.id + "\n" +
            //                "Width:" + this.Width + "\n" +
            //                "height:" + this.Height;

            //MessageBox.Show(debugMessage);
            //END Debug Info

            //Don't process this MouseUp event on the button if it's the Class skill counter (the one that provides the passive damage bonus based on # of points spent)
            //  NOTE: doing this instead of disabling the button entirely because we still want hover capabilities AND disabling actually hides the control
            if (this.skill.name == (this.Parent as TreeTableLayoutPanel).passiveSkillName)
            {
                return;
            }

            if (e.Button == MouseButtons.Left && Control.ModifierKeys == Keys.Shift && this.skillSelectPanel != null)
            {
                //Redisplay the Skill Select Panel if the skill was shift-clicked
                if (!this.skillSelectPanel.Visible)
                {
                    this.skillSelectPanel.Show();
                }
            }
            else
            { 
                if (e.Button == MouseButtons.Left && this.skill.level < this.skill.max_rank && HavePrereqs() && (form.build.level < MainForm.SKILL_POINTS_MAX))
                {
                    //Increase the level
                    this.skill.level++;

                    UpdateSkillPointAndLevelCounter(1);
                }
                else if (e.Button == MouseButtons.Right && this.skill.level > 0)
                {
                    //Decrease the level
                    this.skill.level--;

                    UpdateSkillPointAndLevelCounter(-1);
                }

                //Update the displayed level on the skill button
                this.lblSkillLevel.Text = this.skill.level.ToString();

                //Update the tooltip description based on the new level of the skill
                this.skillTooltipPanel.PopulateDescription();

                 //TODO update stats (damage, health, mana, etc)
            }
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
                if (preReqSkillButton.skill.level < 1)
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
            TreeTableLayoutPanel ttlpTree = (TreeTableLayoutPanel)this.Parent;

            //Adjust the total spent skill points counter

            //Adjust the level of the Build
            form.build.level += levelAdjust;

            //Update the total spent skill points on this particular Tree for the passive bonus stats
            ttlpTree.skillPointsAllocated = ttlpTree.skillPointsAllocated + levelAdjust;
            SkillButton passiveBonusBtn = (SkillButton)ttlpTree.Controls.Find(ttlpTree.passiveSkillId.ToString(), true).First();
            passiveBonusBtn.skill.level = ttlpTree.skillPointsAllocated;
            passiveBonusBtn.lblSkillLevel.Text = passiveBonusBtn.skill.level.ToString();

            //Update the label that shows remaining points that can be spent
           (form.Controls.Find("lblSkillPointsRemaining", true).First() as Label).Text = (MainForm.SKILL_POINTS_MAX - form.build.level).ToString();

            //Update the current level of the character based on how many skill points have been spent
            (form.Controls.Find("lblLevel", true).First() as Label).Text = form.build.level.ToString();

            //Update the level as shown in the SkillTooltipPanel
            this.skillTooltipPanel.UpdateRankText(this.skill.level);
        }

        private void PbSkillIcon_MouseHover(object sender, EventArgs e)
        {
            if (!this.skillTooltipPanel.Visible)
            {
                this.skillTooltipPanel.Location = GetTooltipLocation();
                this.skillTooltipPanel.BringToFront();
                this.skillTooltipPanel.Visible = true;
            }
        }

        private void PbSkillIcon_MouseLeave(object sender, EventArgs e)
        {
            if (this.skillTooltipPanel.Visible)
            {
                this.skillTooltipPanel.Visible = false;
            }
        }

        private Point GetTooltipLocation()
        {
            int availableHeight;
            int availableWidth;
            int tooltipHeight;
            int tooltipWidth;
            
            Point cursorPosition;
            Point location;
            Point pointToClient;
            Point parentContainerLocationOffsetLeft;
            Point parentContainerLocationOffsetTop;
            
            //Get the position of the cursor relative to the Application
            cursorPosition = Cursor.Position;
            pointToClient = this.form.PointToClient(cursorPosition);
            parentContainerLocationOffsetLeft = this.form.Controls.Find("pnlClassData", false).First().Location;
            parentContainerLocationOffsetTop = this.skillTooltipPanel.Parent.Location;

            //Location to display the tooltip based on where the mouse cursor hover event occured
            location = new Point(pointToClient.X - parentContainerLocationOffsetLeft.X + 10, pointToClient.Y - parentContainerLocationOffsetTop.Y);
            
            //Check if the tooltip is going off one of the edges of the screen and adjust as necessary
            // Height of the panel the tooltip is contained within
            availableHeight = this.Parent.Parent.Height;
            // Width of the panel the tooltip is contained within
            availableWidth = this.Parent.Parent.Width;
            // Height of the tooltip
            tooltipHeight = this.skillTooltipPanel.Height;
            // Width of the tooltip
            tooltipWidth = this.skillTooltipPanel.Width;
            
            if (location.X + tooltipWidth > availableWidth)
            {
                //The tooltip went off the right edge of the screen, so position it left of the mouse cursor instead of the right
                location.X = location.X - tooltipWidth - 20;
            }

            if (location.Y + tooltipHeight > availableHeight)
            {
                //The tooltip went off the lower edge of the screen, so position it above the mouse cursor instead of below
                location.Y = location.Y - tooltipHeight;

                //See if the tooltip now goes off the top edge of the screen
                if (location.Y < 0)
                {
                    //It does, so just position it along the top edge
                    location.Y = 0;
                }
            }

            return location;
        }
    }
}
