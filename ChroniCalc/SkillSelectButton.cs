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
        private MainForm form;
        public Skill skill;
        public TreeTableLayoutPanel treeControl;

        public SkillSelectButton(MainForm inForm)
        {
            InitializeComponent();

            //Specify defaults for this custom control

            this.form = inForm;

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

            //Create a new button to hold the selected Skill
            SkillTooltipPanel pnlSkillTooltip = form.CreateSkillTooltip(this.skill);
            SkillButton btnSkill = new SkillButton(this.skill, this.treeControl, pnlSkillTooltip, form);

            //Remove the current control at the currently-selected position
            // NOTE: Removing a control moves all controls after it "up" 1 cell by index, but adding a control in its place immediately after will put all skills back in their place
            MultiSkillSelectButton btnMultiSkillSelect = (MultiSkillSelectButton)this.treeControl.GetControlFromPosition(this.skill.x, this.skill.y);
            this.treeControl.Controls.Remove(btnMultiSkillSelect);

            //Add the skill button to the Tree at the currently-selected position
            this.treeControl.Controls.Add(btnSkill, this.skill.x, this.skill.y);

            //Hide the SkillSelectPanel now that the user chose a skill //TODOSSG delete it to reduce instantiated controls in memory?
            this.Parent.Hide();

        }
    }
}
