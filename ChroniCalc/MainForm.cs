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
        const int SKILL_BUTTON_PADDING = 6;
        public const int SKILL_POINTS_MAX = 100;
        const string XML_EXT = ".xml";

        private string BuildsDirectory;
        private TreeStatus treeStatus;

        //Resource Managers for pulling assets (ie. data, images, etc.) which is a reflection of the Assets directory
        ResourceManager ResourceManagerData;
        ResourceManager ResourceManagerImageClass;
        ResourceManager ResourceManagerImageSkill;
        ResourceManager ResourceManagerImageTree;

        //Lists and Objects
        public Build build;
        public BuildShareForm buildShareForm;
        List<CharacterClass> characterClasses;
        List<Tree> trees;
        List<Skill> skills;

        List<Button> treeButtons;
        List<TreeTableLayoutPanel> treePanels;

        public MainForm()
        {
            try
            {
                InitializeComponent();

                // Create a new BuildShare form that will display when the Build Sharing button is clicked
                buildShareForm = new BuildShareForm();
                buildShareForm.ParentForm = this;

                //Set the directory where Builds are stored
                BuildsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ChroniCalc\\Builds";

                //Create the directory where saved builds are to be stored, if it doesn't yet exist
                if (!Directory.Exists(BuildsDirectory))
                {
                    Directory.CreateDirectory(BuildsDirectory);
                }

                //Set the # of available skill points that can be spent to build the character
                lblSkillPointsRemaining.Text = SKILL_POINTS_MAX.ToString();

                //Init resource managers for pulling assets (ie. data, images, etc.)
                ResourceManagerData = new ResourceManager("ChroniCalc.ResourceData", Assembly.GetExecutingAssembly());
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
                treeButtons.Add(btnTreeMastery);

                //Add 5 trees to the list of available Trees for a Class (4 Class Skill Trees and 1 Mastery Tree)
                treePanels = new List<TreeTableLayoutPanel>();
                for (int i = 0; i <= 4; i++)
                {
                    TreeTableLayoutPanel ttlp;

                    if (i == 4)
                    {
                        //The last tree added is the Mastery tree and has some different properties
                        ttlp = new TreeTableLayoutPanel(pnlTrees, 11, 7, 516, 274);
                        ttlp.Name = "Mastery";
                    }
                    else
                    {
                        //Create a new Class Skill tree
                        ttlp = new TreeTableLayoutPanel(pnlTrees, 10, 7, 475, 279);
                    }

                    treePanels.Add(ttlp);
                }

                PopulateSkillTrees();
                LoadBuildsIntoBuildsList();

                //Show the Builds list at start
                pnlBuilds.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error on load of application: " + ex.ToString());
            }
        }

        private void AddMasteryPassiveRowCounters(string className, ref List<Skill> skills)
        {
            const string PLACEHOLDER_TREE = "PLACEHOLDER_TREE";
            const string PLACEHOLDER_VALUE = "MASTERY_BONUS_PLACEHOLDER";
            const string DESCRIPTION_CLASS_ROW = "Increases the damage of all " + PLACEHOLDER_TREE + " skills for each point spent in this line.  Current bonus is " + PLACEHOLDER_VALUE + "%.";
            const string DESCRIPTION_GENERIC_ROW = "Increases Health, Damage, and Mana for each point spent in this line.  Current bonus is " + PLACEHOLDER_VALUE + "%.";

            // Add Class-specific Mastery row countrers
            switch (className)
            {
                case "Berserker":
                    AddMasteryPassiveRowCounter("Guardian Mastery", 100011, 0, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Guardian"), ref skills);
                    AddMasteryPassiveRowCounter("Sky Lord Mastery", 100013, 1, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Sky Lord"), ref skills);
                    AddMasteryPassiveRowCounter("Dragonkin Mastery", 100012, 5, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Dragonkin"), ref skills);
                    AddMasteryPassiveRowCounter("Frostborn Mastery", 100014, 6, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Frostborn"), ref skills);
                    break;

                case "Templar":
                    AddMasteryPassiveRowCounter("Vengeance Mastery", 100007, 0, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Vengeance"), ref skills);
                    AddMasteryPassiveRowCounter("Wrath Mastery", 100009, 1, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Wrath"), ref skills);
                    AddMasteryPassiveRowCounter("Conviction Mastery", 100008, 5, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Conviction"), ref skills);
                    AddMasteryPassiveRowCounter("Redemption Mastery", 100010, 6, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Redemption"), ref skills);
                    break;

                case "Warden":
                    AddMasteryPassiveRowCounter("Wind Ranger Mastery", 100003, 0, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Wind Ranger"), ref skills);
                    AddMasteryPassiveRowCounter("Druid Mastery", 100004, 1, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Druid"), ref skills);
                    AddMasteryPassiveRowCounter("Storm Caller Mastery", 100005, 5, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Storm Caller"), ref skills);
                    AddMasteryPassiveRowCounter("Winter Herald Mastery", 100006, 6, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Winter Herald"), ref skills);
                    break;

                case "Warlock":
                    AddMasteryPassiveRowCounter("Corruptor Mastery", 100015, 0, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Corruptor"), ref skills);
                    AddMasteryPassiveRowCounter("Lich Mastery", 100017, 1, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Lich"), ref skills);
                    AddMasteryPassiveRowCounter("Demonologist Mastery", 100016, 5, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Demonologist"), ref skills);
                    AddMasteryPassiveRowCounter("Reaper Mastery", 100018, 6, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Reaper"), ref skills);
                    break;

                default:
                    // Throw error that class not found (ie. if a new class is added)
                    throw new Exception("AddMasteryPassiveRowCounters(): className of " + className + " not found.");
            }

            // Add the 3 generic Mastery row counters
            for (int i = 0; i <= 2; i++)
            {
                AddMasteryPassiveRowCounter("Class Mastery " + (i + 1).ToString(), 100000 + i, i + 2, DESCRIPTION_GENERIC_ROW, ref skills);
            }
        }

        private void AddMasteryPassiveRowCounter(string passiveName, int masteryId, int rowIndex, string description, ref List<Skill> skills)
        {
            //Add a new Skill to hold this mastery
            Skill passiveMasterySkill = new Skill();
            passiveMasterySkill.description = description;
            passiveMasterySkill.element = "Ethereal";
            passiveMasterySkill.id = masteryId;
            passiveMasterySkill.level = 0;
            passiveMasterySkill.name = passiveName;
            passiveMasterySkill.max_rank = int.MaxValue;
            passiveMasterySkill.min_level = -1;
            passiveMasterySkill.type = "Passive Skill";
            passiveMasterySkill.value = new double[] { 0.3 };  //TODO find the right value for this, it may differ between each row so perhaps change it to a param and define it in the call
            passiveMasterySkill.x = 0;
            passiveMasterySkill.y = rowIndex;

            // Add the passive skill counter specifically to the Mastery tree
            skills.Add(passiveMasterySkill);
        }

        private int GetSkillMinLevel(XmlNode skillMinLevelNode, int xPos)
        {
            int minLevel = -1;

            if (!(skillMinLevelNode is null))
            {
				// A min level value exists in the data so use it
                minLevel = Convert.ToInt32(skillMinLevelNode.InnerXml);
            }
            else
            {
                // No min level requirement is assigned, which is the case for Skills on the Class Trees, so calculate it based on its X position
                //   Rule: Column 1 min level req is 0, Column 2 min level req is 4, then it's increments of 5 from there on over
                // NOTE: Have to offset xPos by 2 because in the TreeTableLayoutPanel the first selectable skill is actually in column index 2
                minLevel = Convert.ToInt32(minLevel + ((xPos - 2) * 5));
            }

            return minLevel;
        }

        private string GetSkillType(XmlNode skillTypeNode, string treeName, ref Skill skill)
        {
            string skillType = "N/A";

            if (!(skillTypeNode is null))
            {
                skillType = skillTypeNode.InnerXml;
            }
            else if (treeName == "Mastery")
            {
                //The Skill's Type node is null but if we're on the Mastery tree we know how to set Type
                skillType = (skill.max_rank == 1 ? "Perk" : "Passive Skill");
            }

            return skillType;
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

                    UpdateBuildsList(loadedBuild);
                }
                catch (Exception)
                {
                    //The file loaded was xml but did not serialize cleanly into the Build object; explain this to the user and continue on to loading the next file
                    MessageBox.Show("Unable to load Build file '" + Path.GetFileName(buildFile) + "' into the saved Builds list in the Builds tab.  If this is not an actual Build file, please remove it from the Builds folder on disk to stop seeing this message.");
                }

            }
        }

        private void UpdateBuildsList(Build buildToList)
        {
            DataGridViewRow existingRow;
            DataGridViewRow buildRow = null;
            int rowIndex;
            string buildStats = buildToList.characterClass.name + " Lvl" + buildToList.Level + " M" + buildToList.MasteryLevel;

            if (dgvBuilds.RowCount > 0)
            {
                // Look for the Build in the Builds list
                buildRow = dgvBuilds.Rows.Cast<DataGridViewRow>()
                            .Where(r => r.Cells["BuildName"].Value.ToString().Equals(buildToList.name)).FirstOrDefault();
            }

            if (!(buildRow is null))
            {
                // Update the Build in the Builds list since it already exists
                rowIndex = buildRow.Index;

                existingRow = dgvBuilds.Rows[rowIndex];
                existingRow.Cells["Stats"].Value = buildStats;
                existingRow.Selected = true;
            }
            else
            {
                // Add the Build to the Builds list
                buildRow = new DataGridViewRow();
                buildRow.CreateCells(dgvBuilds);
                buildRow.Cells[0].Value = buildToList.name;
                buildRow.Cells[1].Value = buildStats;
                dgvBuilds.Rows.Add(buildRow);
            }
        }

        public void PopulateSkillTrees()
        {
            //Set the filepaths of the json to local variables for referencing easier
            string jsonSample = (ConfigurationManager.AppSettings["SkillDataSample"]);
            string jsonBerserker = (ConfigurationManager.AppSettings["SkillDataBerserker"]);
            string jsonAll = (ConfigurationManager.AppSettings["SkillDataAll"]);
            string jsonAsXml = (string)ResourceManagerData.GetObject("convertjson");

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
            skillData.LoadXml(jsonAsXml);

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

                        // Read the data from the Skill xml into a Skill object
                        //TODOSSG convert the rest (all?) to utilize NodeHasValue with additional conditions as necessary
                        //Map all Skill values into the Object's properties, handling situations of multiple Ranks and when the Node doesn't exist
                        // (using NodeHasValue to handle situations where the node exists but has no value ie. <duration></duration> and if there are additinoal values I need to exclude such as "none")
                        skill.effect = NodeHasValue(skillNode.SelectSingleNode("effect")) ? skillNode.SelectSingleNode("effect").InnerXml.Split(',') : new string[] { }; //TODOSSG convert this to hold just the value and remove the "%"?
                        skill.cooldown = !(skillNode.SelectSingleNode("cooldown") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("cooldown").InnerXml) : -1;
                        skill.duration = NodeHasValue(skillNode.SelectSingleNode("duration")) ? Array.ConvertAll(skillNode.SelectSingleNode("duration").InnerXml.Split(','), double.Parse) : new double[] { };
                        skill.cost100 = !(skillNode.SelectSingleNode("cost100") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("cost100").InnerXml) : -1;
                        skill.skill_requirement = NodeHasValue(skillNode.SelectSingleNode("skill_requirement"), new string[] { "none" }) ? Array.ConvertAll(skillNode.SelectSingleNode("skill_requirement").InnerXml.Trim('[', ']').Split(','), int.Parse) : new int[] { };
                        skill.x = !(skillNode.SelectSingleNode("x") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("x").InnerXml) : -1;
                        skill.damage = !(skillNode.SelectSingleNode("damage") is null) ? skillNode.SelectSingleNode("damage").InnerXml.Split(',') : new string[] { }; //TODOSSG convert this to hold just the value and remove the "%"?
                        skill.range2 = NodeHasValue(skillNode.SelectSingleNode("range2")) ? Array.ConvertAll(skillNode.SelectSingleNode("range2").InnerXml.Split(','), double.Parse) : new double[] { };
                        skill.family = !(skillNode.SelectSingleNode("family") is null) ? skillNode.SelectSingleNode("family").InnerXml : "None";
                        skill.min_level = GetSkillMinLevel(skillNode.SelectSingleNode("min_level"), skill.x);
                        skill.id = !(skillNode.SelectSingleNode("id") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("id").InnerXml) : -1;
                        skill.range = NodeHasValue(skillNode.SelectSingleNode("range")) ? Array.ConvertAll(skillNode.SelectSingleNode("range").InnerXml.Split(','), double.Parse) : new double[] { };
                        skill.element = !(skillNode.SelectSingleNode("element") is null) ? skillNode.SelectSingleNode("element").InnerXml : "N/A";
                        skill.value = NodeHasValue(skillNode.SelectSingleNode("value")) ? Array.ConvertAll(skillNode.SelectSingleNode("value").InnerXml.Split(','), double.Parse) : new double[] { };
                        skill.proc = NodeHasValue(skillNode.SelectSingleNode("proc")) ? Array.ConvertAll(skillNode.SelectSingleNode("proc").InnerXml.Split(','), int.Parse) : new int[] { };
                        skill.description_next = !(skillNode.SelectSingleNode("description_next") is null) ? skillNode.SelectSingleNode("description_next").InnerXml : "";
                        skill.description = !(skillNode.SelectSingleNode("description") is null) ? skillNode.SelectSingleNode("description").InnerXml : "";
                        skill.max_rank = (!(skillNode.SelectSingleNode("max_rank") is null) && (skillNode.SelectSingleNode("max_rank").InnerXml.All(Char.IsDigit))) ? Convert.ToInt32(skillNode.SelectSingleNode("max_rank").InnerXml) : int.MaxValue;  // The data contains "infinite" for skills that don't have a max value, so represent that via int.MaxValue
                        skill.type = GetSkillType(skillNode.SelectSingleNode("type"), tree.name, ref skill); // Type is dependant on max rank for Mastery skills, so need to call a method and do it after we set max rank
                        skill.y = !(skillNode.SelectSingleNode("y") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("y").InnerXml) : -1;
                        skill.cost1 = !(skillNode.SelectSingleNode("cost1") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("cost1").InnerXml) : -1;
                        skill.name = !(skillNode.SelectSingleNode("name") is null) ? skillNode.SelectSingleNode("name").InnerXml : "ERROR: Missing Name";

                        skills.Add(skill);
                    }

                    // For the Mastery Tree, copy Row 3 Skills to also exist in Row 4 and 5
                    if (tree.name == "Mastery")
                    {
                        Skill duplicateSkill;

                        // Get all skills in Row 3 on the Mastery Tree (ie. skill.y=2)
                        List<Skill> skillsToDuplicate = skills.FindAll(s => s.y == 2);

                        // With indices 3 and 4 (for Row 4 and 5), duplicate each skill in Row 3 and change its y value to the new row
                        for (int i = 3; i <= 4; i++)
                        {
                            foreach (Skill skillToDuplicate in skillsToDuplicate)
                            {
                                duplicateSkill = skillToDuplicate.Duplicate();
                                duplicateSkill.y = i;

                                // Add the duplicated Skill to the available Skills list
                                skills.Add(duplicateSkill);
                            }
                        }
                    }

                    if (tree.name == "Mastery")
                    {
                        AddMasteryPassiveRowCounters(classNode.Name, ref skills);
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
            // Don't continue on if there is no build initialized/loaded yet (ie. on initial load of application)
            if (build.characterClass is null)
            {
                return;
            }

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
                    lblTreeCaption.Text = ttlp.passiveSkillName;
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
            DialogResult dialogResult;

            string characterClass = (sender as ComboBox).SelectedItem.ToString();

            // See if we have a Build loaded or not (it's possible this was executed on initial load of the aplication and there is no Build loaded yet
            if (!(build.characterClass is null))
            {
                //Prompt user ensuring they want to reset their character
                dialogResult = MessageBox.Show("Changing Class will reset this character.  Continue?", "Change Class", MessageBoxButtons.YesNo);
            }
            else
            {
                // No Build is loaded, so set the dialogResult to Yes to allow the new Build to be created
                dialogResult = DialogResult.Yes;
            }

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
                build.name = "<Unnamed>";

                InitializeBuild(build);

                // Show the Trees, incase a different view (e.g. Inventory, Builds, etc) was being shown
                pnlTrees.BringToFront();
            }
            else
            {
                //TODO set cboClass back to the previously selected characterClass
                // Suppress change events
                //cboClass.SelectedIndexChanged -= CboClass_SelectedIndexChanged;

                //cboClass.SelectedIndex = cboClass.Items.IndexOf(build.characterClass.name);

                //// Unsuppress change events
                //cboClass.SelectedIndexChanged += CboClass_SelectedIndexChanged;
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
            lblLevel.Text = build.Level.ToString();

            //Update Mastery level
            lblMastery.Text = build.MasteryLevel.ToString();

            //Update # of Skill Points Remaining
            lblSkillPointsRemaining.Text = (SKILL_POINTS_MAX - build.Level).ToString();

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
            MultiSkillSelectButton multiSkillSelectButton;
            SkillSelectButton skillSelectButton;

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
                        if (ttlpTree.Name == "Mastery")
                        {
                            build.MasteryLevel -= skillButton.skill.level;
                        }
                        else
                        {
                            build.Level -= skillButton.skill.level;
                        }
                    }

                    //Lastly, reset the level of the skill
                    skillButton.skill.level = 0;

                    //GARBAGE COLLECTION
                    // The Tooltip panel is not added as a child of the Skill button, so it needs to be manually cleaned up
                    skillButton.skillTooltipPanel.Dispose();
					// The SkillSelect panel needs to be manually removed if one was imposed on a SkillButton (ie. a Skill was selected from a MultiSkillSelect button)
                    if (!(skillButton.skillSelectPanel is null))
                    {
                        skillButton.skillSelectPanel.Dispose();
                    }
					//END GARBAGE COLLECTION
                }
                else if (control is MultiSkillSelectButton)
                {
                    multiSkillSelectButton = (control as MultiSkillSelectButton);

                    //GARBAGE COLLECTION
					//  Loop through each SkillSelect button contained within the SkillSelect panel to remove its controls
                    while (multiSkillSelectButton.skillSelectPanel.Controls.Count > 0)
                    {
                        skillSelectButton = (multiSkillSelectButton.skillSelectPanel.Controls[0] as SkillSelectButton);
                        if (!(skillSelectButton is null) && !(skillSelectButton.skillTooltipPanel is null))
                        {
                            // The Tooltip panel is not added as a child to the MultiSkill button, so it needs to be manually cleaned up
                            skillSelectButton.skillTooltipPanel.Dispose();
                        }
                        // This doesn't actually free USER_OBJECTS but is necessary for the sake of the While loop
                        multiSkillSelectButton.skillSelectPanel.Controls[0].Dispose();
                    }

                    multiSkillSelectButton.skillSelectPanel.Dispose();
					//END GARBAGE COLLECTION
                }
            }

			//GARBAGE COLLECTION
			// Instead of calling ttlpTree.Controls.Clear(), loop through all controls (ie. Skill buttons) to dispose of them individually
            DisposeChildControlsOf(ttlpTree);

            //Reset the total number of skill points allocated on the Tree control
            ttlpTree.skillPointsAllocated = 0;

            //Update Stats here because we may have cleared a bunch of active/passive skills
            UpdateStats(build);
        }

        /// <summary>
        /// Dispose a control and all its children
        /// </summary>
        public void DisposeControl(Control c)
        {
            if (null != c)
                using (c)
                    DisposeChildControlsOf(c);
        }

        /// <summary>
        /// Dispose (and remove) all the children of a control
        /// </summary>
        public void DisposeChildControlsOf(Control c)
        {
            if (null != c)
            {
                if (null != c.Controls)
                {
                    while (c.Controls.Count > 0)
                    {
                        Control child = c.Controls[0];
                        c.Controls.RemoveAt(0);
                        DisposeControl(child);
                    }
                }
            }
        }

        /// <summary>
        /// Determine the correct prefix to use for the button image's filename, depending on if this is for the Mastery tree or a Class tree
        ///   as they're named slightly different
        /// </summary>
        public static string GetSkillButtonIconFilename(string treeName, int skillId)
        {
            const string IMAGE_FILENAME_PREFIX_MASTERY = "spr_masteryicons_";
            const string IMAGE_FILENAME_PREFIX_SKILL = "spr_spellicons_";

            //NOTE: if the following is too process-intensive, an alternate to this shared method would be to either adjust all "100"-padded skill ids in the skill data OR rename the mastery image files to contain the "100" padding
            if (treeName == "Mastery")
            {
                // Combine the mastery filenames prefix with the id of the skill excluding the padding in the skill.id that's used to make Mastery skill ids unique from Class skill ids
                //   This needs to be done because the filenames are named with the padding, because instead what makes Mastery Skill files unique from Class Skill files is the text "mastery" vs. "spell"
                //  (it's an inconsistency in the data that we just have to account for, so work around it)
                return IMAGE_FILENAME_PREFIX_MASTERY + (skillId - 100000).ToString();
            }
            else
            {
                return IMAGE_FILENAME_PREFIX_SKILL + skillId.ToString();
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

            List<Skill> MultiSelectionSkills = new List<Skill>();  //TODO could rename to LoadedMultiSelectionSkills/isLoadedMultiSelectionSkill for readability and consistency with isImportedMultiSelectionSkill
            Dictionary<int, SkillSelectPanel> importedSkillsAndTheirSkillSelectPanels = new Dictionary<int, SkillSelectPanel>();

            //Give the current Tree object to the Tree control that will be showing it as it'll be needed for future reference
            ttlpTree.tree = tree;

            //Load any skill slot that begins with a "+" for the user to pick between multiple skills
            LoadMultiSelectionSkills(tree, ttlpTree, ref MultiSelectionSkills, ref importedSkillsAndTheirSkillSelectPanels);

            //Loop through each Skill and place it into the correct slot in the TableLayoutPanel
            foreach (Skill skill in tree.skills)
            {
                // Check if the current skill is derived from a MultiSkill selection and should have a SkillSelectPanel appended to it
                bool isImportedMultiSelectionSkill = importedSkillsAndTheirSkillSelectPanels.ContainsKey(skill.id);
                // Check if the current skill hasn't already been loaded by the LoadMultiSelectionSkills process
                bool isMultiSelectionSkill = MultiSelectionSkills.Contains(skill);

                //Load all remaining skill slots, ensuring it's not a Mutli-skill slot that was already loaded
                if (!(isMultiSelectionSkill) || isImportedMultiSelectionSkill)
                {
                    SkillButton btnSkill;
                    SkillSelectPanel importedSkillSelectPanel;
                    SkillTooltipPanel pnlSkillTooltip;

                    pnlSkillTooltip = CreateSkillTooltip(skill, ttlpTree);
                    //Create a new control to hold this skill at the skills X and Y location
                    btnSkill = new SkillButton(skill, ttlpTree, pnlSkillTooltip, this);

                    //Specify the passive bonus skill button as being such (when it's found to be the Tree name as the Skill name OR it's one of the Tree names in the Mastery Tree), we'll need this info for other situations
                    if ((skill.name == ttlpTree.passiveSkillName) || (tree.name == "Mastery" && skill.name.Contains(tree.name))) //TODO This'll fail if the Mastery Tree ever gets a selectable skill with the name "Mastery" in it; a fooler-proof solution may be to hard-code the skillId's into a list and check if skill.id IN MasteryPassiveSkillIds
                    {
                        btnSkill.isPassiveBonusButton = true;
                    }

                    //Add the skill button to the tree
                    ttlpTree.Controls.Add(btnSkill, skill.x, skill.y);

                    // Give the new SkillButton a SkillSelectPanel if it's a MultiSkill that has had a skill selected by the user
                    if (importedSkillsAndTheirSkillSelectPanels.TryGetValue(skill.id, out importedSkillSelectPanel))
                    {
                        // Assign the SkillSelectPanel to the current Skill button
                        btnSkill.skillSelectPanel = importedSkillSelectPanel;

                        // Set the correct location for the SkillSelectPanel to appear if opened, now that the SkillButton has been added to the Tree
                        btnSkill.skillSelectPanel.SetLocation(btnSkill, btnSkill.skill.x, btnSkill.skill.y);
                    }                    

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
        private void LoadMultiSelectionSkills(Tree tree, TreeTableLayoutPanel tlpTree, ref List<Skill> MultiSelectionSkills, ref Dictionary<int, SkillSelectPanel> importedSkillsAndTheirSkillSelectPanels)
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
                //  TODO I can check if they leveled it but if they picked 1 and didn't level it it'll go back to a "+" button. Do I care? Prob not.
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
                if (MultipleSkills.Count > 1)
                {
                    //Instantiate a new SkillSelect Panel to hold the multiple skills
                    SkillSelectPanel pnlSkillSelect = new SkillSelectPanel(tlpTree);

                    //Set its Parent control to actually be the Panel that holds all Trees and not the TableLayoutPanel for the current Tree 
                    //  so it can display OVER the SkillSelectButton (without setting this, it's trying to display in the same cell in the tlpTree)
                    pnlSkillSelect.Parent = pnlTrees;

                    //Set the width of the SkillSelect Panel to the number of skill buttons it will contain (30 = width of SkillSelectButton)
                    pnlSkillSelect.Width = ((MultipleSkills.Count + 1) * 30) + ((MultipleSkills.Count + 1) * SKILL_BUTTON_PADDING - ((MultipleSkills.Count + 1) * SKILL_BUTTON_PADDING / 2)); //last # is accounting for 3px margin on each side of each button

                    //Fill up the SkillSelectPanel with all available skill options
                    // Create a button for each skill and place it in the SkillSelect Panel
                    foreach (Skill multiSkill in MultipleSkills)
                    {
                        SkillTooltipPanel pnlSkillTooltip = CreateSkillTooltip(multiSkill, tlpTree);
                        btnSkillSelect = new SkillSelectButton(this, pnlSkillTooltip);
                        btnSkillSelect.skill = multiSkill;
                        btnSkillSelect.treeControl = tlpTree;

                        if (!((Image)ResourceManagerImageSkill.GetObject(GetSkillButtonIconFilename(tree.name, multiSkill.id)) is null))
                        {
                            btnSkillSelect.BackgroundImage = (Image)ResourceManagerImageSkill.GetObject(GetSkillButtonIconFilename(tree.name, multiSkill.id));
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
                    pnlSkillSelect.Controls.Add(new UnassignSkillButton(pnlSkillSelect.Controls.Count, SKILL_BUTTON_PADDING, xPos, yPos));

                    if (!alreadyChoseASkill)  //TODO should flip these to make it more readable
                    {
                        //Add a SkillSelect Button ("+" button) to the shared X,Y position on the tree
                        MultiSkillSelectButton btnMultiSkillSelect = new MultiSkillSelectButton(xPos, yPos, tree.name, MultipleSkills[0].max_rank);
                        tlpTree.Controls.Add(btnMultiSkillSelect, xPos, yPos);

                        //Tie the panel to the SkillSelect Button that holds the position for which this panel and its skills apply to
                        btnMultiSkillSelect.skillSelectPanel = pnlSkillSelect;
                        // Set the correct location for the SkillSelectPanel to appear if opened, now that the MultiSkill button has been added to the Tree
                        btnMultiSkillSelect.skillSelectPanel.SetLocation(btnMultiSkillSelect, btnMultiSkillSelect.xPos, btnMultiSkillSelect.yPos);
                    }
                    else
                    {
                        // Add the selected Skill and its SkillSelectPanel to a list for use after we've loaded all skills into the Tree so we can then assign the remaining SkillSelectPanels
                        //   (the SkillSelectPanel cannot be added to the selected Skill because the Skill hasn't been created as a SkillButton and loaded onto the tree yet
                        importedSkillsAndTheirSkillSelectPanels.Add(skill.id, pnlSkillSelect);
                    }
                }
            }
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
                SkillSelectPanel skillSelectPanel = null;

                //There may be a SkillSelectPanel assigned to this current skill on the tree so grab it
                switch (skill)
                {
                    case MultiSkillSelectButton m:
                        skillSelectPanel = m.skillSelectPanel;
                        break;
                    case SkillButton s:
                        skillSelectPanel = s.skillSelectPanel;
                        break;
                    default:
                        break;
                }

                //Populate descriptions of all skills in the SkillSelectPanel if one was found at this tree location
                if (!(skillSelectPanel is null))
                {
                    //Populate description for each skill option
                    foreach (SkillSelectButton singleSkill in skillSelectPanel.Controls.OfType<SkillSelectButton>())
                    {
                        singleSkill.skillTooltipPanel.PopulateDescription();
                    }
                }

                //Populate description of the skill if this skill is directly on the tree (ie. not a MultiSkillSelect button)
                if (skill is SkillButton)
                {
                    (skill as SkillButton).skillTooltipPanel.PopulateDescription();
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
            TreeTableLayoutPanel ttlpTree = null;

            // Ensure the user wants to reset this tree
            dialogResult = MessageBox.Show("Are you sure you want to reset this Tree?", "Reset Tree", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                treeStatus = TreeStatus.Resetting;

                // Find the currently-shown tree
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
                if (!(ttlpTree is null) && ttlpTree.HasChildren)
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
                    UpdateBuildsList(build);
                }
                else
                {
                    // The user changed their mind and doesn't want to overwrite the current Build so allow them to save it as a new one
                    BtnNavSaveAs_Click(sender, e);
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
            if (build.characterClass is null)
            {
                MessageBox.Show("No Build has been created or loaded.  Please start a Build before attempting to save it.");
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.InitialDirectory = BuildsDirectory;
            saveFileDialog.RestoreDirectory = true;

            // Save the build as a new build
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                if (!(File.Exists(saveFileDialog.FileName)))
                {
                    // Update the Build name if this is a new Build being saved (ie. not a file being overwritten)
                    build.name = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                    lblBuildName.Text = build.name;
                }

                SaveBuild(saveFileDialog.FileName);
                UpdateBuildsList(build);

                //TODO Set the SaveAs'd Build as selected in the Builds list
            }
        }

        private void SaveBuild(string fileNameAndPath)
        {
            XmlSerializer serializer;

            // Set a few last-minute properties
            build.lastSaved = DateTime.Now;
            build.buildStatus = Build.BuildStatus.Saved;

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
            
            //TODO clear treeStatus?
        }

        public void OpenBuild(string buildContent, bool pasteBinImport = false)
        {
            XmlSerializer serializer;

			// Set the Build Status so we don't trigger certain events (e.g. Build.SetAsModified())
            build.buildStatus = Build.BuildStatus.Opening;

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

			// Clear the Build Status
            build.buildStatus = Build.BuildStatus.None;
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

            // Load the mastery tree regardless of Class
            treeName = "Mastery";
            LoadTreeIconButtonImage(ResourceManagerImageTree, btnTreeMastery, treeName);
            btnTreeMastery.Tag = treeName;
            treePanels[4].Name = treeName;
            //treePanels[4].passiveSkillId = 93; //TODO test that it's OK we're not setting a passiveSkillId/Name on the Mastery tree (or is there 1 we can/should set that's used in game but isn't in the skill data?)
            //treePanels[4].passiveSkillName = treeName;
            treePanels[4].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

            //Load all trees for the selected class into their corresponding tree table layout panels
            LoadTrees(build.characterClass);

            //Default focus to the first Tree
            ShowTree(treeButtons.First(), new EventArgs());
        }

        private void btnBuildSharing_Click(object sender, EventArgs e)
        {
            // Show the PasteBin form as a child of the Main Form so the Main Form cannot be interacted with while this is open
            buildShareForm.StartPosition = FormStartPosition.CenterParent;
            buildShareForm.ShowDialog(this);
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
			//Prompt for save/if user really wants to save the build before closing the application //TODO fix this, application still exists on selecting "Cancel"
            //if (SaveBuildShouldContinue())
            //{
            //    Application.Exit();
            //}
        }

        public bool SaveBuildShouldContinue()
        {
            bool shouldContinue = false;
            DialogResult dialogResult;
            string fileNameAndPath;

            fileNameAndPath = BuildsDirectory + "\\" + build.name + XML_EXT;

            // Check if the Build is modified or there isn't yet a save file for it and it needs to be Saved before continuing
            if (!File.Exists(fileNameAndPath) && !(build.characterClass is null) || (build.IsModified()))
            {
                // Ensure the user wants to overwrite this build
                dialogResult = MessageBox.Show("The current Build is not saved.  Would you like to save it before continuing?", "Save Build", MessageBoxButtons.YesNoCancel);

                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        BtnNavSave_Click(this, EventArgs.Empty);
                        shouldContinue = true;
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
