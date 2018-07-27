using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace craftersmine.GameEngine.System
{
    /// <summary>
    /// Game window object. Represents main game frame.
    /// </summary>
    public class GameWindow : Form
    {
        /// <summary>
        /// Current game tick
        /// </summary>
        public int Tick { get; internal set; }
        /// <summary>
        /// Gets or sets window title
        /// </summary>
        public string Title { get { return this.Text; } set { this.Text = value; } }
        /// <summary>
        /// Gets or sets <code>true</code> if application and game running, else <code>false</code>
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Gets height of window
        /// </summary>
        public new int Height { get { return this.ClientSize.Height; } }
        /// <summary>
        /// Gets width of window
        /// </summary>
        public new int Width { get { return this.ClientSize.Width; } }
        /// <summary>
        /// Gets current showing scene
        /// </summary>
        public Scene CurrentScene { get; internal set; }

        /// <summary>
        /// Gets current texture interpolation mode
        /// </summary>
        public InterpolationMode TextureInterpolation { get; internal set; }
        
        /// <summary>
        /// Gets or sets is game objects texture boundings is draws
        /// </summary>
        public bool DrawGameObjectTextureBoundings { get; set; }

        /// <summary>
        /// Gets or sets is game objects collision boundings is draws
        /// </summary>
        public bool DrawGameObjectCollisionBoundings { get; set; }
        
        /// <summary>
        /// Gets or sets is FPS/TPS/UPS counters is draws
        /// </summary>
        public bool DrawUtilizationDebug { get; set; }
        /// <summary>
        /// Gets count of drawed frames
        /// </summary>
        public int Frame { get; internal set; }
        /// <summary>
        /// Gets count of performed collision updates
        /// </summary>
        public int CollisionUpdate { get; internal set; }

        public WindowSize WindowSize { get; internal set; }

        /// <summary>
        /// Gets or sets is game window frame is disabled
        /// </summary>
        public bool IsWindowFrameDisabled
        {
            get
            {
                if (FormBorderStyle == FormBorderStyle.FixedSingle)
                    return true;
                else if (FormBorderStyle == FormBorderStyle.None)
                    return false;
                else return false;
            }
            set
            {
                if (value == true)
                    FormBorderStyle = FormBorderStyle.None;
                else FormBorderStyle = FormBorderStyle.FixedSingle;
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="GameWindow"/>
        /// </summary>
        /// <param name="title">Window title</param>
        /// <param name="width">Width of window</param>
        /// <param name="height">Height of window</param>
        [Obsolete("Use GameWindow(string title, WindowSize size) instead")]
        public GameWindow(string title, int width, int height)
        {
            this.FormClosing += GameWindow_FormClosing;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ClientSize = new Size(width, height);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            Title = title;
            SetBackgroundColor(Color.Black);
        }
        /// <summary>
        /// Creates a new instance of <see cref="GameWindow"/>
        /// </summary>
        /// <param name="title">Window title</param>
        /// <param name="size">Window size</param>
        public GameWindow(string title, WindowSize size)
        {
            this.WindowSize = size;
            this.FormClosing += GameWindow_FormClosing;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.ClientSize = size.GetSize();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            Title = title;
            SetBackgroundColor(Color.Black);
        }

        private void GameWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            OnExit();
        }

        private Dictionary<int, Scene> scenes { get; } = new Dictionary<int, Scene>();

        /// <summary>
        /// Sets background color of window
        /// </summary>
        /// <param name="color">Background color</param>
        public void SetBackgroundColor(Color color)
        {
            this.BackColor = color;
        }

        /// <summary>
        /// Adds new scene at window storage
        /// </summary>
        /// <param name="scene">Addable scene</param>
        public void AddScene(Scene scene)
        {
            scene.Visible = false;
            scene.Size = this.ClientSize;
            scene.CameraBounds = new Rectangle(scene.Location, scene.Size);
            scene.TextureInterpolationMode = TextureInterpolation;
            scenes.Add(scene.Id, scene);
            scene.OnCreate();
            this.Controls.Add(scene);
        }
        /// <summary>
        /// Shows later added scene
        /// </summary>
        /// <param name="id">Identifier of scene</param>
        public void ShowScene(int id)
        {
            foreach (var scene in scenes)
            {
                scene.Value.Visible = false;
                scene.Value.StopAllAudioChannels();
            }
            scenes[id].Visible = true;
            CurrentScene = scenes[id];
        }

        /// <summary>
        /// Sets texture interpolation mode
        /// </summary>
        /// <param name="mode">Texture interpolation mode</param>
        public void SetTextureInterpolationMode(InterpolationMode mode)
        {
            TextureInterpolation = mode;
            foreach (var scn in scenes)
            {
                scn.Value.TextureInterpolationMode = TextureInterpolation;
            }
        }

        /// <summary>
        /// Calls at window creation
        /// </summary>
        public virtual void OnCreated()
        {

        }
        /// <summary>
        /// Calls at game update
        /// </summary>
        public virtual void OnUpdate()
        {

        }
        /// <summary>
        /// Calls at game exit
        /// </summary>
        public virtual void OnExit()
        {
            GameApplication.Exit(0);
        }
    }

    /// <summary>
    /// Represents window size. This class cannot be inherited
    /// </summary>
    public sealed class WindowSize
    {
        /// <summary>
        /// Gets current width of window
        /// </summary>
        public int Width { get; internal set; }
        /// <summary>
        /// Gets current height of window
        /// </summary>
        public int Height { get; internal set; }

        /// <summary>
        /// Creates new window size from specified width and height
        /// </summary>
        /// <param name="width">Window width</param>
        /// <param name="height">Window height</param>
        public WindowSize(int width, int height)
        {
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Creates new window size from exist preset
        /// </summary>
        /// <param name="preset">Window size exist preset</param>
        public WindowSize(WindowSize preset) : this(preset.Width, preset.Height)
        {

        }

        public Size GetSize()
        {
            return new Size(Width, Height);
        }

        public readonly static WindowSize Empty = new WindowSize(0, 0);
    }

    /// <summary>
    /// Contains presets of window sizes. This class cannot be inherited
    /// </summary>
    public sealed class WindowSizePresets
    {
        /// <summary>
        /// FullHD size (1920x1080) (16:9)
        /// </summary>
        public static WindowSize FullHD { get; } = new WindowSize(1920, 1080);
        /// <summary>
        /// HD size (1280x720) (16:9)
        /// </summary>
        public static WindowSize HD { get; } = new WindowSize(1280, 720);
        /// <summary>
        /// HD+ size (1600x900) (16:9)
        /// </summary>
        public static WindowSize HDPlus { get; } = new WindowSize(1600, 900);
        /// <summary>
        /// WQHD size or 2k size (2560x1440) (16:9)
        /// </summary>
        [Obsolete("This resolution is too large and because it not recommended", false)]
        public static WindowSize WQHD { get; } = new WindowSize(2560, 1440);
        /// <summary>
        /// UHD size or 4k size (3840x2160) (16:9)
        /// </summary>
        [Obsolete("This resolution is too large and because it not recommended", false)]
        public static WindowSize UHD { get; } = new WindowSize(3840, 2160);
        /// <summary>
        /// SVGA size (800x600) (4:3)
        /// </summary>
        [Obsolete("This resolution is too old and because it not recommended", false)]
        public static WindowSize SVGA { get; } = new WindowSize(800, 600);
        /// <summary>
        /// XGA size (1024x768) (4:3)
        /// </summary>
        public static WindowSize XGA { get; } = new WindowSize(1024, 768);
        /// <summary>
        /// WXGA size (1280x768) (5:3)
        /// </summary>
        public static WindowSize WXGA { get; } = new WindowSize(1280, 768);
        /// <summary>
        /// SXGA size (1280x1024) (5:4)
        /// </summary>
        [Obsolete("This resolution is too old and because it not recommended", false)]
        public static WindowSize SXGA { get; } = new WindowSize(1280, 1024);
    }
}
