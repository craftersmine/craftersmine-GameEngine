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
        private static GameWindow gameWnd;
        private static Logger _logger;

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
            CrashHandler = new GameCrashHandler();
            try
            {
                AppDataGameRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), applicationDataFolder);
                if (!Directory.Exists(AppDataGameRoot))
                    Directory.CreateDirectory(AppDataGameRoot);
                LoadAssemblyData();
                gameWnd = gameWindow;
                gameWnd.KeyDown += GameWnd_KeyDown;
                gameWnd.KeyUp += GameWnd_KeyUp;
                gameWnd.IsActive = true;
                gameTicker.Tick += GameTicker_Tick;
                gameTicker.Interval = 16;
                gameTicker.Start();
                Application.Run(gameWnd);
            }
            catch (Exception ex)
            {
                CrashHandler.Crash(ex);
            }
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
            gameTicker.Stop();
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
        /// <summary>
        /// Calls on game tick event
        /// </summary>
        
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
                        CallGameObjectsUpdates();
                        UpdateCollisions();
                        GameWindowBridge.CurrentTick = gameWnd.Tick;
                        gameWnd.CurrentScene.OnUpdate();
                    }
                }
            }
            catch (Exception ex)
            {
                gameTicker.Stop();
                CrashHandler.Crash(ex);
            }
        }

        private static void UpdateCollisions()
        {
            foreach (GameObject gObj in gameWnd.CurrentScene.Controls.OfType<GameObject>())
            {
                foreach (GameObject gObjCollision in gameWnd.CurrentScene.Controls.OfType<GameObject>())
                {
                    if (gObj != gObjCollision)
                    {
                        if (gObj.BoundingBox.IntersectsWith(gObjCollision.BoundingBox) && gObj.IsCollidable)
                        {
                            gObj.OnCollide(gObjCollision);
                            gObj.IsCollided = true;
                        }
                        else if (gObjCollision.BoundingBox.IntersectsWith(gObj.BoundingBox) && gObjCollision.IsCollidable)
                        {
                            gObjCollision.OnCollide(gObj);
                            gObjCollision.IsCollided = true;
                        }
                        else
                        {
                            gObj.IsCollided = false;
                            gObjCollision.IsCollided = false;
                        }
                    }

                }
            }
        }

        [Obsolete]
        private static void UpdateBoundingBoxes()
        {
            foreach (GameObject gObj in gameWnd.CurrentScene.Controls.OfType<GameObject>())
            {
                gObj.UpdateCollider(gObj.BoundingBox.X, gObj.BoundingBox.Y, gObj.BoundingBox.Width, gObj.BoundingBox.Height);
            }
        }

        private static void CallGameObjectsUpdates()
        {
            foreach (GameObject gObj in gameWnd.CurrentScene.Controls.OfType<GameObject>())
            {
                gObj.OnUpdate();
                gObj.InternalUpdate();
            }
        }
    }
}
