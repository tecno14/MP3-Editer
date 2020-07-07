using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VarispeedDemo.SoundTouch;
using MetroFramework.Forms;
using MetroFramework.Controls;
using mmaudio.Clss;
using System.Threading;

namespace mmaudio.Frms
{
    public partial class Mainfrm : MetroForm
    {
        [Obsolete]
        public Mainfrm()
        {
            InitializeComponent();
            groupBox3.Visible = false;
            tableLayoutPanel1.Dock = DockStyle.Fill;
            this.Size = new Size(500, 500);
            ChgTheme(MetroFramework.MetroThemeStyle.Dark, MetroFramework.MetroColorStyle.Blue);
            this.Theme = MetroFramework.MetroThemeStyle.Default;
            Program.Editer = new MyEditer(this, metroPanel1);
            Program.MSM = metroStyleManager1;
            RefrechTrackbarWidth();
            LbStatus.Text = "Ready";
            LbTime.Text = Program.Editer.GetTimeStatus();
            PBVolume.Value = 100;
            Program.Editer.SetVolume(1);
        }
        public void ChgTheme(MetroFramework.MetroThemeStyle Theme, MetroFramework.MetroColorStyle Style)
        {
            metroStyleManager1.Style = Style;
            metroStyleManager1.Theme = Theme;
            tableLayoutPanel1.BackColor = Theme == MetroFramework.MetroThemeStyle.Light ? Color.FromArgb(0, 174, 220) : Color.FromArgb(23, 23, 23);
            tableLayoutPanel3.BackColor = Theme == MetroFramework.MetroThemeStyle.Light ? Color.FromArgb(0, 174, 220) : Color.FromArgb(32, 32, 32);
            this.StyleManager = metroStyleManager1;
        }
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string tmpstatus = LbStatus.Text;
            LbStatus.Text = "Adding";
            //open new file 
            try 
            {
                OpenFileDialog d = new OpenFileDialog
                {
                    Filter = "MP3 file|*.mp3",
                    Title = "Choose file"
                };

                if (d.ShowDialog() == DialogResult.Cancel)
                {
                    LbStatus.Text = tmpstatus;
                    return;
                }

                TrackUC frm = Program.Editer.Add(d.FileName);
                frm.Location = new Point(0, 0);// GetBarHigh() + (frm.Size.Height + 6) * (Program.Editer.GetTrackCount() - 1));
                metroPanel1.VerticalScroll.Value = metroPanel1.VerticalScroll.Maximum;
                metroPanel1.HorizontalScroll.Value = metroPanel1.HorizontalScroll.Minimum;
                metroPanel1.Controls.Add(frm);
                RefrechTracksPosiotions();
                //RefrechTrackbarWidth();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            LbStatus.Text = tmpstatus;
        }

        public int RefrechTrackbarWidth()
        {
            metroPanel1.Refresh();
            int max = 0;
            for (int i = 0; i < metroPanel1.Controls.Count; i++)         
                if (metroPanel1.Controls[i] is TrackUC)
                    max = Math.Max(metroPanel1.Controls[i].Size.Width + metroPanel1.Controls[i].Location.X, max);

            TbAll.Size = new Size(max, TbAll.Size.Height);
            metroPanel1.Refresh();
            return max;
        }
        public void RefrechTracksPosiotions()
        {
            int j = 0;
            for (int i = 0; i < metroPanel1.Controls.Count; i++)
                if (metroPanel1.Controls[i] is TrackUC)
                {
                    metroPanel1.Controls[i].Location = new Point(metroPanel1.Controls[i].Location.X, GetBarHigh() + (metroPanel1.Controls[i].Size.Height + 6) * j);
                    j++;
                }
        }
        public int GetBarHigh()
        {
            return TbAll.Size.Height + TbAll.Margin.Top + TbAll.Margin.Bottom;
        }

        [Obsolete]
        public void BtnPlay_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.Editer.GetTrackCount() == 0)
                    BtnAdd_Click(sender, e);
                if (Program.Editer.GetTrackCount() < 1)
                    return;
                ///...
                //scroll left and resize trackbar
                metroPanel1_Click(sender, e);
                //merge
                //foreach track add offset then merge
                metroPanel1.HorizontalScroll.Value = metroPanel1.HorizontalScroll.Minimum;
                LbStatus.Text = "Merging";

                Program.Editer.MeargeAll(RefrechTrackbarWidth());
                if (TbAll.Value == TbAll.Maximum)
                    BtnStop_Click(sender, e);
                Program.Editer.Play(TbAll.Value);
                timer1.Enabled = true;
                LbStatus.Text = "Playing";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }
        public void SetPanalWidth(int x)
        {
            metroPanel5.Size = new Size(x, metroPanel5.Size.Height);
        }
        public int GetPanalWidth()
        {
            return metroPanel5.Size.Width;
        }
        public int GetTrackbarWidth()
        {
            return TbAll.Size.Width;
        }
        private void metroPanel1_Click(object sender, EventArgs e)
        {
            metroPanel1.HorizontalScroll.Value = metroPanel1.HorizontalScroll.Minimum;
            RefrechTrackbarWidth();
        }

        private void metroPanel1_MouseEnter(object sender, EventArgs e)
        {
            Program.Editer.UnFocuseAll();
        }

        private void BtnPause_Click(object sender, EventArgs e)
        {
            Program.Editer.Pause();
            timer1.Enabled = false;
            LbStatus.Text = "Paused";
        }

        public void BtnStop_Click(object sender, EventArgs e)
        {
            Program.Editer.Stop();
            timer1.Enabled = false;
            LbStatus.Text = "Stoped";
        }

        public void SeekBar(int x)
        {
            hScrollBar1.Value = x;
            TbAll.Value = x;
            LbTime.Text = Program.Editer.GetTimeStatus();
        }
        bool ScrollByCode = false;
        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            if (ScrollByCode)
                return;
            ScrollByCode = true;
            //x=lenght*value/1000
            long pos = e.NewValue * Program.Editer.GetLength() / 1000;
            SeekBar(e.NewValue);
            Program.Editer.SeekReader(pos);
            LbTime.Text = Program.Editer.GetTimeStatus();
            ScrollByCode = false;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            SeekBar((int)(Program.Editer.GetPosition() * 1000 / Program.Editer.GetLength()));        
        }

        bool VolumeMouseDown = false;

        [Obsolete]
        private void PBVolume_MouseDown(object sender, MouseEventArgs e)
        {
            VolumeMouseDown = true;
            PBVolume_MouseMove(sender, e);
        }

        private void PBVolume_MouseUp(object sender, MouseEventArgs e)
        {
            VolumeMouseDown = false;
        }
        [Obsolete]
        private void PBVolume_MouseMove(object sender, MouseEventArgs e)
        {
            if (!VolumeMouseDown)
                return;
            float vol = (float)(e.Location.X * 100 / PBVolume.Width);
            vol = Math.Max(Math.Min(vol, 100), 0);
            PBVolume.Value = (int)vol;
            Program.Editer.SetVolume(vol / 100);
        }




















        /// <summary>
        /// Old Code
        /// </summary>

        WaveStream reader;
        //WaveOut wout;
        IWavePlayer wout;
        VarispeedSampleProvider speedControl;
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Filter = "MP3 file|*.mp3";
            d.Title = "Choose file";
            if (d.ShowDialog() == DialogResult.Cancel)
                return;
            
            Mp3FileReader reader = new Mp3FileReader(d.FileName);
            
            listView1.Items.Clear();
            listView1.Items.Add(new ListViewItem(
                    new String[] { "Average bytes per second", reader.Mp3WaveFormat.AverageBytesPerSecond.ToString() }
                ));
            listView1.Items.Add(new ListViewItem(
                    new String[] { "Bits per sample", reader.WaveFormat.BitsPerSample.ToString() }
                ));
            listView1.Items.Add(new ListViewItem(
                    new String[] { "Channels", reader.WaveFormat.Channels.ToString() }
                ));
            listView1.Items.Add(new ListViewItem(
                    new String[] { "Encoding", reader.Mp3WaveFormat.Encoding.ToString() }
                ));
            listView1.Items.Add(new ListViewItem(
                    new String[] { "Sample rate", reader.WaveFormat.SampleRate.ToString() }
                ));


            this.reader?.Dispose();
            this.reader = null;

            speedControl?.Dispose();
            speedControl = null;

            wout?.Dispose();
            wout = null;

            this.reader = reader;
            hScrollBar11.Value = 0;
            progressBar1.Value = 0;

            var useTempo = radioButton1.Checked;
            speedControl = new VarispeedSampleProvider(this.reader.ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));

        }
        private void playToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reader == null)
                return;

            reader.Seek(0, System.IO.SeekOrigin.Begin);

            wout?.Dispose();
            wout = new WaveOut();
            wout.Init(speedControl);
            wout.Play();

            timer1.Enabled = true;
        }
        private void pauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reader == null)
                return;

            wout.Pause();
            timer1.Enabled = false;
        }
        private void resumeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reader == null)
                return;

            if (wout?.PlaybackState == PlaybackState.Playing)
                return;

            wout?.Dispose();
            wout = new WaveOut();///
            wout.Init(speedControl);
            wout.Play();

            timer1.Enabled = true;
            /*
            wout = new WaveOut();
            wout.Init(speedControl);
            wout.Play();
            timer1.Enabled = true;
            */
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reader == null)
                return;

            if (wout == null)//|| wout.PlaybackState != PlaybackState.Playing)
                return;

            timer1.Enabled = false;
            wout.Stop();
            //reader.Seek(0, System.IO.SeekOrigin.Begin);
            hScrollBar11.Value = 0;
            progressBar1.Value = 0;
        }


        private void progressBar1_MouseClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show("progressBar1_MouseClick" + e.X.ToString());
            if (reader == null)
                return;

            long pos = e.X * reader.Length / progressBar1.Width;
            reader.Seek(pos, System.IO.SeekOrigin.Begin);
        }

        short[] GetDataOf16Bit2ChannelFile(WaveStream reader)
        {
            reader.Seek(0, System.IO.SeekOrigin.Begin);
            byte[] data = new byte[reader.Length];
            reader.Read(data, 0, data.Length);
            short[] res = new short[data.Length / 2];
            for (int i = 0; i < data.Length; i += 2)
            {
                res[i / 2] = BitConverter.ToInt16(data, i);
            }
            return res;
        }

        /*private void reverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reader == null)
                return;
            var data = GetDataOf16Bit2ChannelFile(reader);
            data = data.Reverse().ToArray();

            //reader.Seek(0, System.IO.SeekOrigin.Begin);
            //byte[] result = new byte[data.Length * sizeof(short)];
            //Buffer.BlockCopy(data, 0, result, 0, result.Length);
            
            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            foreach (var item in data)
                writer.Write(item);
            
            //reader.Write(result, 0, result.Length);

            //reader = new RawSourceWaveStream(mstream, reader.WaveFormat);

            var useTempo = radioButton1.Checked;
            
            speedControl = new VarispeedSampleProvider(new RawSourceWaveStream(mstream, reader.WaveFormat).ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));

            //speedControl = new VarispeedSampleProvider(new RawSourceWaveStream(mstream, reader2.WaveFormat).ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));

            wout?.Stop();
            wout = new WaveOut();
            wout.Init(speedControl);

            return;
            /*
            WaveStream waveStream = WaveFormatConversionStream.CreatePcmStream(reader2);
            WaveFileWriter waveFileWriter = new WaveFileWriter(reader2, waveStream.WaveFormat);

            waveFileWriter.WriteAsync(result, 0, result.Length);
            waveFileWriter.Flush();
            
        }*/

        private void reverseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reader == null)
                return;

            var data = GetDataOf16Bit2ChannelFile(reader);
            data = data.Reverse().ToArray();

            /*
            reader.Seek(0, System.IO.SeekOrigin.Begin);
            byte[] result = new byte[data.Length * sizeof(short)];
            Buffer.BlockCopy(data, 0, result, 0, result.Length);
            */

            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            foreach (var item in data)
                writer.Write(item);

            var useTempo = radioButton1.Checked;
            reader = new RawSourceWaveStream(mstream, reader.WaveFormat);
            speedControl = new VarispeedSampleProvider(reader.ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));

            var state = wout?.PlaybackState == PlaybackState.Playing;
            wout?.Stop();
            hScrollBar11.Value = 0;
            progressBar1.Value = 0;
            wout = new WaveOut();///
            wout.Init(speedControl);

            if (state)
                wout.Play();

            return;
        }
        private void makeFasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (reader == null)
                return;
            var data = GetDataOf16Bit2ChannelFile(reader);
            short[] newData = new short[data.Length * 2];
            for (int i = 0; i < data.Length; i += 2)
            {
                newData[i * 2] = data[i];
                newData[i * 2 + 1] = data[i + 1];
                newData[i * 2 + 2] = data[i];
                newData[i * 2 + 3] = data[i + 1];
            }

            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            foreach (var item in data)
                writer.Write(item);

            var useTempo = radioButton1.Checked;
            reader = new RawSourceWaveStream(mstream, reader.WaveFormat);
            speedControl = new VarispeedSampleProvider(reader.ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));

            var state = wout?.PlaybackState == PlaybackState.Playing;
            wout?.Stop();
            hScrollBar11.Value = 0;
            progressBar1.Value = 0;
            wout = new WaveOut();///
            wout.Init(speedControl);

            if (state)
                wout.Play();

            return;
        }
        private void makeFasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (reader == null)
                return;
            var data = GetDataOf16Bit2ChannelFile(reader);

            int newSize = data.Length / 2;

            if (data.Length % 2 != 0)
                newSize++;

            short[] newData = new short[newSize];
            for (int i = 0; i < newSize; i += 4)
            {
                newData[i / 2] = data[i];
                newData[i / 2 + 1] = data[i + 1];
            }

            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            foreach (var item in data)
                writer.Write(item);

            var useTempo = radioButton1.Checked;
            reader = new RawSourceWaveStream(mstream, reader.WaveFormat);
            speedControl = new VarispeedSampleProvider(reader.ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));

            var state = wout?.PlaybackState == PlaybackState.Playing;
            wout?.Stop();
            hScrollBar11.Value = 0;
            progressBar1.Value = 0;
            wout = new WaveOut();///
            wout.Init(speedControl);

            if (state)
                wout.Play();

            return;
        }

        private void Editbtn_Click(object sender, EventArgs e)
        {
            if (reader == null)
                return;
            try
            {
                float volume;
                if (!float.TryParse(Volumetb.Text, out volume))
                    throw new Exception("volume value not valid");

                var data = GetDataOf16Bit2ChannelFile(reader);
                volume /= 100;
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = (short)Math.Max(Math.Min(data[i] * volume, (int)short.MaxValue), (int)short.MinValue);
                }

                MemoryStream mstream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(mstream);
                foreach (var item in data)
                    writer.Write(item);

                var useTempo = radioButton1.Checked;
                reader = new RawSourceWaveStream(mstream, reader.WaveFormat);
                speedControl = new VarispeedSampleProvider(reader.ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));

                var state = wout?.PlaybackState == PlaybackState.Playing;
                wout?.Stop();
                hScrollBar11.Value = 0;
                progressBar1.Value = 0;
                wout = new WaveOut();///
                wout.Init(speedControl);

                if (state)
                    wout.Play();

                return;

                /*var state = wout.PlaybackState == PlaybackState.Playing;
                wout?.Stop();
                wout = new WaveOut();///
                wout.Init(speedControl);

                if (state)
                    wout.Play();
                //wout.Init(reader2);*/
                /*
                var data = GetDataOf16Bit2ChannelFile(reader);
                //short[] newData = new short[data.Length];
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] *= volume;
                }

                MemoryStream mstream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(mstream);
                foreach (var item in data)
                    writer.Write(item);

                var useTempo = radioButton1.Checked;
                reader = new RawSourceWaveStream(mstream, reader.WaveFormat);
                speedControl = new VarispeedSampleProvider(reader.ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));

                var state = wout?.PlaybackState == PlaybackState.Playing;
                wout?.Stop();
                hScrollBar1.Value = 0;
                progressBar1.Value = 0;
                wout = new WaveOut();///
                wout.Init(speedControl);

                if (state)
                    wout.Play();

                return;*/
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void Speedtb_TextChanged(object sender, EventArgs e)
        {
            if (reader == null)
                return;
            try
            {
                float speed;
                if (!float.TryParse(Speedtb.Text, out speed))
                    throw new Exception("speed value not valid");

                //speed
                var useTempo = radioButton1.Checked;
                speedControl = new VarispeedSampleProvider(reader.ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));
                speedControl.PlaybackRate = speed;

                var state = wout.PlaybackState == PlaybackState.Playing;
                wout?.Stop();
                wout = new WaveOut();///
                wout.Init(speedControl);

                if (state)
                    wout.Play();
                //wout.Init(reader2);
            }
            catch 
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (reader == null)
                return;
            try
            {
                float volume;
                if (!float.TryParse(Volumetb2.Text, out volume))
                    throw new Exception("speed value not valid");

                volume = Math.Max(0, volume);
                volume = Math.Min(volume, 100);
                volume /= 100;
                //volume               
                var state = wout.PlaybackState == PlaybackState.Playing;
                wout?.Stop();
                wout = new WaveOut();///
                //wout.Volume = volume;
                wout.Init(speedControl);

                if (state)
                    wout.Play();
                //wout.Init(reader2);
            }
            catch 
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        [Obsolete]
        private void BtnSaveas_Click(object sender, EventArgs e)
        {
            try
            {
                Program.Editer.SaveFile(true);
                LbStatus.Text = "Saved Done";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }

        [Obsolete]
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Program.Editer.SaveFile(false);
                LbStatus.Text = "Saved Done";
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }

        }
    }
}
