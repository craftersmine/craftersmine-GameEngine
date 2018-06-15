using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio;
using NAudio.Wave;

namespace craftersmine.GameEngine.Content
{
    public class Audio
    {
        private WaveFileReader waveFile { get; set; }
        public TimeSpan FileLength { get; internal set; }

        public Audio(WaveFileReader waveFile)
        {
            this.waveFile = waveFile;
            FileLength = this.waveFile.TotalTime;
        }

        public WaveFileReader GetWaveFile()
        {
            return waveFile;
        }
    }
}
