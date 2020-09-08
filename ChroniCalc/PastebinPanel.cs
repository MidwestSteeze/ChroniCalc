using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ChroniCalc
{
    public partial class PastebinPanel : UserControl
    {
        public PastebinPanel()
        {
            InitializeComponent();
        }

        private void BtnPastebinShare_Click(object sender, EventArgs e)
        {
            // Generate a Pastebin URL of the current Build for the user to share
            string buildAsText;
            string pasteUrl;
            XmlSerializer serializer;
            var client = new PasteBinClient(PasteBinClient.PBType.BuildShare);

            // Optional; will publish as a guest if not logged in; could enable this to see how many pastes people are using but
            //   this exposes username/password since it's on Github
            //client.Login(userName, password); //this'll set User Key

            serializer = new XmlSerializer((this.ParentForm as BuildShareForm).ParentForm.build.GetType());

            try
            {
                // Save the build to a string format
                using (StringWriter writer = new StringWriter())
                {
                    serializer.Serialize(writer, (this.ParentForm as BuildShareForm).ParentForm.build);
                    buildAsText = writer.ToString();
                }
            }
            catch (Exception ex)
            {
                // Display error that the build could not be serialized to be shared via Pastebin
                Alerts.DisplayError("PastebinShare:  Unable to serialize the build to be shared with Pastebin." + Environment.NewLine + ex.ToString());
                return;
            }

            // Setup the data for the Pastebin
            var entry = new PasteBinEntry
            {
                Title = "ChroniCalc Build Share",
                Text = buildAsText,
                Expiration = PasteBinExpiration.Never,
                Private = false,
                Format = "xml"
            };

            try
            {
                // Call through the Pastebin API to get the URL
                pasteUrl = client.Paste(entry);

                // Show the Pastebin URL in the textbox
                txtPastebinShare.Text = pasteUrl;
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Unable to reach Pastebin.  This function is not currently available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (PasteBinApiException ex)
            {
                MessageBox.Show("Unable to retrieve Build URL from Pastebin.  This function is not currently available." + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Something else happened and we're not sure what, so provide error details to the user to pass along to me
                throw new EChroniCalcException("PasteBinShare: Unable to reach Pastebin." + Environment.NewLine + ex.Message);
            }
        }

        private void BtnPastebinLoad_Click(object sender, EventArgs e)
        {
            // Decipher the content of the Pastebin URL into a Build
            PasteBinClient pasteBinClient;
            string pastebinExtract;

            // Ensure there was an attempt to put a Build URL into the Load textbox before trying to load a build
            if (txtPastebinLoad.Text == string.Empty)
            {
                MessageBox.Show("Please enter a Build URL.", "Load Build");
                return;
            }

            //Prompt for save/if user really wants to load, overwriting current build
            if (!(this.ParentForm as BuildShareForm).ParentForm.SaveBuildShouldContinue())
            {
                return;
            }

            pasteBinClient = new PasteBinClient(PasteBinClient.PBType.BuildShare);
            try
            {
                pastebinExtract = pasteBinClient.Extract(txtPastebinLoad.Text);

                (this.ParentForm as BuildShareForm).ParentForm.OpenBuild(pastebinExtract, true);
            }
            catch (System.Net.WebException)
            {
                MessageBox.Show("Unable to reach Pastebin.  Invalid URL or this function is not currently available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Throw an exception that the pastebin build either failed to be retrieved or failed to open; as such, we have to throw an exception because it's possible 
                //  a true exception stemmed from the above OpenBuild() call and we don't want to lose that information by instead just showing a generic Alerts.DisplayError message and allowing the program to continue
                throw new EChroniCalcException("PastebinLoad:  Unable to retrieve and open the build from Pastebin" + Environment.NewLine + ex.ToString());
            }

            // Close the Form now that the build has been loaded and opened
            this.ParentForm.Close();
        }
    }
}
