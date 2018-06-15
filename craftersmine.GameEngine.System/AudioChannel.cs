using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.GameEngine.Content;
using NAudio.Wave;
using craftersmine.GameEngine.Utils;

namespace craftersmine.GameEngine.Objects
{
    public class AudioChannel
    {
        private float chlVolume;

        private WaveOut waveOut { get; set; }
        private LoopStream loopStream { get; set; }
        public string ChannelName { get; internal set; }
        public Audio AssignedAudio { get; internal set; }
        public float ChannelVolume { get { return chlVolume; } set { chlVolume = value; waveOut.Volume = chlVolume; } }
        public bool IsPlaying { get; internal set; }
        public bool IsPaused { get; internal set; }
        public bool IsRepeating { get { return loopStream.EnableLooping; } set { loopStream.EnableLooping = value; } }

        public AudioChannel(string name, Audio audio)
        {
            ChannelName = name;
            AssignedAudio = audio;
            waveOut = new WaveOut();
            loopStream = new LoopStream(AssignedAudio.GetWaveFile());
            waveOut.Init(loopStream);
        }

        public void Play()
        {
            waveOut.Play();
            IsPlaying = true;
        }

        public void Stop()
        {
            waveOut.Stop();
            IsPlaying = false;
        }

        public void Pause()
        {
            waveOut.Pause();
            IsPaused = true;
        }

        public void Resume()
        {
            waveOut.Resume();
            IsPaused = false;
        }

        public void SetOutputDevice(int deviceNumber)
        {
            waveOut.DeviceNumber = deviceNumber;
        }
    }
}
