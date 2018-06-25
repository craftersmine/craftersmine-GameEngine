using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.GameEngine.System;
using WebSocketSharp;

namespace craftersmine.GameEngine.Network
{
    public sealed class GameClient
    {
        private WebSocket client;
        private GameWindow _gameWindow;

        public GameClient(GameWindow gameWindow)
        {
            _gameWindow = gameWindow;
        }

        public string ServerAddress { get; internal set; }
        public int ServerPort { get; internal set; }

        public void Connect(string address, int port)
        {

        }
    }
}
