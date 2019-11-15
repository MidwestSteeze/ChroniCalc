using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    // This class holds Skill properties and the Skill's corresponding SkillSelectPanel for slots
    //   on the Tree where the user picked a Skill from a MultiSkillSelect Panel; these
    //   properties are used to distinguish it from others and ensure it's populated into the
    //   correct cell when importing/loading a saved Build
    public class ImportedSkillandPanel
    {
        public int id;
        public int x;
        public int y;
        public SkillSelectPanel skillSelectPanel;

        public ImportedSkillandPanel()
        {

        }

        public ImportedSkillandPanel(int inId, int inX, int inY, SkillSelectPanel inSkillSelectPanel)
        {
            this.id = inId;
            this.x = inX;
            this.y = inY;
            this.skillSelectPanel = inSkillSelectPanel;
        }
    }
}
