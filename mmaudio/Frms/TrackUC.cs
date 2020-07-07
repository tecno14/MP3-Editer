using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using System.Diagnostics;
using System.Threading;
using NAudio.Wave;
using System.Drawing.Drawing2D;

namespace mmaudio
{
    public partial class TrackUC : MetroUserControl
    {
        public int id;
        //public object track;
        //public WaveStream reader;
        public string txt;
        Bitmap bkupImg;

        public TrackUC(int id = -1)
        {
            InitializeComponent();
            this.id = id;
            ///this.StyleManager = Program.MSM;
            this.BtnDeleteTrack.StyleManager = Program.MSM;
            this.BtnMove.StyleManager = Program.MSM;
            this.TbTrackName.StyleManager = Program.MSM;
            this.metroContextMenu1.StyleManager = Program.MSM;
            timer1.Interval = 500;
            timer1.Start();
        }
        public void DrowWave(WaveStream reader,int Sample)
        {
            //Method1:
            //waveViewer1.SamplesPerPixel = Sample / waveViewer1.Width;
            //waveViewer1.WaveStream = reader;
            
            //Method2:

            int SamplePerPixel = Sample / BtView.Width;

            int size = (short.MaxValue * 2);

            short[] data = Program.Editer.GetDataOf16Bit2ChannelFile(reader);
            int[] Map = new int[((data.Length / 2) / SamplePerPixel) + 1];//[BtView.Height];
            short t=0;
            int j = 0;
            int Mid = BtView.Height / 2;
            for (int i = 0; i < data.Length; i+=2)
            {
                t += (short)((data[i] + data[i + 1]) / 2);
                if((i/2)%SamplePerPixel==0)
                {
                    //Debug.WriteLine("datalength " + data.Length + " j " + j);
                    Map[j] = (int)(t * BtView.Height / size);//x = value * hight / size
                    t = 0;j++;
                }
            }
            Bitmap bmp = new Bitmap(Map.Length, BtView.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(32, 32, 32));
                Pen pen = new Pen(Color.FromArgb(0, 174, 219), 1);
                for (int i = 0; i < Map.Length; i++)
                {
                    g.DrawLine(pen, i, Mid, i, -Map[i] + Mid);
                }
            }
            BtnDeleteTrack.BackColor = Color.FromArgb(32, 32, 32);
            bkupImg = bmp;
            BtView.BackgroundImage = bmp;
        }

        private void BtnDeleteTrack_Click(object sender, EventArgs e)
        {
            Program.Editer.DeleteTrack(this.id);
        }

        public void SetName(string txt, bool WaterMark = true)
        {
            if (WaterMark)
                TbTrackName.WaterMark = txt;
            else
                TbTrackName.Text = txt;
            this.txt = txt;
        }
        public void SetToolTib(string txt)
        {
            metroToolTip1.SetToolTip(this.TbTrackName, txt);
        }

        public void SetLength(int l)
        {
            this.Size = new Size(l, this.Size.Height);
        }
        public void ChgTheme()
        {
            ///this.StyleManager = Program.MSM;
        }
        int MPoint1;
        int BPoint1;
        bool down = false;
        private void BtnMove_MouseDown(object sender, MouseEventArgs e)
        {
            MPoint1 = e.X;
            BPoint1 = Program.Editer.GetTrackLocation(id).X;
            //Point1 = Program.Editer.GetLocation(id).X;
            //Point1 = e.X;
            down = true;
            //MessageBox.Show("e.Location.X " + e.Location.X +
            //    "e.Location.Y " + e.Location.Y +
            //    "e.X " + e.X
            //    );
        }
        
        private void BtnMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (!down)
                return;

            int tmp = e.X - MPoint1;
            tmp += BPoint1;
            //tmp = Math.Min(tmp, Program.Editer.MP.Width - this.Width);//tmp < Program.Editer.MP.Width - this.Width ? tmp : Program.Editer.MP.Width - this.Width;//
            tmp = Math.Max(0, tmp);
            //Debug.WriteLine(tmp);
            Program.Editer.SetTrackLocation(id, tmp);
        }
        private void BtnMove_MouseUp(object sender, MouseEventArgs e)
        {
            if (!down)
                return;
            down = false;
            //Program.Editer.RefrechTrackBarAndPanalSize();
        }
        private void BtView_Click(object sender, EventArgs e)
        {
            //Program.Editer.SetTrackLocation(id, 10);
        }

        private void TbTrackName_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            TbTrackName.Text = txt;
        }

        private void TbTrackName_MouseLeave(object sender, EventArgs e)
        {
            //txt = TbTrackName.Text;
            timer1.Start();
        }
        public void UnFocuse()
        {
            TbTrackName_MouseLeave(null, null);
            BtnMove.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (TbTrackName.Text.Length > (TbTrackName.Size.Width / 10))
                TbTrackName.Text = TbTrackName.Text.Remove(0, Math.Min(5, TbTrackName.Text.Length));
            else
                TbTrackName.Text = txt;
        }

        private void TbTrackName_MouseEnter(object sender, EventArgs e)
        {
            timer1.Stop();
            TbTrackName.Text = txt;
        }

        private void TbTrackName_TextChanged(object sender, EventArgs e)
        {
            if (timer1.Enabled)
                return;
            txt = TbTrackName.Text;
        }

        private void TbTrackName_MouseHover(object sender, EventArgs e)
        {
            timer1.Stop();
            TbTrackName.Text = txt;
        }

        private void reverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Editer.Reverse(this.id);
        }

        private void BtView_MouseUp(object sender, MouseEventArgs e)
        {
            if (downSelect)//e.Button == MouseButtons.Right)
            {
                Position2 = e.X;
                copyToolStripMenuItem.Enabled = false;
                pasteToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem.Enabled = false;

                if ((Position1 != -1) && (Position2 != -1) && (Position1 != Position2))
                {
                    copyToolStripMenuItem.Enabled = true;
                    cutToolStripMenuItem.Enabled = true;
                }
                if (Program.Editer.clipbord != null)
                    pasteToolStripMenuItem.Enabled = true;

                BtView.ContextMenuStrip.Show(BtView, new Point(e.X, e.Y));
            }
            downSelect = false;
        }

        int Position1 = -1, Position2 = -1;
        bool downSelect = false;
        private void BtView_MouseMove(object sender, MouseEventArgs e)
        {
            if (!downSelect)
                return;
            if (Position1 == -1)
                return;

            this.Refresh();
            int width = Math.Abs(e.X - Position1), height = BtView.Height;
            Rectangle rect = new Rectangle(
                       Math.Min(e.X, Position1),
                       0,
                       width,
                       height - (BtView.Margin.Bottom * 2));

            Graphics formGraphics = BtView.CreateGraphics();
            Pen drwaPen = new Pen(Color.White, 1.5f) { DashStyle = DashStyle.Solid };
            formGraphics.DrawRectangle(drwaPen, rect);

        }

        private void BtView_MouseDown(object sender, MouseEventArgs e)
        {
            //Position2 = -1;
            Position1 = e.X;
            downSelect = true;
            BtView_MouseMove(sender, e);
        }

        private void doToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Editer.ChgVolume(this.id, tbVolume.Text);//Volume
        }

        private void doToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.Editer.ChgSpeed(this.id, tbSpeed.Text);//Speed
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Editer.Copy(id, Position1, Position2);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Editer.Paste(id, Position2);
        }

        private void BtView_MouseClick(object sender, MouseEventArgs e)
        {
            Position2 = e.X;
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Editer.Cut(id, Position1, Position2);
        }

       
    }
}
