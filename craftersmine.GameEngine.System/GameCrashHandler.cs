using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace craftersmine.GameEngine.System
{
    /// <summary>
    /// Represents Game crash handler
    /// </summary>
    public sealed class GameCrashHandler
    {
        private Exception exception;

        /// <summary>
        /// Gets or sets true if message will throwed to user, else false
        /// </summary>
        public bool ThrowMessageToUser { get; set; }

        /// <summary>
        /// Creates new <see cref="GameCrashHandler"/> instance and sets <see cref="GameCrashHandler.ThrowMessageToUser"/>
        /// </summary>
        /// <param name="throwMessageToUserIfCrashed">true if message will throwed to user, else false</param>
        public GameCrashHandler(bool throwMessageToUserIfCrashed)
        {
            ThrowMessageToUser = throwMessageToUserIfCrashed;
        }

        /// <summary>
        /// Creates new <see cref="GameCrashHandler"/> instance and sets <see cref="GameCrashHandler.ThrowMessageToUser"/> to true
        /// </summary>
        public GameCrashHandler() : this(true)
        {

        }

        /// <summary>
        /// Throws message with <see cref="Exception"/> data to user with <paramref name="messageTitle"/> and <paramref name="messageIcon"/> with OK button
        /// </summary>
        /// <param name="messageTitle">Message title</param>
        /// <param name="messageIcon">Message icon</param>
        public void ThrowToUser(string messageTitle, MessageBoxIcon messageIcon)
        {
            MessageBox.Show($"The {GameApplication.GameName} was crashed!{Environment.NewLine}Crash message: {exception.Message}{Environment.NewLine}Stack trace: {Environment.NewLine}{exception.StackTrace}{Environment.NewLine}{Environment.NewLine}Application will be closed!", messageTitle, MessageBoxButtons.OK, messageIcon);
        }

        /// <summary>
        /// Crashes game. If <see cref="GameCrashHandler.ThrowMessageToUser"/> true, throws message and exits from application, else just exits from application
        /// </summary>
        /// <param name="exception"><see cref="Exception"/> that crashed game</param>
        /// <param name="messageTitle">Message title</param>
        /// <param name="messageIcon">Message icon</param>
        public void Crash(Exception exception, string messageTitle = "craftersmine GameEngine - Game was crashed!", MessageBoxIcon messageIcon = MessageBoxIcon.Error)
        {
            this.exception = exception;
            GameApplication.LogException(this.exception);
            if (ThrowMessageToUser)
                ThrowToUser(messageTitle, messageIcon);
            GameApplication.Exit(0);
        }
    }
}
