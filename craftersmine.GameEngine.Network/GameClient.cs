using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace craftersmine.GameEngine.Network
{
    public sealed class GameClient
    {
        private WebSocket client;

        public string ServerAddress { get; internal set; }
        public int ServerPort { get; internal set; }

        public void Connect(string address, int port)
        {

        }
    }
}
