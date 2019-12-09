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
    //The buttons that exist within the pop-up panel when the user needs to choose one to put into the current slot on the tree
    public partial class SkillSelectButton : Button
    {
        readonly ResourceManager ResourceManagerImageSkill;
        private MainForm form;
        public Skill skill;
        public TreeTableLayoutPanel treeControl;
        public SkillTooltipPanel skillTooltipPanel;

        public SkillSelectButton(MainForm inForm, SkillTooltipPanel skillTooltip)
        {
            InitializeComponent();

            //Specify defaults for this custom control

            this.form = inForm;
            this.skillTooltipPanel = skillTooltip;

            //Size
            this.Height = 30;
            this.Width = 30;

            //Image Layout
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void SkillSelectButton_Click(object sender, EventArgs e)
        {
            bool canSelectSkill = true;

            //START Debug Info
            //string debugMessage;

            //debugMessage = "Skill: " + this.skill.name + "\n" +
            //                "XPos: " + this.skill.x + "\n" +
            //                "YPos:" + this.skill.y;
            //;
            //MessageBox.Show(debugMessage);
            //END Debug Info            

            // Get the tree control that the clicked button exists in
            TreeTableLayoutPanel treeControl = (this.Parent as SkillSelectPanel).treeControl;

            // Get the current control that will be replaced with the newly-selected one
            Control btnSkill = treeControl.GetControlFromPosition(this.skill.x, this.skill.y);

            // Since the Mastery Tree has 3 shared General Rows with the same Skills, check that the chosen skill isn't already assigned in a different General Row
            if (treeControl.tree.name == "Mastery")
            {
                canSelectSkill = CanSelectSkill(treeControl, this.skill);
            }

            // See if we've discovered that we cannot select this Skill
            if (!canSelectSkill)
            {
                // Explain to the user that they cannot select this Skill because it's already been selected in a different General Row
                MessageBox.Show("Skill '" + this.skill.name + "' has already been selected.", "Skill Selection Error");
                
                // Hide the SkillSelectPanel
                this.Parent.Hide();
                
                // Get out of this method, we don't need to do anything else here
                return;
            }

            // Incase there's a skill at this position already, subtract the level of the skill from the build before we remove it
            if (canSelectSkill && btnSkill is SkillButton)
            {
                // Get the skill that was previously selected at this location
                SkillButton btnPreviousSkill = (btnSkill as SkillButton);

                // Create a new button to hold the selected Skill and change it on the tree
                (this.Parent as SkillSelectPanel).ChangeSelectedSkill(btnPreviousSkill, new SkillButton(this.skill, treeControl, this.skillTooltipPanel, form));
            }
            else if (canSelectSkill && btnSkill is MultiSkillSelectButton)
            {
                // No skill has yet been selected at this position
                MultiSkillSelectButton btnMultiSkillSelect = (btnSkill as MultiSkillSelectButton);

                // Create a new button to hold the selected Skill and change it on the tree
                (this.Parent as SkillSelectPanel).ChangeSelectedSkill(btnMultiSkillSelect, new SkillButton(this.skill, treeControl, this.skillTooltipPanel, form));
            }
            else if (!(btnSkill is SkillButton) && !(btnSkill is MultiSkillSelectButton))
            {
                // Display error for unknown control type found (ie. user clicked an unknown control type on the Tree to bring up the skill select panel and click a SkillSelectButton
                Alerts.DisplayError("SkillSelectButton_Click: Unknown control type found.  The control type of '" + btnSkill.GetType().ToString() + "' is not being accounted for or has a click event on it that needs to be removed.");
                return;
            }
        }

        private void SkillSelectButton_MouseHover(object sender, EventArgs e)  //TODOSSG duplicate code of SkillButton MouseHover/Leave events
        {
            if (!this.skillTooltipPanel.Visible)
            {
                this.skillTooltipPanel.Location = GetTooltipLocation();
                this.skillTooltipPanel.BringToFront();
                this.skillTooltipPanel.Visible = true;
            }
        }

        private void SkillSelectButton_MouseLeave(object sender, EventArgs e)
        {
            if (this.skillTooltipPanel.Visible)
            {
                this.skillTooltipPanel.Visible = false;
            }

            // Mark the control as no longer being in focus by the mouse, so it can be automatically hidden when the mouse leaves its parent panel
            (this.Parent as SkillSelectPanel).mouseFocused = false;
        }

        private bool CanSelectSkill(TreeTableLayoutPanel treeControl, Skill selectedSkill)
        {
            bool result = true;

            // Check the selected Skill has not already been selected in any of the other 3 General Rows at the same X position
            if (treeControl.Controls.OfType<SkillButton>().Where(s => s.skill.id == selectedSkill.id && s.skill.x == selectedSkill.x && s.skill.y >= 2 && s.skill.y <= 4).Count() > 0)
            {
                return false;
            }

            return result;
        }

        private Point GetTooltipLocation() //TODOSSG duplicate code of SkillButton.GetTooltipLocation
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
