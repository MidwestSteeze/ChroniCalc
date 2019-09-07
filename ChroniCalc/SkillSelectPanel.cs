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
    public partial class SkillSelectPanel : Control
    {
        readonly ResourceManager ResourceManagerImageUI;

        public SkillSelectPanel()
        {
            InitializeComponent();

            //Specify defaults for this custom control

            //Size
            this.Height = 36;
            //this.Width = AssignedAtRuntime;

            //Background Image
            ResourceManagerImageUI = new ResourceManager("ChroniCalc.ResourceImageUI", Assembly.GetExecutingAssembly());
            this.BackgroundImage = (Image)ResourceManagerImageUI.GetObject("SkillSelectPanel");

            //Image Layout
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //Visbility
            this.Visible = false;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void SkillSelectPanel_Click(object sender, EventArgs e)
        {
            //It's here if you need it for something
        }
    }
}
