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
        private static Logger logger;

        public static Version GameVersion { get; internal set; }
        public static string GameDeveloperCompanyName { get; internal set; }
        public static string GameName { get; internal set; }
        public static GameCrashHandler CrashHandler { get; internal set; }

        public static string AppDataGameRoot { get; internal set; }

        /// <summary>
        /// Runs game process creates game application data directory and shows <paramref name="gameWindow"/>
        /// </summary>
        /// <param name="gameWindow">Main game window</param>
        public static void Run(GameWindow gameWindow, string applicationDataFolder)
        {
            CrashHandler = new GameCrashHandler();
            try
            {
                AppDataGameRoot = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), applicationDataFolder);
                if (!Directory.Exists(AppDataGameRoot))
                    Directory.CreateDirectory(AppDataGameRoot);
                LoadAssemblyData();
                logger = new Logger(Path.Combine(AppDataGameRoot, "Logs"), GameName);
                gameWnd = gameWindow;
                gameWnd.KeyDown += GameWnd_KeyDown;
                gameWnd.KeyUp += GameWnd_KeyUp;
                gameWnd.IsActive = true;
                gameTicker.Tick += Timer_Tick;
                gameTicker.Interval = 16;
                gameTicker.Start();
                Application.Run(gameWnd);
            }
            catch (Exception ex)
            {
                CrashHandler.Crash(ex);
            }
        }

        public static void Run(GameWindow gameWindow)
        {
            LoadAssemblyData();
            Run(gameWindow, Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), GameDeveloperCompanyName, GameName));
        }

        public static void Exit(int exitCode)
        {
            gameTicker.Stop();
            gameWnd.Close();
            Log(LogEntryType.Info, "Game exited!");
            Environment.Exit(exitCode);
        }

        public static void Log(LogEntryType prefix, string contents, bool onlyConsole = false)
        {
            logger?.Log(prefix, contents, onlyConsole);
        }

        public static void LogException(Exception exception, LogEntryType prefix = LogEntryType.Crash)
        {
            logger?.LogException(prefix, exception);
        }

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

        private static void Timer_Tick(object sender, EventArgs e)
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
                        //UpdateBoundingBoxes();
                        UpdateCollisions();
                        GameWindowBridge.CurrentTick = gameWnd.Tick;
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
                            gObj.OnCollide(gObjCollision.InternalName);
                            gObj.IsCollided = true;
                        }
                        else if (gObjCollision.BoundingBox.IntersectsWith(gObj.BoundingBox) && gObjCollision.IsCollidable)
                        {
                            gObjCollision.OnCollide(gObj.InternalName);
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
