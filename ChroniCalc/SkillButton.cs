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

            //Set the .Name property based on the Skill's ID
            this.Name = skill.id.ToString();

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

            //Anchor location within its parent control
            //this.Anchor = (AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top);
            this.Anchor = AnchorStyles.None;

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
                            "YPos:" + this.skill.y + "\n" + 
                            "Width:" + this.Width + "\n" +
                            "height:" + this.Height;
            ;
            MessageBox.Show(debugMessage);
            //END Debug Info

            //Adjust the level of the skill, ensuring we're not beyond the min/max allowed
            if (e.Button == MouseButtons.Left && this.level < this.skill.max_rank && HavePrereqs())
            {
                this.level++;
            }
            else if (e.Button == MouseButtons.Right && this.level > 0)
            {
                //TODO don't let user de-level if it's going to set level to 0 and this skill is a needed prereq
                this.level--;
            }

            //Update the displayed level on the skill button
            if (this.level == 0)
            {
                this.Text = "";
            }
            else
            {
                this.Text = this.level.ToString();
            }

            //TODO update stats (damage, health, mana, etc)
        }

        private bool HavePrereqs()
        {
            bool result = true;
            SkillButton preReqSkillButton;

            //Get the Tree this Skill belong to
            TreeTableLayoutPanel ttlp = (TreeTableLayoutPanel)this.Parent;

            //Compile list of prereq skill ids from the current skill's skill_requirement property
            int[] prereqs = this.skill.skill_requirement;

            //Analyze each prereq skill id to see if it's leveled
            foreach (int prereqSkillId in prereqs)
            {
                //Get the Skill button from the Tree by ID
                preReqSkillButton = (SkillButton)ttlp.Controls.Find(prereqSkillId.ToString(), false).First();

                //Verify the prereq skill is leveled
                if (preReqSkillButton.level < 1)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
