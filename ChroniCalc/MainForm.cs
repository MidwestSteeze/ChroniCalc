using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ChroniCalc
{
    public partial class MainForm : Form
    {
        enum TreeStatus
        {
            Importing,
            Resetting
        }

        //Skill Tree
        public const string IMAGE_FILENAME_PREFIX = "spr_spellicons_";
        const int SKILL_BUTTON_PADDING = 6;
        public const int SKILL_POINTS_MAX = 100;
        const string XML_EXT = ".xml";

        private string BuildsDirectory;
        private TreeStatus treeStatus;

        //Resource Managers for pulling assets (ie. data, images, etc.) which is a reflection of the Assets directory
        ResourceManager ResourceManagerImageClass;
        ResourceManager ResourceManagerImageSkill;
        ResourceManager ResourceManagerImageTree;

        //Lists and Objects
        public Build build;
        List<CharacterClass> characterClasses;
        List<Tree> trees;
        List<Skill> skills;

        List<Button> treeButtons;
        List<TreeTableLayoutPanel> treePanels;

        public MainForm()
        {
            InitializeComponent();

            //Assign ParentForm to UserControls dropped on the Form that need it
            this.pbpBuildShare.ParentForm = this;

            //Updates the version as shown on screen
            lblVersion.Text = "v" + this.ProductVersion;

            //Set the directory where Builds are stored
            BuildsDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\Builds";

            //Create the directory where saved builds are to be stored, if it doesn't yet exist
            if (!Directory.Exists(BuildsDirectory))
            {
                Directory.CreateDirectory(BuildsDirectory);
            }

            //Set the # of available skill points that can be spent to build the character
            lblSkillPointsRemaining.Text = SKILL_POINTS_MAX.ToString();

            //Init resource managers for pulling assets (ie. data, images, etc.)
            ResourceManagerImageClass = new ResourceManager("ChroniCalc.ResourceImageClass", Assembly.GetExecutingAssembly());
            ResourceManagerImageSkill = new ResourceManager("ChroniCalc.ResourceImageSkill", Assembly.GetExecutingAssembly());
            ResourceManagerImageTree = new ResourceManager("ChroniCalc.ResourceImageTree", Assembly.GetExecutingAssembly());

            //Init global variables
            build = new Build("", null, 0, 0);
            characterClasses = new List<CharacterClass>();

            //Add all tree buttons to a list for looping over and finding within later as needed
            treeButtons = new List<Button>();
            treeButtons.Add(btnTree1);
            treeButtons.Add(btnTree2);
            treeButtons.Add(btnTree3);
            treeButtons.Add(btnTree4);

            //Add 4 trees to the list of available Trees for a Class
            treePanels = new List<TreeTableLayoutPanel>();
            for (int i = 0; i <= 3; i++)
            {
                TreeTableLayoutPanel ttlp = new TreeTableLayoutPanel(pnlTrees);
                treePanels.Add(ttlp);
            }

            PopulateSkillTrees();
            LoadBuildsIntoBuildsList();

            //Show the Builds list at start
            pnlBuilds.BringToFront();
        }

        private void LoadBuildsIntoBuildsList()
        {
            string[] buildFiles;
            Build loadedBuild;
            XmlSerializer serializer;

            buildFiles = Directory.GetFiles(BuildsDirectory, "*.xml");

            foreach (string buildFile in buildFiles)
            {
                try
                {
                    // Load the saved build content from the file into a Build object
                    using (var stream = new StreamReader(buildFile))
                    {
                        serializer = new XmlSerializer(typeof(Build));
                        loadedBuild = serializer.Deserialize(stream) as Build;
                    }

                    AddBuildToBuildsList(loadedBuild);
                }
                catch (Exception)
                {
                    //The file loaded was xml but did not serialize cleanly into the Build object; explain this to the user and continue on to loading the next file
                    MessageBox.Show("Unable to load Build file '" + Path.GetFileName(buildFile) + "' into the saved Builds list in the Builds tab.  If this is not an actual Build file, please remove it from the Builds folder on disk to stop seeing this message.");
                }

            }
        }

        private void AddBuildToBuildsList(Build loadedBuild)
        {
            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(dgvBuilds);
            row.Cells[0].Value = loadedBuild.name;
            row.Cells[1].Value = loadedBuild.characterClass.name + " Lvl" + loadedBuild.level + " M" + loadedBuild.masteryLevel;
            dgvBuilds.Rows.Add(row);
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
                        skill.damage = !(skillNode.SelectSingleNode("damage") is null) ? skillNode.SelectSingleNode("damage").InnerXml.Split(',') : new string[] { }; //TODOSSG convert this to hold just the value and remove the "%"?
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
                    //There are no additional conditions to apply against this node, 
					// so we have a value
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

            //Show the corresponding tlpTree by finding a match and bringing it in front of the others
            foreach (TreeTableLayoutPanel ttlp in treePanels)
            {
                if (ttlp.Name == btnTree.Tag.ToString())
                {
                    ttlp.Show();
                    ttlp.BringToFront();
                    lblTree.Text = ttlp.passiveSkillName;
                }
                else
                {
                    //Hide the non-selected Tree
                    ttlp.Hide();
                }
            }

            //Highlight the selected Tree button so it stands out from the rest like it's the active one
            foreach (Button btn in treeButtons)
            {
                if (btn == btnTree)
                {
                    //Highlight the button by adding a border
                    btn.FlatAppearance.BorderSize = 1;
                }
                else
                {
                    //Unhighlight the non-selected buttons by removing the border
                    btn.FlatAppearance.BorderSize = 0;
                }
            }
        }

        private void CboClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            CharacterClass selectedClass;
            string characterClass = (sender as ComboBox).SelectedItem.ToString();

            //Prompt user ensuring they want to reset their character
            DialogResult dialogResult = MessageBox.Show("Changing Class will reset this character.  Continue?", "Change Class", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
				//Prompt for save/if user really wants to start a new character, overwriting current build
            	if (!SaveBuildShouldContinue())
            	{
                    //Reset the selected class since the user chose to cancel
                    // Suppress change events
                    cboClass.SelectedIndexChanged -= CboClass_SelectedIndexChanged;

                    cboClass.SelectedIndex = cboClass.Items.IndexOf(build.characterClass.name);

                    // Unsuppress change events
                    cboClass.SelectedIndexChanged += CboClass_SelectedIndexChanged;

                    return;
	            }

                //Clear everything related to the previous build
                ClearCharacter(build);

                //Get the newly-selected class
                selectedClass = characterClasses.Find(x => x.name == characterClass);

                if (selectedClass is null)
                {
                    //TODOSSG throw error that the selected class was not found in the current list of character classes
                }

                //Update data on the build (everything not listed here was handled in the ResetCharacter() code (e.g. level, masteryLevel, trees, skills, etc)
                build.characterClass = selectedClass;
                build.name = "";

                InitializeBuild(build);

                // Show the Trees, incase a different view (e.g. Inventory, Builds, etc) was being shown
                pnlTrees.BringToFront();
            }
        }

        private void ClearCharacter(Build build)
        {
            ClearGear();

            // Clear all Trees if we currently have a build in progress
            //  (read: this basically covers our butt during the first selection of a class from the drop-down, or load from a saved build file, where no build is in place yet)
            if (!(build.characterClass is null))
            {
                //Change which trees are available
                ClearTrees(build.characterClass.trees);
            }

            //UpdateStats(build);
        }

        private void UpdateStats(Build build)
        {
            //Update Character Name and Level
            lblBuildName.Text = build.name;
            lblLevel.Text = build.level.ToString();

            //Update Mastery level
            lblMastery.Text = build.masteryLevel.ToString();

            //Update # of Skill Points Remaining
            lblSkillPointsRemaining.Text = (SKILL_POINTS_MAX - build.level).ToString();

            //TODOSSG
            //Update all Stats (dmg, health, etc)
        }

        private void ClearGear()
        {
            //TODOSSG
        }

        private void ClearTrees(List<Tree> trees)
        {
            Tree tree;

            //Clear all content from the existing trees (ie. treePanels)
            foreach (TreeTableLayoutPanel ttlpTree in treePanels)
            {
                tree = new Tree();
                ClearTree(tree, ttlpTree);
            }

            //Clear the images on the tree selection buttons
            foreach (Button treeButton in treeButtons)
            {
                treeButton.BackgroundImage = null;
                treeButton.Tag = "";
            }
        }

        private void ClearTree(Tree tree, TreeTableLayoutPanel ttlpTree)
        {
            SkillButton skillButton;

            //Reset the skill points allocated on the Tree control's skills
            foreach (Control control in ttlpTree.Controls)
            {
                if (control is SkillButton)
                {
                    skillButton = (control as SkillButton);

                    //Update the level of the build since we're removing skill points spent in this Tree
                    // NOTE: Only if this is a build we're importing, not one we're resetting //TODOSSG test this, this may not be conditioned on anymore but is working
                    if (!skillButton.isPassiveBonusButton) // && build.level > 0) //TODOSSG this is a hack for the NOTE above //TODOSSG this note is maybe no longer applicable
                    {
                        build.level -= skillButton.skill.level;
                    }                    

                    //Lastly, reset the level of the skill
                    skillButton.skill.level = 0;
                }
            }

            ttlpTree.Controls.Clear();

            //Reset the total number of skill points allocated on the Tree control
            ttlpTree.skillPointsAllocated = 0;

            //Update Stats here because we may have cleared a bunch of active/passive skills
            UpdateStats(build);
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
            foreach (TreeTableLayoutPanel ttlpTree in treePanels)
            {
                //Pull the correct Tree object for the current TreeTableLayoutPanel from the currently selected Class
                tree = selectedClass.trees.Find(x => x.name == ttlpTree.Name);

                if (!(tree is null))
                {
                    //Load every aspect of the current Tree into the current Tree Panel
                    LoadTree(tree, ttlpTree);
                }
                else
                {
                    //TODOSSG throw error that tlpTree.Name is not found in the currently-loaded Trees object for the selected Class
                    MessageBox.Show("LoadTrees(): Tree not found: " + ttlpTree.Name);  //TODO replace this with an exception so it gets logged?
                }
            }
        }

        private void LoadTree(Tree tree, TreeTableLayoutPanel ttlpTree)
        {
            int treeSkillPointsAllocated = 0;

            List<Skill> MultiSelectionSkills = new List<Skill>();

            //Load any skill slot that beings with a "+" for the user to pick between multiple skills
            LoadMultiSelectionSkills(tree, ttlpTree, ref MultiSelectionSkills);

            //Loop through each Skill and place it into the correct slot in the TableLayoutPanel
            foreach (Skill skill in tree.skills)
            {
                //Load all remaining skill slots, ensuring it's not a Mutli-skill slot that was already loaded
                if (!(IsMultiSelectionSkill(skill, MultiSelectionSkills)))
                {
                    SkillTooltipPanel pnlSkillTooltip = CreateSkillTooltip(skill, ttlpTree);
                    //Create a new control to hold this skill at the skills X and Y location
                    SkillButton btnSkill = new SkillButton(skill, ttlpTree, pnlSkillTooltip, this);

                    //Specify the passive bonus skill button as being such, we'll need this info for other situations
                    if (skill.name == ttlpTree.passiveSkillName)
                    {
                        btnSkill.isPassiveBonusButton = true;
                    }

                    //Add the skill button to the tree
                    ttlpTree.Controls.Add(btnSkill, skill.x, skill.y);

                    //Keep a running total of skill points spent on this tree (if we're loading a saved build)
                    // NOTE: Don't include the level of the passive bonus with the running total of points spent on the tree
                    if (!btnSkill.isPassiveBonusButton)
                    {
                        treeSkillPointsAllocated += skill.level;
                    }
                }
            }

            //Update the Tree's level
            ttlpTree.skillPointsAllocated = treeSkillPointsAllocated;

            //Update Stats here because we may have loaded a build with invested skill points
            UpdateStats(build);

            //With all Skills added to the tree, populate all descriptions for each skill
            PopulateSkillDescriptions(ttlpTree);
        }

        //Look at all skills within the current Tree to see if there are multiples that share the same position (ie. Dive, Jump, Flame Dash)
        // where only 1 should be selected by the user
        private void LoadMultiSelectionSkills(Tree tree, TreeTableLayoutPanel tlpTree, ref List<Skill> MultiSelectionSkills)
        {
            bool alreadyChoseASkill;
            int xPos;
            int yPos;
            SkillSelectButton btnSkillSelect;
            List<Skill> MultipleSkills = new List<Skill>();

            //Loop through each skill in the tree to analyze its X and Y properties and see if more than 1 skill exists at this position
            foreach (Skill skill in tree.skills)
            {
                //Reset the flag since we're working with a new skill/group of Skills
                alreadyChoseASkill = false;

                //Get the current position of this skill on the tree
                xPos = skill.x;
                yPos = skill.y;

                //Check you haven't already captured skills for this position
                if (MultiSelectionSkills.FindAll(ms => ms.x == xPos && ms.y == yPos).Count > 1)
                {
                    //This skill position has already been captured into the MultiSelectionSkills list
                    continue;
                }

                //Capture all possible skills at the current position
                MultipleSkills.Clear();
                MultipleSkills = tree.skills.FindAll(s => s.x == xPos && s.y == yPos);

                //Check for if we're loading a saved build and the user actually already picked a skill
                //  TODO I can check if they leveled it but if they picked 1 and didn't level it it'll go back to a "+" button. Do I care?
                if ((treeStatus == TreeStatus.Importing) && MultipleSkills.FindAll(ms => ms.level > 0).Count > 0)
                {                
                    foreach (Skill multiSkill in MultipleSkills)
                    {
                        if (multiSkill.level > 0)
                        {
                            //Mark this section as already having been picked and don't add it to the MultiSelectionSkills list;
                            //  this'll ensure it gets loaded as an individual skill back in LoadTree()
                            alreadyChoseASkill = true;
                        }
                        else
                        {
                            //Add the not-selected skill to the MultiSelectionSkills list so we don't load it
                            MultiSelectionSkills.Add(multiSkill);  //TODO is this being doubly-added with the MultiSelectionSkills.Add in the below-for each loop as well?
                        }
                    }
                }

                //Check if more than 1 skill does infact exist at the current position
                if (MultipleSkills.Count > 1 && !alreadyChoseASkill)
                {
                    //Add a SkillSelect Button ("+" button) to the shared X,Y position on the tree
                    MultiSkillSelectButton btnMultiSkillSelect = new MultiSkillSelectButton(xPos, yPos);
                    tlpTree.Controls.Add(btnMultiSkillSelect, xPos, yPos);

                    //Instantiate a new SkillSelect Panel to hold the multiple skills
                    SkillSelectPanel pnlSkillSelect = new SkillSelectPanel();

                    //Set its Parent control to actually be the Panel that holds all Trees and not the TableLayoutPanel for the current Tree 
                    //  so it can display OVER the SkillSelectButton (without setting this, it's trying to display in the same cell in the tlpTree)
                    pnlSkillSelect.Parent = pnlTrees;

                    //Tie the panel to the SkillSelect Button that holds the position for which this panel and its skills apply to
                    btnMultiSkillSelect.skillSelectPanel = pnlSkillSelect;

                    //Set the width of the SkillSelect Panel to the number of skill buttons it will contain (30 = width of SkillSelectButton)
                    pnlSkillSelect.Width = ((MultipleSkills.Count + 1) * 30) + ((MultipleSkills.Count + 1) * SKILL_BUTTON_PADDING - ((MultipleSkills.Count + 1) * SKILL_BUTTON_PADDING / 2)); //last # is accounting for 3px margin on each side of each button

                    //Create a button for each skill and place it in the SkillSelect Panel
                    foreach (Skill multiSkill in MultipleSkills)
                    {
                        SkillTooltipPanel pnlSkillTooltip = CreateSkillTooltip(multiSkill, tlpTree);
                        btnSkillSelect = new SkillSelectButton(this, pnlSkillTooltip);
                        btnSkillSelect.skill = multiSkill;
                        btnSkillSelect.treeControl = tlpTree;

                        if (!((Image)ResourceManagerImageSkill.GetObject(IMAGE_FILENAME_PREFIX + multiSkill.id.ToString()) is null))
                        {
                            btnSkillSelect.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject(IMAGE_FILENAME_PREFIX + multiSkill.id.ToString());
                        }
                        else
                        {
                            //Use an ImageNotFound image as a placeholder until the skill's image is created and added to the Resource Manager
                            btnSkillSelect.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject("ImageNotFound");
                        }

                        //Add the button to the panel  //TOODSSG how to get the correct order? may need to hard-code this?
                        pnlSkillSelect.Controls.Add(btnSkillSelect);

                        //Specify the position the button within the panel
                        btnSkillSelect.Location = new Point(((pnlSkillSelect.Controls.Count - 1) * btnSkillSelect.Width) + SKILL_BUTTON_PADDING, 3);

                        //Add these skills to a list for future reference so we know which are multi-selection skills
                        MultiSelectionSkills.Add(multiSkill);
                    }

                    //Add a default UnassignSkill button at the end incase the user chooses to not pick a skill at this time but wants to close the panel
                    pnlSkillSelect.Controls.Add(new UnassignSkillButton(pnlSkillSelect.Controls.Count, SKILL_BUTTON_PADDING));
                }
            }

        }

        private bool IsMultiSelectionSkill(Skill skill, List<Skill> MultiSelectionSkills)
        {
            //Verify the current skill hasn't already been loaded by the LoadMultiSelectionSkills process
            return MultiSelectionSkills.Contains(skill);
        }

        public SkillTooltipPanel CreateSkillTooltip(Skill skill, TreeTableLayoutPanel tlpTree)
        {
            SkillTooltipPanel pnlSkillTooltip;

            pnlSkillTooltip = new SkillTooltipPanel(skill, tlpTree, this);

            return pnlSkillTooltip;
        }

        private void PopulateSkillDescriptions(TreeTableLayoutPanel treeTableLayoutPanel)
        {
            //For each skill in the tree, populate its description by loading values in place of the placeholders
            foreach (Control skill in treeTableLayoutPanel.Controls)
            {
                if (skill is MultiSkillSelectButton)
                {
                    //Populate description for each skill
                    foreach (SkillSelectButton singleSkill in (skill as MultiSkillSelectButton).skillSelectPanel.Controls.OfType<SkillSelectButton>())
                    {
                        singleSkill.skillTooltipPanel.PopulateDescription();
                    }
                }
                else if (skill is SkillButton)
                {
                    //Populate description of the skill
                    (skill as SkillButton).skillTooltipPanel.PopulateDescription();
                }
                else
                {
                    //A non-skill button control has been found within the TreeTableLayoutPanel, should I be concerned?
                }
            }
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

        // Used to reduce appearance of Control Paint lag when toggling visibility of TreeTableLayoutPanels (ie. Trees)
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED
                return cp;
            }
        }

        private void BtnResetTree_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult;
            Tree tree;
            TreeTableLayoutPanel ttlpTree;

            // Ensure the user wants to reset this tree
            dialogResult = MessageBox.Show("Are you sure you want to reset this Tree?", "Reset Tree", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {

                treeStatus = TreeStatus.Resetting;

                // Find the currently-shown tree
                ttlpTree = new TreeTableLayoutPanel(pnlTrees);

                foreach (TreeTableLayoutPanel ttlp in treePanels)
                {
                    if (ttlp.Visible)
                    {
                        ttlpTree = ttlp;
                        break;
                    }
                }

                // Only reset the tree if a TreeTableLayoutPanel is visible and is loaded with controls (ie. skills)
                //   (this rules out if the application has done an initial load and no character was selected/loaded)
                if (ttlpTree.HasChildren)
                {
                    // Find the corresponding Tree to the current TreeTableLayoutPanel shown
                    tree = new Tree();

                    tree = build.characterClass.trees.Find(x => x.name == ttlpTree.Name);

                    // Throw an error if we don't have a ttlp or tree, since we made assumptions when finding it
                    if (ttlpTree is null || tree is null)
                    {
                        throw new Exception("BtnResetTree_Click(): Tree not found");
                    }

                    // Remove all controls from the currently-shown tree
                    ClearTree(tree, ttlpTree);

                    // Add all controls back onto the currently-shown tree
                    LoadTree(tree, ttlpTree);
                }
            }
        }

        private void BtnNavBuilds_Click(object sender, EventArgs e)
        {
            // Show the Builds tab
            pnlBuilds.BringToFront();
        }

        private void BtnNavInventory_Click(object sender, EventArgs e)
        {
            // Show the Inventory tab
            pnlInventory.BringToFront();
        }

        private void BtnNavTrees_Click(object sender, EventArgs e)
        {
            // Show the Trees tab
            pnlTrees.BringToFront();
        }

        private void BtnNavSave_Click(object sender, EventArgs e)
        {
            DataGridViewRow dgvRow;
            DialogResult dialogResult;
            string fileNameAndPath;
            int rowIndex = -1;

            fileNameAndPath = BuildsDirectory + "\\" + build.name + XML_EXT;

            // Find the build file within the Builds directory, by name, and open it
            if (File.Exists(fileNameAndPath))
            {
                // Ensure the user wants to overwrite this build
                dialogResult = MessageBox.Show("Are you sure you want to overwrite Build \"" + build.name + "\"?", "Save Build", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                     SaveBuild(fileNameAndPath);

                    // Update the row in the Builds list
                    DataGridViewRow row = dgvBuilds.Rows.Cast<DataGridViewRow>()
                        .Where(r => r.Cells["BuildName"].Value.ToString().Equals(build.name)).First();

                    rowIndex = row.Index;

                    dgvRow = dgvBuilds.Rows[rowIndex];
                    dgvRow.Cells["Stats"].Value = build.characterClass.name + " Lvl" + build.level + " M" + build.masteryLevel;
                    dgvRow.Selected = true;
                }
            }
            else
            {
                // The build has not yet been saved so allow the user to save it as a new build
                BtnNavSaveAs_Click(sender, e);
            }
            
        }

        private void BtnNavSaveAs_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.InitialDirectory = BuildsDirectory;
            saveFileDialog.RestoreDirectory = true;

            // Save the build as a new build
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Update the name of the build based on the filename
                build.name = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                lblBuildName.Text = build.name;

                SaveBuild(saveFileDialog.FileName);

                // Add the newly-saved build to the Builds list so it can be loaded later if needed
                AddBuildToBuildsList(build);
            }
        }

        private void SaveBuild(string fileNameAndPath)
        {
            XmlSerializer serializer;

            // Save the build to the specified file
            using (var writer = new StreamWriter(fileNameAndPath))
            {
                // Serialize the build object into xml (this should make saving/loading builds easy)
                serializer = new XmlSerializer(build.GetType());
                serializer.Serialize(writer, build);  //TODO add try/catch around this incase serialization fails
                writer.Flush();
            }
        }

        private void BtnBuildDelete_Click(object sender, EventArgs e)
        {
            string buildName;
            DataGridViewRow dgvRow;
            DialogResult dialogResult;
            string fileNameAndPath;
            
            // Get the row in the builds list that the user has selected to delete
            dgvRow = (dgvBuilds.SelectedRows[0] as DataGridViewRow);  //TODO ensure the user can only select 1 row in the grid
            buildName = dgvRow.Cells["BuildName"].Value.ToString();

            // Ensure the user wants to delete this build
            dialogResult = MessageBox.Show("Are you sure you want to delete Build \"" + buildName + "\"?", "Delete Build", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                fileNameAndPath = BuildsDirectory + "\\" + buildName + XML_EXT;
                // Find the build file within the Builds directory, by name, and delete it
                if (File.Exists(BuildsDirectory + "\\" + buildName + XML_EXT))
                {
                    File.Delete(BuildsDirectory + "\\" + buildName + XML_EXT);

                    // Remove the build from the builds list
                    dgvBuilds.Rows.Remove(dgvRow);
                }
                else
                {
                    MessageBox.Show("No save file found to delete for Build \"" + buildName + "\".");
                }
            }
        }

        private void BtnBuildOpen_Click(object sender, EventArgs e)
        {
            string fileNameAndPath;

			//Prompt for save/if user really wants to open the Build, overwriting current build
            if (!SaveBuildShouldContinue())
            {
                return;
            }

            fileNameAndPath = BuildsDirectory + "\\" + dgvBuilds.CurrentRow.Cells["BuildName"].Value.ToString() + XML_EXT;

            treeStatus = TreeStatus.Importing;

            // Find the build file within the Builds directory, by name, and open it
            if (File.Exists(fileNameAndPath))
            {
                OpenBuild(fileNameAndPath);
            }
            else
            {
                //TODO display message that build not found for some reason
            }
            
        }

        public void OpenBuild(string buildContent, bool pasteBinImport = false)
        {
            XmlSerializer serializer;

            // Clear everything related to the previous build
            ClearCharacter(build);

            // Deserialize the content the appropriate way depending on what was passed in (ie. pastebin Raw Text extract or a filepath to an XML file on disk)
            if (pasteBinImport)
            {
                using (var stringReader = new StringReader(buildContent))
                {
                    serializer = new XmlSerializer(typeof(Build));
                    build = serializer.Deserialize(stringReader) as Build;  //TODO add try/catch around this incase Deserialization fails
                }
            }
            else
            {
                // Load the saved build content from the file into a Build object
                using (var stream = new StreamReader(buildContent))
                {
                    serializer = new XmlSerializer(typeof(Build));
                    build = serializer.Deserialize(stream) as Build;  //TODO add try/catch around this incase Deserialization fails
                }
            }

            // Using the newly assigned Build object, create and populate all controls for this build
            InitializeBuild(build);

            // Set the correct class in the Class selector to match the build that was loaded
            // Suppress change events
            cboClass.SelectedIndexChanged -= CboClass_SelectedIndexChanged;

            cboClass.SelectedIndex = cboClass.Items.IndexOf(build.characterClass.name);

            // Unsuppress change events
            cboClass.SelectedIndexChanged += CboClass_SelectedIndexChanged;

            // Show the Trees
            pnlTrees.BringToFront();
        }

        private void InitializeBuild(Build build)
        {
            string treeName;

            //Change the Class image  //TODO move this logic into the switch below it since it's the same condition
            switch (build.characterClass.name)
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
                    throw new Exception("ChangeClass: characterClass of " + build.characterClass.name + " was not added to Switch for setting the Class iamge.");
            }

            //Depending on Class, load up Image and Tag on Tree tabs and update the Mastery tree
            switch (build.characterClass.name)
            {
                case "Berserker":
                    treeName = "Guardian";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].Name = treeName;
                    treePanels[0].passiveSkillId = 380;
                    treePanels[0].passiveSkillName = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "SkyLord";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].Name = treeName;
                    treePanels[1].passiveSkillId = 478;
                    treePanels[1].passiveSkillName = "Sky Lord";  //Overridden from treeName;  //this is manually written by looking it up in the xml data (this wouldn't be an issue if you could work with the raw json)
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Dragonkin";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].Name = treeName;
                    treePanels[2].passiveSkillId = 82;
                    treePanels[2].passiveSkillName = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Frostborn";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].Name = treeName;
                    treePanels[3].passiveSkillId = 93;
                    treePanels[3].passiveSkillName = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                case "Templar":
                    treeName = "Vengeance";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].Name = treeName;
                    treePanels[0].passiveSkillId = 241;
                    treePanels[0].passiveSkillName = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Wrath";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].Name = treeName;
                    treePanels[1].passiveSkillId = 242;
                    treePanels[1].passiveSkillName = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Conviction";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].Name = treeName;
                    treePanels[2].passiveSkillId = 273;
                    treePanels[2].passiveSkillName = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Redemption";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].Name = treeName;
                    treePanels[3].passiveSkillId = 274;
                    treePanels[3].passiveSkillName = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                case "Warden":
                    treeName = "WindRanger";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].Name = treeName;
                    treePanels[0].passiveSkillId = 512;
                    treePanels[0].passiveSkillName = "Wind Ranger";  //Overridden from treeName
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Druid";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].Name = treeName;
                    treePanels[1].passiveSkillId = 545;
                    treePanels[1].passiveSkillName = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "StormCaller";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].Name = treeName;
                    treePanels[2].passiveSkillId = 124;
                    treePanels[2].passiveSkillName = "Storm Caller";  //Overridden from treeName
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "WinterHerald";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].Name = treeName;
                    treePanels[3].passiveSkillId = 609;
                    treePanels[3].passiveSkillName = "Winter Herald";  //Overridden from treeName
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                case "Warlock":
                    treeName = "Corruptor";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].Name = treeName;
                    treePanels[0].passiveSkillId = 642;
                    treePanels[0].passiveSkillName = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Lich";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].Name = treeName;
                    treePanels[1].passiveSkillId = 748;
                    treePanels[1].passiveSkillName = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Demonologist";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].Name = treeName;
                    treePanels[2].passiveSkillId = 707;
                    treePanels[2].passiveSkillName = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Reaper";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].Name = treeName;
                    treePanels[3].passiveSkillId = 182;
                    treePanels[3].passiveSkillName = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                default:
                    //TODOSSG what's the best way to handle exceptions? error message to user? debug log? email call stack?
                    throw new Exception("InitializeBuild(): characterClass of " + build.characterClass.name + " not found.");
                    //break;
            }

            //Update each Tree thumbnail
            foreach (Button treeButton in treeButtons)
            {
                //Look at the the button's Tag to determine which image to load into it
                LoadTreeIconButtonImage(ResourceManagerImageTree, treeButton, treeButton.Tag.ToString());
            }

            //Load all trees for the selected class into their corresponding tree table layout panels
            LoadTrees(build.characterClass);

            //Set the Mastery tree button's Tag to the currently-selected Class
            btnTreeMastery.Tag = build.characterClass.name;

            //TODOSSG
            //Update the rows in the Mastery tree to be specific to the newly-selected Class
            //ResetMasteryTree(build.characterClass.name);

            //Default focus to the first Tree
            ShowTree(treeButtons.First(), new EventArgs());
        }

        private void btnBuildSharing_Click(object sender, EventArgs e)
        {
            if (!pbpBuildShare.Visible)
            {
                pbpBuildShare.Show();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			//Prompt for save/if user really wants to save the build before closing the application //TODO fix this, application still exists on selecting "Cancel"
            if (SaveBuildShouldContinue())
            {
                Application.Exit();
            }
        }

        public bool SaveBuildShouldContinue()
        {
            bool shouldContinue = false;
            DialogResult dialogResult;
            string fileNameAndPath;

            fileNameAndPath = BuildsDirectory + "\\" + build.name + XML_EXT;

            // Find the build file within the Builds directory, by name, and open it
            if (!File.Exists(fileNameAndPath) && !(build.characterClass is null))
            {
                // Ensure the user wants to overwrite this build
                dialogResult = MessageBox.Show("The current Build is not saved.  Would you like to save it before continuing?", "Save Build", MessageBoxButtons.YesNoCancel);

                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        BtnNavSaveAs_Click(this, EventArgs.Empty);
                        shouldContinue = true; //TODO what if the SaveAs procs but the user decides not to save it?
                        break;
                    case DialogResult.No:
                        // They don't want to save the build, but want to continue
                        shouldContinue = true;
                        break;
                    case DialogResult.Cancel:
                        //They dont want to continue with the requested process
                        shouldContinue = false;
                        break;
                    default:
                        break;
                }
            }
            else
            {
                shouldContinue = true;
            }

            return shouldContinue;
        }
    }
}
