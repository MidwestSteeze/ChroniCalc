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
        public int skillPointsAllocated = 0;
        public int passiveSkillId;
        public string passiveSkillName; //Need this to differentiate between the spelling differences of the xml node name and the skill's name property (spaces and underscores)
        public Tree tree;

        public TreeTableLayoutPanel(Panel parentControl, int columnCount, int rowCount, int width, int height)
        {
            InitializeComponent();

            //Specify defaults for this custom control

            //Column/Row Counts and Sizing of them
            this.ColumnCount = columnCount;
            this.RowCount = rowCount;

            for (int i = 0; i < this.ColumnCount; i++)
            {
                this.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, (float)(100 / columnCount)));
            }

            for (int i = 0; i < this.RowCount; i++)
            {
                this.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, (float)(100 / rowCount)));
            }

            //Size
            /*FOR FUTURE REFERENCE: Determining TLP Width/Height based on Control Count and ControlWidth/ControlHeight
             * (see: https://stackoverflow.com/questions/37415113/creating-same-size-cells-with-tablelayoutpanel)

             * (columns * cellWidth + paddingLeft + paddingRight, rows * cellHeight + paddingTop + paddingBottom)
             * width = this.ColumnCount * cellWidth + 0 + 0
             * height = this.RowCount + cellHeight + 0 + 0

             * you either have to:
             *  set the cellWidth/cellHeight to meet the needs of the TTLP width/height OR
             *  set the width/height after all cells are populated in LoadTree() to meet the needs of the cellWidth/cellHeight
			 */
            this.Size = new System.Drawing.Size(width, height);

            //Assign the Parent Control
            parentControl.Controls.Add(this);

            //Location
            this.Location = new System.Drawing.Point(24, 57);

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

        private void TreeTableLayoutPanel_Click(object sender, EventArgs e)
        {
            //Show debug info of cell clicked to figure out sizing issue
            //START Debug Info
            //Panel skillButtonPanel = (Panel)sender;
            //TableLayoutPanelCellPosition pos = this.GetCellPosition((Panel)sender);
            //int width = this.GetColumnWidths()[0];
            //int height = this.GetRowHeights()[0];

            //string debugMessage;

            //debugMessage = "Width:" + width + "\n" +
            //               "height:" + height;

            //MessageBox.Show(debugMessage);
            //END Debug Info
        }
    }
}
