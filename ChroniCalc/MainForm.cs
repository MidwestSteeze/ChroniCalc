using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading;
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
        private string ExportsDirectory;
        private string TempDirectory;
        private TreeStatus treeStatus;

        //Resource Managers for pulling assets (ie. data, images, etc.) which is a reflection of the Assets directory
        ResourceManager ResourceManagerData;
        ResourceManager ResourceManagerImageClass;
        ResourceManager ResourceManagerImageSkill;
        ResourceManager ResourceManagerImageTree;

        //Lists and Objects
        public Build build;
        public BuildShareForm buildShareForm;
        public CultureInfo CultureSystem;
        public CultureInfo CultureEnglish;

        Dictionary<int, string> masterySlotIDs;
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

				// Create variables to hold the original System Locale setting and also one specifically defined as English
                CultureSystem = Thread.CurrentThread.CurrentCulture;
                CultureEnglish = new CultureInfo("en-US", false);

                // Add the version to the form's Title
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    this.Text = string.Format(this.Text + " - v{0}",
                        ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(4));
                }

                // Create a new BuildShare form that will display when the Build Sharing button is clicked
                buildShareForm = new BuildShareForm();
                buildShareForm.ParentForm = this;

                //Set the directories for Builds, Exports, and Temp Files
                BuildsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ChroniCalc\\Builds";
                ExportsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\ChroniCalc\\Builds\\Exports";
                TempDirectory = Path.GetTempPath() + "\\ChroniCalc";

                //Create the directory where saved builds are to be stored, if it doesn't yet exist
                if (!Directory.Exists(BuildsDirectory))
                {
                    Directory.CreateDirectory(BuildsDirectory);
                }

                //Create the directory where exported builds are to be stored, if it doesn't yet exist
                if (!Directory.Exists(ExportsDirectory))
                {
                    Directory.CreateDirectory(ExportsDirectory);
                }

                //Create the directory where temp files will be generated to, if it doesn't yet exist
                if (!Directory.Exists(TempDirectory))
                {
                    Directory.CreateDirectory(TempDirectory);
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
                masterySlotIDs = new Dictionary<int, string>();

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
                        ttlp = new TreeTableLayoutPanel(pnlTrees, 11, 7, 506, 273);
                        ttlp.Name = "Mastery";
                    }
                    else
                    {
                        //Create a new Class Skill tree
                        ttlp = new TreeTableLayoutPanel(pnlTrees, 10, 7, 470, 273);
                    }

                    treePanels.Add(ttlp);
                }

                // Build a list of Mastery Tree X,Y Positions and their corresponding Slot IDs for future reference throughout the application
                PopulateMasterySlotIDs();
                // Load the Tree and Skill data from the game into their corresponding objects
                PopulateSkillTrees();
                // Populate the list of available Builds that have been created in ChroniCalc
                LoadBuildsIntoBuildsList();

                // Show the Builds list at start
                pnlBuilds.BringToFront();
            }
            catch (Exception ex)
            {
                // An exception occurred on load of the application that was not handled immediately; display exception details and terminate the application
                throw new EChroniCalcException("Error on initial load of application.  Unable to continue." + Environment.NewLine + ex.ToString());
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
                    AddMasteryPassiveRowCounter("Guardian Mastery", Constants.SkillIDs.MASTERY_BERSERKER_GUARDIAN, 0, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Guardian"), ref skills);
                    AddMasteryPassiveRowCounter("Sky Lord Mastery", Constants.SkillIDs.MASTERY_BERSERKER_SKY_LORD, 1, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Sky Lord"), ref skills);
                    AddMasteryPassiveRowCounter("Dragonkin Mastery", Constants.SkillIDs.MASTERY_BERSERKER_DRAGONKIN, 5, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Dragonkin"), ref skills);
                    AddMasteryPassiveRowCounter("Frostborn Mastery", Constants.SkillIDs.MASTERY_BERSERKER_FROSTBORN, 6, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Frostborn"), ref skills);
                    break;

                case "Templar":
                    AddMasteryPassiveRowCounter("Vengeance Mastery", Constants.SkillIDs.MASTERY_TEMPLAR_VENGEANCE, 0, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Vengeance"), ref skills);
                    AddMasteryPassiveRowCounter("Wrath Mastery", Constants.SkillIDs.MASTERY_TEMPLAR_WRATH, 1, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Wrath"), ref skills);
                    AddMasteryPassiveRowCounter("Conviction Mastery", Constants.SkillIDs.MASTERY_TEMPLAR_CONVICTION, 5, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Conviction"), ref skills);
                    AddMasteryPassiveRowCounter("Redemption Mastery", Constants.SkillIDs.MASTERY_TEMPLAR_REDEMPTION, 6, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Redemption"), ref skills);
                    break;

                case "Warden":
                    AddMasteryPassiveRowCounter("Wind Ranger Mastery", Constants.SkillIDs.MASTERY_WARDEN_WIND_RANGER, 0, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Wind Ranger"), ref skills);
                    AddMasteryPassiveRowCounter("Druid Mastery", Constants.SkillIDs.MASTERY_WARDEN_DRUID, 1, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Druid"), ref skills);
                    AddMasteryPassiveRowCounter("Storm Caller Mastery", Constants.SkillIDs.MASTERY_WARDEN_STORM_CALLER, 5, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Storm Caller"), ref skills);
                    AddMasteryPassiveRowCounter("Winter Herald Mastery", Constants.SkillIDs.MASTERY_WARDEN_WINTER_HERALD, 6, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Winter Herald"), ref skills);
                    break;

                case "Warlock":
                    AddMasteryPassiveRowCounter("Corruptor Mastery", Constants.SkillIDs.MASTERY_WARLOCK_CORRUPTOR, 0, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Corruptor"), ref skills);
                    AddMasteryPassiveRowCounter("Lich Mastery", Constants.SkillIDs.MASTERY_WARLOCK_LICH, 1, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Lich"), ref skills);
                    AddMasteryPassiveRowCounter("Demonologist Mastery", Constants.SkillIDs.MASTERY_WARLOCK_DEMONOLOGIST, 5, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Demonologist"), ref skills);
                    AddMasteryPassiveRowCounter("Reaper Mastery", Constants.SkillIDs.MASTERY_WARLOCK_REAPER, 6, DESCRIPTION_CLASS_ROW.Replace(PLACEHOLDER_TREE, "Reaper"), ref skills);
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

        private void AddMasterySharedClassSpecificAffinities(ref List<Skill> skills)
        {
            Skill duplicateSkill;
            int[] rowIndices = { 1, 5, 6 };
            int insertionIndex;

            // Get the shared Affinity Skills (e.g. Ultimate, Heritage, Aura, etc.) in the final column of Row 1 on the Mastery Tree (ie. skill.y=0 && skill.x=10)
            List<Skill> skillsToDuplicate = skills.FindAll(s => s.y == 0 && s.x == 10 && s.element == "Ethereal"); //TODOSSG this will break if a class-specific final Affinity is ever added of element "Ethereal";  if so, you'll need to implement this similar to the procedure below AddMasterySharedClassSpecificGenericSkills (not sure which solution idea is best in this case...)

            // With indices 1, 5, and 6 (for Rows 2, 6, and 7), duplicate each of the shared Affinity Skills in Row 1 and change its y value to the new Row
            for (int i = 0; i <= rowIndices.Length - 1; i++)
            {
                // Set the index to insert the duplicated Skill for the new Row so that the Skills have the same order as the first Row
                insertionIndex = 0;

                foreach (Skill skillToDuplicate in skillsToDuplicate)
                {
                    duplicateSkill = skillToDuplicate.Duplicate();
                    duplicateSkill.y = rowIndices[i];

                    // Add the duplicated Skill into the available Skills list
                    skills.Insert(insertionIndex, duplicateSkill);
                    insertionIndex++;
                }
            }
        }

        private void AddMasterySharedClassSpecificGenericSkills(ref List<Skill> skills, string className)
        {
            Skill duplicateSkill;
            int[] rowIndices = { 1, 5, 6 };
            int insertionIndex;
            List<Skill> skillsToDuplicate;

            skillsToDuplicate = new List<Skill>();

            // Insert Shared Generic Skills, found in the Column 1 and 8 of the first Class-specific Row, into the other Class-specific Rows
            //  (there's no good way to identify the Shared Generic Skills that should be in these columns of each Class-specific row, so we have to hard-code it unfortunately;
            //   this is because the data from the game only contains 1 record for each Shared Generic Skill and places them in the first Class-specific row)
            try
            {
                switch (className)
                {
                    case "Berserker":
                        // Column 1
                        //   Thorns
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100221));
                        //   Health Regeneration
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100220));

                        // Column 8
                        //   Wide Synergy
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100295));
                        break;
                    case "Templar":
                        // Column 1
                        //   Thorns
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100221));
                        //   Companion Health
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100028));
                        //   Companion Damage
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100027));

                        // Column 8
                        //   Wide Synergy
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100295));
                        break;
                    case "Warden":
                        // Column 1
                        //   Companion Health
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100028));
                        //   Companion Damage
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100027));

                        // Column 8
                        //   Wide Synergy
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100295));
                        break;
                    case "Warlock":
                        // Column 1
                        //   Companion Health
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100028));
                        //   Companion Damage
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100027));
                        
                        // Column 8
                        //   Wide Synergy
                        skillsToDuplicate.Add(skills.Find(s => s.id == 100295));
                        break;
                    default:
                        throw new Exception("AddMasterySharedClassSpecificGenericSkills(): Class " + className + " not found.");
                }
            }
            catch (ArgumentNullException ex)
            {
                Alerts.DisplayError("AddMasterySharedClassSpecificGenericSkills: Class " + className + " has had its Generic Skills, found in each Class-specific row's first column, changed since the last update of Chronicon.  The list needs to be revised.  Unable to continue." + Environment.NewLine + ex.ToString());
                throw;
            }

            // With indices 1, 5, and 6 (for Rows 2, 6, and 7), duplicate each of the shared Generic Skills in Row 1 and change its y value to the new Row
            for (int i = 0; i <= rowIndices.Length - 1; i++)
            {
                // Set the index to insert the duplicated Skill for the new Row so that the Skills have the same order as the first Row
                insertionIndex = 0;

                foreach (Skill skillToDuplicate in skillsToDuplicate)
                {
                    duplicateSkill = skillToDuplicate.Duplicate();
                    duplicateSkill.y = rowIndices[i];

                    // Add the duplicated Skill into the available Skills list
                    skills.Insert(insertionIndex, duplicateSkill);
                    insertionIndex++;
                }
            }
        }

        private void AddMasterySharedGenericRows(ref List<Skill> skills)
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

        private void AddMasterySlotIDs(ref List<Skill> skills)
        {
            string position;
            int slotID;
            int x;
            int y;
            List<Skill> skillsInPosition;

            //Loop through each node within all the nodes
            foreach (var entry in masterySlotIDs)
            {
                // Seperate the data in the current node to get the x,y position and the Skill ID
                position = entry.Value;
                slotID = entry.Key;

                // Extract the x,y from the position
                x = Convert.ToInt32(position.Substring(0, position.IndexOf(',')));
                y = Convert.ToInt32(position.Substring(position.IndexOf(',') + 1));

                // Find the corresponding Skill in the Mastery tree based on the current x,y positions being iterated over
                skillsInPosition = skills.FindAll(s => s.x == x && s.y == y);

                foreach (Skill skill in skillsInPosition)
                {
                    // Assign the Slot ID
                    skill.slotID = slotID;
                }
            }

            // Now that we've assigned SlotIDs to all the Skills, double-check if there are any leftover that didn't get a Slot ID
            //  (but exclude Skills in the first Row as they are passive row counters)
            if (skills.FindAll(s => s.slotID == 0 && s.x != 0).Count > 0)
            {
                // TODO throw exception that the skill wasn't found in the tree
                // NOTE 1: (this would mean you have outdated slot_id.xml and the udpate process is pretty crappy since this data isn't embedded directly into the Skill Data export; perhaps run this by squarebit)
            }
        }

        private void ChangeLocale(bool setAsEnglish)
        {
            // Change the Locale as requested
            if (setAsEnglish)
            {
                Thread.CurrentThread.CurrentCulture = CultureEnglish;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureSystem;
            }
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
                catch (Exception ex)
                {
                    //The file loaded was xml but did not serialize cleanly into the Build object; explain this to the user and continue on to loading the next file
                    Alerts.DisplayWarning("Unable to load Build file '" + Path.GetFileName(buildFile) + "' into the saved Builds list in the Builds tab.  If this is not an actual Build file, please remove it from the Builds folder on disk to stop seeing this message.");
                }

            }
        }

        /// <summary>
        /// Opening a saved build requires the saved Level of the Build be merged into the global Build object used by this application
        ///     NOTE: The reason for this is to allow/force Builds to ALWAYS use the most recent SKill Data extracted from the game, which is loaded on open of ChroniCalc
        /// </summary>
        private Build MergeImportedBuildIntoBuild(Build importedBuild)
        {
            Build mergedBuild = new Build();
            Tree currentTree;
            Skill currentSkill;

            // Set the imported Build data onto a Build object that will be sent back
            mergedBuild.characterClass = characterClasses.Find(c => c.name == importedBuild.characterClass.name);
            mergedBuild.lastModified = importedBuild.lastModified;
            mergedBuild.lastSaved = importedBuild.lastSaved;
            mergedBuild.Level = importedBuild.Level;
            mergedBuild.MasteryLevel = importedBuild.MasteryLevel;
            mergedBuild.name = importedBuild.name;

            // Loop through each imported Tree
            foreach (Tree importedTree in importedBuild.characterClass.trees)
            {
                // Hold onto the current Tree being merged from the imported Build for easier reference when we're iterating through all Skills within it, below
                currentTree = mergedBuild.characterClass.trees.Find(t => t.name == importedTree.name);
                // Set the necessary imported Tree data
                currentTree.level = importedTree.level;

                // Loop through each imported Skill within each imported Tree
                foreach (Skill importedSkill in importedTree.skills)
                {
                    currentSkill = mergedBuild.characterClass.trees.Find(t => t.name == importedTree.name).skills.Find(s => s.id == importedSkill.id && s.x == importedSkill.x && s.y == importedSkill.y);

                    // Set the necessary imported Skill data
                    currentSkill.level = importedSkill.level; //TODO wrap this in a try/catch, if it fails because a Skill ID was not found then it needs to be handled in BuildConvert.ConvertBuild()
                }
            }

            return mergedBuild;
        }

        private DialogResult PromptForBuildReset()
        {
            DialogResult dialogResult;

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

            return dialogResult;
        }

        /// <summary>
        /// This is an incredibly in-depth way of making sure the 3 Rows in the Mastery Tree show the
		/// 	Points Required label, and the correct Text, if points spent in each of the 3 Rows do not meet the minimum
		/// 	required to assign points into the last Skill that is linked to the end of each of these 3 Rows
		/// 	(It's not super necessary but is very helpful for those not familiar enough with the game, I guess)
        /// </summary>
        public void SetMasteryGeneralRowsPointsRequiredVisibility(TreeTableLayoutPanel ttlpTree)
        {
            const int MIN_LEVEL = 65;

            bool visible = false;
            int width;
            string pointsRequiredLabelText = "Requires Rank " + MIN_LEVEL.ToString() + " in ";

            Label pointsRequiredLabel;
            List<string> rowNamesNeedingMorePoints = new List<string>();
            Control control;
            SkillButton passiveBonusButton;

            for (int row = 2; row <= 4; row++)
            {
                passiveBonusButton = (SkillButton)ttlpTree.GetControlFromPosition(0, row);

                if (passiveBonusButton.skill.level < MIN_LEVEL)
                {
                    // Set visibility of the PointsRequired label
                    visible = true;

                    rowNamesNeedingMorePoints.Add(passiveBonusButton.skill.name);
                }
            }

            // Build up the text that we'll be placing on the PointsRequired label, incase many Rows don't meet the min_level
            for (int i=0; i <= rowNamesNeedingMorePoints.Count - 1; i++)
            {
                pointsRequiredLabelText += rowNamesNeedingMorePoints[i];

                // Add a comma seperator if there are more Row Names to add to this label text, but it's not the last one in the list
                if (i < (rowNamesNeedingMorePoints.Count - 1))
                {
                    pointsRequiredLabelText += ", ";
                }
            }

            // Get the control that Rows 2-4 are linked to at the end that we'll be adjusting
            control = ttlpTree.GetControlFromPosition(ttlpTree.ColumnCount - 1, 3);

            // Set the visibility and text of the PointsRequired label based on the control type
            if (control is MultiSkillSelectButton)
            {
                // Adjust visibility and the Points Required text on each of the SkillSelectButtons contained within
                foreach (SkillSelectButton skillSelectButton in (control as MultiSkillSelectButton).skillSelectPanel.Controls.OfType<SkillSelectButton>())
                {
                    pointsRequiredLabel = (Label)skillSelectButton.skillTooltipPanel.Controls.Find("lblPointsRequired", true).First();
                    pointsRequiredLabel.Visible = visible;
                    pointsRequiredLabel.Text = pointsRequiredLabelText;

                    //Adjust the width of the Tooltip based on if the lblPointsRequired text is running off the right edge, otherwise use the min width defined
                    width = ((pointsRequiredLabel.Width + pointsRequiredLabel.Left) > skillSelectButton.skillTooltipPanel.DEFAULT_WIDTH) ? (pointsRequiredLabel.Width + pointsRequiredLabel.Left) : skillSelectButton.skillTooltipPanel.DEFAULT_WIDTH;
                    skillSelectButton.skillTooltipPanel.Controls.Find("pnlTooltip", true).First().Width = width;
                    skillSelectButton.skillTooltipPanel.Width = width;
                }
            }
            else if (control is SkillButton)
            {
                // Adjust visibility and the Points Required text on the SkillButton
                pointsRequiredLabel = (Label)(control as SkillButton).skillTooltipPanel.Controls.Find("lblPointsRequired", true).First();
                pointsRequiredLabel.Visible = visible;
                pointsRequiredLabel.Text = pointsRequiredLabelText;

                //Adjust the width of the Tooltip based on if the lblPointsRequired text is running off the right edge, otherwise use the min width defined
                width = ((pointsRequiredLabel.Width + pointsRequiredLabel.Left) > (control as SkillButton).skillTooltipPanel.DEFAULT_WIDTH) ? (pointsRequiredLabel.Width + pointsRequiredLabel.Left) : (control as SkillButton).skillTooltipPanel.DEFAULT_WIDTH;
                (control as SkillButton).skillTooltipPanel.Controls.Find("pnlTooltip", true).First().Width = width;
                (control as SkillButton).skillTooltipPanel.Width = width;

                // See if the Skill Button has a SkillSelectPanel
                if (!((control as SkillButton).skillSelectPanel is null))
                {
                    // Adjust visibility and the Points Required text on each of the SkillSelectButtons contained within
                    foreach (SkillSelectButton skillSelectButton in (control as SkillButton).skillSelectPanel.Controls.OfType<SkillSelectButton>())
                    {
                        pointsRequiredLabel = (Label)skillSelectButton.skillTooltipPanel.Controls.Find("lblPointsRequired", true).First();
                        pointsRequiredLabel.Visible = visible;
                        pointsRequiredLabel.Text = pointsRequiredLabelText;

                        //Adjust the width of the Tooltip based on if the lblPointsRequired text is running off the right edge, otherwise use the min width defined
                        skillSelectButton.skillTooltipPanel.Controls.Find("pnlTooltip", true).First().Width = width;
                        skillSelectButton.skillTooltipPanel.Width = width;
                    }
                }
            }
        }

        public void SetPointsRequiredVisibilities(TreeTableLayoutPanel ttlpTree, Skill skill, int totalPointsAllocated)
        {
            if (ttlpTree.Name == "Mastery")
            {
                Control control;

                // Adjust all Skills in the Mastery Tree's Row, not just the one we're on (NOTE: the hard-coded 2 is the first column where a Skill Button can be found)
                for (int x = 2; x <= ttlpTree.ColumnCount - 1; x++)
                {
                    // Don't perform this functionality if we're on the last Skill that's linked to General Rows 3, 4, and 5; it's being done outside of this loop
                    if (skill.y == 3 && x == (ttlpTree.ColumnCount - 1))
                    {
                        continue;
                    }

                    // Get the control at this X position, but within the same Row (ie. this.skill.y)
                    control = ttlpTree.GetControlFromPosition(x, skill.y);

                    SetPointsRequiredVisibility(control, totalPointsAllocated);
                }

                // Adjust the Class Mastery final Skill that's linked to General Rows 3, 4 and 5 and requires they all meet the min_level requirement of that Skill
                if (skill.y == 2 || skill.y == 3 || skill.y == 4)
                {
                    SetMasteryGeneralRowsPointsRequiredVisibility(ttlpTree);
                }
            }
            else
            {
                List<Control> controls = new List<Control>();

                // Adjust all Skills in the Class Tree, not just the one we're on (NOTE: the hard-coded 2 is the first column where a Skill Button can be found)
                for (int x = 2; x <= ttlpTree.ColumnCount - 1; x++)
                {
                    // Get all controls at the current X position (there may be multiple across different Y positions)
                    for (int y = 0; y <= ttlpTree.RowCount - 1; y++)
                    {
                        if (!(ttlpTree.GetControlFromPosition(x, y) is null))
                        {
                            controls.Add(ttlpTree.GetControlFromPosition(x, y));
                        }
                    }

                    // Loop through each of the controls found and adjust PointsReq visibility
                    foreach (Control control in controls)
                    {
                        SetPointsRequiredVisibility(control, totalPointsAllocated);
                    }
                }
            }
        }

        public void SetPointsRequiredVisibility(Control control, int totalPointsAllocated)
        {
            bool visible = false;

            if (control is MultiSkillSelectButton)
            {
                //Adjust visibility on each of the SkillSelectButtons contained within
                foreach (SkillSelectButton skillSelectButton in (control as MultiSkillSelectButton).skillSelectPanel.Controls.OfType<SkillSelectButton>())
                {
                    visible = (!(skillSelectButton.skill.min_level <= totalPointsAllocated) && (skillSelectButton.skill.min_level > 0));
                    skillSelectButton.skillTooltipPanel.Controls.Find("lblPointsRequired", true).First().Visible = visible;
                }
            }
            else if (control is SkillButton)
            {
                // Adjust visibility on the SkillButton
                visible = (!((control as SkillButton).skill.min_level <= totalPointsAllocated) && ((control as SkillButton).skill.min_level > 0));
                (control as SkillButton).skillTooltipPanel.Controls.Find("lblPointsRequired", true).First().Visible = visible;

                // See if the Skill Button has a SkillSelectPanel
                if (!((control as SkillButton).skillSelectPanel is null))
                {
                    // Adjust visibility on each of the SkillSelectButtons contained within
                    foreach (SkillSelectButton skillSelectButton in (control as SkillButton).skillSelectPanel.Controls.OfType<SkillSelectButton>())
                    {
                        visible = (!(skillSelectButton.skill.min_level <= totalPointsAllocated) && (skillSelectButton.skill.min_level > 0));
                        skillSelectButton.skillTooltipPanel.Controls.Find("lblPointsRequired", true).First().Visible = visible;
                    }
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

        // TO CONVERT SLOT_IDS.TXT to .XML FOR USE IN CHRONICALC:
		// slot_ids.txt was provided from Chronicon dev
        // i've converted it to xml by performing the following:
        // copy/paste in Excel
        // remove the header row and blank row in rows 1-2
        // export to CSV
        // goto http://www.convertcsv.com/csv-to-xml.htm
        // paste the csv information
        // copy/paste the following, without quotes, as the top line in the data:  "position	ID"
        // Top-Level Root Name = nodes
        // Each Record XML Name = node
        // click Convert CSV to XML
        // save the generated XML file to it's existing location and it should auto-update in the ResourceData resource manager
        private void PopulateMasterySlotIDs()
        {
            int slotID;
            string position;
            string slotIDsXml = (string)ResourceManagerData.GetObject("slot_ids");
            int x;
            int y;

            // Read the slot_ids.xml file which contains the x,y --> Slot ID mapping to populate a stored Dictionary of this data
            XmlDocument slotData = new XmlDocument();
            slotData.LoadXml(slotIDsXml);

            // Loop through each x,y --> SlotID mapping node within all the nodes
            foreach (XmlNode node in slotData.SelectSingleNode("nodes"))
            {
                // Seperate the data in the current node to get the x,y position and its corresponding Slot ID
                position = node.SelectSingleNode("position").InnerXml;
                slotID = Convert.ToInt32(node.SelectSingleNode("ID").InnerXml);

                // Extract the individual x,y positions from the position string
                x = Convert.ToInt32(position.Substring(0, position.IndexOf(',')));
                y = Convert.ToInt32(position.Substring(position.IndexOf(',') + 1));

                // Adjust the indicies to match ChroniCalc's Mastery Tree control (as it's not identical with the Chronicon Tree Control because CCalc was built prior to having this information)
                //  Ie. X-positions in slot_ids.xml are stored as 1-based indicies but we need to offset it by 2, from 0, because our Mastery Tree control includes the first two columns containing
                //      the passive row counters and the blank column between them and the selectable Skills (e.g the first Skill x-position in ChroniCalc is actually 2, not 1)
                //  Ie. Y-positions in slot_ids.xml are stored as 1-based indicies; we need to convert the position over to 0-based indicies by subtracting 1 (e.g. the first Skill y-position in ChroniCalc is actually 0, not 1)
                position = string.Concat((x + 1).ToString(), ",", (y - 1).ToString());

                // Add this data to a Dictionary since we'll need this information in other areas of the application (e.g. Import/Export to Game)
                masterySlotIDs.Add(slotID, position);
            }
        }

        public void PopulateSkillTrees()
        {
            /*************************************************/
            string CleanSkillData_PreLoad(string data)
            {
                // Remove apostrophes
                data = data.Replace("'", "");

                // Save the cleaned data to a text file for comparison //TODO only do this in debug mode
                // File.WriteAllText(TempDirectory + "\\SkillData_Cleaned_PreLoad.txt", data);  //TODOSSG print a translated version with escaped characters translated to their actual special characters

                return data;
            }
            /*************************************************/
            XmlDocument CleanSkillData_PostLoad(XmlDocument xmlData)
            {
                // Remove Templar's None skill in its Vengeance tree //TODOSSG make this more gloabl to remove any <None> nodes found (and only doing it if some were found)
                XmlNode TemplarVengeance = xmlData.SelectSingleNode("root/Templar/Vengeance");
                if (!(TemplarVengeance.SelectSingleNode("None") is null))
                {
                    TemplarVengeance.RemoveChild(TemplarVengeance.SelectSingleNode("None"));
                }

                // Save the cleaned data to a text file for comparison //TODO only do this in debug mode
                // File.WriteAllText(TempDirectory + "\\SkillData_Cleaned_PostLoad_TemplarVengeanceNoneRemoved.txt", xmlData.OuterXml);

                // Default all Mastery skills with "infinite" max rank to have a starting value of 10
                XmlNodeList infiniteMasteries = xmlData.SelectNodes("//max_rank[text()='infinite']");

                foreach (XmlNode infiniteMastery in infiniteMasteries)
                {
                    infiniteMastery.ParentNode.SelectSingleNode("value").InnerText = "10";
                }

                // Save the cleaned data to a text file for comparison //TODO only do this in debug mode
                // File.WriteAllText(TempDirectory + "\\SkillData_PostLoad_Cleaned_All.txt", xmlData.OuterXml);

                return xmlData;
            }
            /*************************************************/

            // Set the Locale to English to avoid any manipulation by the system settings to the Skill Data Export data (e.g. Decimals and comma-separator discrepancies)
            ChangeLocale(true);

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

            // Clean up the skill data so it will load as valid XML
            jsonAsXml = CleanSkillData_PreLoad(jsonAsXml);

            //Load the skill data in as XML format because the Json isn't an ideal format for 
            // iterating over and squarebit is a busy bro and doesn't need to be bothered to change it
            XmlDocument skillData = new XmlDocument();
            skillData.LoadXml(jsonAsXml);

            // Clean up the skill data some more now that we have it loaded as XML to work with
            CleanSkillData_PostLoad(skillData);

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
                        skill.effect = NodeHasValue(skillNode.SelectSingleNode("effect")) ? Array.ConvertAll(skillNode.SelectSingleNode("effect").InnerXml.Replace("%", "").Split(','), double.Parse) : new double[] { };
                        skill.cooldown = !(skillNode.SelectSingleNode("cooldown") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("cooldown").InnerXml) : -1;
                        skill.duration = NodeHasValue(skillNode.SelectSingleNode("duration")) ? Array.ConvertAll(skillNode.SelectSingleNode("duration").InnerXml.Split(','), double.Parse) : new double[] { };
                        skill.cost100 = !(skillNode.SelectSingleNode("cost100") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("cost100").InnerXml) : -1;
                        skill.skill_requirement = NodeHasValue(skillNode.SelectSingleNode("skill_requirement"), new string[] { "none" }) ? Array.ConvertAll(skillNode.SelectSingleNode("skill_requirement").InnerXml.Trim('[', ']').Split(','), int.Parse) : new int[] { };
                        skill.x = !(skillNode.SelectSingleNode("x") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("x").InnerXml) : -1;
                        skill.damage = NodeHasValue(skillNode.SelectSingleNode("damage")) ? Array.ConvertAll(skillNode.SelectSingleNode("damage").InnerXml.Replace("%", "").Split(','), double.Parse) : new double[] { };
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
                        skill.max_rank = (!(skillNode.SelectSingleNode("max_rank") is null) && (skillNode.SelectSingleNode("max_rank").InnerXml.All(Char.IsDigit))) ? Convert.ToInt32(skillNode.SelectSingleNode("max_rank").InnerXml) : int.MaxValue;  // The data contains "infinite" for some mastery skills that don't have a max rank, so represent that via int.MaxValue
                        skill.type = GetSkillType(skillNode.SelectSingleNode("type"), tree.name, ref skill); // Type is dependant on max rank for Mastery skills, so need to call a method and do it after we set max rank
                        skill.y = !(skillNode.SelectSingleNode("y") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("y").InnerXml) : -1;
                        skill.cost1 = !(skillNode.SelectSingleNode("cost1") is null) ? Convert.ToInt32(skillNode.SelectSingleNode("cost1").InnerXml) : -1;
                        skill.name = !(skillNode.SelectSingleNode("name") is null) ? skillNode.SelectSingleNode("name").InnerXml : "ERROR: Missing Name";

                        skills.Add(skill);
                    }

                    // In the Mastery Tree, duplicate some Skills where necessary to fill it out completely;
					// 	(this is because Skills shared between Rows are only in the Skill Data at their first row)
                    if (tree.name == "Mastery")
                    {
                        // Copy Row 3 Generic Skills to also exist in Rows 4 and 5
                        AddMasterySharedGenericRows(ref skills);

                        // Copy shared Class-specific Generic Skills from Row 1 to exist in the SkillSelectPanel for all Class-specific Rows (e.g. Rows 1, 2, 6, and 7)
                        AddMasterySharedClassSpecificGenericSkills(ref skills, classNode.Name);

                        // Copy shared Class-specific Affinity Skills from Row 1 to exist in the SkillSelectPanel for all Class-specific Rows (e.g. Rows 1, 2, 6, and 7)
                        AddMasterySharedClassSpecificAffinities(ref skills);

                        // Create all of the passive row counter buttons
                        AddMasteryPassiveRowCounters(classNode.Name, ref skills);

                        // Add custom slot IDs defined by squarebit that help define each cells position in the TreeTableLayoutPanel (ie. it's just a different way than using x,y)
                        AddMasterySlotIDs(ref skills);
                    }

                    //Add all available Skills to the Tree
                    tree.skills = skills;

                    //Add the Tree to the List of Trees
                    trees.Add(tree);
                }

                //Add the populated Class to the list of all available Classes
                characterClasses.Add(new CharacterClass(classNode.Name, trees));
            }

            // Set the Locale back to the System setting so values are displayed that the user is familiar with
            ChangeLocale(false);
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
                // Check for an active search on the previous tree that was displayed
                if (ttlp.Visible && txtTreeSearch.Text != string.Empty)
                {
                    // This is the previous tree that was visible, unhighlight controls from the previous search before we hide this tree
                    foreach (Control treeControl in ttlp.Controls)
                    {
                        TreeSearch_Unhighlight(treeControl, null);
                    }
                }

                if (ttlp.Name == btnTree.Tag.ToString())
                {
                    ttlp.Show();
                    ttlp.BringToFront();
                    lblTreeCaption.Text = ttlp.passiveSkillName;

                    // If there is a Tree Search active, highlight the Skills in the new tree being displayed
                    if (txtTreeSearch.Text != string.Empty)
                    {
                        // Call the event that prompts a search of the Tree //TODOSSG what's the consequence of passing on an unexpected object and eventargs from a different control?
                        TxtTreeSearch_TextChanged(sender, e);
                    }
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

            dialogResult = PromptForBuildReset();

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

                //Get the newly-selected class
                selectedClass = characterClasses.Find(x => x.name == characterClass);

                if (selectedClass is null)
                {
                    // Throw error that the selected class was not found in the current list of character classes
                    Alerts.DisplayError("SelectClass:  Class '" + characterClass + "' was not found in the Class options extracted from the Game Data.");

                    // Set the Class drop-down back to the previously selected characterClass
                    // Suppress change events
                    cboClass.SelectedIndexChanged -= CboClass_SelectedIndexChanged;

                    // Avoid a fringe case where the first Class selected from the drop-down was the invalid one, since we can't reset to a null class
                    if (!(build.characterClass is null))
                    {
                        cboClass.SelectedIndex = cboClass.Items.IndexOf(build.characterClass.name);
                    }

                    // Unsuppress change events
                    cboClass.SelectedIndexChanged += CboClass_SelectedIndexChanged;

                    return;
                }

                //Clear everything related to the previous build
                ClearCharacter(build);

                //Update data on the build (everything not listed here was handled in the ResetCharacter() code (e.g. level, masteryLevel, trees, skills, etc)
                build.characterClass = selectedClass;
                build.name = "<Not Saved>";
                build.ApplicationVersion = Application.ProductVersion;

                InitializeBuild(build);

                // Show the Trees, incase a different view (e.g. Inventory, Builds, etc) was being shown
                pnlTrees.BringToFront();
            }
            else
            {
                // Set the Class drop-down back to the previously selected characterClass
                // Suppress change events
                cboClass.SelectedIndexChanged -= CboClass_SelectedIndexChanged;

                // TODO // Avoid a fringe case where the first Class selected from the drop-down was the invalid one, since we can't reset to a null class
                //if (!(build.characterClass is null))
                //{
                cboClass.SelectedIndex = cboClass.Items.IndexOf(build.characterClass.name);
				//}

                // Unsuppress change events
                cboClass.SelectedIndexChanged += CboClass_SelectedIndexChanged;
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
						//  Loop through each SkillSelect button contained within the SkillSelect panel to remove its controls
                        while (skillButton.skillSelectPanel.Controls.Count > 0)
                        {
                            skillSelectButton = (skillButton.skillSelectPanel.Controls[0] as SkillSelectButton);
                            if (!(skillSelectButton is null) && !(skillSelectButton.skillTooltipPanel is null))
                            {
                                // The Tooltip panel is not added as a child to the SkillSelect button, so it needs to be manually cleaned up
                                skillSelectButton.skillTooltipPanel.Dispose();
                            }
                            // This doesn't actually free USER_OBJECTS but is necessary for the sake of the While loop
                            skillButton.skillSelectPanel.Controls[0].Dispose();
                        }

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
                else
                {
                    // An unhandled control type was found and could lead to a memory leak if it's not included in the above or below Garbage Collection
                    //  (NOTE: This will occur if you ever place controls in the TreeTableLayoutPanel that aren't Skill-related types and perhaps could be shutoff 
                    //         but we'll determine that if/when this Error starts showing up)
                    Alerts.DisplayError("ClearTree: Unhandled Control type of " + control.GetType().ToString() + " found.  It may need to be handled and disposed of.");
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
                    // Throw error that tlpTree.Name is not found in the currently-loaded Trees object for the selected Class
                    throw new EChroniCalcException("LoadTrees(): Tree '" + ttlpTree.Name + "' was not found in the Game Data but is implemented into a Control that requires it.  Unable to continue.");
                }
            }
        }

        private void LoadTree(Tree tree, TreeTableLayoutPanel ttlpTree)
        {
            SkillButton passiveBonusButton;
            int treeSkillPointsAllocated = 0;

            List<Skill> MultiSelectionSkills = new List<Skill>();  //TODO could rename to LoadedMultiSelectionSkills/isLoadedMultiSelectionSkill for readability and consistency with isImportedMultiSelectionSkill
            List<ImportedSkillandPanel> importedSkillsAndTheirSkillSelectPanels = new List<ImportedSkillandPanel>();

            //Give the current Tree object to the Tree control that will be showing it as it'll be needed for future reference
            ttlpTree.tree = tree;

            //Load any skill slot that begins with a "+" for the user to pick between multiple skills
            LoadMultiSelectionSkills(tree, ttlpTree, ref MultiSelectionSkills, ref importedSkillsAndTheirSkillSelectPanels);

            //Loop through each Skill and place it into the correct slot in the TableLayoutPanel
            foreach (Skill skill in tree.skills)
            {
                // Check if the current skill is derived from a MultiSkill selection and should have a SkillSelectPanel appended to it
                bool isImportedMultiSelectionSkill = !(importedSkillsAndTheirSkillSelectPanels.Find(s=>s.id == skill.id && s.x == skill.x && s.y == skill.y) is null);
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
                    if ((skill.name == ttlpTree.passiveSkillName) || (tree.name == "Mastery" && skill.name.Contains(tree.name))) //TODO This'll fail if the Mastery Tree ever gets a selectable skill with the name "Mastery" in it; a fooler-proof solution may be to hard-code the skillId's into a list and check if skill.id IN MasteryPassiveSkillIds //TODOSSG this might fubar on Sky_Lord_Focus on mastery tree condition
                    {
                        btnSkill.isPassiveBonusButton = true;
                    }

                    //Add the skill button to the tree
                    ttlpTree.Controls.Add(btnSkill, skill.x, skill.y);

                    // Give the new SkillButton a SkillSelectPanel if it's a MultiSkill that has had a skill selected by the user
                    if (!(importedSkillsAndTheirSkillSelectPanels.Find(s => s.id == skill.id && s.x == skill.x && s.y == skill.y) is null))
                    {
                        importedSkillSelectPanel = importedSkillsAndTheirSkillSelectPanels.Find(s => s.id == skill.id && s.x == skill.x && s.y == skill.y).skillSelectPanel;

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

            // Set the visibility of the Points Required labels on each of the Skills //TODO don't bother doing this if it's not a build being loaded to save on processing?
            if (tree.name == "Mastery")
            {
                // Set visibility of all Skill Buttons within each individual row in the Mastery tree
                for (int row = 0; row <= ttlpTree.RowCount - 1; row++)
                {
                    passiveBonusButton = (ttlpTree.GetControlFromPosition(0, row) as SkillButton);
                    SetPointsRequiredVisibilities(ttlpTree, passiveBonusButton.skill, passiveBonusButton.skill.level);
                }
            }
            else
            {
                // Set visibility for all controls in the Class Tree
                passiveBonusButton = (ttlpTree.GetControlFromPosition(0, 3) as SkillButton);
                SetPointsRequiredVisibilities(ttlpTree, passiveBonusButton.skill, ttlpTree.skillPointsAllocated);
            }            

            //Update Stats here because we may have loaded a build with invested skill points
            UpdateStats(build);

            //With all Skills added to the tree, populate all descriptions for each skill
            PopulateSkillDescriptions(ttlpTree);
        }

        //Look at all skills within the current Tree to see if there are multiples that share the same position (ie. Dive, Jump, Flame Dash)
        // where only 1 should be selected by the user
        private void LoadMultiSelectionSkills(Tree tree, TreeTableLayoutPanel tlpTree, ref List<Skill> MultiSelectionSkills, ref List<ImportedSkillandPanel> importedSkillsAndTheirSkillSelectPanels)
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
                        if (!MultiSelectionSkills.Contains(multiSkill))
                        { 
                            MultiSelectionSkills.Add(multiSkill);
                        }
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
                        // Add the leveled Skill and its SkillSelectPanel to a list for use after we've loaded all skills into the Tree so we can then assign the remaining SkillSelectPanels
                        //   (the SkillSelectPanel cannot be added to the selected Skill because the Skill hasn't been created as a SkillButton and loaded onto the tree yet
                        Skill importedSkill = MultipleSkills.Find(s => s.level > 0);
                        importedSkillsAndTheirSkillSelectPanels.Add(new ImportedSkillandPanel(importedSkill.id, importedSkill.x, importedSkill.y, pnlSkillSelect));
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

        private void TreeSearch_Highlight(Control treeControl, Skill skill)
        {
            if (treeControl is SkillButton)
            {
                // Highlight the control if it's not already highlighted
                ((treeControl as SkillButton).Controls.Find("lblSkillHighlight", true).First() as Label).BackColor = Color.Yellow;
            }
            else if (treeControl is MultiSkillSelectButton)
            {
                // Look at the MultiSkillSelectButton's child controls to find the Skill Button that matched the search in order to highlight it
                foreach (SkillSelectButton skillSelectButton in (treeControl as MultiSkillSelectButton).skillSelectPanel.Controls.OfType<SkillSelectButton>())  //TODO this could probably be optimized to save on processing
                {
                    if (skillSelectButton.skill == skill)
                    {
                        // Search match found, highlight the child SKill Button
                        skillSelectButton.FlatAppearance.BorderColor = Color.Yellow;

                        // Also highlight the MultiSkillSelectButton so the user knows a Skill Button within it does match the search
                        ((treeControl as MultiSkillSelectButton).Controls.Find("lblMultiSkillSelectHighlight", true).First() as Label).BackColor = Color.Yellow;

                        // No need to look at the remaining Skill Buttons
                        break;
                    }
                }
            }
        }

        private void TreeSearch_Unhighlight(Control treeControl, Skill skill)
        {
            // Unhighlight the control on the Tree
            if (treeControl is SkillButton)
            {
                // Unhighlight the Skill Button
                ((treeControl as SkillButton).Controls.Find("lblSkillHighlight", true).First() as Label).BackColor = Color.Transparent;
            }
            else if (treeControl is MultiSkillSelectButton)
            {
                bool anotherSkillIsHighlighted = false;

                // Look at the MultiSkillSelectButton's child controls to find the Skill Button that didn't match the search in order to unhighlight it
                foreach (SkillSelectButton skillSelectButton in (treeControl as MultiSkillSelectButton).skillSelectPanel.Controls.OfType<SkillSelectButton>())
                {
                    if (skill == null)
                    {
                        // No particular skill passed in, so the intent is to unhighlight all child controls
                        skillSelectButton.FlatAppearance.BorderColor = Color.LightGray;
                    }
                    else if (skillSelectButton.skill == skill)
                    {
                        // No search match not found, unhighlight the child SKill Button
                        skillSelectButton.FlatAppearance.BorderColor = Color.LightGray;

                        // No need to look at the remaining Skill Buttons
                        //break;
                    }
                    else
                    {
                        // See if this other Skill is still highlighted
                        if (skillSelectButton.FlatAppearance.BorderColor == Color.Yellow)
                        {
                            // Keep track of a different Skill in the SkillSelectPanel still being highlighted
                            anotherSkillIsHighlighted = true;
                        }
                    }
                }

                // If no other skill in the SkillSelectPanel is highlighted, also unhighlight the MultiSkillSelectButton so the user knows there is no remaining search matchin within it
                if (!anotherSkillIsHighlighted)
                {
                    ((treeControl as MultiSkillSelectButton).Controls.Find("lblMultiSkillSelectHighlight", true).First() as Label).BackColor = Color.Transparent;
                }
            }
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
                // Update the Build name regardless if this is a new Build being saved or an old build being overwritten
                build.name = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                lblBuildName.Text = build.name;

                SaveBuild(saveFileDialog.FileName);
                UpdateBuildsList(build);

                //TODO Set the SaveAs'd Build as selected in the Builds list
            }
        }

        private void SaveBuild(string fileNameAndPath)
        {
            XmlSerializer serializer;

            // Set the Last Saved timestamp to Now
            build.lastSaved = DateTime.Now;

            try
            {
                // Save the build to the specified file
                using (var writer = new StreamWriter(fileNameAndPath))
                {
                    // Serialize the build object into xml (this should make saving/loading builds easy)
                    serializer = new XmlSerializer(build.GetType());
                    serializer.Serialize(writer, build);
                    writer.Flush();
                }
            }
            catch (Exception ex)
            {
                // Display message that something failed when attempting to Serialize and Write the Build to a file
                Alerts.DisplayError("SaveBuild: Unable to serialze the Build.  Your Build has not been saved." + Environment.NewLine + ex.ToString());
                return;
            }

            // Update the Build Status to be Saved, since the save logic has completed
            build.buildStatus = Build.BuildStatus.Saved;
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
                // Display message that the build was not found for some reason
                Alerts.DisplayWarning("OpenBuildClick: The following Build file was not found.  It's possible the file cannot be accessed.  Please try saving your Build files in your My Documents folder." + Environment.NewLine + "File: " + fileNameAndPath);
                return;
            }
            
            //TODO clear treeStatus?
        }

        public void OpenBuild(string buildContent, bool pasteBinImport = false)
        {
            Build importedBuild;
            XmlSerializer serializer;

			// Set the Build Status so we don't trigger certain events (e.g. Build.SetAsModified())
            build.buildStatus = Build.BuildStatus.Opening;

            // Deserialize the content the appropriate way depending on what was passed in (ie. pastebin Raw Text extract or a filepath to an XML file on disk)
            if (pasteBinImport)
            {
                try
                {
                    using (var stringReader = new StringReader(buildContent))
                    {
                        serializer = new XmlSerializer(typeof(Build));
                        importedBuild = serializer.Deserialize(stringReader) as Build;
                    }
                }
                catch (Exception ex)
                {
                    // Display a message that the Build fetched from pastebin could not be opened
                    Alerts.DisplayError("OpenBuild: Unable to deserialize the build fetched from Pastebin." + Environment.NewLine + ex.ToString());

                    // Clear the Build Status
                    build.buildStatus = Build.BuildStatus.None;

                    return;
                }
            }
            else
            {
                try
                {
                    // Load the saved build content from the file into a Build object
                    using (var stream = new StreamReader(buildContent))
                    {
                        serializer = new XmlSerializer(typeof(Build));
                        importedBuild = serializer.Deserialize(stream) as Build;
                    }
                }
                catch (Exception ex)
                {
                    // Display a message that the Build file could not be opened
                    Alerts.DisplayError("OpenBuild: Unable to deserialize the build loaded from the save file." + Environment.NewLine + ex.ToString());

                    // Clear the Build Status
                    build.buildStatus = Build.BuildStatus.None;

                    return;
                }
            }

            // Clear everything related to the previous build
            ClearCharacter(build);

            // Convert the Build to the newest version
            BuildConvert buildConvert = new BuildConvert();  //TODO minor detail: is there anyway to call ConvertBuild without having to create a BuildConvert object?
            buildConvert.ConvertBuild(importedBuild);

            // Merge the imported Build's data into the global build object
            build = MergeImportedBuildIntoBuild(importedBuild);

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
                    // The selected Class was not accounted for in the above Switch statement
                    Alerts.DisplayError("ChangeClass: Class '" + build.characterClass.name + "' was not considered when setting the Class image.");
                    return;
            }

            //Depending on Class, load up Image and Tag on Tree tabs and update the Mastery tree
            switch (build.characterClass.name)
            {
                case "Berserker":
                    treeName = "Guardian";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].Name = treeName;
                    treePanels[0].passiveSkillId = Constants.SkillIDs.BERSERKER_GUARDIAN;
                    treePanels[0].passiveSkillName = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Sky_Lord";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].Name = treeName;
                    treePanels[1].passiveSkillId = Constants.SkillIDs.BERSERKER_SKY_LORD;
                    treePanels[1].passiveSkillName = "Sky Lord";  //Overridden from treeName;  //this is manually written by looking it up in the xml data (this wouldn't be an issue if you could work with the raw json)
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Dragonkin";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].Name = treeName;
                    treePanels[2].passiveSkillId = Constants.SkillIDs.BERSERKER_DRAGONKIN;
                    treePanels[2].passiveSkillName = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Frostborn";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].Name = treeName;
                    treePanels[3].passiveSkillId = Constants.SkillIDs.BERSERKER_FROSTBORN;
                    treePanels[3].passiveSkillName = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                case "Templar":
                    treeName = "Vengeance";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].Name = treeName;
                    treePanels[0].passiveSkillId = Constants.SkillIDs.TEMPLAR_VENGEANCE;
                    treePanels[0].passiveSkillName = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Wrath";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].Name = treeName;
                    treePanels[1].passiveSkillId = Constants.SkillIDs.TEMPLAR_WRATH;
                    treePanels[1].passiveSkillName = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Conviction";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].Name = treeName;
                    treePanels[2].passiveSkillId = Constants.SkillIDs.TEMPLAR_CONVICTION;
                    treePanels[2].passiveSkillName = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Redemption";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].Name = treeName;
                    treePanels[3].passiveSkillId = Constants.SkillIDs.TEMPLAR_REDEMPTION;
                    treePanels[3].passiveSkillName = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                case "Warden":
                    treeName = "Wind_Ranger";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].Name = treeName;
                    treePanels[0].passiveSkillId = Constants.SkillIDs.WARDEN_WIND_RANGER;
                    treePanels[0].passiveSkillName = "Wind Ranger";  //Overridden from treeName
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Druid";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].Name = treeName;
                    treePanels[1].passiveSkillId = Constants.SkillIDs.WARDEN_DRUID;
                    treePanels[1].passiveSkillName = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Storm_Caller";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].Name = treeName;
                    treePanels[2].passiveSkillId = Constants.SkillIDs.WARDEN_STORM_CALLER;
                    treePanels[2].passiveSkillName = "Storm Caller";  //Overridden from treeName
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Winter_Herald";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].Name = treeName;
                    treePanels[3].passiveSkillId = Constants.SkillIDs.WARDEN_WINTER_HERALD;
                    treePanels[3].passiveSkillName = "Winter Herald";  //Overridden from treeName
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                case "Warlock":
                    treeName = "Corruptor";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree1, treeName);
                    btnTree1.Tag = treeName;
                    treePanels[0].Name = treeName;
                    treePanels[0].passiveSkillId = Constants.SkillIDs.WARLOCK_CORRUPTOR;
                    treePanels[0].passiveSkillName = treeName;
                    treePanels[0].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Lich";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree2, treeName);
                    btnTree2.Tag = treeName;
                    treePanels[1].Name = treeName;
                    treePanels[1].passiveSkillId = Constants.SkillIDs.WARLOCK_LICH;
                    treePanels[1].passiveSkillName = treeName;
                    treePanels[1].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Demonologist";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree3, treeName);
                    btnTree3.Tag = treeName;
                    treePanels[2].Name = treeName;
                    treePanels[2].passiveSkillId = Constants.SkillIDs.WARLOCK_DEMONOLOGIST;
                    treePanels[2].passiveSkillName = treeName;
                    treePanels[2].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);

                    treeName = "Reaper";
                    LoadTreeIconButtonImage(ResourceManagerImageTree, btnTree4, treeName);
                    btnTree4.Tag = treeName;
                    treePanels[3].Name = treeName;
                    treePanels[3].passiveSkillId = Constants.SkillIDs.WARLOCK_REAPER;
                    treePanels[3].passiveSkillName = treeName;
                    treePanels[3].BackgroundImage = (Image)ResourceManagerImageTree.GetObject(treeName);
                    break;

                default:
                    // The selected Class was not accounted for in the above Switch statement //TODO should this instead be an exception? need to see what calls InitializeBuild and what the fallout may be of "return"ing
                    Alerts.DisplayError("InitializeBuild: Class '" + build.characterClass.name + "' not found.  It's possible a new Class was added to the Game Data but not yet configured in ChroniCalc.");
                    return;
            }

            // Load the mastery tree regardless of Class
            treeName = "Mastery";
            LoadTreeIconButtonImage(ResourceManagerImageTree, btnTreeMastery, treeName);
            btnTreeMastery.Tag = treeName;
            treePanels[4].Name = treeName;
            //treePanels[4].passiveSkillId = 93; //TODO test that it's OK we're not setting a passiveSkillId on the Mastery tree (or is there 1 we can/should set that's used in game but isn't in the skill data?)
            treePanels[4].passiveSkillName = treeName;
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

            // Clean up any temp files left in the Temp Directory
            DirectoryInfo directoryInfo = new DirectoryInfo(TempDirectory);

            foreach (FileInfo file in directoryInfo.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo subDirectoryInfo in directoryInfo.GetDirectories())
            {
                subDirectoryInfo.Delete(true);
            }
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

        // Generate a .build file that can be imported directly into Chronicon
        private void BtnNavExportToGame_Click(object sender, EventArgs e)
        {
            Dictionary<string, int> leveledSkills;
            IEnumerable<SkillButton> skillButtons;
            IEnumerable<Skill> skillsInTree;
            SaveFileDialog saveFileDialog;
            SkillButton skillButton;
            string json;

            // Ensure we have a Build started before attempting to export
            if (build.characterClass is null)
            {
                MessageBox.Show("No Build has been created or loaded.  Please open a Build before attempting to export it.");
                return;
            }

            // Create a dictionary that will hold the key/value pair of Skill ID/Level for converting to JSON
            leveledSkills = new Dictionary<string, int>();

            // Generate the Class Key/ID pair (hard-coded values provided by Chronicon Developer: Templar = 1, Berserker = 2, Warden = 3, Warlock = 4)
            switch (build.characterClass.name)
            {
                case "Berserker":
                    leveledSkills.Add("class", 2);
                    break;
                case "Templar":
                    leveledSkills.Add("class", 1);
                    break;
                case "Warden":
                    leveledSkills.Add("class", 3);
                    break;
                case "Warlock":
                    leveledSkills.Add("class", 4);
                    break;
                default:
                    break;
            }

            // For the current Build, loop through all Trees to gather all leveled Skills
            foreach (Tree tree in build.characterClass.trees)
            {
                // Get all the Skills currently leveled in this Tree
                skillsInTree = tree.skills.Where(s => s.level > 0);

                // Loop through each Skill and add it to the Dictionary as an "ÏD": Level pair
                foreach (Skill skill in skillsInTree)
                {
                    // Get the Skill's corresponding SkillButton control in order to check that it's not the PassiveBonusButton that we shouldn't be saving to the JSON
                    skillButtons = treePanels.Find(t => t.Name == tree.name).Controls.OfType<SkillButton>();
                    skillButton = skillButtons.Where(s => s.skill.id == skill.id).First();
                    if (!skillButton.isPassiveBonusButton)
                    {
                        leveledSkills.Add(skill.id.ToString(), skill.level);

                        if (tree.name == "Mastery")
                        {
                            // Per squarebit, need to add a slight variation of the Skill ID (appending an "s") and its corresponding Slot ID to help decipher the position this Skill was leveled at
                            leveledSkills.Add(skill.id.ToString() + "s", skill.slotID);
                        }
                    }
                }
            }

            // Generate the JSON based on the data that was setup into the Leveled Skills dictionary
            json = Newtonsoft.Json.JsonConvert.SerializeObject(leveledSkills);

            // Setup the Save File Dialog and prompt the User to save the file
            saveFileDialog = new SaveFileDialog();

            saveFileDialog.FileName = build.name;
            saveFileDialog.Filter = "BUILD files (*.build)|*.build|All files (*.*)|*.*";
            saveFileDialog.FilterIndex = 0;
            saveFileDialog.InitialDirectory = ExportsDirectory;
            saveFileDialog.RestoreDirectory = true;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Save the build string to the file specified by the user
                using (var writer = new StreamWriter(saveFileDialog.FileName))
                {
                    writer.Write(json);
                    writer.Flush();
                }
            }
        }

        // Let the user pick a .build file and import it into ChroniCalc
        private void BtnNavImportFromGame_Click(object sender, EventArgs e)
        {
            //
            bool IsPassiveBonusSkill(int skillID)
            {
                bool isPassiveBonusSkill = false;

                if (Constants.SkillIDs.PassiveBonusSkillIDs.Contains(skillID))
                {
                    isPassiveBonusSkill = true;
                }

                return isPassiveBonusSkill;
            }
            //
            CharacterClass selectedClass;
            DialogResult dialogResult;
            Dictionary<string, string> jsonIdsAndValues;
            Dictionary<string, int> leveledSkills;
            IEnumerable<Skill> skillsInTree;
            int classID;
            int importedLevel;
            OpenFileDialog openFileDialog;
            string className;
            string json;

            dialogResult = PromptForBuildReset();

            if (dialogResult == DialogResult.Yes)
            {
                //Prompt for save/if user really wants to import a build from the game, overwriting current build
                if (!SaveBuildShouldContinue())
                {
                    // The User chose not to continue with Importing a build, so exit
                    return;
                }

                // Setup the Open File Dialog and allow the user to pick a .build file
                openFileDialog = new OpenFileDialog();

                openFileDialog.Filter = "BUILD files (*.build)|*.build|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 0;
                openFileDialog.InitialDirectory = ExportsDirectory;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    treeStatus = TreeStatus.Importing;

                    // Open the build string from the file specified by the user
                    using (var reader = new StreamReader(openFileDialog.FileName))
                    {
                        json = reader.ReadToEnd();
                    }

                    // Deserialize the .build content into a string/int Dictionary of Skill ID/Level pairs
                    leveledSkills = new Dictionary<string, int>();

                    //   First, get the id and value as strings, we'll cast them into the correct datatypes after the deserialization is done so we can ignore certain nodes that are mixed in with the skill data (e.g. "metadata" : "960E180A09...")
                    jsonIdsAndValues = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

                    //   Loop through each Id and Value in the deserialized JSON and only keep the Skill IDs and their corresponding Levels (basically, if it doesn't cast into string:int, we don't want to keep it)
                    foreach (KeyValuePair<string, string> idAndValue in jsonIdsAndValues)
                    {
                        //  Copy the Id and Value pair into the dictionary of Skills to be imported
                        try
                        {
                            leveledSkills.Add(idAndValue.Key, Convert.ToInt32(idAndValue.Value));
                        }
                        catch (FormatException ex)
                        {
                            // The KeyValuePair is not a Skill and its Level (of <string, int> type) (e.g. "metadata" : "960E180A09...") so we're going to skip over it
                            continue;
                        }
                        catch (Exception ex)
                        {
                            // An unknown exception occurred when importing the build from the game; display exception details and exit the Import process since we can't continue importing this build
                            Alerts.DisplayError("Error importing the Build from the game.  Unable to continue." + Environment.NewLine + ex.ToString());
                            return;
                        }
                    }

                    if (leveledSkills.TryGetValue("class", out classID))
                    {
                        // Set the selected Class which will initialize a new build
                        switch (classID)
                        {
                            case 1:
                                className = "Templar";
                                break;
                            case 2:
                                className = "Berserker";
                                break;
                            case 3:
                                className = "Warden";
                                break;
                            case 4:
                                className = "Warlock";
                                break;
                            default:
                                className = "";
                                break;
                        }

                        //Get the newly-selected class
                        selectedClass = characterClasses.Find(x => x.name == className);

                        if (selectedClass is null)
                        {
                            // Display error that the selected class was not found in the current list of character classes
                            Alerts.DisplayError("Import From Game:  Class '" + className + "' was not found in the list of Class options.  It's possible a new class was added to the program, but the Game Data being used is outdated.");
                            return;
                        }

                        // Change the selected Class drop-down
                        // Suppress change events
                        cboClass.SelectedIndexChanged -= CboClass_SelectedIndexChanged;

                        cboClass.SelectedIndex = cboClass.Items.IndexOf(className);

                        // Unsuppress change events
                        cboClass.SelectedIndexChanged += CboClass_SelectedIndexChanged;

                        //Clear everything related to the previous build
                        ClearCharacter(build);

                        //Update data on the build (everything not listed here was handled in the ResetCharacter() code (e.g. level, masteryLevel, trees, skills, etc)
                        build.characterClass = selectedClass;
                        build.name = "<Not Saved>";
                        build.ApplicationVersion = Application.ProductVersion;                    

                        // With the imported Build initialized, set all the of the assigned Skill levels
                        //  by looping through all the Trees and their Skills to set the level
                        foreach (Tree tree in build.characterClass.trees)
                        {
                            // Get all the Skills imported from the .build file that were leveled by the user
                            skillsInTree = tree.skills.Where(s => leveledSkills.ContainsKey(s.id.ToString()));

                            // Loop through each Skill in the tree that was leveled by the user in-game
                            foreach (Skill skill in skillsInTree)
                            {
                                if (IsPassiveBonusSkill(skill.id))
                                {
                                    // Do not import any Passive Bonus Skills or Mastery Tree Passive Row Counters
                                    continue;
                                }

                                // If we're importing a Mastery skill, make sure it's the Skill in the correct position selected and leveled by the user, as there is some overlapping of Skills occurring
                                //  due to the existence of 3 shared Mastery Rows with the same Skills using the same IDs (this is where SlotIDs will help us identify the right one)
                                if (tree.name == "Mastery")
                                {
                                    int slotID;
                                    int slotIdX;
                                    int slotIdY;
                                    string position = "-1,-1";

                                    // Determine if we'll need to lookup a SlotID for the current Skill, which will be indicated in the list of the imported user's leveled skills using a key of "skill.id + 's'" (eg. 100400s)
                                    if (leveledSkills.TryGetValue(skill.id.ToString() + "s", out slotID))
                                    {
                                        // This Skill requires a lookup of a SlotID, as it's a generic skill that could be located in multiple different slots on the Tree, so we'll need to find which one exactly
                                        //   Cross-check the retreived SlotID with the dictionary of Mastery SlotIDs and their respective x,y position to get this SlotIDs actual x,y position
                                        if (!(masterySlotIDs.TryGetValue(slotID, out position)))
                                        {
                                            // If the above resulted as false, then there is no corresponding position for the given SlotID in the reference Dictionary of all MasterySlotIDs;
                                            //   we can't continue on because we won't know what x,y position to place/level the Skill at
                                            //   NOTE: It's possible the slot_ids.txt/xml is outdated from what squarebit has defined
                                            throw new EChroniCalcException("Import From Game: No corresponding Position was found for Skill " + skill.name + " in the Mastery Tree Slot IDs dictionary for Slot ID: " + slotID + ".  Unable to Import this Build.  Please include the content of your .build file in your Bug post.  Unable to continue.");
                                        }

                                        // Extract the x and y positions from the retrieved position
                                        slotIdX = Convert.ToInt32(position.Substring(0, position.IndexOf(',')));
                                        slotIdY = Convert.ToInt32(position.Substring(position.IndexOf(',') + 1));

                                        // Ensure that the current Skill's x,y position matches the SlotID of the Skill we're attempting to import
                                        //  (this will make sure the Skill gets assigned to the correct location in the Mastery Tree)
                                        if (!((slotIdX == skill.x) && (slotIdY == skill.y)))
                                        {
                                            // The x and y positions don't match the leveled slot, so we know we're not actually on the correct Skill; skip this and continue on to the next Skill
                                            continue;
                                        }
                                    }
                                }

                                // Retrieve and Assign the level of the Skill
                                if (!(leveledSkills.TryGetValue(skill.id.ToString(), out importedLevel)))
                                {
                                    // If the above resulted as false, then there was no level value found for the Skill being imported;
                                    //  NOTE: This is likely an error in the BtnNavExportToGame code, or the file was modified manually
                                    throw new EChroniCalcException("Import From Game: No Level value was found for Skill " + skill.name + " (ID: " + skill.id + ").  Unable to Import this Build.  Please include the content of your .build file in your Bug post.  Unable to continue.");
                                }

                                skill.level = importedLevel;

                                if (tree.name == "Mastery")
                                {
                                    // Update the Mastery level of the Build
                                    build.MasteryLevel += importedLevel;
                                    // Update the level of the Row Counter for the passive bonus
                                    tree.skills.Where(s => s.x == 0 && s.y == skill.y).First().level += importedLevel;
                                }
                                else
                                {
                                    // Update the level of the Build
                                    build.Level += importedLevel;
                                    // Update the level of the Passive Bonus on this Tree
                                    string alternateTreeName = tree.name.Replace("_", " ");
                                    tree.skills.Where(s => (s.x == 0 && s.name.Contains(tree.name)) || (s.x == 0 && s.name.Contains(alternateTreeName))).First().level += importedLevel;  // TODO Multiword Tree Names require an alternate name lookup cus tree.name is stored as "Winter_Herald", fix this later but get patch out asap
                                }
                            }
                        }

                        InitializeBuild(build);

                        // Update controls displaying Build/Mastery Level and other Class-wide things since they aren't stored in the import file
                        UpdateStats(build);

                        // Show the Trees, incase a different view (e.g. Inventory, Builds, etc) was being shown
                        pnlTrees.BringToFront();
                    }
                    else
                    {
                        // No Class ID was found in the json
                        //throw EChroniCalcException()
                    }
                }
            }
        }

        private void TxtTreeSearch_TextChanged(object sender, EventArgs e)
        {
            string searchString = txtTreeSearch.Text;
            bool searchStringFound;
            List<Skill> skillsToSearch;

            skillsToSearch = new List<Skill>();
            TreeTableLayoutPanel activeTreeTableLayoutPanel = null;

            // Find the active Tree
            foreach (TreeTableLayoutPanel ttlp in treePanels)
            {
                if (ttlp.Visible)
                {
                    activeTreeTableLayoutPanel = ttlp;
                    break;
                }
            }

            // Don't complete a search if we didn't find an active Tree for some reason
            if (activeTreeTableLayoutPanel == null)
            {
                return;
            }

            // Get all Skill Buttons in the Tree (or at least the controls, since different Skill Button control types can exist on the Tree)
            TableLayoutControlCollection treeControls = activeTreeTableLayoutPanel.Controls;

            // Unhighlight all if the search string isn't a valid search (e.g. "")
            if (searchString == string.Empty)
            {
                // Unhighlight all skills on the tree
                foreach (Control treeControl in treeControls)
                {
                    TreeSearch_Unhighlight(treeControl, null);
                }

                return;
            }

            // Loop through each Control (Skill) on the Tree
            foreach (Control treeControl in treeControls)
            {
                // Reset the result from the search through the last control
                searchStringFound = false;
                skillsToSearch.Clear();

                // Retrieve the Skill from the Control if we know one exists
                if (treeControl is SkillButton)
                {
                    //skill = (treeControl as SkillButton).skill;
                    skillsToSearch.Add((treeControl as SkillButton).skill);
                }
                else if (treeControl is MultiSkillSelectButton)
                {
                    foreach (SkillSelectButton skillSelectButton in (treeControl as MultiSkillSelectButton).skillSelectPanel.Controls.OfType<SkillSelectButton>())
                    {
                        skillsToSearch.Add(skillSelectButton.skill);
                    }
                }
                else
                {
                    // This control is not one that will hold a Skill to search within, move onto the next control
                    continue;
                }

                // Assert a skill was found so we can analyze it (this is perhaps more of a fail safe for the future than anything else)
                if (skillsToSearch.Count <= 0)
                {
                    continue;
                }

                foreach (Skill skill in skillsToSearch)
                {
                    // Retrieve the properties from the Skill
                    PropertyInfo[] skillProperties = skill.GetType().GetProperties();

                    // Reset the Found flag since we're searching a new Skill
                    searchStringFound = false;

                    // Loop through every property on the Skill
                    foreach (PropertyInfo skillProperty in skillProperties)
                    {
                        if (searchStringFound)
                        {
                            // We found what we were searching for in a previous iteration, stop searching the rest of this skill's properties
                            break;
                        }

                        // See if the search string is found within the current property's value, ignoring case when searching //TODO this may cause issues for non-US where the same word with different capitalization has different meaning (e.g. Turkish)
                        if (skillProperty.GetValue(skill).ToString().IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            searchStringFound = true;

                            // We don't need to look at anymore properties on this skill, so move on to the next one
                            break;
                        }
                    }

                    // Adjust highlight of skills on the tree based on if they contain the search string or not
                    if (searchStringFound)
                    {
                        TreeSearch_Highlight(treeControl, skill);
                    }
                    else
                    {
                        TreeSearch_Unhighlight(treeControl, skill);
                    }
                }
            }
        }
    }
}
