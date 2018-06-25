using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp.Server;
using WebSocketSharp;
using System.Net;
using System.IO;

namespace craftersmine.GameEngine.Network
{
    public sealed class GameServer
    {
        private WebSocketServer server;
        public static Utils.Logger Logger { get; internal set; }

        public GameServer(string boundAddress, int port)
        {
            Logger = new Utils.Logger(Path.Combine(Environment.CurrentDirectory, "logs"), "serverLog");
            server = new WebSocketServer(IPAddress.Parse(boundAddress), port);
            server.Log.Output = new Action<LogData, string>((LogData logData, string input) => {
                switch (logData.Level)
                {
                    case LogLevel.Info:
                        foreach (var ln in logData.Message.Split(new string[] { "\r", "\r\n", "\n" }, StringSplitOptions.None))
                        {
                            Logger.Log(Utils.LogEntryType.Info, ln);
                        }
                        break;
                    case LogLevel.Error:
                        foreach (var ln in logData.Message.Split(new string[] { "\r", "\r\n", "\n" }, StringSplitOptions.None))
                        {
                            Logger.Log(Utils.LogEntryType.Error, ln);
                        }
                        break;
                    case LogLevel.Warn:
                        foreach (var ln in logData.Message.Split(new string[] { "\r", "\r\n", "\n" }, StringSplitOptions.None))
                        {
                            Logger.Log(Utils.LogEntryType.Warning, ln);
                        }
                        break;
                    case LogLevel.Fatal:
                        foreach (var ln in logData.Message.Split(new string[] { "\r", "\r\n", "\n" }, StringSplitOptions.None))
                        {
                            Logger.Log(Utils.LogEntryType.Critical, ln);
                        }
                        break;
                    case LogLevel.Debug:
                        foreach (var ln in logData.Message.Split(new string[] { "\r", "\r\n", "\n" }, StringSplitOptions.None))
                        {
                            Logger.Log(Utils.LogEntryType.Debug, ln);
                        }
                        break;
                    case LogLevel.Trace:
                        foreach (var ln in logData.Message.Split(new string[] { "\r", "\r\n", "\n" }, StringSplitOptions.None))
                        {
                            Logger.Log(Utils.LogEntryType.Stacktrace, ln);
                        }
                        break;
                }
            });
            server.AddWebSocketService<GameServerBehaviour>("/");
        }

        public void StartServer()
        {
            Logger.Log(Utils.LogEntryType.Info, "Starting game server at " + server.Address.ToString() + ":" + server.Port);
            server.Start();
        }

        public void StopServer(CloseStatusCode closeStatusCode, string closeReason)
        {
            Logger.Log(Utils.LogEntryType.Info, "Stopping server...");
            server.Stop(closeStatusCode, closeReason);
            Logger.Log(Utils.LogEntryType.Done, "Server stopped with reason: " + closeReason + ". Close status code: " + closeStatusCode);
        }
    }
}
