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
    //The buttons that exist within the pop-up panel when the user needs to choose one to put into the current slot on the tree
    public partial class SkillSelectButton : Button
    {
        readonly ResourceManager ResourceManagerImageSkill;
        public Skill skill;

        public SkillSelectButton() //TODOSSG change constructor to require things so theyre not forgotten? (ie. skill and image)
        {
            InitializeComponent();

            //Specify defaults for this custom control

            //Size
            this.Height = 30;
            this.Width = 30;

            //Image Layout
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void SkillSelectButton_Click(object sender, EventArgs e)
        {
            //START Debug Info
            string debugMessage;

            debugMessage = "Skill: " + this.skill.name + "\n" +
                            "XPos: " + this.skill.x + "\n" +
                            "YPos:" + this.skill.y;
            ;
            MessageBox.Show(debugMessage);
            //END Debug Info

            //Put the contents of this button into the Tree (ie. TableLayoutPanel) and make it the active skill
            //TODO

            /*
             * get the TLPTree control
             * ...TableLayoutPanel tlpTree = this.Parent as TableLayoutPanel;
            
             * remove the current control at this.skill.x and this.skill.y
             * ...Control btnSkill = tlpTree.GetControlFromPosition(xPos, yPos);
             * 
             * create a new SkillButton and populate its defaults (ie. skill, image, etc)
             * ...
             * 
             * add the SKillButton to the TLP Tree at position this.skill.x and this.skill.y
             * tlpTree.Controls.Add(skillButton, this.skill.x, this.skill.y);
             * 
             * 
             */
        }
    }
}
