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
    //A custom button that has the ability to hold a Skill object
    public partial class SkillButton : Button
    {
        private int level;
        readonly ResourceManager ResourceManagerImageSkill;
        public Skill skill;

        public SkillButton(Skill inSkill)
        {
            InitializeComponent();

            //Set the Skill
            skill = inSkill;

            //Specify defaults for this custom control

            //Size
            this.Height = 30;
            this.Width = 30;

            //Background Image
            ResourceManagerImageSkill = new ResourceManager("ChroniCalc.ResourceImageSkill", Assembly.GetExecutingAssembly());

            if (!((Image)ResourceManagerImageSkill.GetObject(skill.id.ToString()) is null))
            {                
                this.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject(skill.id.ToString());
            }
            else
            {
                //Use an ImageNotFound image as a placeholder until the skill's image is created and added to the Resource Manager
                this.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject("ImageNotFound");
            }

            //Image Layout
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //Font
            this.Font = new System.Drawing.Font("TechnicBold", 12F, FontStyle.Bold);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void SkillButton_MouseUp(object sender, MouseEventArgs e)
        {
            //START Debug Info
            string debugMessage;

            debugMessage = "Skill: " + this.skill.name + "\n" +
                            "XPos: " + this.skill.x + "\n" +
                            "YPos:" + this.skill.y;
            ;
            MessageBox.Show(debugMessage);
            //END Debug Info

            //Adjust the level of the skill, ensuring we're not beyond the min/max allowed
            if (e.Button == MouseButtons.Left && this.level < this.skill.max_rank)
            {
                this.level++;
            }
            else if (e.Button == MouseButtons.Right && this.level > 0)
            {
                this.level--;
            }

            //Update the displayed level on the skill button
            this.Text = this.level.ToString();

            //TODO update stats (damage, health, mana, etc)
        }
    }
}
