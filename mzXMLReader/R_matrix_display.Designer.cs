using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace R_matrix_visualization
{
    public class PictureBoxWithInterpolationMode : PictureBox
    {
        public InterpolationMode InterpolationMode { get; set; }

        protected override void OnPaint(PaintEventArgs paintEventArgs)
        {
            paintEventArgs.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            paintEventArgs.Graphics.InterpolationMode = InterpolationMode;
            base.OnPaint(paintEventArgs);
        }
    }

    public class DropDownButton : Button
    {
        int rightPadding = 16;

        public DropDownButton()
        {
            Padding = new Padding(0, 0, rightPadding, 0);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            var rect = new Rectangle(ClientRectangle.Width - rightPadding, ClientRectangle.Top + 3, rightPadding - 3, ClientRectangle.Height - 6);

            int x = ClientRectangle.Width - rightPadding + 3;
            int y = ClientRectangle.Height / 2 - 1;

            var brush = Enabled ? SystemBrushes.ControlText : SystemBrushes.ButtonShadow;
            var arrows = new Point[] { new Point(x, y), new Point(x + 7, y), new Point(x + 3, y + 4) };
            pevent.Graphics.FillPolygon(brush, arrows);
            pevent.Graphics.DrawLine(Pens.Silver, rect.Left, rect.Top, rect.Left, rect.Bottom);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.X > Width - rightPadding && e.Button == MouseButtons.Left)
            {
                if (ContextMenuStrip != null)
                {
                    ContextMenuStrip.MinimumSize = new Size(Width - rightPadding, 0);
                    ContextMenuStrip.Show(this, 0, Height);
                }
            }
            else
                base.OnMouseDown(e);
        }
    }

    partial class Form1
    {

        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btn_select_R = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.picture_R = new R_matrix_visualization.PictureBoxWithInterpolationMode();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.zgc = new ZedGraph.ZedGraphControl();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.sliderOutliers = new Accord.Controls.SliderControl();
            this.sliderMSThreshold = new Accord.Controls.SliderControl();
            this.panel4 = new System.Windows.Forms.Panel();
            this.labelOutliers = new System.Windows.Forms.Label();
            this.labelThrLvl = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBoxScale = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Button();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnSave = new R_matrix_visualization.DropDownButton();
            this.contextMenuStripSave = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveImageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel10 = new System.Windows.Forms.Panel();
            this.buttonRemoveRange = new System.Windows.Forms.Button();
            this.ccb_Added = new R_matrix_visualization.CheckedComboBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.panel7 = new System.Windows.Forms.Panel();
            this.btnReset = new System.Windows.Forms.Button();
            this.labelR = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picture_R)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel9.SuspendLayout();
            this.contextMenuStripSave.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel10.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_select_R
            // 
            this.btn_select_R.Dock = System.Windows.Forms.DockStyle.Top;
            this.btn_select_R.Location = new System.Drawing.Point(0, 0);
            this.btn_select_R.Name = "btn_select_R";
            this.btn_select_R.Size = new System.Drawing.Size(81, 22);
            this.btn_select_R.TabIndex = 1;
            this.btn_select_R.Text = "Open data";
            this.btn_select_R.UseVisualStyleBackColor = true;
            this.btn_select_R.Click += new System.EventHandler(this.btn_select_R_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel2.Controls.Add(this.zgc);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(933, 480);
            this.splitContainer1.SplitterDistance = 544;
            this.splitContainer1.TabIndex = 5;
            // 
            // splitContainer3
            // 
            this.splitContainer3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.Panel1.Controls.Add(this.picture_R);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer3.Panel2.Controls.Add(this.pictureBox1);
            this.splitContainer3.Size = new System.Drawing.Size(544, 480);
            this.splitContainer3.SplitterDistance = 480;
            this.splitContainer3.TabIndex = 4;
            // 
            // picture_R
            // 
            this.picture_R.BackColor = System.Drawing.SystemColors.Control;
            this.picture_R.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picture_R.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Default;
            this.picture_R.Location = new System.Drawing.Point(0, 0);
            this.picture_R.Name = "picture_R";
            this.picture_R.Size = new System.Drawing.Size(480, 480);
            this.picture_R.TabIndex = 2;
            this.picture_R.TabStop = false;
            this.picture_R.Paint += new System.Windows.Forms.PaintEventHandler(this.picture_R_Paint);
            this.picture_R.MouseUp += new System.Windows.Forms.MouseEventHandler(this.picture_R_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 480);
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
            // 
            // zgc
            // 
            this.zgc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zgc.Location = new System.Drawing.Point(0, 45);
            this.zgc.Name = "zgc";
            this.zgc.ScrollGrace = 0D;
            this.zgc.ScrollMaxX = 0D;
            this.zgc.ScrollMaxY = 0D;
            this.zgc.ScrollMaxY2 = 0D;
            this.zgc.ScrollMinX = 0D;
            this.zgc.ScrollMinY = 0D;
            this.zgc.ScrollMinY2 = 0D;
            this.zgc.Size = new System.Drawing.Size(385, 382);
            this.zgc.TabIndex = 5;
            this.zgc.ContextMenuBuilder += new ZedGraph.ZedGraphControl.ContextMenuBuilderEventHandler(this.zgc_ContextMenuBuilder);
            this.zgc.DoubleClick += new System.EventHandler(this.zgc_DoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 427);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(385, 53);
            this.panel2.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.sliderOutliers);
            this.panel5.Controls.Add(this.sliderMSThreshold);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(115, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(177, 53);
            this.panel5.TabIndex = 8;
            // 
            // sliderOutliers
            // 
            this.sliderOutliers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sliderOutliers.Location = new System.Drawing.Point(0, 30);
            this.sliderOutliers.ManipulatorPosition = -1F;
            this.sliderOutliers.Name = "sliderOutliers";
            this.sliderOutliers.NegativeAreaBrush = System.Drawing.Color.White;
            this.sliderOutliers.ResetPositionOnMouseRelease = false;
            this.sliderOutliers.Size = new System.Drawing.Size(177, 23);
            this.sliderOutliers.TabIndex = 1;
            this.sliderOutliers.Text = "sliderControl2";
            this.sliderOutliers.PositionChanged += new Accord.Controls.SliderControl.PositionChangedHandler(this.sliderOutliers_PositionChanged);
            this.sliderOutliers.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sliderOutliers_MouseUp);
            // 
            // sliderMSThreshold
            // 
            this.sliderMSThreshold.Dock = System.Windows.Forms.DockStyle.Top;
            this.sliderMSThreshold.Location = new System.Drawing.Point(0, 0);
            this.sliderMSThreshold.ManipulatorPosition = -1F;
            this.sliderMSThreshold.Name = "sliderMSThreshold";
            this.sliderMSThreshold.NegativeAreaBrush = System.Drawing.Color.White;
            this.sliderMSThreshold.ResetPositionOnMouseRelease = false;
            this.sliderMSThreshold.Size = new System.Drawing.Size(177, 23);
            this.sliderMSThreshold.TabIndex = 0;
            this.sliderMSThreshold.Text = "sliderControl1";
            this.sliderMSThreshold.PositionChanged += new Accord.Controls.SliderControl.PositionChangedHandler(this.sliderMSThreshold_PositionChanged);
            this.sliderMSThreshold.MouseUp += new System.Windows.Forms.MouseEventHandler(this.sliderMSThreshold_MouseUp);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.labelOutliers);
            this.panel4.Controls.Add(this.labelThrLvl);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(115, 53);
            this.panel4.TabIndex = 7;
            // 
            // labelOutliers
            // 
            this.labelOutliers.AutoSize = true;
            this.labelOutliers.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelOutliers.Location = new System.Drawing.Point(0, 35);
            this.labelOutliers.Name = "labelOutliers";
            this.labelOutliers.Padding = new System.Windows.Forms.Padding(5, 0, 0, 5);
            this.labelOutliers.Size = new System.Drawing.Size(89, 18);
            this.labelOutliers.TabIndex = 6;
            this.labelOutliers.Text = "Outliers level 0%";
            // 
            // labelThrLvl
            // 
            this.labelThrLvl.AutoSize = true;
            this.labelThrLvl.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelThrLvl.Location = new System.Drawing.Point(0, 0);
            this.labelThrLvl.Name = "labelThrLvl";
            this.labelThrLvl.Padding = new System.Windows.Forms.Padding(5, 5, 0, 0);
            this.labelThrLvl.Size = new System.Drawing.Size(101, 18);
            this.labelThrLvl.TabIndex = 5;
            this.labelThrLvl.Text = "Threshold level 0%";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBoxScale);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(292, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(93, 53);
            this.panel3.TabIndex = 6;
            // 
            // checkBoxScale
            // 
            this.checkBoxScale.AutoSize = true;
            this.checkBoxScale.Checked = true;
            this.checkBoxScale.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxScale.Location = new System.Drawing.Point(5, 6);
            this.checkBoxScale.Name = "checkBoxScale";
            this.checkBoxScale.Size = new System.Drawing.Size(91, 17);
            this.checkBoxScale.TabIndex = 1;
            this.checkBoxScale.Text = "Same X scale";
            this.checkBoxScale.UseVisualStyleBackColor = true;
            this.checkBoxScale.CheckedChanged += new System.EventHandler(this.checkBoxScale_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSettings);
            this.panel1.Controls.Add(this.panel9);
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.labelR);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(385, 45);
            this.panel1.TabIndex = 5;
            // 
            // btnSettings
            // 
            this.btnSettings.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSettings.Location = new System.Drawing.Point(48, 0);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(56, 22);
            this.btnSettings.TabIndex = 12;
            this.btnSettings.Text = "Settings";
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.btnSave);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(0, 22);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(104, 23);
            this.panel9.TabIndex = 14;
            // 
            // btnSave
            // 
            this.btnSave.ContextMenuStrip = this.contextMenuStripSave;
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSave.Location = new System.Drawing.Point(40, 0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.btnSave.Size = new System.Drawing.Size(64, 23);
            this.btnSave.TabIndex = 13;
            this.btnSave.Text = "Save results";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // contextMenuStripSave
            // 
            this.contextMenuStripSave.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveImageToolStripMenuItem1,
            this.saveImageToolStripMenuItem,
            this.saveDataToolStripMenuItem});
            this.contextMenuStripSave.Name = "contextMenuStrip1";
            this.contextMenuStripSave.Size = new System.Drawing.Size(158, 70);
            this.contextMenuStripSave.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStripSave_ItemClicked);
            // 
            // saveImageToolStripMenuItem1
            // 
            this.saveImageToolStripMenuItem1.Name = "saveImageToolStripMenuItem1";
            this.saveImageToolStripMenuItem1.Size = new System.Drawing.Size(157, 22);
            this.saveImageToolStripMenuItem1.Text = "Save images";
            // 
            // saveImageToolStripMenuItem
            // 
            this.saveImageToolStripMenuItem.Name = "saveImageToolStripMenuItem";
            this.saveImageToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.saveImageToolStripMenuItem.Text = "Save image as...";
            // 
            // saveDataToolStripMenuItem
            // 
            this.saveDataToolStripMenuItem.Name = "saveDataToolStripMenuItem";
            this.saveDataToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.saveDataToolStripMenuItem.Text = "Save data";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.panel10);
            this.panel8.Controls.Add(this.panel6);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel8.Location = new System.Drawing.Point(104, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(200, 45);
            this.panel8.TabIndex = 10;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.buttonRemoveRange);
            this.panel10.Controls.Add(this.ccb_Added);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel10.Location = new System.Drawing.Point(0, 22);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(200, 23);
            this.panel10.TabIndex = 9;
            // 
            // buttonRemoveRange
            // 
            this.buttonRemoveRange.Dock = System.Windows.Forms.DockStyle.Left;
            this.buttonRemoveRange.Location = new System.Drawing.Point(0, 0);
            this.buttonRemoveRange.Name = "buttonRemoveRange";
            this.buttonRemoveRange.Size = new System.Drawing.Size(55, 23);
            this.buttonRemoveRange.TabIndex = 8;
            this.buttonRemoveRange.Text = "Remove";
            this.buttonRemoveRange.UseVisualStyleBackColor = true;
            this.buttonRemoveRange.Click += new System.EventHandler(this.buttonRemoveRange_Click);
            // 
            // ccb_Added
            // 
            this.ccb_Added.CheckOnClick = true;
            this.ccb_Added.Dock = System.Windows.Forms.DockStyle.Right;
            this.ccb_Added.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.ccb_Added.DropDownHeight = 1;
            this.ccb_Added.FormattingEnabled = true;
            this.ccb_Added.IntegralHeight = false;
            this.ccb_Added.Location = new System.Drawing.Point(61, 0);
            this.ccb_Added.Name = "ccb_Added";
            this.ccb_Added.Size = new System.Drawing.Size(139, 21);
            this.ccb_Added.TabIndex = 7;
            this.ccb_Added.ValueSeparator = ", ";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnRemove);
            this.panel6.Controls.Add(this.btnAdd);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(200, 22);
            this.panel6.TabIndex = 8;
            // 
            // btnRemove
            // 
            this.btnRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnRemove.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnRemove.Location = new System.Drawing.Point(118, 0);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(82, 22);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "-";
            this.btnRemove.UseVisualStyleBackColor = false;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.btnAdd.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAdd.Location = new System.Drawing.Point(0, 0);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(84, 22);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "+";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.btn_select_R);
            this.panel7.Controls.Add(this.btnReset);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel7.Location = new System.Drawing.Point(304, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(81, 45);
            this.panel7.TabIndex = 9;
            // 
            // btnReset
            // 
            this.btnReset.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnReset.Location = new System.Drawing.Point(0, 22);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(81, 23);
            this.btnReset.TabIndex = 5;
            this.btnReset.Text = "Reset ranges";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // labelR
            // 
            this.labelR.AutoSize = true;
            this.labelR.Location = new System.Drawing.Point(4, 4);
            this.labelR.Name = "labelR";
            this.labelR.Size = new System.Drawing.Size(15, 13);
            this.labelR.TabIndex = 2;
            this.labelR.Text = "R";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(933, 480);
            this.Controls.Add(this.splitContainer1);
            this.Name = "Form1";
            this.Text = "SSM display";
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picture_R)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.contextMenuStripSave.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel10.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_select_R;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private PictureBoxWithInterpolationMode picture_R;
        private PictureBox pictureBox1;
        private SplitContainer splitContainer3;
        private CheckBox checkBoxScale;
        private Label labelR;
        private Panel panel4;
        private Label labelOutliers;
        private Label labelThrLvl;
        private Panel panel3;
        private Panel panel5;
        private Accord.Controls.SliderControl sliderOutliers;
        private Accord.Controls.SliderControl sliderMSThreshold;
        private ZedGraph.ZedGraphControl zgc;
        private Button btnRemove;
        private Button btnAdd;
        private Button btnReset;
        private CheckedComboBox ccb_Added;
        private Panel panel7;
        private Panel panel8;
        private Button btnSettings;
        //private Button btnSave;
        private DropDownButton btnSave;
        private ContextMenuStrip contextMenuStripSave;
        private ToolStripMenuItem saveImageToolStripMenuItem;
        private ToolStripMenuItem saveDataToolStripMenuItem;
        private Panel panel9;
        private Panel panel10;
        private Button buttonRemoveRange;
        private Panel panel6;
        private ToolStripMenuItem saveImageToolStripMenuItem1;
    }
}

