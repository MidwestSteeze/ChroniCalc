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
            switch (elementType) //TODO Update colors for all that are currently Color.White
            {
                case "Ethereal":
                    color = Color.White;
                    name = "Ethereal";
                    break;
                case "Fire":
                    color = Color.Red;
                    name = "Fire";
                    break;
                case "Frost":
                    color = Color.Cyan;
                    name = "Frost";
                    break;
                case "Holy":
                    color = Color.White;
                    name = "Holy";
                    break;
                case "Lightning":
                    color = Color.LightBlue;
                    name = "Lightning";
                    break;
                case "Poison":
                    color = Color.White;
                    name = "Poison";
                    break;
                case "Physical":
                    color = Color.Gray;
                    name = "Physical";
                    break;
                case "Shadow":
                    color = Color.White;
                    name = "Shadow";
                    break;
                default:
                    break;
            }
        }
    }
}
