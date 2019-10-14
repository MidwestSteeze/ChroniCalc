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

        private void SkillSelectButton_Click(object sender, EventArgs e)
        {
            int left;
            int top;

            //Get the pixel location of the current MultiSkillSelect Button (ie. the "+" button) in the TableLayoutPanel
            TableLayoutPanel tlpTree = this.Parent as TableLayoutPanel;
            Control btnSkill = tlpTree.GetControlFromPosition(xPos, yPos);
            left = (xPos + 1) * Convert.ToInt32(btnSkill.Width);
            top = (yPos + 1) * Convert.ToInt32(btnSkill.Height);

            skillSelectPanel.Location = new Point(left, top);
            skillSelectPanel.BringToFront();


            //Show the SkillSelect Panel
            if (!skillSelectPanel.Visible)
            {
                skillSelectPanel.Show();
            }
        }
    }
}
