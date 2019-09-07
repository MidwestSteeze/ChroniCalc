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
        readonly ResourceManager ResourceManagerImageSkill;
        public Skill skill;

        public SkillButton(Skill inSkill) //TODOSSG change constructor to require things so theyre not forgotten? (ie. skill and image)
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

            if (!((Image)ResourceManagerImageSkill.GetObject(skill.name) is null))
            {                
                this.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject(skill.name);
            }
            else
            {
                //Use an ImageNotFound image as a placeholder until the skill's image is created and added to the Resource Manager
                this.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject("ImageNotFound");
            }

            //Image Layout
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void SkillButton_Click(object sender, EventArgs e)
        {
            //START Debug Info
            string debugMessage;

            debugMessage = "Skill: " + this.skill.name + "\n" +
                            "XPos: " + this.skill.x + "\n" +
                            "YPos:" + this.skill.y;
            ;
            MessageBox.Show(debugMessage);
            //END Debug Info

            /* TODO
             increment/decrement the level of the skill based on left click or right click and update everyting else in the change (any dmg stuff, health, etc)
             */
        }
    }
}
