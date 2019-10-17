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
    //The "+" button that appears on the tree which the user needs to click to open a pop-up in order to select from multiple skill options
    public partial class MultiSkillSelectButton : UserControl
    {
        readonly ResourceManager ResourceManagerImageTree;
        public SkillSelectPanel skillSelectPanel;
        int xPos;
        int yPos;

        public MultiSkillSelectButton(int x, int y)
        {
            InitializeComponent();

            //Specify defaults for this custom control

            //Location in TableLayoutPanel
            this.xPos = x;
            this.yPos = y;

            //Anchor location within its parent control
            this.pnlMultiSkillSelectButton.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);
            this.pnlMultiSkillSelectButton.Dock = DockStyle.Fill;

            //Background Image
            ResourceManagerImageTree = new ResourceManager("ChroniCalc.ResourceImageTree", Assembly.GetExecutingAssembly());
            pbMultiSkillSelectIcon.BackgroundImage = (Image)ResourceManagerImageTree.GetObject("spr_empty_skill_slot_0");

            //Image Layout
            pbMultiSkillSelectIcon.BackgroundImageLayout = ImageLayout.Stretch;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void MultiSkillSelectButton_Click(object sender, EventArgs e)
        {
            //Set the pixel location of the Skill Select Panel to popup for the user
            skillSelectPanel.Location = GetSkillSelectLocation();
            skillSelectPanel.BringToFront();

            //Show the Skill Select Panel
            if (!skillSelectPanel.Visible)
            {
                skillSelectPanel.Show();
            }
        }

        private Point GetSkillSelectLocation()
        {
            int availableWidth;
            int left;
            int top;
            Point location;

            //Get the position of the cursor relative to the Application
            TableLayoutPanel tlpTree = this.Parent as TableLayoutPanel;
            Control btnSkill = tlpTree.GetControlFromPosition(xPos, yPos);

            //Position the Skill Select Panel centered in relation to the MultiSkillSelect (+) button
            left = (xPos + 1) * Convert.ToInt32(btnSkill.Width) - (Convert.ToInt32(this.skillSelectPanel.Width) / 2);

            //Position the Skill Select Panel above the MultiSkillSelect (+) button
            top = (yPos + 1) * Convert.ToInt32(btnSkill.Height) - Convert.ToInt32(this.skillSelectPanel.Height);

            //Location to display the Skill Select Panel based on where the clicked MultiSkillSelect (+) button is
            location = new Point(left, top); 

            //Check if the Skill Select Panel is going off one of the edges of the screen and adjust as necessary
            // Width of the panel the Skill Select Panel is contained within
            availableWidth = this.Parent.Parent.Width;

            if (location.X + this.skillSelectPanel.Width > availableWidth)
            {
                //The Skill Select Panel went off the right edge of the available area, so position it left of the MultiSkillSelect (+) button that was clicked, instead of the right
                location.X = availableWidth - this.skillSelectPanel.Width;
            }

            if (location.Y < Convert.ToInt32(this.skillSelectPanel.Height))
            {
                //The Skill Select Panel went off the top edge of the available area, so position it below the MultiSkillSelect (+) button that was clicked, instead of above
                location.Y = Convert.ToInt32(this.skillSelectPanel.Height * 2);
            }

            return location;
        }
    }
}
