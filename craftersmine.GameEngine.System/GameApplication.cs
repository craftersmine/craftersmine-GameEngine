using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.GameEngine.Utils;
using System.Drawing;
using craftersmine.GameEngine.Input;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using craftersmine.GameEngine.Objects;

namespace craftersmine.GameEngine.System
{
    /// <summary>
    /// Represents a main game application. This class cannot be inherited
    /// </summary>
    public sealed class GameApplication
    {
        private static Timer gameTicker = new Timer();
        private static Timer gameDrawer = new Timer();
        private static Timer gameCollisionUpdater = new Timer();
        private static GameWindow gameWnd;
        private static Timer tickrateCounter = new Timer();
        private static Logger _logger;
        private static int tickrateCounted = 0;
        private static int framerateCounted = 0;
        private static int tickLast = 0;
        private static int frameLast = 0;
        private static int collUpdateLast = 0;
        private static PerformanceCounter cpuCounter;
        private static PerformanceCounter ramCounter;

        /// <summary>
        /// Gets current game tickrate (TPS)
        /// </summary>
        public static int CurrentGameTickrate { get; internal set; }
        /// <summary>
        /// Gets current game framerate (FPS)
        /// </summary>
        public static int CurrentGameFramerate { get; internal set; }
        /// <summary>
        /// Gets current game collisions updates rate (UPS)
        /// </summary>
        public static int CurrentGameCollisionUpdateRate { get; internal set; }
        /// <summary>
        /// Gets is current game process is active
        /// </summary>
        public static bool IsProcessActive { get; internal set; }

        //public static int CPUUtilization { get { return (int)cpuCounter.NextValue(); } }
        
        //public static int RAMUsage { get { return (int)GC.GetTotalMemory(true); } }

        /// <summary>
        /// Gets Game Version setted in AssemblyInfo.cs
        /// </summary>
        public static Version GameVersion { get; internal set; }
        /// <summary>
        /// Gets Company name as Game Developer setted in AssemblyInfo.cs
        /// </summary>
        public static string GameDeveloperCompanyName { get; internal set; }
        /// <summary>
        /// Gets Product name as Game Name setted in AssemblyInfo.cs
        /// </summary>
        public static string GameName { get; internal set; }
        /// <summary>
        /// Gets game Crash Handler
        /// </summary>
        public static GameCrashHandler CrashHandler { get; internal set; }
        /// <summary>
        /// Game Application Data (%AppData%/*) path root
        /// </summary>
        public static string AppDataGameRoot { get; internal set; }

        /// <summary>
        /// Runs game process, creates game application data directory with path <paramref name="applicationDataFolder"/> and shows <paramref name="gameWindow"/>
        /// </summary>
        /// <param name="gameWindow">Main game window</param>
        /// <param name="applicationDataFolder">Path to root of Game at %AppData%</param>
        public static void Run(GameWindow gameWindow, string applicationDataFolder)
        {
            try
            {
                cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                ramCounter = new PerformanceCounter("Memory", "Available MBytes");
                AppDataGameRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), applicationDataFolder);
                if (!Directory.Exists(AppDataGameRoot))
                    Directory.CreateDirectory(AppDataGameRoot);
                LoadAssemblyData();
                gameWnd = gameWindow;
                gameWnd.KeyDown += GameWnd_KeyDown;
                gameWnd.KeyUp += GameWnd_KeyUp;
                gameWnd.IsActive = true;
                gameTicker.Tick += GameTicker_Tick;
                tickrateCounter.Tick += TickrateCounter_Tick;
                gameDrawer.Tick += GameDrawer_Tick;
                gameCollisionUpdater.Tick += GameCollisionUpdater_Tick;
                gameTicker.Interval = 16;
                tickrateCounter.Interval = 1500;
                gameDrawer.Interval = 16;
                gameCollisionUpdater.Interval = 16;
                gameTicker.Start();
                gameCollisionUpdater.Start();
                gameDrawer.Start();
                tickrateCounter.Start();
                IsProcessActive = true;
                Application.Run(gameWnd);
            }
            catch (Exception ex)
            {
                if (CrashHandler != null)
                    CrashHandler.Crash(ex);
                else throw ex;
            }
        }

        private static void GameCollisionUpdater_Tick(object sender, EventArgs e)
        {
            try
            {
                if (gameWnd.Tick > 10)
                    if (gameWnd.CurrentScene != null)
                        UpdateCollisions();
            }
            catch (Exception ex)
            {
                gameCollisionUpdater.Stop();
                CrashHandler?.Crash(ex);
            }
        }

        private static void GameDrawer_Tick(object sender, EventArgs e)
        {
            try
            {
                gameWnd.Frame++;
                if (gameWnd.Tick > 10)
                {
                    if (gameWnd.CurrentScene != null)
                    {
                        gameWnd.CurrentScene.Draw();
                    }
                }
            }
            catch (Exception ex)
            {
                gameDrawer.Stop();
                CrashHandler?.Crash(ex);
            }
        }

        private static void TickrateCounter_Tick(object sender, EventArgs e)
        {
            CurrentGameTickrate = GetGameTick() - tickLast;
            CurrentGameFramerate = GetFrameNumber() - frameLast;
            CurrentGameCollisionUpdateRate = GetCollisionUpdateTick() - collUpdateLast;
            collUpdateLast = GetCollisionUpdateTick();
            frameLast = GetFrameNumber();
            tickLast = GetGameTick();
        }

        /// <summary>
        /// Sets game crash handler
        /// </summary>
        /// <param name="crashHandler"><see cref="GameCrashHandler"/> instance</param>
        public static void SetCrashHandler(GameCrashHandler crashHandler)
        {
            CrashHandler = crashHandler;
        }

        /// <summary>
        /// Sets game logger
        /// </summary>
        /// <param name="logger"><see cref="Logger"/> instance</param>
        public static void SetLogger(Logger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Runs game process, creates game application data directory with default path and shows <paramref name="gameWindow"/>
        /// </summary>
        /// <param name="gameWindow"></param>
        public static void Run(GameWindow gameWindow)
        {
            LoadAssemblyData();
            Run(gameWindow, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GameDeveloperCompanyName, GameName));
        }

        /// <summary>
        /// Closes <see cref="GameWindow"/>, stops Game Update Processes and exites from application with <paramref name="exitCode"/>
        /// </summary>
        /// <param name="exitCode">Application exit code</param>
        public static void Exit(int exitCode)
        {
            IsProcessActive = false;
            gameTicker.Stop();
            gameDrawer.Stop();
            gameCollisionUpdater.Stop();
            tickrateCounter.Stop();
            Log(LogEntryType.Info, "Game exited! Exit code: " + exitCode);
            Environment.Exit(exitCode);
        }

        /// <summary>
        /// Closes <see cref="GameWindow"/>, stops Game Update Processes and exites from application with 0 exit code
        /// </summary>
        public static void Exit()
        {
            Exit(0);
        }

        /// <summary>
        /// Adds new log entry with <paramref name="prefix"/> <paramref name="contents"/>
        /// </summary>
        /// <param name="prefix">Log entry prefix</param>
        /// <param name="contents">Log entry contents</param>
        /// <param name="onlyConsole">If <code>true</code> shows only in console and don't writes it to file, else writes to file</param>
        public static void Log(LogEntryType prefix, string contents, bool onlyConsole = false)
        {
            _logger?.Log(prefix, contents, onlyConsole);
        }

        /// <summary>
        /// Wraps exception information and creates log entries from this
        /// </summary>
        /// <param name="exception"><see cref="Exception"/> with error</param>
        /// <param name="prefix">Log entries prefixes</param>
        public static void LogException(Exception exception, LogEntryType prefix = LogEntryType.Crash)
        {
            _logger?.LogException(prefix, exception);
        }

        /// <summary>
        /// Gets current game tick
        /// </summary>
        /// <returns><see cref="int"/> value of current tick</returns>
        public static int GetGameTick()
        {
            return gameWnd.Tick;
        }

        public static int GetCollisionUpdateTick()
        {
            return gameWnd.CollisionUpdate;
        }

        public static int GetFrameNumber()
        {
            return gameWnd.Frame;
        }

        /// <summary>
        /// Gets or sets is game objects colliders draws
        /// </summary>
        public static bool DrawColliderBoundings { get { return gameWnd.DrawGameObjectCollisionBoundings; } set { gameWnd.DrawGameObjectCollisionBoundings = value; } }
        /// <summary>
        /// Gets or sets is game objects texture boundings draws
        /// </summary>
        public static bool DrawTextureBoundings { get { return gameWnd.DrawGameObjectTextureBoundings; } set { gameWnd.DrawGameObjectTextureBoundings = value; } }
        //public static bool DrawInputDebugger { get { return gameWnd.DrawInputDebug; } set { gameWnd.DrawInputDebug = value; } }
        /// <summary>
        /// Gets or sets is game tick and frame rates draws
        /// </summary>
        public static bool DrawUtilizationDebugger { get { return gameWnd.DrawUtilizationDebug; } set { gameWnd.DrawUtilizationDebug = value; } }

        /// <summary>
        /// Sets time of one game update call
        /// </summary>
        /// <param name="tickTime">Time of update call</param>
        public static void SetGameTickTime(int tickTime)
        {
            if (tickTime > 0)
            {
                gameTicker.Interval = tickTime;
                gameCollisionUpdater.Interval = tickTime; 
            }
            else throw new ArgumentException("Tick time must be more than 0", "tickTime");
        }

        /// <summary>
        /// Sets time of one drawing frame
        /// </summary>
        /// <param name="frameTime">Time of frame draw</param>
        public static void SetGameFrameTime(int frameTime)
        {
            if (frameTime > 0)
            {
                gameDrawer.Interval = frameTime;
            }
            else throw new ArgumentException("Frame time must be more than 0", "frameTime");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void OnGameTickEventDelegate(object sender, EventArgs e);
        /// <summary>
        /// Calls on game tick event
        /// </summary>
        public static event OnGameTickEventDelegate OnGameTickEvent;
        
        private static void LoadAssemblyData()
        {
            FileVersionInfo vi = FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location);
            GameVersion = Version.Parse(vi.ProductVersion);
            GameDeveloperCompanyName = vi.CompanyName;
            GameName = vi.ProductName;
        }

        private static void GameWnd_KeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = false;
        }

        private static void GameWnd_KeyDown(object sender, KeyEventArgs e)
        {
            Keyboard.UpdateKeys(e.KeyCode, e.Modifiers);
            e.Handled = true;
        }

        private static void GameTicker_Tick(object sender, EventArgs e)
        {
            try
            {
                gameWnd.Tick++;
                if (gameWnd.Tick == 10)
                    gameWnd.OnCreated();
                else if (gameWnd.Tick > 10)
                {
                    gameWnd.OnUpdate();
                    if (gameWnd.CurrentScene != null)
                    {
                        gameWnd.CurrentScene.OnUpdate();
                        CallGameObjectsUpdates();
                        GameWindowBridge.CurrentTick = gameWnd.Tick;
                        OnGameTickEvent?.Invoke(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                gameTicker.Stop();
                CrashHandler?.Crash(ex);
            }
        }

        private static void UpdateCollisions()
        {
            gameWnd.CollisionUpdate++;
            if (gameWnd.CurrentScene != null)
            {
                for (int i = 0; i < gameWnd.CurrentScene.GameObjects.Count; i++)
                {
                    if (gameWnd.CurrentScene.GameObjects[i].IsCollidable)
                    {
                        gameWnd.CurrentScene.GameObjects[i].IsCollided = false;
                        gameWnd.CurrentScene.GameObjects[i].CheckCollisions(gameWnd.CurrentScene.GameObjects);
                    }
                }
            }
        }

        private static void CallGameObjectsUpdates()
        {
            foreach (GameObject gObj in gameWnd.CurrentScene.GameObjects)
            {
                gObj.OnUpdate();
                gObj.InternalUpdate();
            }
        }
    }
}
