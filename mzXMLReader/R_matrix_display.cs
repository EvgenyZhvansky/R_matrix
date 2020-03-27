//using Accord.Imaging;
using Accord.IO;
using Accord.Math;
using Numpy;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using ZedGraph;

namespace R_matrix_visualization
{

    public partial class Form1 : Form
    {

        /// <summary>
        /// 
        /// </summary>
        /// 
        #region variables
        Settings fm_settings = new Settings();
        static bool open = false;
        static string file = "";
        double xCoordinate = 0;
        double yCoordinate = 0;
        static int scannames_count = 0;
        //double[][] scans_rows = new double[scannames_count][];
        List<string> scanNamesList = new List<string>();
        List<string> scanNamesListSelected = new List<string>();
        List<string> sampleNameList = new List<string>();
        List<string> sampleNameListSelected = new List<string>();
        List<double> sampleCoordList = new List<double>();
        List<double> sampleCoordListSelected = new List<double>();
        List<string> sampleScanList = new List<string>();
        double[,] rf = new double[1, 1];
        double[,] rf2 = new double[1, 1];
        static int X = 0, Y = 0;
        public static double threshold = 0;
        public static double threshold_outliers = 0;
        double[,] spectra_m = new double[1, 1];
        public static double[,] rr = new double[1, 1];
        double[,] rr_selected = new double[1, 1];
        float scale = 0;
        MasterPane master = null;
        public static ColorMap cm = new ColorMap();
        public static int[,] cmap = new int[256, 4];
        //double euq = 0;
        //List<int[]> indexes_of_mz = new List<int[]>();
        public static double mzmin = 150;
        public static double mzmax = 2000;
        public static double n_precision = 4;
        double[] entr = new double[1];
        static int[] mz_id_to_process = new int[1];
        #endregion

        public Form1()
        {
            InitializeComponent();
            master = zgc.MasterPane;
            master.PaneList.Clear();
            master.Title.IsVisible = true;
            master.Margin.All = 10;
            master.Fill = new Fill(SystemColors.Control);
            master.Border.Color = SystemColors.Control;
            master.PaneList.Clear();
            for (int i = 0; i < 2; i++)
            {
                GraphPane pane = new GraphPane();
                master.Add(pane);
                if (i == 0)
                    master.PaneList[0].Title.Text = "MS";
                if (i == 1)
                    master.PaneList[1].Title.Text = "MS";
            }
            zgc.AxisChange();
            using (Graphics g = this.CreateGraphics())
            {
                // Refigure the axis ranges for the GraphPanes
                master.AxisChange(g);

                // Layout the GraphPanes using a default Pane Layout
                master.SetLayout(g, PaneLayout.SingleColumn);
            }
            zgc.IsSynchronizeXAxes = true;
        }

        public static Form1 _Instance;
        public static Form1 GetInstance()
        {
            if (_Instance == null)
                _Instance = Program.myForm;
            return _Instance;
        }
        public static void Refresh_all()
        {
            GetInstance().R_pic_refresh();
            GetInstance().clicked_pixel(X, Y);
        }

        public void clicked_pixel(int X, int Y)
        {
            picture_R.Refresh();

            GraphPane plot1 = master.PaneList[0];
            GraphPane plot2 = master.PaneList[1];
            plot1.CurveList.Clear();
            plot2.CurveList.Clear();
            plot1.YAxis.Scale.Min = 0;
            plot2.YAxis.Scale.Min = 0;

            LineItem myCurve1 = plot1.AddCurve("Included in calculations", R_matrix.mz_array, R_matrix.scan(Y).Multiply(mz_id_to_process.Add(-0.5).Multiply(2)), Color.Blue, SymbolType.None);
            LineItem myCurve2 = plot2.AddCurve("", R_matrix.mz_array, R_matrix.scan(X).Multiply(mz_id_to_process.Add(-0.5).Multiply(2)), Color.Blue, SymbolType.None);
            LineItem myCurve3 = plot1.AddCurve("Excluded from calculations", R_matrix.mz_array, R_matrix.scan(Y).Multiply(mz_id_to_process.Add(-0.5).Multiply(-2)), Color.Red, SymbolType.None);
            LineItem myCurve4 = plot2.AddCurve("", R_matrix.mz_array, R_matrix.scan(X).Multiply(mz_id_to_process.Add(-0.5).Multiply(-2)), Color.Red, SymbolType.None);

            plot1.Legend.Position = LegendPos.Float;
            plot1.Legend.Location.CoordinateFrame = CoordType.ChartFraction;
            plot1.Legend.Location.AlignH = AlignH.Right;
            plot1.Legend.Location.AlignV = AlignV.Top;
            plot1.Legend.Location.TopLeft = new PointF(1.0f - 0.02f, 0.02f);

            plot1.XAxis.Title.FontSpec.Size = 20.0f;
            plot2.XAxis.Title.FontSpec.Size = 20.0f;
            plot1.YAxis.Title.FontSpec.Size = 20.0f;
            plot2.YAxis.Title.FontSpec.Size = 20.0f;
            plot1.Title.FontSpec.Size = 20.0f;
            plot2.Title.FontSpec.Size = 20.0f;
            

            plot1.XAxis.Title.Text = "m/z, Th";
            plot2.XAxis.Title.Text = "m/z, Th";
            plot1.YAxis.Title.Text = "intensity";
            plot2.YAxis.Title.Text = "intensity";

            plot1.Title.Text = scanNamesList[Y] + " — " + sampleScanList[Y];

            plot2.Title.Text = scanNamesList[X] + " — " + sampleScanList[X];

            labelR.Text = "R=" + (R_matrix.matrix[X, Y]).ToString();

            zgc.AxisChange();
            zgc.Invalidate();
            zgc.Refresh();
        }

        private void picture_R_MouseUp(object sender, MouseEventArgs e)
        {
            /*controlToMove = this.GetChildAtPoint(this.PointToClient(MousePosition));
            Graphics g = controlToMove.CreateGraphics();
            Pen pen = new Pen(Color.Red);
            g.DrawEllipse(pen, xCoordinate, yCoordinate, 2, 2);*/
            if (rr.Length <= 1)
                return;
            Point mousePointerLocation = e.Location;
            xCoordinate = e.X;
            yCoordinate = e.Y;
            var r_width = picture_R.Width;
            var r_height = picture_R.Height;


            var loc2 = splitContainer1.Panel1.ClientSize;
            int shift = (int)(((double)(r_width - r_height)) / 2);
            if (shift > 0)
            {
                X = (int)(((double)((xCoordinate - shift) * scannames_count)) / (double)r_height);
                Y = (int)((double)(yCoordinate * scannames_count) / (double)r_height);
            }
            if (shift < 0)
            {
                X = (int)((double)(xCoordinate * scannames_count) / (double)r_width);
                Y = (int)((double)((yCoordinate + shift) * scannames_count) / (double)r_width);
            }
            if (shift == 0)
            {
                X = (int)((double)(xCoordinate * scannames_count) / (double)r_width);
                Y = (int)((double)(yCoordinate * scannames_count) / (double)r_width);
            }
            if (X > scannames_count - 1)
                X = scannames_count - 1;
            if (Y > scannames_count - 1)
                Y = scannames_count - 1;
            if (X < 0)
                X = 0;
            if (Y < 0)
                Y = 0;
            clicked_pixel(X, Y);
        }

        private void btn_select_R_Click(object sender, EventArgs e)
        {
            Thread newThread = new Thread(new ThreadStart(ThreadMethod));
            newThread.SetApartmentState(ApartmentState.STA);
            newThread.Start();
            while (!open)
            {
                Thread.Sleep(500);
            }
            open = false;
            read_r_matrix(file);
        }

        static void ThreadMethod()
        {

            OpenFileDialog dlg = new OpenFileDialog();

            dlg.Multiselect = false;
            DialogResult result = dlg.ShowDialog();
            if (result == DialogResult.Cancel)
                file = @"E:\ms\Neurosurgery\data\Mening655,499,728_Neur738\data.mat";
            if (result == DialogResult.OK)
                file = dlg.FileName;
            open = true;
        }

        public void ThreadSafeDelegate(MethodInvoker method)
        {
            if (InvokeRequired)
                BeginInvoke(method);
            else
                method.Invoke();
        }

        public void read_r_matrix(string fileName)
        { 
            try
            {
                //var mzxml = new LIMPFileReader.LimpXml(fileName, (float)0, true);

                Cursor = Cursors.WaitCursor;
                var reader = new MatReader(fileName);
                var structure = reader["data"];
                var cType = structure["spectra"].ValueType;
                var fields = structure.Fields;
                
                //var scans = (reader["result"]["scans"].GetValue<int[,]>()).ToDouble();
                var scans = reader["data"]["spectra"].GetValue<double[,]>().Transpose();
                R_matrix.mz_array = reader["data"]["mz"].GetValue<double[,]>().ToJagged()[0];
                mzmin = R_matrix.mz_array[0];
                mzmax = R_matrix.mz_array[R_matrix.mz_array.Length - 1];
                n_precision = (int)(Math.Round(1.0 / (R_matrix.mz_array[1] - R_matrix.mz_array[0])));


                rf = new double[scans.Columns(), scans.Columns()];
                R_matrix.matrix = rf;
                rf2 = new double[rf.Columns(), rf.Rows()];

                R_matrix.scans = scans.Transpose();
                int mzlength = scans.GetLength(0);
                var scannames = reader["data"]["filenames"];
                var scan_of_file = (reader["data"]["scan_of_file"].GetValue<double[,]>()).ToInt32();
                scannames_count = scan_of_file.Length;
                sampleNameList.Clear();
                sampleNameList.Add(scannames[scan_of_file[0,0]].Value.ToString());
                sampleCoordList.Clear();
                sampleCoordList.Add(0.0);
                scanNamesList.Clear();
                int intercept = 0;
                for (int i = 0; i < scannames_count; i++)
                {
                    scanNamesList.Add(scannames[scan_of_file[0,i]].Value.ToString());
                    if (i > 0)
                        if (scannames[scan_of_file[0,i]].Value.ToString() != scannames[scan_of_file[0,i - 1]].Value.ToString())
                        {
                            sampleNameList.Add(scannames[scan_of_file[0,i]].Value.ToString());
                            sampleCoordList.Add(i);
                            intercept = i;
                        }
                    sampleScanList.Add(Convert.ToString(i + 1 - intercept));
                }
                R_matrix.scan_names = sampleScanList;
                R_matrix.file_names = sampleNameList;
                R_matrix.refresh();
                update_figure(threshold);
                Cursor = Cursors.Arrow;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                Cursor = Cursors.Arrow;
                MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                return;
            }
            
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
            display_text();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            display_text();

        }

        public void display_text()
        {
            var r_width = picture_R.Width;
            var r_height = picture_R.Height;
            scale = 0;
            float shift = ((float)(r_width - r_height)) / 2;
            if (shift >= 0)
            {
                scale = ((float)(r_height)) / (float)scannames_count;
                shift = 0;
            }
            if (shift < 0)
            {
                scale = ((float)(r_width)) / (float)scannames_count;
            }
            Graphics gr = pictureBox1.CreateGraphics();
            gr.Clear(System.Drawing.SystemColors.Control);
            Font myFont = new Font("Arial", 10);
            for (int i = 0; i < sampleNameListSelected.Count; i++)
                gr.DrawString(sampleNameListSelected[i], myFont, Brushes.Black, new PointF((float)0.0, -shift + scale * (float)sampleCoordListSelected[i]));
        }

        private void picture_R_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawEllipse(new Pen(Color.GreenYellow, 1f), (float)xCoordinate - 3, (float)yCoordinate - 3, 6, 6);
            e.Graphics.DrawEllipse(new Pen(Color.Red, 1f), (float)xCoordinate - 4, (float)yCoordinate - 4, 8, 8);
            if (WindowsFormsApplication1.Settings_store.Default._grid)
                for (int i = 0; i < sampleCoordListSelected.Count; i++)
                {
                    e.Graphics.DrawLine(new Pen(Color.Red, 1f), (float)(sampleCoordListSelected[i] * scale), (picture_R.Height - scannames_count * scale) / 2, (float)(sampleCoordListSelected[i] * scale), (picture_R.Height + scannames_count * scale) / 2);
                    e.Graphics.DrawLine(new Pen(Color.Red, 1f), (picture_R.Width - scannames_count * scale) / 2, (float)(sampleCoordListSelected[i] * scale), (picture_R.Width + scannames_count * scale) / 2, (float)(sampleCoordListSelected[i] * scale));
                }
            
        }

        public Bitmap double_to_bitmap(double[,] data)
        {
            int[,] cm_local = (int[,])cmap.Clone();

            if (WindowsFormsApplication1.Settings_store.Default._reverse_colormap)
            {
                for (int i = 0, j = 255; i < 256; i++, j--)
                    for (int k = 1; k < 4; k++)
                        cm_local[i, k] = cmap[j, k];
                //data=data.Multiply(-1.0).Add(1.0);
            }

            int width = data.GetLength(1);
            int height = data.GetLength(0);

            Bitmap bitmap;

            int[,] integers = new int[height, width];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    byte[] bgra = (new byte[] { (byte)(cm_local[(int)(data[i, j] * 255),3]),
                        (byte)(cm_local[(int)(data[i, j] * 255),2]),
                        (byte)(cm_local[(int)(data[i, j] * 255),1]),
                        (byte)(cm_local[(int)(data[i, j] * 255),0]) });
                    integers[i, j] = BitConverter.ToInt32(bgra, 0);
                }
            unsafe
            {
                fixed (int* intPtr = &integers[0, 0])
                {
                    bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppRgb, new IntPtr(intPtr));
                }
            }
            Bitmap bmp = new Bitmap(bitmap);
            return bmp;
        }

        public void update_figure(double threshold)
        {
            Cursor = Cursors.WaitCursor;
            GC.Collect();
            
            //spectra_m = scans_rows.ToMatrix();

            var matr = np.array(R_matrix.scans); // shape = N_scans, N_bins_of_mz
            var mz_act = np.array(mz_id_to_process);
            var mat = np.multiply(matr,mz_act);
            mat[mat < threshold * mat.max(new int[1] { 1 }).reshape(new int[2] { -1, 1 })] = (NDarray)0.0;
            
            var matr_norm = np.divide(mat, np.sqrt(np.sum((mat * mat), new int[1] { 1 })).reshape(-1, 1));
            
            matr_norm[np.isnan(matr_norm)] = np.array(0);
            var resR = np.dot(matr_norm, matr_norm.T);

            var rrr = resR.GetData<double>();
            int sz = (int)resR.shape[0];
            rr = new double[sz, sz];
            Buffer.BlockCopy(rrr, 0, rr, 0, rrr.Length * sizeof(double));
            for (int i = 0; i < sz; i++)
                for (int j = 0; j < sz; j++)
                {
                    if (rr[i, j] > 1)
                        rr[i, j] = 1;
                    if (rr[i, j] < 0)
                        rr[i, j] = 0;
                }
            
            GC.Collect();
            Cursor = Cursors.Arrow;

            update_figure_outliers(threshold_outliers);
        }

        public void update_figure_outliers(double threshold_outliers)
        {
            GC.Collect();

            double[] r_profile = rr.Sum(1);
            int[] id_of_selected = r_profile.Find(el => el >= r_profile.Max() - (r_profile.Max() - r_profile.Min()) * (1 - threshold_outliers));
            scannames_count = id_of_selected.Length;
            R_matrix.matrix = new double[scannames_count, scannames_count];

            for (int i = 0; i < scannames_count; i++)
                for (int j = 0; j < scannames_count; j++)
                    R_matrix.matrix[i, j] = rr[id_of_selected[i], id_of_selected[j]];
            
            sampleNameListSelected.Clear();
            sampleCoordListSelected.Clear();
            scanNamesListSelected.Clear();
            sampleCoordListSelected.Add(0.0);
            int intercept = 0;
            sampleNameListSelected.Add(scanNamesList[id_of_selected[0]]);

            for (int i = 0; i < scannames_count; i++)
            {
                scanNamesListSelected.Add(scanNamesList[id_of_selected[i]]);
                if (i > 0)
                    if (scanNamesListSelected[i] != scanNamesListSelected[i - 1])
                    {
                        sampleNameListSelected.Add(scanNamesListSelected[i]);
                        sampleCoordListSelected.Add(i);
                        intercept = i;
                    }
                sampleScanList.Add(Convert.ToString(i + 1 - intercept));
            }

            if (picture_R.Image != null)
                picture_R.Image.Dispose();
            Bitmap bit_map;
            bit_map = double_to_bitmap(R_matrix.matrix);
            picture_R.Image = (System.Drawing.Image)bit_map;
            picture_R.SizeMode = PictureBoxSizeMode.Zoom;
            picture_R.InterpolationMode = InterpolationMode.NearestNeighbor;
            picture_R.Refresh();
            picture_R.Update();
            picture_R.Show();

            clicked_pixel(0, 0);

            if (WindowsFormsApplication1.Settings_store.Default._grid)
            {
                Graphics gr = picture_R.CreateGraphics();
                for (int i = 0; i < sampleCoordListSelected.Count; i++)
                {
                    gr.DrawLine(new Pen(Color.Red, 1f), (float)(sampleCoordListSelected[i] * scale), (pictureBox1.Height - scannames_count * scale) / 2, (float)(sampleCoordListSelected[i] * scale), (pictureBox1.Height + scannames_count * scale) / 2);
                    gr.DrawLine(new Pen(Color.Red, 1f), (pictureBox1.Width - scannames_count * scale) / 2, (float)(sampleCoordListSelected[i] * scale), (pictureBox1.Width + scannames_count * scale) / 2, (float)(sampleCoordListSelected[i] * scale));
                }
            }

            GC.Collect();
            display_text();
        }

        public void R_pic_refresh()
        {
            if (picture_R.Image != null)
                picture_R.Image.Dispose();
            Bitmap bit_map;
            bit_map = double_to_bitmap(R_matrix.matrix);
            picture_R.Image = (System.Drawing.Image)bit_map;
            picture_R.SizeMode = PictureBoxSizeMode.Zoom;
            picture_R.InterpolationMode = InterpolationMode.NearestNeighbor;
            picture_R.Refresh();
            picture_R.Update();
            picture_R.Show();
        }

        private void sliderMSThreshold_MouseUp(object sender, MouseEventArgs e)
        {
            //picture_R.Dispose();
            threshold = (1 + sliderMSThreshold.ManipulatorPosition) / 20;
            if (rr.Length > 1)
                update_figure(threshold);
        }

        private void sliderMSThreshold_PositionChanged(object sender, float position)
        {
            threshold = (1 + sliderMSThreshold.ManipulatorPosition) / 20;
            labelThrLvl.Text = String.Format("Threshold level {0:P1}", threshold);
        }

        private void sliderOutliers_PositionChanged(object sender, float position)
        {
            threshold_outliers = (1 + sliderOutliers.ManipulatorPosition) / 2;
            labelOutliers.Text = String.Format("Outliers level {0:P1}", threshold_outliers);
        }

        private void sliderOutliers_MouseUp(object sender, MouseEventArgs e)
        {
            threshold_outliers = (1 + sliderOutliers.ManipulatorPosition) / 2;
            if (rr.Length > 1)
                update_figure_outliers(threshold_outliers);
        }

        private void zgc_DoubleClick(object sender, EventArgs e)
        {
            Point point = zgc.PointToClient(Cursor.Position);
            if (point.Y > zgc.Height / 2)
                zgc.ZoomOutAll(master.PaneList[1]);
            else
                zgc.ZoomOutAll(master.PaneList[0]);
            zgc.AxisChange();

        }

        private void checkBoxScale_CheckedChanged(object sender, EventArgs e)
        {
            zgc.ZoomOutAll(master.PaneList[0]);
            zgc.ZoomOutAll(master.PaneList[1]);
            if (checkBoxScale.Checked)
            {
                zgc.IsSynchronizeXAxes = true;
            }
            else
                zgc.IsSynchronizeXAxes = false;

        }

        public void update_ccbox()
        {
            ccb_Added.Items.Clear();
            int st_inc = 0;
            int st_exc = 0;
            if (mz_id_to_process.Sum() > 0 && mz_id_to_process.Sum() < R_matrix.mz_array.Length)
            {
                for (int i = 1; i < mz_id_to_process.Length; i++)
                {
                    if (mz_id_to_process[i] != mz_id_to_process[i - 1])
                        if (mz_id_to_process[i - 1] == 0)
                        {
                            CCBoxItem item = new CCBoxItem(R_matrix.mz_array[st_exc], R_matrix.mz_array[i - 1], false);
                            ccb_Added.Items.Add(item);
                            st_inc = i;
                        }
                        else
                        {
                            CCBoxItem item = new CCBoxItem(R_matrix.mz_array[st_inc], R_matrix.mz_array[i - 1], true);
                            ccb_Added.Items.Add(item);
                            st_exc = i;
                        }
                }
                if (mz_id_to_process[mz_id_to_process.Length - 1] == 0)
                {
                    CCBoxItem item = new CCBoxItem(R_matrix.mz_array[st_exc], R_matrix.mz_array[mz_id_to_process.Length - 1], false);
                    ccb_Added.Items.Add(item);
                }
                else
                {
                    CCBoxItem item = new CCBoxItem(R_matrix.mz_array[st_inc], R_matrix.mz_array[mz_id_to_process.Length - 1], true);
                    ccb_Added.Items.Add(item);
                }
            }
            else
            {
                R_matrix.refresh();
            }
        }

        public void update_mz_regions(double st, double en, bool add)
        {
            int st_id = (int)((st - mzmin) * n_precision-1);
            int en_id = (int)((en - mzmin) * n_precision-1);
            if (st_id!=en_id)
                for (int i = st_id; i < en_id; i++)
                    if (add)
                        mz_id_to_process[i] = 1;
                    else
                        mz_id_to_process[i] = 0;

            update_ccbox();
            update_figure(threshold);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!checkBoxScale.Checked)
            {
                checkBoxScale.Checked = true;
                MessageBox.Show("Scale should be the same. It is corrected now.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                try
                {
                    GraphPane plot1 = master.PaneList[0];
                    double st = plot1.XAxis.Scale.Min;
                    double en = plot1.XAxis.Scale.Max;
                    if (st < R_matrix.mz_array.Min())
                        st = R_matrix.mz_array.Min();
                    if (en > R_matrix.mz_array.Max())
                        en = R_matrix.mz_array.Max();
                    if (mz_id_to_process.Sum() == R_matrix.mz_array.Length)
                        mz_id_to_process.Clear();
                    update_mz_regions(st, en, true);
                }
                catch(Exception)
                { }
                //CCBoxItem item = new CCBoxItem(st,en, true);
                //ccb_Added.Items.Add(item);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (!checkBoxScale.Checked)
            {
                checkBoxScale.Checked = true;
                MessageBox.Show("Scale should be the same. It is corrected now.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                try { 
                GraphPane plot1 = master.PaneList[0];
                double st = plot1.XAxis.Scale.Min;
                double en = plot1.XAxis.Scale.Max;
                if (st < R_matrix.mz_array.Min())
                    st = R_matrix.mz_array.Min();
                if (en > R_matrix.mz_array.Max())
                    en = R_matrix.mz_array.Max();
                update_mz_regions(st, en, false);
                    //CCBoxItem item = new CCBoxItem(st, en, false);
                    //ccb_Added.Items.Add(item);
                }
                catch (Exception)
                { }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (ccb_Added.Items.Count != 0)
            {
                mz_id_to_process.Clear();
                ccb_Added.Items.Clear();
                R_matrix.refresh();
                update_figure(threshold);
            }
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Settings"] == null)
            {
                fm_settings.Show();
                //new Settings().Show();
            }
            else
            {
                fm_settings.Show();
                fm_settings.Focus();
            }
        }

        private bool CheckOpened(string name)
        {
            FormCollection fc = Application.OpenForms;

            foreach (Form frm in fc)
            {
                if (frm.Text == name)
                {
                    return true;
                }
            }
            return false;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (rf.Length > 1)
            {
                if (keyData == Keys.Up)
                {
                    double[] sz = { picture_R.Width, picture_R.Height };
                    Y--;
                    yCoordinate -= (sz.Min() / scannames_count);
                    if (yCoordinate < picture_R.Height / 2 - scale * scannames_count / 2)
                        yCoordinate = (picture_R.Height / 2 - scale * scannames_count / 2);
                    if (Y < 0)
                        Y = 0;
                    clicked_pixel(X, Y);
                    return true;
                }
                if (keyData == Keys.Down)
                {
                    double[] sz = { picture_R.Width, picture_R.Height };
                    Y++;
                    yCoordinate += (sz.Min() / scannames_count);
                    if (yCoordinate > picture_R.Height / 2 + scale * scannames_count / 2)
                        yCoordinate = (picture_R.Height / 2 + scale * scannames_count / 2);
                    if (Y > scannames_count - 1)
                        Y = scannames_count - 1;
                    clicked_pixel(X, Y);
                    return true;
                }
                if (keyData == Keys.Left)
                {
                    double[] sz = { picture_R.Width, picture_R.Height };
                    X--;
                    xCoordinate -= (sz.Min() / scannames_count);
                    if (xCoordinate < picture_R.Width / 2 - scale * scannames_count / 2)
                        xCoordinate = (picture_R.Width / 2 - scale * scannames_count / 2);
                    if (X < 0)
                        X = 0;
                    clicked_pixel(X, Y);
                    return true;
                }
                if (keyData == Keys.Right)
                {
                    double[] sz = { picture_R.Width, picture_R.Height };
                    X++;
                    xCoordinate += (sz.Min() / scannames_count);
                    if (xCoordinate > picture_R.Width / 2 + scale * scannames_count / 2)
                        xCoordinate = (picture_R.Width / 2 + scale * scannames_count / 2);
                    if (X > scannames_count - 1)
                        X = scannames_count - 1;
                    clicked_pixel(X, Y);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void contextMenuStripSave_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem==contextMenuStripSave.Items[0])
            {
                save_image();
            }
            else
                if (e.ClickedItem==contextMenuStripSave.Items[1])
            {
                save_data();
            }
        }

        public void save_image()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PNG Image|*.png|EMF Image|*.emf";
            saveFileDialog1.Title = "Save an Image File";
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        picture_R.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        picture_R.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        picture_R.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case 4:
                        picture_R.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Png);
                        break;
                    case 5:
                        picture_R.Image.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Emf);
                        break;
                }

                fs.Close();
            }
        }

        public void save_data()
        {
            
        }

        private void buttonRemoveRange_Click(object sender, EventArgs e)
        {
            if (ccb_Added.CheckedItems.Count == 0)
            {
                MessageBox.Show("You have no selected m/z range.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
            else
            {
                foreach (CCBoxItem item in ccb_Added.CheckedItems)
                {
                    int st_id = (int)((item.Min - mzmin) * n_precision - 1);
                    int en_id = (int)((item.Max - mzmin) * n_precision);
                    for (int i = st_id; i < en_id; i++)
                        mz_id_to_process[i] = 1 - mz_id_to_process[i];
                }
                update_ccbox();
                update_figure(threshold);
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                clicked_pixel(X, Y);
            }
            catch (Exception)
            { }
        }

        public class R_matrix : Form1
        {
            public static double[,] matrix { get; set; }

            public static double[,] scans { get; set; }

            public static double[] scan(int scan_number)
            {
                int width = scans.GetLength(1);
                double[] scan_local = new double[width];
                for (int i = 0; i < width; i++)
                    scan_local[i] = scans[scan_number,i];
                //Buffer.BlockCopy(scans, width * scan_number, scan_local, 0, width * sizeof(double));
                return scan_local;
            }

            public static List<string> file_names { get; set; }

            public static List<string> scan_names { get; set; }

            public static double[] mz_array { get; set; }

            public static void refresh()
            {
                int mz_array_length = scans.GetLength(1);
                mz_array = new double[mz_array_length];

                mz_id_to_process = new int[mz_array_length];

                for (int i = 0; i < mz_array_length; i++)
                {
                    mz_id_to_process[i] = 1;
                    mz_array[i] = (double)(i + 1) / n_precision + mzmin;
                }
            }
        }


    }


    public class ColorMap
    {
        private int colormapLength = 256;
        private int alphaValue = 255;

        public ColorMap()
        {
        }

        public ColorMap(int colorLength)
        {
            colormapLength = colorLength;
        }

        public ColorMap(int colorLength, int alpha)
        {
            colormapLength = colorLength;
            alphaValue = alpha;
        }

        public int[,] Spring()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] spring = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                spring[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 255;
                cmap[i, 2] = (int)(255 * spring[i]);
                cmap[i, 3] = 255 - cmap[i, 1];
            }
            return cmap;
        }

        public int[,] Summer()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] summer = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                summer[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (int)(255 * summer[i]);
                cmap[i, 2] = (int)(255 * 0.5f * (1 + summer[i]));
                cmap[i, 3] = (int)(255 * 0.4f);
            }
            return cmap;
        }

        public int[,] Autumn()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] autumn = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                autumn[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 255;
                cmap[i, 2] = (int)(255 * autumn[i]);
                cmap[i, 3] = 0;
            }
            return cmap;
        }

        public int[,] Winter()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] winter = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                winter[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = 0;
                cmap[i, 2] = (int)(255 * winter[i]);
                cmap[i, 3] = (int)(255 * (1.0f - 0.5f * winter[i]));
            }
            return cmap;
        }

        public int[,] Gray()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] gray = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                gray[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (int)(255 - 255 * gray[i]);
                cmap[i, 2] = (int)(255 - 255 * gray[i]);
                cmap[i, 3] = (int)(255 - 255 * gray[i]);
            }
            return cmap;
        }

        public int[,] Jet()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[,] cMatrix = new float[colormapLength, 3];
            int n = (int)Math.Ceiling(colormapLength / 4.0f);
            int nMod = 0;
            float[] fArray = new float[3 * n - 1];
            int[] red = new int[fArray.Length];
            int[] green = new int[fArray.Length];
            int[] blue = new int[fArray.Length];

            if (colormapLength % 4 == 1)
            {
                nMod = 1;
            }

            for (int i = 0; i < fArray.Length; i++)
            {
                if (i < n)
                    fArray[i] = (float)(i + 1) / n;
                else if (i >= n && i < 2 * n - 1)
                    fArray[i] = 1.0f;
                else if (i >= 2 * n - 1)
                    fArray[i] = (float)(3 * n - 1 - i) / n;
                green[i] = (int)Math.Ceiling(n / 2.0f) - nMod + i;
                red[i] = green[i] + n;
                blue[i] = green[i] - n;
            }

            int nb = 0;
            for (int i = 0; i < blue.Length; i++)
            {
                if (blue[i] > 0)
                    nb++;
            }

            for (int i = 0; i < colormapLength; i++)
            {
                for (int j = 0; j < red.Length; j++)
                {
                    if (i == red[j] && red[j] < colormapLength)
                    {
                        cMatrix[i, 0] = fArray[i - red[0]];
                    }
                }
                for (int j = 0; j < green.Length; j++)
                {
                    if (i == green[j] && green[j] < colormapLength)
                        cMatrix[i, 1] = fArray[i - (int)green[0]];
                }
                for (int j = 0; j < blue.Length; j++)
                {
                    if (i == blue[j] && blue[j] >= 0)
                        cMatrix[i, 2] = fArray[fArray.Length - 1 - nb + i];
                }
            }

            for (int i = 0; i < colormapLength; i++)
            {
                cmap[i, 0] = alphaValue;
                for (int j = 0; j < 3; j++)
                {
                    cmap[i, j + 1] = (int)(cMatrix[i, j] * 255);
                }
            }
            return cmap;
        }

        public int[,] Hot()
        {
            int[,] cmap = new int[colormapLength, 4];
            int n = 3 * colormapLength / 8;
            float[] red = new float[colormapLength];
            float[] green = new float[colormapLength];
            float[] blue = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                if (i < n)
                    red[i] = 1.0f * (i + 1) / n;
                else
                    red[i] = 1.0f;
                if (i < n)
                    green[i] = 0f;
                else if (i >= n && i < 2 * n)
                    green[i] = 1.0f * (i + 1 - n) / n;
                else
                    green[i] = 1f;
                if (i < 2 * n)
                    blue[i] = 0f;
                else
                    blue[i] = 1.0f * (i + 1 - 2 * n) / (colormapLength - 2 * n);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (int)(255 * red[i]);
                cmap[i, 2] = (int)(255 * green[i]);
                cmap[i, 3] = (int)(255 * blue[i]);
            }
            return cmap;
        }

        public int[,] Cool()
        {
            int[,] cmap = new int[colormapLength, 4];
            float[] cool = new float[colormapLength];
            for (int i = 0; i < colormapLength; i++)
            {
                cool[i] = 1.0f * i / (colormapLength - 1);
                cmap[i, 0] = alphaValue;
                cmap[i, 1] = (int)(255 * cool[i]);
                cmap[i, 2] = (int)(255 * (1 - cool[i]));
                cmap[i, 3] = 255;
            }
            return cmap;
        }
    }
}
