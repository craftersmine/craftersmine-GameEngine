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
    /// <summary>
    /// Represents game audio channel with setted <see cref="Audio"/>
    /// </summary>
    public class AudioChannel
    {
        private float chlVolume;

        private WaveOut waveOut { get; set; }
        private LoopStream loopStream { get; set; }

        /// <summary>
        /// Gets audio channel name
        /// </summary>
        public string ChannelName { get; internal set; }
        /// <summary>
        /// Gets assigned <see cref="Audio"/> with this channel
        /// </summary>
        public Audio AssignedAudio { get; internal set; }
        /// <summary>
        /// Gets or sets channel sound volume
        /// </summary>
        public float ChannelVolume { get { return chlVolume; } set { chlVolume = value; waveOut.Volume = chlVolume; } }
        /// <summary>
        /// Gets true if <see cref="AudioChannel"/> is playing sound, else false
        /// </summary>
        public bool IsPlaying { get; internal set; }
        /// <summary>
        /// Gets true if <see cref="AudioChannel"/> is paused sound, else false
        /// </summary>
        public bool IsPaused { get; internal set; }
        /// <summary>
        /// Gets true if <see cref="AudioChannel"/> is looping sound, else false
        /// </summary>
        public bool IsRepeating { get { return loopStream.EnableLooping; } set { loopStream.EnableLooping = value; } }

        /// <summary>
        /// Creates new <see cref="AudioChannel"/> instance with <paramref name="name"/> and assigned <paramref name="audio"/>
        /// </summary>
        /// <param name="name">Audio channel name</param>
        /// <param name="audio">Assigned <see cref="Audio"/></param>
        public AudioChannel(string name, Audio audio)
        {
            ChannelName = name;
            AssignedAudio = audio;
            waveOut = new WaveOut();
            loopStream = new LoopStream(AssignedAudio.GetWaveFile());
            waveOut.Init(loopStream);
        }

        /// <summary>
        /// Plays assigned <see cref="Audio"/> from start
        /// </summary>
        public void Play()
        {
            waveOut.Play();
            IsPlaying = true;
        }

        /// <summary>
        /// Stops assigned <see cref="Audio"/> and sets position to 0
        /// </summary>
        public void Stop()
        {
            waveOut.Stop();
            IsPlaying = false;
        }

        /// <summary>
        /// Pauses assigned <see cref="Audio"/>
        /// </summary>
        public void Pause()
        {
            waveOut.Pause();
            IsPaused = true;
        }

        /// <summary>
        /// Resumes assigned paused <see cref="Audio"/> from pause position
        /// </summary>
        public void Resume()
        {
            waveOut.Resume();
            IsPaused = false;
        }

        /// <summary>
        /// Sets output device
        /// </summary>
        /// <param name="deviceNumber">Output device number</param>
        public void SetOutputDevice(int deviceNumber)
        {
            waveOut.DeviceNumber = deviceNumber;
        }
    }
}
