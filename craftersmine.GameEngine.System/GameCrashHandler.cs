using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace craftersmine.GameEngine.System
{
    public sealed class GameCrashHandler
    {
        private Exception exception;

        public bool ThrowMessageToUser { get; set; }

        public GameCrashHandler(bool throwMessageToUserIfCrashed)
        {
            ThrowMessageToUser = throwMessageToUserIfCrashed;
        }

        public GameCrashHandler() : this(true)
        {

        }

        public void ThrowToUser(string messageTitle, MessageBoxIcon messageIcon)
        {
            MessageBox.Show($"The {GameApplication.GameName} was crashed!{Environment.NewLine}Crash message: {exception.Message}{Environment.NewLine}Stack trace: {Environment.NewLine}{exception.StackTrace}{Environment.NewLine}{Environment.NewLine}Application will be closed!", messageTitle, MessageBoxButtons.OK, messageIcon);
        }

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
