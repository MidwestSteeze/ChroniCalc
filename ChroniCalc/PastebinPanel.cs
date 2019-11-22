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

            string apiKey = "9074d08a3c19871f793663a0361c6976";
            var client = new PasteBinClient(apiKey);

            // Optional; will publish as a guest if not logged in; could enable this to see how many pastes people are using but
            //   this exposes username/password since it's on Github (CCalcShare//CCalcSharer)
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
                throw new EChroniCalcException("PastebinShare:  Unable to serialize the build to be shared with Pastebin." + Environment.NewLine + ex.ToString());
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

            // Call through the Pastebin API to get the URL
            pasteUrl = client.Paste(entry);

            // Show the Pastebin URL in the textbox
            txtPastebinShare.Text = pasteUrl;
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

            pasteBinClient = new PasteBinClient();
            try
            {
                pastebinExtract = pasteBinClient.Extract(txtPastebinLoad.Text);

                (this.ParentForm as BuildShareForm).ParentForm.OpenBuild(pastebinExtract, true);
            }
            catch (Exception ex)
            {
                throw new EChroniCalcException("PastebinLoad:  Unable to retrieve the build from Pastebin" + Environment.NewLine + ex.ToString());
            }

            // TODO If the PasteBin retrieval fails, can send back this.ParentForm.DialogResult=Yes/No

            // Close the Form now that the build has been loaded and opened
            this.ParentForm.Close();
        }
    }
}
