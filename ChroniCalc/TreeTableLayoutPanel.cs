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
        public string name;
        public TreeTableLayoutPanel(Panel parentControl)
        {
            InitializeComponent();

            //Specify defaults for this custom control

            //Column Count and Size
            this.ColumnCount = 10;

            for (int i = 0; i < this.ColumnCount - 1; i++)
            {
                this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, (100 / this.ColumnCount))); //10F, maybe (single)?
            }

            //Row Count and Size
            this.RowCount = 7;

            for (int i = 0; i < this.RowCount - 1; i++)
            {
                this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, (100 / this.RowCount))); //14.28571F, maybe (single)?
            }

            //Assign the Parent Control
            parentControl.Controls.Add(this); //TODOSSG this next

            //Size
            this.Size = new System.Drawing.Size(475, 280);

            //Location
            this.Location = new System.Drawing.Point(24, 37);

            //Background
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImageLayout = ImageLayout.Stretch;

            //Visibility
            this.Visible = false;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
