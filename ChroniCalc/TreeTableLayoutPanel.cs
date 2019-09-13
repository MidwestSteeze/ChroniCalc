using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChroniCalc
{
    public partial class TreeTableLayoutPanel : TableLayoutPanel
    {
        public TreeTableLayoutPanel(Panel parentControl)
        {
            InitializeComponent();

            //Specify defaults for this custom control

            //Column/Row Counts and Size
            this.ColumnCount = 10;
            this.RowCount = 7;

            for (int i = 0; i < this.ColumnCount; i++)
            {
                this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, (10F)));
            }

            for (int i = 0; i < this.RowCount; i++)
            {
                this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, (14.28571F)));
            }

            //Size
            this.Size = new System.Drawing.Size(475, 279);

            //Assign the Parent Control
            parentControl.Controls.Add(this); //TODOSSG this next

            //Location
            this.Location = new System.Drawing.Point(24, 37);

            //Background
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //Visibility
            this.Visible = false;

            //Cell Borderes (TODO delete before commit)
            this.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            this.BorderStyle = BorderStyle.FixedSingle;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private void TreeTableLayoutPanel_Click(object sender, EventArgs e)
        {
            //Show debug info of cell clicked to figure out sizing issue
            //TODO            
        }
    }
}
