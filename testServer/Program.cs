using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.GameEngine.Network;

namespace testServer
{
    class Program
    {
        static void Main(string[] args)
        {
            GameServer gameServer = new GameServer("0.0.0.0", 2000);
            gameServer.StartServer();
            Console.ReadLine();
        }
    }
}
