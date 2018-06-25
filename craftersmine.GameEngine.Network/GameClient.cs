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
            GameApplication.Log(Utils.LogEntryType.Connection, "Connecting to " + address + ":" + port + "...");
            client = new WebSocket("ws://" + address + ":" + port);
            client.Log.Output = new Action<LogData, string>((LogData logData, string input) => {
                switch (logData.Level)
                {
                    case LogLevel.Info:
                        foreach (var ln in logData.Message.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                        {
                            GameApplication.Log(Utils.LogEntryType.Info, ln);
                        }
                        break;
                    case LogLevel.Error:
                        foreach (var ln in logData.Message.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                        {
                            GameApplication.Log(Utils.LogEntryType.Error, ln);
                        }
                        break;
                    case LogLevel.Warn:
                        foreach (var ln in logData.Message.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                        {
                            GameApplication.Log(Utils.LogEntryType.Warning, ln);
                        }
                        break;
                    case LogLevel.Fatal:
                        foreach (var ln in logData.Message.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                        {
                            GameApplication.Log(Utils.LogEntryType.Critical, ln);
                        }
                        break;
                    case LogLevel.Debug:
                        foreach (var ln in logData.Message.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                        {
                            GameApplication.Log(Utils.LogEntryType.Debug, ln);
                        }
                        break;
                    case LogLevel.Trace:
                        foreach (var ln in logData.Message.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None))
                        {
                            GameApplication.Log(Utils.LogEntryType.Stacktrace, ln);
                        }
                        break;
                }
            });
            client.OnClose += Client_OnClose;
            client.OnError += Client_OnError;
            client.OnMessage += Client_OnMessage;
            client.OnOpen += Client_OnOpen;
            client.Compression = CompressionMethod.None;
            client.Connect();
        }

        private void Client_OnOpen(object sender, EventArgs e)
        {
            GameApplication.Log(Utils.LogEntryType.Connection, "Connected to " + client.Url.Host + ":" + client.Url.Port + "!");
        }

        private void Client_OnMessage(object sender, MessageEventArgs e)
        {
            GameApplication.Log(Utils.LogEntryType.Connection, "Received message: " + e.Data);
        }

        private void Client_OnError(object sender, ErrorEventArgs e)
        {
            GameApplication.Log(Utils.LogEntryType.Connection, "An error has occured in network connection: " + e.Message);
            GameApplication.LogException(e.Exception, Utils.LogEntryType.Connection);
        }

        private void Client_OnClose(object sender, CloseEventArgs e)
        {
            GameApplication.Log(Utils.LogEntryType.Connection, "Connection to " + client.Url.Host + ":" + client.Url.Port + " closed with code " + e.Code);
        }
    }
}
