using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace craftersmine.GameEngine.Network
{
    /// <summary>
    /// Represents Game server behaviour. This class cannot be inherited
    /// </summary>
    public sealed class GameServerBehaviour : WebSocketBehavior
    {
        /// <summary>
        /// Calls at connection close
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }

        /// <summary>
        /// Calls at connection error
        /// </summary>
        /// <param name="e"></param>
        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }

        /// <summary>
        /// Calls at received message
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMessage(MessageEventArgs e)
        {
            GameServer.Logger.Log(Utils.LogEntryType.Info, "Received message: " + e.Data);
        }

        /// <summary>
        /// Calls at connection open
        /// </summary>
        protected override void OnOpen()
        {
            this.SendAsync("TEST MESSAGE", new Action<bool>((s) => { }));
        }
    }
}
