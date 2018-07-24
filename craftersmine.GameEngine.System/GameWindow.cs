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
        private bool drawIDbg;
        private bool drawGObjColl;

        /// <summary>
        /// Current game tick
        /// </summary>
        public int Tick { get; internal set; }
        /// <summary>
        /// Window title
        /// </summary>
        public string Title { get { return this.Text; } set { this.Text = value; } }
        /// <summary>
        /// <code>true</code> if application and game running, else <code>false</code>
        /// </summary>
        public bool IsActive { get; set; }
        /// <summary>
        /// Height of window
        /// </summary>
        public new int Height { get { return this.ClientSize.Height; } }
        /// <summary>
        /// Width of window
        /// </summary>
        public new int Width { get { return this.ClientSize.Width; } }
        /// <summary>
        /// Current showing scene
        /// </summary>
        public Scene CurrentScene { get; internal set; }

        public InterpolationMode TextureInterpolation { get; internal set; }
        
        public bool DrawGameObjectTextureBoundings { get; set; }

        public bool DrawGameObjectCollisionBoundings { get; set; }

        //public bool DrawInputDebug { get; set; }

        public bool DrawUtilizationDebug { get; set; }

        public int Frame { get; internal set; }

        /// <summary>
        /// Creates a new instance of <see cref="GameWindow"/>
        /// </summary>
        /// <param name="title">Window title</param>
        /// <param name="width">Width of window</param>
        /// <param name="height">Height of window</param>
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
}
