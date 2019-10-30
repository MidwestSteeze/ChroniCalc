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
    public partial class UnassignSkillButton : Button
    {
        readonly ResourceManager ResourceManagerImageSkill;
        private int tlpXPos;
        private int tlpYPos;

        public UnassignSkillButton(int skillSelectButtonCount, int buttonMargin, int xPos, int yPos)
        {
            InitializeComponent();

            this.tlpXPos = xPos;
            this.tlpYPos = yPos;

            //Specify defaults for this custom control

            //Size
            this.Height = 30;
            this.Width = 30;

            //Location - use the parameters to place it at the end of the other SkillSelect buttons in the panel
            this.Location = new Point(skillSelectButtonCount * this.Width + buttonMargin, 3);

            //Background Image
            ResourceManagerImageSkill = new ResourceManager("ChroniCalc.ResourceImageSkill", Assembly.GetExecutingAssembly());
            this.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject("spr_menu_icon_11");

            //Image Layout
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        // Replace the clicked location with a MultiSkillSelect button and reset all data of both the Skill being removed and linked skills (e.g. description, level, etc.)
        private void UnassignSkillButton_Click(object sender, EventArgs e)
        {
			// Get the tree control that the clicked button exists in
            TreeTableLayoutPanel treeControl = (this.Parent as SkillSelectPanel).treeControl;

            // Get the current control that will be unassigned
            Control btnSkill = treeControl.GetControlFromPosition(this.tlpXPos, this.tlpYPos);

            //Incase there's a skill at this position already, subtract the level of the skill from the build before we remove it
            if (btnSkill is SkillButton)
            {
                //Create a new MultiSkillSelect button to hold the selectable skills and change it on the tree
                (this.Parent as SkillSelectPanel).ChangeSelectedSkill(btnSkill, new MultiSkillSelectButton(this.tlpXPos, this.tlpYPos));
            }
            else
            {
                // No selected Skill exists at this location, so do nothing but
                //   hide the SkillSelectPanel since the user chose not to select a skill
                this.Parent.Hide();
            }
        }

        private void UnassignSkillButton_MouseLeave(object sender, EventArgs e)
        {
            // Mark the control as no longer being in focus by the mouse, so it can be automatically hidden when the mouse leaves its parent panel
            (this.Parent as SkillSelectPanel).mouseFocused = false;
        }
    }
}
