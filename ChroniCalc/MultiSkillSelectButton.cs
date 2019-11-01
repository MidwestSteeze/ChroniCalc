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
        public int xPos;
        public int yPos;

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
            //Show the Skill Select Panel
            if (!skillSelectPanel.Visible)
            {
                skillSelectPanel.Show();
                skillSelectPanel.BringToFront();
            }
            else
            {
                // This is a safety incase the SkillSelectPanel was left visible but then a different Tree control was displayed on top of it; we need to ensure it's always in front when it's visible, so it can be seen
                skillSelectPanel.BringToFront();
            }
        }
    }
}
