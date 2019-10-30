﻿using System;
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
        public TreeTableLayoutPanel treeControl;

        public SkillSelectPanel(TreeTableLayoutPanel tlpTree)
        {
            InitializeComponent();
            WireMouseLeaveEventToAllChildren(this);

            this.treeControl = tlpTree;

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

        public void ChangeSelectedSkill(Control previousSkill, Control selectedSkill)
        {
            int previousSkillId = -1;
            int selectedSkillId = -1;

            if (previousSkill is SkillButton && selectedSkill is SkillButton)
            {
				// We are swapping a previously-selected Skill for a new Skill
                SkillButton btnPreviousSkill = (previousSkill as SkillButton);
                SkillButton btnSelectedSkill = (selectedSkill as SkillButton);

                // Only adjust the levels if the skill selected is different than the previous one
                if (btnPreviousSkill.skill.name != btnSelectedSkill.skill.name)
                {
                    // A SkillButton exists at this location and it's different than the previously-selected Skill;
					//   Subtract the level of the skill from the build before we remove it to put the new one in place
                    btnPreviousSkill.UpdateSkillPointAndLevelCounter(btnPreviousSkill.skill.level, 0);

                    previousSkillId = btnPreviousSkill.skill.id;
                    selectedSkillId = btnSelectedSkill.skill.id;

                    RemoveAddAndUpdate(btnPreviousSkill, btnSelectedSkill, previousSkillId, selectedSkillId);
                }
                else
                {
                    // The same skill was chosen, so the PreviousSkill will remain; BUT if we don't remove the SelectedSkill, it will remain on the TreeTableLayoutPanel
                    //   (this is because when the selected SkillButton was created, its Parent is automatically assigned as the TreeTableLayoutPanel which puts it in the first available cell)
                    this.treeControl.Controls.Remove(selectedSkill);
                }
            }
            else if (previousSkill is SkillButton && selectedSkill is MultiSkillSelectButton)
            {
				// We are unassigning the previously-selected Skill
                SkillButton btnPreviousSkill = (previousSkill as SkillButton);
                btnPreviousSkill.UpdateSkillPointAndLevelCounter(btnPreviousSkill.skill.level, 0);

                previousSkillId = btnPreviousSkill.skill.id;
                RemoveAddAndUpdate(previousSkill, selectedSkill, previousSkillId, selectedSkillId);
            }
            else if (previousSkill is MultiSkillSelectButton && selectedSkill is SkillButton)
            {
				// We are assigning a new Skill in a location that doesn't yet have a Skill
                SkillButton btnSelectedSkill = (selectedSkill as SkillButton);

                selectedSkillId = btnSelectedSkill.skill.id;
                RemoveAddAndUpdate(previousSkill, selectedSkill, previousSkillId, selectedSkillId);
            }
            else
            {
                // TODO They're both MultiSkillSelect Buttons or unknown control types, this shouldn't ever happen but throw an exception incase a 3rd control type is added to the SkillSelectPanel and not handled
            }

            // Hide the SkillSelectPanel now that the user chose a skill
			//   (not deleting it so it can be re-used incase the user wants to change this MultiSkill at a later time)
            this.Hide();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void RemoveAddAndUpdate(Control previousSkill, Control selectedSkill, int oldSkillId, int newSkillId)
        {
            int skillIdForClearingPostReqs;

            // Remove the existing control at the currently-selected position
            //   NOTE: Removing a control moves all controls after it "up" 1 cell by index, but adding a control in its place immediately after will put all skills back in their place
            this.treeControl.Controls.Remove(previousSkill);

            // Add the selected button to the Tree at the currently-selected position and
            //   move the SkillSelectPanel over to the newly-selected button so it can be re-displayed if wanted
            switch (selectedSkill)
            {
                case MultiSkillSelectButton m:
                    this.treeControl.Controls.Add(m, m.xPos, m.yPos);
                    m.skillSelectPanel = this;
                    break;
                case SkillButton s:
                    this.treeControl.Controls.Add(s, s.skill.x, s.skill.y);
                    s.skillSelectPanel = this;
                    break;
                default:
                    //TODO throw exception that control type not specified
                    break;
            }

            // Determine which skill id to use for clearing any skills linked to the one that is being swapped out
            if (oldSkillId != -1 && newSkillId != -1) //TODO surely there's a better way to do this...
            {
                // The user is swapping from one Skill to another Skill
                skillIdForClearingPostReqs = oldSkillId;
            }
            else if (oldSkillId != -1 && newSkillId == -1)
            {
                // The user is removing a Skill to be reset to the MultiSkillSelect button
                skillIdForClearingPostReqs = oldSkillId;
            }
            else if (oldSkillId == -1 && newSkillId != -1)
            {
                // The user is selecting a skill from a MultiSkillSelect button/panel
                skillIdForClearingPostReqs = newSkillId;
            }
            else
            {
                // The user didn't select a skill from the SkillSelect panel (ie. they picked Unassign Skill)
                skillIdForClearingPostReqs = -1;
            }

            // Update any/all necessary data of all linked (read: postreq) Skills (e.g. level, description)
            if ((skillIdForClearingPostReqs != -1) &&
                (!(this.treeControl.tree.skills.Where(x => x.skill_requirement.Contains(skillIdForClearingPostReqs)) is null)))
            {
                UpdatePostReqSkills(skillIdForClearingPostReqs);
            }
        }

        private void SkillSelectPanel_Click(object sender, EventArgs e)
        {
            //It's here if you need it for something
        }

        private void UpdatePostReqSkills(int skillId)
        {
            IEnumerable<Skill> postReqSkills;
            SkillButton postReqSkillButton;

            // Find the Skills that have the skillId as its prereq
            postReqSkills = treeControl.tree.skills.Where(x => x.skill_requirement.Contains(skillId));

            foreach (Skill skill in postReqSkills)
            {
                // Get the SkillButton for the Skill so we can access and update its tooltip description
                postReqSkillButton = (SkillButton)treeControl.Controls.Find(skill.id.ToString(), false).First();

                // Reset the Skill's description and level
                postReqSkillButton.UpdateSkillPointAndLevelCounter(postReqSkillButton.skill.level, 0);

                // Call recursively incase the current postReq skill has a postReq skill of its own that needs to be updated
                UpdatePostReqSkills(postReqSkillButton.skill.id);
            }
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
