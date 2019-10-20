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
        public new MainForm ParentForm;

        public PastebinPanel()
        {
            InitializeComponent();
        }

        private void BtnPastebinClose_Click(object sender, EventArgs e)
        {
            this.Hide();
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

            serializer = new XmlSerializer(ParentForm.build.GetType());

            // Save the build to a string format
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, ParentForm.build);  //TODO add try/catch around this incase serialization fails
                buildAsText = writer.ToString();
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

            //Prompt for save/if user really wants to load, overwriting current build
            if (!ParentForm.SaveBuildShouldContinue())
            {
                return;
            }

            pasteBinClient = new PasteBinClient();
            pastebinExtract = pasteBinClient.Extract(txtPastebinLoad.Text);

            ParentForm.OpenBuild(pastebinExtract, true);
        }
    }
}
