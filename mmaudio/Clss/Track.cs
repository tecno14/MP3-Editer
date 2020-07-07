using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VarispeedDemo.SoundTouch;

namespace mmaudio.Clss
{
    class Track
    {
        public WaveStream reader;
        //WaveOut wout;
        //public IWavePlayer wout;
        //public VarispeedSampleProvider speedControl;
        public Dictionary<string, string> Info;
        public TrackUC UI;
        public int id;
        public Track(int id)
        {
            this.Info = new Dictionary<string, string>();
            this.id = id;
            UI = new TrackUC(id);
            UI.SetName("Track " + id);
            //UI.track = this;
        }
        public Track(int id, string FileName) : this(id)
        {
            Mp3FileReader reader = new Mp3FileReader(FileName);

            this.reader = reader;
            //chk:
            int c = this.reader.WaveFormat.Channels;
            if (c != 1 && c != 2)
                throw new Exception("unsupported channel count" + c);

            StandardizBitsPerSample();
            StandardizChannels();

            StandardizSampleRate();

            this.Info.Clear();
            this.Info.Add("Name", Path.GetFileNameWithoutExtension(FileName));
            this.Info.Add("Average bytes per second", reader.Mp3WaveFormat.AverageBytesPerSecond.ToString());
            this.Info.Add("Bits per sample", reader.WaveFormat.BitsPerSample.ToString());
            this.Info.Add("Channels", reader.WaveFormat.Channels.ToString());
            this.Info.Add("Encoding", reader.Mp3WaveFormat.Encoding.ToString());
            this.Info.Add("Sample rate", reader.WaveFormat.SampleRate.ToString());
           
                  
            UI.SetName(string.Format("TrackID {0} - {1}", id, Path.GetFileNameWithoutExtension(FileName)), false);
            UI.SetToolTib(Program.DictionaryToString(Info));
            SetLength();
            DrowWave();
            //this.speedControl = new VarispeedSampleProvider(reader.ToSampleProvider(), 100, new SoundTouchProfile(true, false));
        }
        public void SetLength()
        {
            if (reader == null)
                return;
            UI.SetLength((int)this.reader.TotalTime.TotalSeconds + 1);
        }
        public void DrowWave()
        {
            if (reader == null)
                return;
            UI.DrowWave(reader, reader.WaveFormat.SampleRate * (int)reader.TotalTime.TotalSeconds);
        }
        public void StandardizChannels()
        {
            if (reader == null)
                return;
            if (reader.WaveFormat.Channels == Program.Editer.Channels)
                return;
            WaveFormat wf = new WaveFormat(reader.WaveFormat.SampleRate, reader.WaveFormat.BitsPerSample, Program.Editer.Channels);
            var data = Program.Editer.GetDataOf16Bit2ChannelFile(reader);
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
            Program.Editer.EditeFlag = true;
            reader = new RawSourceWaveStream(mstream, wf);
        }
        public void StandardizBitsPerSample()
        {
            if (reader == null)
                return;
            if (reader.WaveFormat.BitsPerSample == Program.Editer.BitsPerSample)
                return;

            reader.Seek(0, System.IO.SeekOrigin.Begin);
            byte[] data = new byte[reader.Length];
            reader.Read(data, 0, data.Length);

            int bps = reader.WaveFormat.BitsPerSample;
            short[] res = new short[data.Length / (bps / 8)];

            for (int i = 0, j = 0; i < data.Length; i += bps, j++)
            {
                var b = data.Skip(i).Take(bps).ToArray();
                long value = 0;
                for (int k = 0; k < b.Length; k++)
                    value += ((long)b[k] & 0xffL) << (8 * k);
                //if (bps < Program.Editer.BitsPerSample)
                //    res[j] = (short)value;
                //else
                res[j] = (short)(value * Program.Editer.BitsPerSample / bps);

            }

            for (int i = 0; i < data.Length; i += 2)
                res[i / 2] = BitConverter.ToInt16(data, i);

            var byteArray = new byte[res.Length * 2];
            Buffer.BlockCopy(res, 0, byteArray, 0, byteArray.Length);
            reader = new RawSourceWaveStream(byteArray, 0, byteArray.Length, new WaveFormat(reader.WaveFormat.SampleRate, Program.Editer.BitsPerSample, reader.WaveFormat.Channels));


        }

        public void StandardizSampleRate()
        {
            if (reader == null)
                return;

            if (Program.Editer.SampleRate != Math.Min(Program.Editer.SampleRate, reader.WaveFormat.SampleRate))
            {
                Program.Editer.NewSampleRate = true;
                Program.Editer.SampleRate = Math.Min(Program.Editer.SampleRate, reader.WaveFormat.SampleRate);
            }

            if (reader.WaveFormat.SampleRate == Program.Editer.SampleRate)
                return;

            var data = Program.Editer.GetDataOf16Bit2ChannelFile(reader);
            /*
            int tsize = data.Length / 2;
            if (data.Length % 2 != 0)
                tsize++;
            var d1 = new short[tsize];
            var d2 = new short[tsize];
            for (int i = 0, j = 0; i < data.Length; i += 2, j++)
            {
                d1[j] = data[i];
                d1[j] = data[i + 1];
            }

            double NewDataSize1Chn = Program.Editer.SampleRate * (reader.TotalTime.TotalSeconds);
            if (NewDataSize1Chn % 2 != 0)
                NewDataSize1Chn++;

            var r1 = new short[(int)NewDataSize1Chn];
            var r2 = new short[(int)NewDataSize1Chn];

            var result = new short[d1.Length * 2];

            long indx;

            for (long i = 0; i < (int)NewDataSize1Chn; i++)
            {
                //NewData[i] = data[Program.Editer.SampleRate * i / reader.WaveFormat.SampleRate];
                indx = Math.Min((int)(i * data.Length / result.Length), d1.Length);
                //NewData[i] = data[indx];
                r1.SetValue(d1.GetValue(indx), i);
                r2.SetValue(d2.GetValue(indx), i);
                //System.Diagnostics.Debug.WriteLine(indx+" => NewData " + NewData.Length + " - data " + data.Length);
            }

            

            for (int i = 0, j = 0; i < d1.Length; i++, j += 2)
            {
                result.SetValue(d1.GetValue(i), j);
                result.SetValue(d2.GetValue(i), j + 1);
            }

            var byteArray = new byte[result.Length * 2];
            Buffer.BlockCopy(result, 0, byteArray, 0, byteArray.Length);
            reader = new RawSourceWaveStream(byteArray, 0, byteArray.Length, new WaveFormat(Program.Editer.SampleRate, reader.WaveFormat.BitsPerSample, reader.WaveFormat.Channels));
            return;
            
            */
            //-----------
            //var NewData = new short[(int)(Program.Editer.SampleRate * (reader.TotalTime.TotalSeconds + 1) / reader.WaveFormat.SampleRate)];
            //var NewData = new short[Program.Editer.SampleRate * data.Length / reader.WaveFormat.SampleRate];
            var NewData = new short[(int)(Program.Editer.SampleRate * (reader.TotalTime.TotalSeconds + 1) * 2)];
            long indx;
            for (long i = 0; i < NewData.Length; i+=2)
            {
                //NewData[i] = data[Program.Editer.SampleRate * i / reader.WaveFormat.SampleRate];
                indx= Math.Min(i * data.Length / NewData.Length, data.Length);
                int next, back;
                next = (int)indx;
                if (indx % 2 != 0)
                    next++;
                back = next - 2;
                if ((next - (int)indx) > ((int)indx) - back)
                    indx = back;
                indx = next;
                //NewData[i] = data[indx];
                NewData.SetValue(data.GetValue(indx), i);
                NewData.SetValue(data.GetValue(indx + 1), i + 1);
                //System.Diagnostics.Debug.WriteLine(indx+" => NewData " + NewData.Length + " - data " + data.Length);
            }
            var byteArray = new byte[NewData.Length * 2];
            Buffer.BlockCopy(NewData, 0, byteArray, 0, byteArray.Length);
            reader = new RawSourceWaveStream(byteArray, 0, byteArray.Length, new WaveFormat(Program.Editer.SampleRate, reader.WaveFormat.BitsPerSample, reader.WaveFormat.Channels));
            
        }
    }
}
