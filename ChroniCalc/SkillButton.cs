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

            if (!((Image)ResourceManagerImageSkill.GetObject(MainForm.GetSkillButtonIconFilename(parentControl.tree.name, inSkill.id)) is null))
            {                
                this.pbSkillIcon.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject(MainForm.GetSkillButtonIconFilename(parentControl.tree.name, inSkill.id));
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

            string treeName = (this.Parent as TreeTableLayoutPanel).tree.name;

            //Don't process this MouseUp event on the button if it's the Class or Mastery skill counter (the one that provides the passive damage bonus based on # of points spent)
            //  NOTE: doing this instead of disabling the button entirely because we still want hover capabilities AND disabling actually hides the control
            if (this.isPassiveBonusButton)
            {
                return;
            }

            //Re-display the SkillSelectPanel
            if (e.Button == MouseButtons.Right && this.skillSelectPanel != null)
            {
                //Redisplay the Skill Select Panel if the skill was shift-clicked
                if (!this.skillSelectPanel.Visible)
                {
                    this.skillSelectPanel.Show();
                    this.skillSelectPanel.BringToFront();
                }
                else
                {
                    // This is a safety incase the SkillSelectPanel was left visible but then a different Tree control was displayed on top of it; we need to ensure it's always in front when it's visible, so it can be seen
                    this.skillSelectPanel.BringToFront();
                }
            }
            else
            {
                //De-level the Skill
                if (e.Button == MouseButtons.Left && Control.ModifierKeys == (Keys.Control | Keys.Shift) && this.skill.level > 0)
                {
                    //Decrease the level by 1
                    UpdateSkillPointAndLevelCounter(this.skill.level, this.skill.level - 1);
                }
                //Level-up the Skill
                else if (e.Button == MouseButtons.Left && this.skill.level < this.skill.max_rank && HavePrereqs() && 
                        ((treeName != "Mastery" && form.build.Level < MainForm.SKILL_POINTS_MAX) || treeName == "Mastery"))
                {
                    if (Control.ModifierKeys == Keys.Shift)
                    {
                        int levelsAvailable = (this.skill.max_rank - this.skill.level >= 20) ? 20 : this.skill.max_rank - this.skill.level;
                        
                        //Increase the level by 20
                        UpdateSkillPointAndLevelCounter(this.skill.level, this.skill.level + levelsAvailable);
                    }
                    else if (Control.ModifierKeys == Keys.Control)
                    {
                        int levelsAvailable = (this.skill.max_rank - this.skill.level >= 5) ? 5 : this.skill.max_rank - this.skill.level;

                        //Increase the level by 5
                        UpdateSkillPointAndLevelCounter(this.skill.level, this.skill.level + levelsAvailable);
                    }
                    else
                    {
                        //Increase the level by 1
                        UpdateSkillPointAndLevelCounter(this.skill.level, this.skill.level + 1);
                    }
                }

                 //TODO update stats (damage, health, mana, etc)
            }
        }

        private bool HavePrereqs()
        {
            bool result = true;
            Control[] preReqControls;
            SkillButton preReqSkillButton;

            //Get the Tree this Skill belongs to
            TreeTableLayoutPanel ttlp = (TreeTableLayoutPanel)this.Parent;

            if (ttlp.tree.name == "Mastery")
            {
                // MASTERY TREE PREREQ LOGIC
                // Ensure that all prior controls linked to this one are at least level 1
                //   NOTE: Only going from the current skill.x position down to position 2 (read: columm 3) because the first two cells in a row are not selectable Skills and should not be considered
                for (int x = this.skill.x - 1; x >= 2; x--)
                {
                    if (ttlp.GetControlFromPosition(x, this.skill.y) is SkillButton)
                    {
                        preReqSkillButton = (ttlp.GetControlFromPosition(x,this.skill.y) as SkillButton);
                        if (preReqSkillButton.skill.level < 1)
                        {
                            // The current control is a selected Skill but has not yet been leveled, so the PreReq check has failed
                            result = false;
                            break;
                        }
                    }
                    else
                    {
                        // The current control is not a selected Skill and therefore isn't leveled, so the PreReq check has failed
                        result = false;
                        break;
                    }
                }
            }
            else
            {
                // CLASS TREE PREREQ LOGIC
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
            }

            // Check if we meet the minimum level requirement on a new Skill that was clicked to level up
            if ((result) && (this.skill.min_level > 0) && (this.skill.level < 1))
            {
                if (ttlp.tree.name == "Mastery")
                {
                    // If User selected the final skill in the Mastery Tree's 3 linked General Rows, ensure all 3 rows meet the min_level requirement of that Skill
                    if ((skill.y == 3) && (skill.x == ttlp.ColumnCount - 1))
                    {
                        const int MIN_LEVEL = 65;
                        SkillButton passiveBonusButton;

                        for (int row = 2; row <= 4; row++)
                        {
                            // Get the control at the beginning of the row which holds the total # of points spent in the row
                            passiveBonusButton = (SkillButton)ttlp.GetControlFromPosition(0, row);

                            if (passiveBonusButton.skill.level < MIN_LEVEL)
                            {
                                // The current row does not meet the level requirement of the selected Skill
                                result = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // On the Mastery Tree, check # of skill points spent on this row meets the minimum required to level this new Skill
                        result = ((ttlp.GetControlFromPosition(0, this.skill.y) as SkillButton).skill.level >= this.skill.min_level);
                    }
                }
                else
                {
                    // On a Class Skill Tree, check # of skill points spent on the tree is greater than the # of points required to allocate this skill
                    // NOTE: There are no min_level requirements on Class Skill trees so this is partially uselss right now (TODO but there are skill points allocated implied, like Column2 cannot be leveled until you have 8 points in the tree so you could do some math of treePointsAllocated >= skill.x * 8?)
                    result = (ttlp.skillPointsAllocated >= this.skill.min_level);
                }
            }

            return result;
        }

        public void UpdateSkillPointAndLevelCounter(int oldLevel, int newLevel)
        {
            int levelAdjust;
            TreeTableLayoutPanel ttlpTree;

            ttlpTree = (TreeTableLayoutPanel)this.Parent;

            //Get the value the Skill should have its level adjusted by and adjust the Skill and Build levels
            levelAdjust = newLevel - oldLevel;
            this.skill.level += levelAdjust;
            //Update the total spent skill points on this particular Tree for the passive bonus stats
            ttlpTree.skillPointsAllocated += levelAdjust;

            if (ttlpTree.Name == "Mastery")
            {
                form.build.MasteryLevel += levelAdjust;

                // Update the Mastery passive row counter located in the first cell (x=0) in the same row (via y-coordinate) as this Skill
                SkillButton passiveBonusBtn = (SkillButton)ttlpTree.GetControlFromPosition(0, this.skill.y);  //TODOSSG if Mastery Tree and Final Skill in the 3 General Rows, do all 3 Rows level up 1 or not at all?
                passiveBonusBtn.skill.level += levelAdjust;
                passiveBonusBtn.lblSkillLevel.Text = passiveBonusBtn.skill.level.ToString();

                // Update the rank and then the description on the Passive bonus button's tooltip since we have it here
                passiveBonusBtn.skillTooltipPanel.UpdateRankText(passiveBonusBtn.skill.level);
                passiveBonusBtn.skillTooltipPanel.PopulateDescription();

                // Update the current Mastery Level of the character based on how many Mastery points have been spent
                (form.Controls.Find("lblMastery", true).First() as Label).Text = form.build.MasteryLevel.ToString();
                // Set visibility of the Points Required text on this Skill Button and all Skills after it
                form.SetPointsRequiredVisibilities(ttlpTree, this.skill, passiveBonusBtn.skill.level);
            }
            else
            {
                form.build.Level += levelAdjust;

                SkillButton passiveBonusBtn = (SkillButton)ttlpTree.Controls.Find(ttlpTree.passiveSkillId.ToString(), true).First();
                passiveBonusBtn.skill.level = ttlpTree.skillPointsAllocated;
                passiveBonusBtn.lblSkillLevel.Text = passiveBonusBtn.skill.level.ToString();

                //Update the rank and then the description on the Passive bonus button's tooltip since we have it here
                passiveBonusBtn.skillTooltipPanel.UpdateRankText(passiveBonusBtn.skill.level);
                passiveBonusBtn.skillTooltipPanel.PopulateDescription();

                //Update the label that shows remaining points that can be spent
               (form.Controls.Find("lblSkillPointsRemaining", true).First() as Label).Text = (MainForm.SKILL_POINTS_MAX - form.build.Level).ToString();

                //Update the current Level of the character based on how many skill points have been spent
                (form.Controls.Find("lblLevel", true).First() as Label).Text = form.build.Level.ToString();
                // Set visibility of the Points Required text on this Skill Button and all Skills after it
                form.SetPointsRequiredVisibilities(ttlpTree, this.skill, ttlpTree.skillPointsAllocated);
            }

            //Update the displayed level on the skill button
            this.lblSkillLevel.Text = this.skill.level.ToString();

            //Update the level as shown in the SkillTooltipPanel
            this.skillTooltipPanel.UpdateRankText(this.skill.level);

            //Update the tooltip description based on the new level of the skill
            this.skillTooltipPanel.PopulateDescription();
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
