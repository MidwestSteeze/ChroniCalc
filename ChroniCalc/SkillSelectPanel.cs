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
        public bool mouseFocused = false;

        public SkillSelectPanel()
        {
            InitializeComponent();

            WireMouseLeaveEventToAllChildren(this);

            //Specify defaults for this custom control

            //Size
            this.Height = 36;
            //this.Width = AssignedAtRuntime;

            //Background Image
            ResourceManagerImageUI = new ResourceManager("ChroniCalc.ResourceImageUI", Assembly.GetExecutingAssembly());
            this.BackgroundImage = (Image)ResourceManagerImageUI.GetObject("spr_menu_button_thin_0");

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

        private void WireMouseLeaveEventToAllChildren(Control container)
        {
			// This is required in order to track when the mouse has left all Multi SkillSelect buttons and their container so we can auto-hide it
            foreach (Control c in container.Controls)
            {
                c.MouseHover += (s, e) => OnMouseLeave(e);

                WireMouseLeaveEventToAllChildren(c);
            };
        }

        private void SkillSelectPanel_MouseLeave(object sender, EventArgs e)
        {
            if (!this.mouseFocused)
            {
                this.Hide();
            }
        }

        private void SkillSelectPanel_VisibleChanged(object sender, EventArgs e)
        {
			// Flag the control as having mouse focus when it's displayed
            if (this.Visible)
            {
                this.mouseFocused = true;
            }
        }
    }
}
