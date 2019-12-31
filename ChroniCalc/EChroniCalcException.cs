using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChroniCalc
{
    public class EChroniCalcException : Exception
    {
        const string ERROR_HEADER_OFFLINE = "An error has occurred.  Please post the following information to www.reddit.com/r/ChroniCalc:\r\n\r\n";
        const string ERROR_HEADER_ONLINE  = "An error has occurred.  Would you like to copy the error details link to your clipboard to send it to the developer?\r\n";

        public EChroniCalcException(string message)
        {
            DialogResult dialogResult;
            string pasteUrl;
            PasteBinClient client = new PasteBinClient(PasteBinClient.PBType.Error);

            // Post the error to Pastebin and provide the user a link to send instead of asking them to do screenshots
            // Setup the data for the Pastebin  //TODO enhance to include the Build data if it exists?
            var entry = new PasteBinEntry
            {
                Title = "ChroniCalc Exception",
                Text = message,
                Expiration = PasteBinExpiration.OneMonth,
                Private = false
            };

            try
            {
                // Call through the Pastebin API to get the URL
                pasteUrl = client.Paste(entry, false);

                // Display the message to the user and give them a chance to copy the Pastebin URL containing the call stack
                dialogResult = MessageBox.Show(string.Concat(ERROR_HEADER_ONLINE, Environment.NewLine, message, Environment.NewLine, pasteUrl), "ChroniCalc Exception", MessageBoxButtons.YesNo);

                switch (dialogResult)
                {
                    case DialogResult.Yes:
                        Clipboard.SetText(pasteUrl);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                // If an exception was returned from Pastebin, it's likely the user is not connected to the internet or Pastebin is down, so just show the message to them instead
                MessageBox.Show(string.Concat(ERROR_HEADER_OFFLINE, message), "ChroniCalc Exception");
            }
        }
    }
}
