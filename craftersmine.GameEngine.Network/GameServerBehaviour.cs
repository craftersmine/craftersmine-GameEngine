using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace craftersmine.GameEngine.Network
{
    public sealed class GameServerBehaviour : WebSocketBehavior
    {
        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }

        protected override void OnError(ErrorEventArgs e)
        {
            base.OnError(e);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            GameServer.Logger.Log(Utils.LogEntryType.Info, "Received message: " + e.Data);
        }

        protected override void OnOpen()
        {
            this.SendAsync("TEST MESSAGE", new Action<bool>((s) => { }));
        }
    }
}
