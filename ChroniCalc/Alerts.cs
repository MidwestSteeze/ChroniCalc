using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChroniCalc
{
    public static class Alerts
    {
        public static void DisplayInfo(string message)
        {
            const string HEADER = "Please be aware of the following information:\r\n\r\n";

            MessageBox.Show(string.Concat(HEADER, message), "ChroniCalc Information");
        }

        public static void DisplayWarning(string message)
        {
            const string HEADER = "Please be aware of the following warning:\r\n\r\n";

            MessageBox.Show(string.Concat(HEADER, message), "ChroniCalc Warning");
        }

        public static void DisplayError(string message)
        {
            // Include information for sharing this Error, since Errors are something I'll want to know about and fix but don't want to throw an Exception and hard-stop the application
            const string ERROR_HEADER_OFFLINE = "An error has occurred.  Please post the following information to www.reddit.com/r/ChroniCalc:\r\n\r\n";
            const string ERROR_HEADER_ONLINE = "An error has occurred.  Would you like to copy the error details link to your clipboard to send it to the developer?\r\n";

            DialogResult dialogResult;
            string pasteUrl;
            PasteBinClient client = new PasteBinClient(PasteBinClient.PBType.Error);

            // Post the error to Pastebin and provide the user a link to send instead of asking them to do screenshots
            // Setup the data for the Pastebin  //TODO enhance to include the Build data if it exists?
            var entry = new PasteBinEntry
            {
                Title = "ChroniCalc Error",
                Text = message,
                Expiration = PasteBinExpiration.OneMonth,
                Private = false
            };

            try
            {
                // Call through the Pastebin API to get the URL
                pasteUrl = client.Paste(entry, false);

                // Display the message to the user and give them a chance to copy the Pastebin URL containing the call stack
                dialogResult = MessageBox.Show(string.Concat(ERROR_HEADER_ONLINE, Environment.NewLine, message, Environment.NewLine, pasteUrl), "ChroniCalc Error", MessageBoxButtons.YesNo);

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
                MessageBox.Show(string.Concat(ERROR_HEADER_OFFLINE, message), "ChroniCalc Error");
            }
        }
    }
}
