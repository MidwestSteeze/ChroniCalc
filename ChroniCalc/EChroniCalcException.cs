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
        const string ERROR_HEADER = "An error has occurred.  Please send the following to an administrator:\r\n";

        public EChroniCalcException(string message)
        {
            MessageBox.Show(string.Concat(ERROR_HEADER, message));
        }
    }
}
