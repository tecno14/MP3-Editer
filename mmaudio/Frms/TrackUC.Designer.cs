using System;
using System.Windows.Forms;

namespace mmaudio
{
    partial class TrackUC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.TbTrackName = new MetroFramework.Controls.MetroTextBox();
            this.BtnDeleteTrack = new MetroFramework.Controls.MetroButton();
            this.BtnMove = new MetroFramework.Controls.MetroButton();
            this.metroProgressBar1 = new MetroFramework.Controls.MetroProgressBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.metroToolTip1 = new MetroFramework.Components.MetroToolTip();
            this.BtView = new MetroFramework.Controls.MetroButton();
            this.metroContextMenu1 = new MetroFramework.Controls.MetroContextMenu(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.reverseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.volumeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbVolume = new System.Windows.Forms.ToolStripTextBox();
            this.doToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbSpeed = new System.Windows.Forms.ToolStripTextBox();
            this.doToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.waveViewer1 = new NAudio.Gui.WaveViewer();
            this.tableLayoutPanel2.SuspendLayout();
            this.metroContextMenu1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.TbTrackName, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.BtnDeleteTrack, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.BtnMove, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 68);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(581, 25);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // TbTrackName
            // 
            this.TbTrackName.BackColor = System.Drawing.Color.Black;
            // 
            // 
            // 
            this.TbTrackName.CustomButton.Image = null;
            this.TbTrackName.CustomButton.Location = new System.Drawing.Point(488, 1);
            this.TbTrackName.CustomButton.Name = "";
            this.TbTrackName.CustomButton.Size = new System.Drawing.Size(17, 17);
            this.TbTrackName.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.TbTrackName.CustomButton.TabIndex = 1;
            this.TbTrackName.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.TbTrackName.CustomButton.UseSelectable = true;
            this.TbTrackName.CustomButton.Visible = false;
            this.TbTrackName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TbTrackName.FontWeight = MetroFramework.MetroTextBoxWeight.Light;
            this.TbTrackName.ForeColor = System.Drawing.Color.White;
            this.TbTrackName.Lines = new string[0];
            this.TbTrackName.Location = new System.Drawing.Point(72, 3);
            this.TbTrackName.MaxLength = 32767;
            this.TbTrackName.Name = "TbTrackName";
            this.TbTrackName.PasswordChar = '\0';
            this.TbTrackName.PromptText = "Track name";
            this.TbTrackName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.TbTrackName.SelectedText = "";
            this.TbTrackName.SelectionLength = 0;
            this.TbTrackName.SelectionStart = 0;
            this.TbTrackName.ShortcutsEnabled = true;
            this.TbTrackName.Size = new System.Drawing.Size(506, 19);
            this.TbTrackName.TabIndex = 0;
            this.TbTrackName.UseSelectable = true;
            this.TbTrackName.WaterMark = "Track name";
            this.TbTrackName.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.TbTrackName.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            this.TbTrackName.TextChanged += new System.EventHandler(this.TbTrackName_TextChanged);
            this.TbTrackName.Click += new System.EventHandler(this.TbTrackName_Click);
            this.TbTrackName.MouseEnter += new System.EventHandler(this.TbTrackName_MouseEnter);
            this.TbTrackName.MouseLeave += new System.EventHandler(this.TbTrackName_MouseLeave);
            this.TbTrackName.MouseHover += new System.EventHandler(this.TbTrackName_MouseHover);
            // 
            // BtnDeleteTrack
            // 
            this.BtnDeleteTrack.Location = new System.Drawing.Point(45, 3);
            this.BtnDeleteTrack.Name = "BtnDeleteTrack";
            this.BtnDeleteTrack.Size = new System.Drawing.Size(21, 19);
            this.BtnDeleteTrack.TabIndex = 2;
            this.BtnDeleteTrack.Text = "X";
            this.BtnDeleteTrack.UseSelectable = true;
            this.BtnDeleteTrack.Click += new System.EventHandler(this.BtnDeleteTrack_Click);
            // 
            // BtnMove
            // 
            this.BtnMove.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.BtnMove.Location = new System.Drawing.Point(3, 3);
            this.BtnMove.Name = "BtnMove";
            this.BtnMove.Size = new System.Drawing.Size(36, 19);
            this.BtnMove.TabIndex = 3;
            this.BtnMove.Text = "<>";
            this.BtnMove.UseSelectable = true;
            this.BtnMove.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnMove_MouseDown);
            this.BtnMove.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BtnMove_MouseMove);
            this.BtnMove.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtnMove_MouseUp);
            // 
            // metroProgressBar1
            // 
            this.metroProgressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.metroProgressBar1.Location = new System.Drawing.Point(0, 0);
            this.metroProgressBar1.Name = "metroProgressBar1";
            this.metroProgressBar1.Size = new System.Drawing.Size(581, 3);
            this.metroProgressBar1.TabIndex = 1;
            this.metroProgressBar1.Value = 100;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // metroToolTip1
            // 
            this.metroToolTip1.AutoPopDelay = 10000;
            this.metroToolTip1.InitialDelay = 500;
            this.metroToolTip1.ReshowDelay = 100;
            this.metroToolTip1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroToolTip1.StyleManager = null;
            this.metroToolTip1.Theme = MetroFramework.MetroThemeStyle.Light;
            // 
            // BtView
            // 
            this.BtView.BackgroundImage = global::mmaudio.Properties.Resources.x1080;
            this.BtView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtView.ContextMenuStrip = this.metroContextMenu1;
            this.BtView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtView.Location = new System.Drawing.Point(0, 3);
            this.BtView.Name = "BtView";
            this.BtView.Size = new System.Drawing.Size(581, 65);
            this.BtView.TabIndex = 1;
            this.BtView.UseCustomBackColor = true;
            this.BtView.UseSelectable = true;
            this.BtView.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtView_MouseDown);
            this.BtView.MouseMove += new System.Windows.Forms.MouseEventHandler(this.BtView_MouseMove);
            this.BtView.MouseUp += new System.Windows.Forms.MouseEventHandler(this.BtView_MouseUp);
            // 
            // metroContextMenu1
            // 
            this.metroContextMenu1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.toolStripSeparator1,
            this.reverseToolStripMenuItem,
            this.volumeToolStripMenuItem,
            this.speedToolStripMenuItem});
            this.metroContextMenu1.Name = "metroContextMenu1";
            this.metroContextMenu1.Size = new System.Drawing.Size(115, 142);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(111, 6);
            // 
            // reverseToolStripMenuItem
            // 
            this.reverseToolStripMenuItem.Name = "reverseToolStripMenuItem";
            this.reverseToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.reverseToolStripMenuItem.Text = "Reverse";
            this.reverseToolStripMenuItem.Click += new System.EventHandler(this.reverseToolStripMenuItem_Click);
            // 
            // volumeToolStripMenuItem
            // 
            this.volumeToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbVolume,
            this.doToolStripMenuItem});
            this.volumeToolStripMenuItem.Name = "volumeToolStripMenuItem";
            this.volumeToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.volumeToolStripMenuItem.Text = "Volume";
            // 
            // tbVolume
            // 
            this.tbVolume.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbVolume.Name = "tbVolume";
            this.tbVolume.Size = new System.Drawing.Size(100, 23);
            this.tbVolume.Text = "100";
            // 
            // doToolStripMenuItem
            // 
            this.doToolStripMenuItem.Name = "doToolStripMenuItem";
            this.doToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.doToolStripMenuItem.Text = "Do (0-100)";
            this.doToolStripMenuItem.Click += new System.EventHandler(this.doToolStripMenuItem_Click);
            // 
            // speedToolStripMenuItem
            // 
            this.speedToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tbSpeed,
            this.doToolStripMenuItem1});
            this.speedToolStripMenuItem.Name = "speedToolStripMenuItem";
            this.speedToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.speedToolStripMenuItem.Text = "Speed";
            // 
            // tbSpeed
            // 
            this.tbSpeed.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tbSpeed.Name = "tbSpeed";
            this.tbSpeed.Size = new System.Drawing.Size(100, 23);
            // 
            // doToolStripMenuItem1
            // 
            this.doToolStripMenuItem1.Name = "doToolStripMenuItem1";
            this.doToolStripMenuItem1.Size = new System.Drawing.Size(160, 22);
            this.doToolStripMenuItem1.Text = "Do";
            this.doToolStripMenuItem1.Click += new System.EventHandler(this.doToolStripMenuItem1_Click);
            // 
            // waveViewer1
            // 
            this.waveViewer1.BackColor = System.Drawing.Color.DarkGray;
            this.waveViewer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.waveViewer1.Location = new System.Drawing.Point(0, 3);
            this.waveViewer1.Name = "waveViewer1";
            this.waveViewer1.SamplesPerPixel = 128;
            this.waveViewer1.Size = new System.Drawing.Size(105, 65);
            this.waveViewer1.StartPosition = ((long)(0));
            this.waveViewer1.TabIndex = 2;
            this.waveViewer1.Visible = false;
            this.waveViewer1.WaveStream = null;
            // 
            // TrackUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.waveViewer1);
            this.Controls.Add(this.BtView);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.metroProgressBar1);
            this.Name = "TrackUC";
            this.Size = new System.Drawing.Size(581, 93);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.metroContextMenu1.ResumeLayout(false);
            this.ResumeLayout(false);

        }



        #endregion
        private MetroFramework.Controls.MetroProgressBar metroProgressBar1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private MetroFramework.Controls.MetroTextBox TbTrackName;
        public MetroFramework.Controls.MetroButton BtView;
        private MetroFramework.Controls.MetroButton BtnDeleteTrack;
        private MetroFramework.Controls.MetroButton BtnMove;
        private Timer timer1;
        private MetroFramework.Components.MetroToolTip metroToolTip1;
        private NAudio.Gui.WaveViewer waveViewer1;
        private MetroFramework.Controls.MetroContextMenu metroContextMenu1;
        private ToolStripMenuItem reverseToolStripMenuItem;
        private ToolStripMenuItem volumeToolStripMenuItem;
        private ToolStripMenuItem speedToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripTextBox tbVolume;
        private ToolStripMenuItem doToolStripMenuItem;
        private ToolStripTextBox tbSpeed;
        private ToolStripMenuItem doToolStripMenuItem1;
    }
}
