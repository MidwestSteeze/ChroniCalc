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
            //START Debug Info
            //string debugMessage;

            //debugMessage = "Skill: " + this.skill.name + "\n" +
            //                "XPos: " + this.skill.x + "\n" +
            //                "YPos:" + this.skill.y;
            //;
            //MessageBox.Show(debugMessage);
            //END Debug Info            

            //Create a new button to hold the selected Skill
            SkillButton btnSkill = new SkillButton(this.skill, this.treeControl, this.skillTooltipPanel, form);

            //Remove the current control at the currently-selected position
            // NOTE: Removing a control moves all controls after it "up" 1 cell by index, but adding a control in its place immediately after will put all skills back in their place
            Control btnMultiSkillSelect = this.treeControl.GetControlFromPosition(this.skill.x, this.skill.y);
            this.treeControl.Controls.Remove(btnMultiSkillSelect);

            //Add the skill button to the Tree at the currently-selected position
            this.treeControl.Controls.Add(btnSkill, this.skill.x, this.skill.y);

            //Hide the SkillSelectPanel now that the user chose a skill
            // (Not deleting it so the user can switch it out if they change their mind)
            this.Parent.Hide();

 			//Move the SkillSelectPanel over to the newly-selected SkillButton so it can be re-displayed if wanted
            btnSkill.skillSelectPanel = this.Parent as SkillSelectPanel;
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
