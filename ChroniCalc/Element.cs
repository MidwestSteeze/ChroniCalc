using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChroniCalc
{
    class Element
    {
        //Properties
        public string name;
        public Color color;

        public Element(string elementType)
        {
            switch (elementType) //TODO finish this list and verify all properties
            {
                case "Fire":
                    color = Color.Red;
                    name = "Fire";
                    break;
                case "Frost":
                    color = Color.Cyan;
                    name = "Frost";
                    break;
                case "Lightning":
                    color = Color.LightBlue;
                    name = "Lightning";
                    break;
                case "Physical":
                    color = Color.Gray;
                    name = "Physical";
                    break;
                default:
                    break;
            }
        }
    }
}
