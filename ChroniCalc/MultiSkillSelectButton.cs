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
    public partial class MultiSkillSelectButton : Control
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

            //Size
            this.Height = 30;
            this.Width = 30;

            //Background Image
            ResourceManagerImageTree = new ResourceManager("ChroniCalc.ResourceImageTree", Assembly.GetExecutingAssembly());
            this.BackgroundImage = (Image)ResourceManagerImageTree.GetObject("MultiSkillSelectButton");

            //Image Layout
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //Anchor location within its parent control
            //this.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            this.Anchor = AnchorStyles.None;
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
