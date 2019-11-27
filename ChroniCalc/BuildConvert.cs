using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    public class BuildConvert
    {
        Build build;
        readonly List<Action> buildConverts;

        public BuildConvert()
        {
            // A list of actual function pointers that can be called by the List object itself (e.g. buildConverts[0](); calls method V000())
            buildConverts = new List<Action>();

            buildConverts.Add(V000);
            // add new convert versions here manually as needed, each time a new convert method is written
        }

        public void ConvertBuild(Build originalBuild)
        {
            build = originalBuild;

            // Get the version the Build was saved as, in integer format but removing the last number as we won't write Revision-specific conversions, only "Major.Minor.Build"
            int buildVersion = Convert.ToInt32(build.ApplicationVersion.Substring(0, build.ApplicationVersion.LastIndexOf('.')).Replace(".", ""));

            // Get the starting index in the buildConverts list that Build conversion should begin at
            int startingConversionIndex = GetStartingIndexOfConversionsToRun(buildVersion);

            // Loop through each conversion we need to run, referencing it in the buildConverts[] list by index to be run
            for (int i = startingConversionIndex; i <= buildConverts.Count - 1; i++)
            {
                // Call the conversion by passing the index
                buildConverts[i]();
            }
        }

        private int GetStartingIndexOfConversionsToRun(int buildVersion)
        {
            // Default the startIndex of which convert(s) to run to the total count of buildConverts in the list;
            //  this way, if the build is up-to-date and doesn't need to be converted, the startIndex will be above the available indicies
            //  that could be run and therefore, no convert will execute
            int startIndex = buildConverts.Count;

            // Find where the buildVersion falls with regard to the written converts available
            for (int i = 0; i <= buildConverts.Count - 1; i++)
            {
                Action convert = buildConverts[i];
                string methodName = convert.Method.Name;

                // Method names are written with a "V"; therefore, remove it so the version comparison can be done
                int methodVersion = Convert.ToInt32(methodName.Replace("V", ""));

                // Check if the version the Build was saved in is older than the version in the list we're currently looking at
                if (buildVersion < methodVersion)
                {
                    // The version the Build was saved in is older than the version in the list we're currently looking at, so set this one as the conversion to start with
                    startIndex = i;
                    break;
                }
            }

            return startIndex;
        }

        private void V000()
        {
            // This is just a placeholder/template for future Build convert methods that need to be written

            // REASON FOR CONVERT: Summarize the technical details of why you need to write this convert (e.g. SKill "Apocalyse" was removed and a new skill was added with a different Skill ID)
            // SOLUTION:  Summarize the solution to fix the REASON FOR CONVERT
        }
    }
}
