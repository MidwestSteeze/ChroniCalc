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
            const string HEADER = "An error has occurred.  Please post the following information to www.reddit.com/r/ChroniCalc:\r\n\r\n";

            MessageBox.Show(string.Concat(HEADER, message), "ChroniCalc Error");
        }
    }
}
