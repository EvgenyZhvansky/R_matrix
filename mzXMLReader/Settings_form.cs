using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace R_matrix_visualization
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            cBcolormap.Items.Add("Gray");
            cBcolormap.Items.Add("Jet");
            cBcolormap.Items.Add("Autumn");
            cBcolormap.Items.Add("Cool");
            cBcolormap.Items.Add("Hot");
            cBcolormap.Items.Add("Spring");
            cBcolormap.Items.Add("Summer");
            cBcolormap.Items.Add("Winter");
            cBcolormap.SelectedIndex = WindowsFormsApplication1.Settings_store.Default._colormap;
            cbReverseCmap.Checked=WindowsFormsApplication1.Settings_store.Default._reverse_colormap;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            
            Form1.R_matrix.refresh();
            Form1.Refresh_all();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            WindowsFormsApplication1.Settings_store.Default._colormap = cBcolormap.SelectedIndex;
            WindowsFormsApplication1.Settings_store.Default.Save();
            this.Close();
        }

        private void cBcolormap_SelectedIndexChanged(object sender, EventArgs e)
        {
            WindowsFormsApplication1.Settings_store.Default._colormap = cBcolormap.SelectedIndex;
            switch (cBcolormap.SelectedIndex)
            {

                case 0:
                    Form1.cmap = Form1.cm.Gray();
                    break;
                case 1:
                    Form1.cmap = Form1.cm.Jet();
                    break;
                case 2:
                    Form1.cmap = Form1.cm.Autumn();
                    break;
                case 3:
                    Form1.cmap = Form1.cm.Cool();
                    break;
                case 4:
                    Form1.cmap = Form1.cm.Hot();
                    break;
                case 5:
                    Form1.cmap = Form1.cm.Spring();
                    break;
                case 6:
                    Form1.cmap = Form1.cm.Summer();
                    break;
                case 7:
                    Form1.cmap = Form1.cm.Winter();
                    break;
            }
            pBcolorbar.Refresh();
        }

        private void cbReverseCmap_CheckedChanged(object sender, EventArgs e)
        {
            WindowsFormsApplication1.Settings_store.Default._reverse_colormap = cbReverseCmap.Checked;
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        private void pBcolorbar_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int ymin = 0;
            int ymax = 64;
            int dy = (int)((double)(pBcolorbar.Width )/ (ymax - ymin));
            int m = 256;
            for (int i = 0; i < 64; i++)
            {
                int colorIndex = (int)((i - ymin) * m / (ymax - ymin));
                SolidBrush aBrush = new SolidBrush(Color.FromArgb(
                    Form1.cmap[colorIndex, 0], Form1.cmap[colorIndex, 1],
                    Form1.cmap[colorIndex, 2], Form1.cmap[colorIndex, 3]));
                g.FillRectangle(aBrush, 0 + i * dy, 0, dy, pBcolorbar.Height);
            }
        }

    }
}
