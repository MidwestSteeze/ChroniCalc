using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ChroniCalc
{
    public partial class MainForm : Form
    {
        //Skill Tree
        const int SKILL_BUTTON_PADDING = 6;

        //Resource Managers for pulling assets (ie. data, images, etc.) which is a reflection of the Assets directory
        ResourceManager ResourceManagerImageClass;
        ResourceManager ResourceManagerImageSkill;
        ResourceManager ResourceManagerImageTree;

        //List Objects
        List<CharacterClass> characterClasses;
        List<Tree> trees;
        List<Skill> skills;

        List<Button> treeButtons;
        List<TreeTableLayoutPanel> treePanels;

        public MainForm()
        {
            InitializeComponent();

            //Init resource managers for pulling assets (ie. data, images, etc.)
            ResourceManagerImageClass = new ResourceManager("ChroniCalc.ResourceImageClass", Assembly.GetExecutingAssembly());
            ResourceManagerImageSkill = new ResourceManager("ChroniCalc.ResourceImageSkill", Assembly.GetExecutingAssembly());
            ResourceManagerImageTree = new ResourceManager("ChroniCalc.ResourceImageTree", Assembly.GetExecutingAssembly());

            //Init global variables
            characterClasses = new List<CharacterClass>();

            //Add all tree buttons to a list for looping over and finding within later as needed
            treeButtons = new List<Button>();
            treeButtons.Add(btnTree1);
            treeButtons.Add(btnTree2);
            treeButtons.Add(btnTree3);
            treeButtons.Add(btnTree4);

            treePanels = new List<TreeTableLayoutPanel>();
            //Add 4 trees to the list of available Trees for a Class
            for (int i = 0; i <= 3; i++)
            {
                TreeTableLayoutPanel ttlp = new TreeTableLayoutPanel(pnlTrees);
                treePanels.Add(ttlp);
            }

            PopulateSkillTrees();
        }

        public void PopulateSkillTrees()
        {
            //Set the filepaths of the json to local variables for referencing easier
            string jsonSample = (ConfigurationManager.AppSettings["SkillDataSample"]);
            string jsonBerserker = (ConfigurationManager.AppSettings["SkillDataBerserker"]);
            string jsonAll = (ConfigurationManager.AppSettings["SkillDataAll"]);
            string jsonAsXml = (ConfigurationManager.AppSettings["SkillDataAsXml"]);

            Tree tree;
            Skill skill;

            //SAMPLE - DELETE BEFORE COMMIT - Working example
            //using (StreamReader streamReader = new StreamReader(jsonSample))
            //{
            //    string strSkillData = streamReader.ReadToEnd();
            //    SampleRoot items = JsonConvert.DeserializeObject<SampleRoot>(strSkillData);

            //    JObject jFoo = JObject.Parse(strSkillData);
            //    foreach (JObject jClass in jFoo.Properties().Select(p => p.Value))
            //    {
            //        Console.WriteLine("Class Name: " + jClass["name"]);
            //        foreach (JObject jTree in jClass.Descendants())
            //        {
            //            Console.WriteLine("Tree Name: " + jTree["name"]);
            //            foreach (JObject jSkill in jTree.Descendants())
            //            {
            //                Console.WriteLine("Skill Name: " + jSkill["name"]);
            //            }
            //        }
            //    }
            //}
            // END SAMPLE

            //Load the skill data in as XML format because the Json isn't an ideal format for 
            // iterating over and squarebit is a busy bro and doesn't need to be bothered to change it
            XmlDocument skillData = new XmlDocument();
            skillData.Load(jsonAsXml);

            //Loop through each Class under the root (ie. Berserker, Templar, Warlock, etc)
            foreach (XmlNode classNode in skillData.SelectSingleNode("root"))
            {
                //Create a new Trees list to hold the trees for the current Class
                trees = new List<Tree>();

                //Loop through each Tree within the Class (ie. Dragonkin, Sky Lord, etc)
                foreach (XmlNode treeNode in classNode)
                {
                    //Create a new Tree and set its Name
                    tree = new Tree
                    {
                        name = treeNode.Name
                    };

                    //Create a new Skills list to hold the skills for the current Tree
                    skills = new List<Skill>();

                    //Loop through all available Skills within the Tree (ie. Dragon Punch, Fart Stomp, etc)
                    foreach (XmlNode skillNode in treeNode)
                    {
                        //Create a new Skill
                        skill = new Skill();

                        //TODOSSG document manual changes made to skilldata.json when converted into XML as these will need to be applied for each new skilldata.json release
                        //TODOSSG convert the rest (all?) to utilize NodeHasValue with additional conditions as necessary
                        //Map all Skill values into the Object's properties, handling situations of multiple Ranks and when the Node doesn't exist
                        // (using NodeHasValue to handle situations where the node exists but has no value ie. <duration></duration> and if there are additinoal values I need to exclude such as "none")
                        skill.effect = NodeHasValue(skillNode.SelectSingleNode("effect")) ? skillNode.SelectSingleNode("effect").InnerXml.Split(',') : new string[] { }; //TODOSSG convert this to hold just the value and remove the "%"?
                        skill.cooldown = !(skillNode.SelectSingleNode("cooldown") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("cooldown").InnerXml) : -1;
                        skill.duration = NodeHasValue(skillNode.SelectSingleNode("duration")) ? Array.ConvertAll(skillNode.SelectSingleNode("duration").InnerXml.Split(','), double.Parse) : new double[] { };
                        skill.cost100 = !(skillNode.SelectSingleNode("cost100") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("cost100").InnerXml) : -1;
                        skill.skill_requirement = NodeHasValue(skillNode.SelectSingleNode("skill_requirement"), new string[] { "none"}) ? Array.ConvertAll(skillNode.SelectSingleNode("skill_requirement").InnerXml.Trim('[', ']').Split(','), int.Parse) : new int[] { };
                        skill.x = !(skillNode.SelectSingleNode("x") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("x").InnerXml) : -1;
                        skill.damage = !(skillNode.SelectSingleNode("damage") is null) ? skillNode.SelectSingleNode("damage").InnerXml.Split('.') : new string[] { }; //TODOSSG convert this to hold just the value and remove the "%"?
                        skill.range2 = NodeHasValue(skillNode.SelectSingleNode("range2")) ? Array.ConvertAll(skillNode.SelectSingleNode("range2").InnerXml.Split(','), double.Parse) : new double[] { };
                        skill.family = !(skillNode.SelectSingleNode("family") is null) ? skillNode.SelectSingleNode("family").InnerXml : "None";
                        skill.min_level = !(skillNode.SelectSingleNode("min_level") is null) ? skillNode.SelectSingleNode("min_level").InnerXml : "N/A";
                        skill.id = !(skillNode.SelectSingleNode("id") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("id").InnerXml) : -1;
                        skill.range = NodeHasValue(skillNode.SelectSingleNode("range")) ? Array.ConvertAll(skillNode.SelectSingleNode("range").InnerXml.Split(','), double.Parse) : new double[] { };
                        skill.element = !(skillNode.SelectSingleNode("element") is null) ? skillNode.SelectSingleNode("element").InnerXml : "N/A";
                        skill.value = NodeHasValue(skillNode.SelectSingleNode("value")) ? Array.ConvertAll(skillNode.SelectSingleNode("value").InnerXml.Split(','), double.Parse) : new double[] { };
                        skill.proc = NodeHasValue(skillNode.SelectSingleNode("proc")) ? Array.ConvertAll(skillNode.SelectSingleNode("proc").InnerXml.Split(','), int.Parse) : new int[] { };
                        skill.type = !(skillNode.SelectSingleNode("type") is null) ? skillNode.SelectSingleNode("type").InnerXml : "N/A";
                        skill.description_next = !(skillNode.SelectSingleNode("description_next") is null) ? skillNode.SelectSingleNode("description_next").InnerXml : "";
                        skill.description = !(skillNode.SelectSingleNode("description") is null) ? skillNode.SelectSingleNode("description").InnerXml : "";
                        skill.max_rank = !(skillNode.SelectSingleNode("max_rank") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("max_rank").InnerXml) : -1;
                        skill.y = !(skillNode.SelectSingleNode("y") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("y").InnerXml) : -1;
                        skill.cost1 = !(skillNode.SelectSingleNode("cost1") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("cost1").InnerXml) : -1;
                        skill.name = !(skillNode.SelectSingleNode("name") is null) ? skillNode.SelectSingleNode("name").InnerXml : "ERROR: Missing Name";

                        skills.Add(skill);
                    }

                    //Add all available Skills to the Tree
                    tree.skills = skills;

                    //Add the Tree to the List of Trees
                    trees.Add(tree);
                }

                //Add the populated Class to the list of all available Classes
                characterClasses.Add(new CharacterClass(classNode.Name, trees));
            }
        }

        private bool NodeHasValue(XmlNode node, params string[] additionalConditions)
        {
            bool hasValue = true;

            if ((!(node is null)) &&
                (node.InnerXml != ""))
            {
                if (additionalConditions.Length > 0)
                {
                    foreach (string condition in additionalConditions)
                    {
                        if (node.InnerXml == condition)
                        {
                            hasValue = false;
                            break;
                        }
                    }
                }
                else
                {
                    hasValue = false;
                }
            }
            else
            {
                hasValue = false;
            }

            return hasValue;
        }
        
        private void ShowTree(object sender, EventArgs e)
        {
            Button btnTree = (sender as Button);
            //Determine which tree the user selected to be shown
            string treeName = btnTree.Tag.ToString();
            lblTree.Text = treeName;

            //Change the background image of the TreeTableLayoutPanel that will be shown for the selected Tree
            //TODO

            //Show the corresponding tlpTree by finding a match and bringing it in front of the others
            foreach (TreeTableLayoutPanel ttlp in treePanels)
            {
                if (ttlp.name == btnTree.Tag.ToString())
                {
                    ttlp.Show();
                    ttlp.BringToFront();
                    break;
                }
            }

            //Highlight the selected Tree button so it stands out from the rest like it's the active one
            foreach (Button btn in treeButtons)
            {
                if (btn == btnTree)
                {
                    //Highlight the button
                    btn.FlatAppearance.BorderColor = Color.White;
                }
                else
                {
                    //Unhighlight the non-selected buttons
                    btn.FlatAppearance.BorderColor = Color.Black;
                }
            }
        }

        private void CboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            string characterClass = (sender as ComboBox).SelectedItem.ToString();
            CharacterClass selectedClass;

            //Prompt user ensuring they want to reset their character
            DialogResult dialogResult = MessageBox.Show("Changing Class will reset this character.  Continue?", "Change Class", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                //Get the selected class
                selectedClass = characterClasses.Find(x => x.name == characterClass);

                if (selectedClass is null)
                {
                    //TODOSSG throw error that the selected class was not found in the current list of character classes
                }

                //Change the Class image
                switch (selectedClass.name)
                {
                    case "Berserker":
                        pbClass.Image = (Image)ResourceManagerImageClass.GetObject("Berserker");
                        break;
                    case "Templar":
                        pbClass.Image = (Image)ResourceManagerImageClass.GetObject("Templar");
                        break;
                    case "Warden":
                        pbClass.Image = (Image)ResourceManagerImageClass.GetObject("Warden");
                        break;
                    case "Warlock":
                        pbClass.Image = (Image)ResourceManagerImageClass.GetObject("Warlock");
                        break;
                    default:
                        //TODOSSG what's the best way to handle exceptions? error message to user? debug log? email call stack?
                        throw new Exception("ChangeClass: characterClass of " + selectedClass.name + " was not added to Switch for setting the Class iamge.");
                }

                //Reset all things involved with the current character (ie. stats, trees, gear)        
                ResetCharacter(selectedClass);

                //Default focus to the first Tree
                ShowTree(treeButtons.First(), new EventArgs());
            }
        }

        private void ResetCharacter(CharacterClass selectedClass)
        {
            //TODOSSG Set all stats back to their defaults
            ResetStats();

            //TODOSSG Remove all gear
            ResetGear();

            //Change which trees are available
            ResetTrees(selectedClass);
        }

        private void ResetStats()
        {
            //TODOSSG
        }

        private void ResetGear()
        {
            //TODOSSG
        }

        private void ResetTrees(CharacterClass selectedClass)  //TODOSSG correct the order of the Trees for all classes (Berserker fixed)
        {
            string treeName;

            //Clear all content from the current trees
            ClearTrees();

            //Depending on Class, load up Image and Tag on Tree tabs and update the Mastery tree
            switch (selectedClass.name)
            {
                case "Berserker":
                    treeName = "Guardian";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].name = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "SkyLord";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].name = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Dragonkin";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].name = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Frostborn";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].name = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                case "Templar":
                    treeName = "Redemption";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].name = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Conviction";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].name = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Wrath";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].name = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Vengeance";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].name = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                case "Warden":
                    treeName = "Druid";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].name = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "WinterHerald";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].name = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "StormCaller";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].name = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "WindRanger";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].name = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                case "Warlock":
                    treeName = "Reaper";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].name = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Corruptor";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].name = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Lich";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].name = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Demonologist";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].name = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                default:
                    //TODOSSG what's the best way to handle exceptions? error message to user? debug log? email call stack?
                    throw new Exception("ResetTrees(): characterClass of " + selectedClass.name + " not found.");
                    //break;
            }

            //Update each Tree thumbnail
            foreach (Button treeButton in treeButtons)
            {
                //Look at the the button's Tag to determine which image to load into it
                LoadTreeIconButtonImage(ResourceManagerImageTree, treeButton, treeButton.Tag.ToString());
            }

            //Load all trees for the selected class into their corresponding tree table layout panels
            LoadTrees(selectedClass);

            //Set the Mastery tree button's Tag to the currently-selected Class
            btnTreeMastery.Tag = selectedClass.name;

            //TODOSSG
            //Update the rows in the Mastery tree to be specific to the newly-selected Class
            ResetMasteryTree(selectedClass.name);
        }

        private void ClearTrees()
        {
            //Clear all content from the existing trees (ie. treePanels)
            foreach (TreeTableLayoutPanel tree in treePanels)
            {
                tree.Controls.Clear();
            }

            //Clear the images on the tree selection buttons
            foreach (Button treeButton in treeButtons)
            {
                treeButton.BackgroundImage = null;
                treeButton.Tag = "";
            }
        }

        private void LoadTreeIconButtonImage(ResourceManager resourceManager, Button button, string name)
        {
            //Load the image into the specificed control but fall-back to an ImageNotFound image incase one hasn't been added to the Resource Manager yet
            if (!((Image)resourceManager.GetObject("Icon" + name) is null))
            {
                button.BackgroundImage = (Image)resourceManager.GetObject("Icon" + name);
            }
            else
            {
                //Use an ImageNotFound image as a placeholder until the image is created and added to the Resource Manager
                button.BackgroundImage = (Image)resourceManager.GetObject("ImageNotFound");
            }
        }

        private void ResetMasteryTree(string characterClass)
        {
            //TODOSSG
        }

        private void LoadTrees(CharacterClass selectedClass)
        {
            Tree tree;

            //Load each of the 4 Tree panels with the data for the selected class
            foreach (TreeTableLayoutPanel tlpTree in treePanels)
            {
                //Pull the correct Tree object for the current TreeTableLayoutPanel from the currently selected Class
                tree = selectedClass.trees.Find(x => x.name == tlpTree.name);

                if (!(tree is null))
                {
                    //Load every aspect of the current Tree into the current Tree Panel
                    LoadTree(tree, tlpTree);
                }
                else
                {
                    //TODOSSG throw error that tlpTree.name is not found in the currently-loaded Trees object for the selected Class
                }
            }
        }

        private void LoadTree(Tree tree, TreeTableLayoutPanel tlpTree)
        {
            List<Skill> MultiSelectionSkills = new List<Skill>();

            //Load any skill slot that beings with a "+" for the user to pick between multiple skills
            LoadMultiSelectionSkills(tree, tlpTree, ref MultiSelectionSkills);

            //Loop through each Skill and place it into the correct slot in the TableLayoutPanel
            foreach (Skill skill in tree.skills)
            {
                //Load all remaining skill slots, ensuring it's not a Mutli-skill slot that was already loaded
                if (!(IsMultiSelectionSkill(skill, MultiSelectionSkills)))
                {
                    //Create a new control to hold this skill at the skills X and Y location
                    SkillButton btnSkill = new SkillButton(skill);

                    //Disable button if it's the Class skill counter (the one that provides the passive damage bonus based on # of points spent)
                    if (skill.name == tree.name)
                    {
                        btnSkill.Enabled = false;
                    }

                    //Add the skill button to the tree
                    tlpTree.Controls.Add(btnSkill, skill.x, skill.y);
                }
            }
        }

        //Look at all skills within the current Tree to see if there are multiples that share the same position (ie. Dive, Jump, Flame Dash)
        // where only 1 should be selected by the user
        private void LoadMultiSelectionSkills(Tree tree, TreeTableLayoutPanel tlpTree, ref List<Skill> MultiSelectionSkills)
        {
            int xPos;
            int yPos;
            SkillSelectButton btnSkillSelect;
            List<Skill> MultipleSkills = new List<Skill>();

            //Loop through each skill in the tree to analyze its X and Y properties and see if more than 1 skill exists at this position
            foreach (Skill skill in tree.skills)
            {
                //Get the current position of this skill on the tree
                xPos = skill.x;
                yPos = skill.y;

                //Check you haven't already captured skills for this position
                if (MultiSelectionSkills.FindAll(ms => ms.x == xPos && ms.y == yPos).Count > 1)
                {
                    //This skill position has already been captured into the MultiSelectionSkills list so move onto the next skill
                    continue;
                }

                //Capture all possible skills at the current position
                MultipleSkills.Clear();
                MultipleSkills = tree.skills.FindAll(s => s.x == xPos && s.y == yPos);

                //Check if more than 1 skill does infact exist at the current position
                if (MultipleSkills.Count > 1)
                {
                    //Add a SkillSelect Button ("+" button) to the shared X,Y position on the tree
                    MultiSkillSelectButton btnMultiSkillSelect = new MultiSkillSelectButton(xPos, yPos);
                    tlpTree.Controls.Add(btnMultiSkillSelect, xPos, yPos);
                    //TODOSSG at this same position, set backgroundimage of the cell to be the texture so you don't have to photoshop out the skill counter in Chronicon controls

                    //Instantiate a new SkillSelect Panel to hold the multiple skills
                    SkillSelectPanel pnlSkillSelect = new SkillSelectPanel();

                    //Set its Parent control to actually be the Panel that holds all Trees and not the TableLayoutPanel for the current Tree 
                    //  so it can display OVER the SkillSelectButton (without setting this, it's trying to display in the same cell in the tlpTree)
                    pnlSkillSelect.Parent = pnlTrees;

                    //Tie the panel to the SkillSelect Button that holds the position for which this panel and its skills apply to
                    btnMultiSkillSelect.skillSelectPanel = pnlSkillSelect;

                    //Set the width of the SkillSelect Panel to the number of skill buttons it will contain
                    pnlSkillSelect.Width = ((MultipleSkills.Count + 1) * (new SkillSelectButton().Width)) + ((MultipleSkills.Count + 1) * SKILL_BUTTON_PADDING - SKILL_BUTTON_PADDING / 2); //last # is accounting for 3px padding on each side of each button

                    //Create a button for each skill and place it in the SkillSelect Panel
                    foreach (Skill multiSkill in MultipleSkills)
                    {
                        btnSkillSelect = new SkillSelectButton();
                        btnSkillSelect.skill = multiSkill;
                        btnSkillSelect.treeControl = tlpTree;
                        btnSkillSelect.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject("ImageNotFound"); //TODOSSG change when you have all pics in place (multiSkill.id);

                        //Add the button to the panel  //TOODSSG how to get the correct order? may need to hard-code this?
                        pnlSkillSelect.Controls.Add(btnSkillSelect);

                        //Specify the position the button within the panel
                        btnSkillSelect.Location = new Point((pnlSkillSelect.Controls.Count * btnSkillSelect.Width), 3);

                        //Add these skills to a list for future reference so we know which are multi-selection skills
                        MultiSelectionSkills.Add(multiSkill);
                    }

                    //Add a default UnassignSkill button at the end incase the user chooses to not pick a skill at this time but wants to close the panel
                    pnlSkillSelect.Controls.Add(new UnassignSkillButton());
                }
            }

        }

        private bool IsMultiSelectionSkill(Skill skill, List<Skill> MultiSelectionSkills)
        {
            //Verify the current skill hasn't already been loaded by the LoadMultiSelectionSkills process
            return MultiSelectionSkills.Contains(skill);
        }

        private void ShowSelectedSkillData(object sender, EventArgs e)
        {
            string debugMessage;
            Button btnSkill = (Button)sender;
            Skill skill = (Skill)btnSkill.Tag;

            debugMessage = "Skill: " + skill.name + "\n" +
                            "XPos: " + skill.x + "\n" +
                            "YPos:" + skill.y;
                            ;
            MessageBox.Show(debugMessage);
        }
    }
}
