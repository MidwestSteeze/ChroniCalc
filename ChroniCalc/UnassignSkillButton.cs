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
    public partial class UnassignSkillButton : Control
    {
        readonly ResourceManager ResourceManagerImageSkill;

        public UnassignSkillButton()
        {
            InitializeComponent();


            //Specify defaults for this custom control

            //Size
            this.Height = 30;
            this.Width = 30;

            //Background Image
            ResourceManagerImageSkill = new ResourceManager("ChroniCalc.ResourceImageSkill", Assembly.GetExecutingAssembly());
            this.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject("UnassignSkillButton");

            //Image Layout
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void UnassignSkillButton_Click(object sender, EventArgs e)
        {
            //Close the Parent panel
            this.Parent.Hide();
        }
    }
}
