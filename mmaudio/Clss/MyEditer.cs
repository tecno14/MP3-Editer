using MetroFramework.Controls;
using mmaudio.Clss;
using mmaudio.Frms;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VarispeedDemo.SoundTouch;

namespace mmaudio.Clss
{
    class MyEditer
    {
        public Mainfrm MainUI;
        public MetroPanel MP;
        public List<Track> TrackList;

        public int SampleRate = int.MaxValue;
        public int Channels = 2;
        public int BitsPerSample = 16;

        public bool NewSampleRate = false;

        public short[] clipbord;

        IWavePlayer wout;
        VarispeedSampleProvider speedControl;
        WaveStream reader2;
        bool editeflag = true;
        float Volume = 1;

        public bool EditeFlag { set { editeflag = value; if (value) Stop(); } get { return editeflag; } }

        public MyEditer(Mainfrm MainUI, MetroPanel MP)
        {
            this.MainUI = MainUI;
            this.MP = MP;
            DeleteAll();
        }
        public Track GetTrackById(int id)
        {
            foreach (Track item in TrackList)
                if (item.id == id)
                    return item;
            return null;
        }
        public int GetNextId()
        {
            return GetTrackCount() == 0 ? 0 : TrackList[GetTrackCount() - 1].id + 1;
        }
        public TrackUC Add(string FileName)
        {
            NewSampleRate = false;
            EditeFlag = true;
            Track t = new Track(GetNextId(), FileName);
            StandardizSampleRateAll();
            t.UI.StyleManager = MainUI.StyleManager;

            if (t.reader.TotalTime.TotalSeconds > int.MaxValue)
                throw new Exception("to larg file : " + t.reader.TotalTime.Minutes + " Minutes");

            TrackList.Add(t);
            return t.UI;
        }
        public void DeleteTrack(int id)
        {
            EditeFlag = true;
            for (int i = 0; i < MP.Controls.Count; i++)
                if (MP.Controls[i] is TrackUC)
                    if (((TrackUC)MP.Controls[i]).id == id)
                    {
                        MP.Controls.RemoveAt(i);
                        i--;
                    }
            for (int i = 0; i < TrackList.Count; i++)
                if (TrackList[i].id == id)
                {
                    TrackList.RemoveAt(i);
                    i--;
                }
            MainUI.RefrechTracksPosiotions();
            MainUI.RefrechTrackbarWidth();
        }


        public int GetTrackCount()
        {
            return TrackList.Count;
        }

        public void UnFocuseAll()
        {
            for (int i = 0; i < MP.Controls.Count; i++)
                if (MP.Controls[i] is TrackUC)
                    ((TrackUC)MP.Controls[i]).UnFocuse();
        }

        public void DeleteAll()
        {
            TrackList = new List<Track>();
            for (int i = 0; i < MP.Controls.Count; i++)
                if (MP.Controls[i] is TrackUC)
                {
                    MP.Controls.RemoveAt(i);
                    i--;
                }
        }

        public Point GetTrackLocation(int id)
        {
            for (int i = 0; i < MP.Controls.Count; i++)
                if (MP.Controls[i] is TrackUC)
                    if (((TrackUC)MP.Controls[i]).id == id)
                        return MP.Controls[i].Location;
            return new Point();
        }
        public void SetTrackLocation(int id, int XLocation)
        {
            EditeFlag = true;
            for (int i = 0; i < MP.Controls.Count; i++)
                if (MP.Controls[i] is TrackUC)
                    if (((TrackUC)MP.Controls[i]).id == id)
                    {
                        MP.Controls[i].Location = new Point(XLocation, MP.Controls[i].Location.Y);
                        //MainUI.RefrechTrackbarWidth();
                        return;
                    }
        }
       
        public short[] GetDataOf16Bit2ChannelFile(WaveStream reader)
        {
            reader.Seek(0, System.IO.SeekOrigin.Begin);
            byte[] data2 = new byte[reader.Length];
            reader.Read(data2, 0, data2.Length);

            short[] res2 = new short[data2.Length / 2];
            Buffer.BlockCopy(data2, 0, res2, 0, data2.Length);

            return res2;

            /*
            reader.Seek(0, System.IO.SeekOrigin.Begin);
            byte[] data = new byte[reader.Length];
            reader.Read(data, 0, data.Length);
            short[] res = new short[data.Length / 2];
            for (int i = 0; i < data.Length; i += 2)
                res[i / 2] = BitConverter.ToInt16(data, i);

            
            // do we have the same sequence?
            Debug.WriteLine(res.SequenceEqual(res2));    // True

            return res;
            */
        }
        public void MeargeAll(int MaxLenght)
        {
            if (!EditeFlag)
                return;
            StandardizSampleRateAll();
            //44100*(reader.TotalTime.TotalSeconds+1)*2*2 byte//(/2 short)  
            long Size = (int)(SampleRate * (MaxLenght) * 2);
            short[] result = new short[Size];
            for (int i = 0; i < MP.Controls.Count; i++)
                if (MP.Controls[i] is TrackUC)
                {
                    short[] data = GetDataOf16Bit2ChannelFile(GetTrackById(((TrackUC)MP.Controls[i]).id).reader);
                    //44100*(reader.TotalTime.TotalSeconds+1)*2*2 byte//(/2 short)  
                    //int Start = MP.Controls[i].Location.X;

                    //long Start = (int)(SampleRate * (MP.Controls[i].Location.X + 1) * 2);
                    long Start = Size * (MP.Controls[i].Location.X) / (MaxLenght);
                    //int End = Start + MP.Controls[i].Size.Width;
                    for (long j = 0; j < data.Length; j++)
                    {
                        long ind = j + Start;
                        //result[ind] = (short)((result[ind] + data[j]) / 2);
                        result.SetValue((short)((result[ind] + data[j]) / 2), ind);
                    }
                }

            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            foreach (var item in result)
                writer.Write(item);
            reader2 = new RawSourceWaveStream(
                    mstream, new WaveFormat(SampleRate, BitsPerSample, Channels)
                );

            EditeFlag = false;
        }

        [Obsolete]
        public void Play(long x = 0)
        {
            if (reader2 == null)
                return;

            if (wout?.PlaybackState == PlaybackState.Playing)
                return;

            long x2 = x * Program.Editer.GetLength() / 1000;
            if (x2 > reader2.Length)
                x = x2 = 0;
            reader2.Seek(x2, System.IO.SeekOrigin.Begin);
            var useTempo = true;//radioButton1.Checked;
            speedControl = new VarispeedSampleProvider(this.reader2.ToSampleProvider(), 100, new SoundTouchProfile(useTempo, false));

            wout?.Stop();
            wout?.Dispose();
            //hScrollBar11.Value = 0;
            //progressBar1.Value = 0;
            MainUI.SeekBar((int)x);
            wout = new WaveOut();
            wout.Init(speedControl);
            wout.Volume = Volume;
            wout.Play();
        }
        public void Pause()
        {
            if (reader2 == null)
                return;

            if (wout?.PlaybackState != PlaybackState.Playing)
                return;

            wout?.Pause();
        }
        public void Stop()
        {
            if (reader2 == null)
                return;

            if (wout == null)//|| wout.PlaybackState != PlaybackState.Playing)
                return;

            //timer1.Enabled = false;
            wout.Stop();
            MainUI.SeekBar(0);
        }
        public void SeekReader(long x)
        {
            if (reader2 == null)
                return;
            reader2.Seek(x, System.IO.SeekOrigin.Begin);
        }
        public long GetPosition()
        {
            if (reader2 == null)
                return 0;
            return reader2.Position;
        }
        public long GetLength()
        {
            if (reader2 == null)
                return 0;
            return reader2.Length;
        }
        public string GetTimeStatus()
        {
            //00:00 / 00:00 
            if (reader2 == null)
                return "00:00:00 / 00:00:00 ";
            return new DateTime(reader2.CurrentTime.Ticks).ToString("HH:mm:ss") +
                " / " +
                new DateTime(reader2.TotalTime.Ticks).ToString("HH:mm:ss");
        }

        [Obsolete]
        public void SetVolume(float v)
        {
            //this.Volume = Math.Max(Math.Min(v, 1), 0);
            this.Volume = v;
            if (wout != null)
                wout.Volume = v;
        }
        public void Reverse(int id)
        {
            Pause();
            EditeFlag = true;
            Track t = GetTrackById(id);
            if (t.reader == null)
                return;
            var data = GetDataOf16Bit2ChannelFile(t.reader);
            data = data.Reverse().ToArray();
            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            foreach (var item in data)
                writer.Write(item);
            t.reader = new RawSourceWaveStream(
                    mstream, t.reader.WaveFormat
                );
            t.DrowWave();
        }
        public void ChgVolume(int id, string txt)
        {
            try
            {
                Track t = GetTrackById(id);
                if (t?.reader == null)
                    return;

                float volume;
                if (!float.TryParse(txt, out volume))
                    throw new Exception("volume value not valid");

                var data = GetDataOf16Bit2ChannelFile(t.reader);
                volume /= 100;
                for (int i = 0; i < data.Length; i++)
                {
                    data[i] = (short)Math.Max(Math.Min(data[i] * volume, short.MaxValue), short.MinValue);
                }

                MemoryStream mstream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(mstream);
                foreach (var item in data)
                    writer.Write(item);

                t.reader = new RawSourceWaveStream(mstream, t.reader.WaveFormat);
                t.DrowWave();
                EditeFlag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ChgSpeed(int id, string txt)
        {
            try
            {
                Track t = GetTrackById(id);
                if (t?.reader == null)
                    return;

                float ratio;
                if (!float.TryParse(txt, out ratio))
                    throw new Exception("speed value not valid");

                var data = GetDataOf16Bit2ChannelFile(t.reader);
                int newL = (int)((data.Length / 2) * ratio) * 2;
                var newData = new short[newL];
                for (int i = 0; i < newData.Length; i += 2)
                {
                    int index = (int)((i / 2) / ratio) * 2;
                    newData[i] = data[index];
                    newData[i + 1] = data[index + 1];
                }

                MemoryStream mstream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(mstream);
                foreach (var item in data)
                    writer.Write(item);

                EditeFlag = true;
                t.reader = new RawSourceWaveStream(mstream, t.reader.WaveFormat);
                t.DrowWave();
                t.UI.SetLength((int)t.reader.TotalTime.TotalSeconds + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public void SetChannels(WaveStream ws)
        {
            WaveFormat wf = new WaveFormat(ws.WaveFormat.SampleRate, BitsPerSample, Channels);
            var data = GetDataOf16Bit2ChannelFile(ws);
            var NewData = new short[data.Length * 2];
            for (int i = 0; i < data.Length; i++)
            {
                NewData[i * 2] = data[i];
                NewData[i * 2 + 1] = data[i];
            }
            MemoryStream mstream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(mstream);
            foreach (var item in data)
                writer.Write(item);
            EditeFlag = true;
            ws = new RawSourceWaveStream(mstream, wf);
        }
        public void StandardizSampleRateAll()
        {
            NewSampleRate = false;
            foreach (Track t in TrackList)
            {
                t.StandardizSampleRate();
                t.SetLength();
                t.DrowWave();
                if (!NewSampleRate)
                    continue;
                StandardizSampleRateAll();
                return;
            }
        }
        public Byte[] WavToMP3(WaveStream ws)//byte[] wavFile)
        {
            //using (MemoryStream source = new MemoryStream(wavFile))
            //using (NAudio.Wave.WaveFileReader rdr = new NAudio.Wave.WaveFileReader(source))
            {
                WaveLib.WaveFormat fmt = new WaveLib.WaveFormat(SampleRate, BitsPerSample, Channels);
                //new WaveLib.WaveFormat(rdr.WaveFormat.SampleRate, rdr.WaveFormat.BitsPerSample, rdr.WaveFormat.Channels);

                // convert to MP3 at 96kbit/sec...
                Yeti.Lame.BE_CONFIG conf = new Yeti.Lame.BE_CONFIG(fmt, 96);

                // Allocate a 1-second buffer
                int blen = ws.WaveFormat.AverageBytesPerSecond;
                byte[] buffer = new byte[blen];

                // Do conversion
                using (MemoryStream output = new MemoryStream())
                {
                    Yeti.MMedia.Mp3.Mp3Writer mp3 = new Yeti.MMedia.Mp3.Mp3Writer(output, fmt, conf);

                    int readCount;
                    while ((readCount = ws.Read(buffer, 0, blen)) > 0)
                        mp3.Write(buffer, 0, readCount);

                    mp3.Close();
                    return output.ToArray();
                }
            }
        }

        public string FilePath;
        [Obsolete]
        public void SaveFile(bool New)
        {
            if (Program.Editer.GetTrackCount() < 1)
                return;

            if (New || string.IsNullOrEmpty(FilePath))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "MP3 file|*.mp3";
                if (dialog.ShowDialog() == DialogResult.Cancel)
                    return;
                FilePath = dialog.FileName;
            }

            MainUI.BtnPlay_Click(null, null);
            MainUI.BtnStop_Click(null, null);
            reader2.Seek(0, SeekOrigin.Begin);
            if (reader2 == null)
                return;
         
            using (FileStream file = new FileStream(FilePath, FileMode.Create, FileAccess.Write))
            {
                var data = WavToMP3(reader2);

                byte[] buffer = new byte[5000000];

                MemoryStream mstream = new MemoryStream();
                BinaryWriter writer2 = new BinaryWriter(mstream);
                foreach (var item in data)
                    writer2.Write(item);

                mstream.Seek(0, SeekOrigin.Begin);
                while (true)
                {
                    int r = mstream.Read(buffer, 0, buffer.Length);
                    if (r == 0)
                        break;
                    file.Write(buffer, 0, r);
                }
                mstream.Close();
                file.Close();
                //memoryStream.WriteTo(file);        
            }
        }

        public void Copy(int id,int s,int e)
        {
            try
            {
                if ((s == -1) || (e == -1))
                    return;
                int t1 = s, t2 = e;
                s = Math.Min(t1, t2);
                e = Math.Max(t1, t2);

                Track t = GetTrackById(id);
                if (t?.reader == null)
                    return;

                var data = GetDataOf16Bit2ChannelFile(t.reader);
                long sp = (long)s  * data.Length / t.UI.BtView.Width;
                if (sp % 2 != 0)
                    sp--;
                sp = Math.Max(0, sp);
                long ep = (long)e  * data.Length / t.UI.BtView.Width;
                if (ep % 2 != 0)
                    ep++;
                ep = Math.Min(data.Length, ep);

                clipbord = new short[ep - sp];
                Array.Copy(data, sp, clipbord, 0, clipbord.Length);

                //for (int i = 0; i < clipbord.Length; i+=2)
                //{
                //    clipbord.SetValue(data.GetValue(i + sp), i);
                //}

                EditeFlag = true;

                //t.reader = new RawSourceWaveStream(mstream, t.reader.WaveFormat);
                //t.DrowWave();
                //t.UI.SetLength((int)t.reader.TotalTime.TotalSeconds + 1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Cut(int id, int s, int e)
        {
            Stop();
            try
            {
                if ((s == -1) || (e == -1))
                    return;
                Copy(id, s, e);
                if (clipbord == null)
                    return;

                int t1 = s, t2 = e;
                s = Math.Min(t1, t2);
                e = Math.Max(t1, t2);

                Track t = GetTrackById(id);
                if (t?.reader == null)
                    return;

                var data = GetDataOf16Bit2ChannelFile(t.reader);
                long sp = s * data.Length / t.UI.BtView.Width;
                if (sp % 2 != 0)
                    sp--;
                sp = Math.Max(0, sp);
                long ep = e * data.Length / t.UI.BtView.Width;
                if (ep % 2 != 0)
                    ep++;
                ep = Math.Min(data.Length, ep);

                short[] result = new short[data.Length - (ep - sp)];

                Array.Copy(data, 0, result, 0, sp);
                Array.Copy(data, ep, result, sp, data.Length - ep);

                MemoryStream mstream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(mstream);
                foreach (var item in result)
                    writer.Write(item);
                t.reader = new RawSourceWaveStream(
                        mstream, t.reader.WaveFormat
                    );
                t.SetLength();
                t.DrowWave();

                EditeFlag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void Paste(int id, int e)
        {
            if (clipbord == null)
                return;
            Stop();
            try
            {
                Track t = GetTrackById(id);
                if (t?.reader == null)
                    return;

                var data = GetDataOf16Bit2ChannelFile(t.reader);
                long ep = e * data.Length / t.UI.BtView.Width;
                if (ep % 2 != 0)
                    ep--;
                ep = Math.Max(0, ep);
                ep = Math.Min(data.Length, ep);

                short[] result = new short[data.Length + clipbord.Length];

                Array.Copy(data, 0, result, 0, ep);
                Array.Copy(clipbord, 0, result, ep, clipbord.Length);
                Array.Copy(data, ep, result, ep + clipbord.Length, data.Length - ep);

                MemoryStream mstream = new MemoryStream();
                BinaryWriter writer = new BinaryWriter(mstream);
                foreach (var item in result)
                    writer.Write(item);
                t.reader = new RawSourceWaveStream(
                        mstream, t.reader.WaveFormat
                    );
                t.SetLength();
                t.DrowWave();

                EditeFlag = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //clipbord = null;
        }
    }
}
