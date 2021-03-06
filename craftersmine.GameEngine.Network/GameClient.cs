﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.GameEngine.System;
using WebSocketSharp;

namespace craftersmine.GameEngine.Network
{
    /// <summary>
    /// Represents game network client. This class cannot be inherited
    /// </summary>
    public sealed class GameClient
    {
        private WebSocket client;
        private GameWindow _gameWindow;

        /// <summary>
        /// Creates new network game client instance
        /// </summary>
        /// <param name="gameWindow"></param>
        public GameClient(GameWindow gameWindow)
        {
            _gameWindow = gameWindow;
        }
        /// <summary>
        /// Gets current connected server address
        /// </summary>
        public string ServerAddress { get; internal set; }
        /// <summary>
        /// Gets current connected server port
        /// </summary>
        public int ServerPort { get; internal set; }

        /// <summary>
        /// Connects client to server
        /// </summary>
        /// <param name="address">Server address</param>
        /// <param name="port">Server port</param>
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
            GameApplication.OnGameTickEvent += GameUpdateEvent;
        }

        private void GameUpdateEvent(object sender, EventArgs e)
        {
            if (_gameWindow.CurrentScene != null)
            {
                UpdateTransmittingObjects();
            }
        }

        private void UpdateTransmittingObjects()
        {
            foreach (var transObject in _gameWindow.CurrentScene.Controls.OfType<NetworkGameObject>())
            {
                if (transObject.IsTransmittingLocation)
                {
                    SendEnginePacket("");
                }
            }
        }

        internal void SendEnginePacket(string packetData)
        {
            string packetCtor = "$CGENG#PACKET:" + packetData + ":PACKETEND";
            client.SendAsync(packetCtor, new Action<bool>((s) => { }));
        }
        /// <summary>
        /// Sends packet to server
        /// </summary>
        /// <param name="packetData"></param>
        public void SendPacket(string packetData)
        {
            SendEnginePacket("USERPACKET@" + packetData);
        }

        /// <summary>
        /// Requests network objects from server and assignes it to client game objects
        /// </summary>
        public void RequestObjectsIds()
        {
            if (_gameWindow.CurrentScene != null)
            {
                SendEnginePacket("REQOBJECTSIDS@" + _gameWindow.CurrentScene.Id);
            }
        }

        private void Client_OnOpen(object sender, EventArgs e)
        {
            GameApplication.Log(Utils.LogEntryType.Connection, "Connected to " + client.Url.Host + ":" + client.Url.Port + "!");
        }

        private void Client_OnMessage(object sender, MessageEventArgs e)
        {
            GameApplication.Log(Utils.LogEntryType.Connection, "Received message: " + e.Data);
            string[] splitPacketBase = e.Data.Split(':');
            if (splitPacketBase[0] == "$CGENG#PACKET" && splitPacketBase[2] == "PACKETEND")
            {
                string[] splitPacket = splitPacketBase[1].Split('@');
                string packetType = splitPacket[0];
                string packetContents = splitPacket[1];
                switch (packetType)
                {
                    case "INITIALIZEHANDSHAKE":
                        OnInitializeHandshake?.Invoke(this, null);
                        break;
                    case "REQOBJECTSIDSRESPONSE":
                        if (_gameWindow.CurrentScene != null)
                        {
                            string[] objectData = packetContents.Split('=');
                            foreach (var nObject in _gameWindow.CurrentScene.Controls.OfType<NetworkGameObject>())
                            {
                                if (nObject.InternalName == objectData[0] && nObject.NetworkObjectName == objectData[1] && nObject.Id == int.Parse(objectData[2]))
                                {
                                    nObject.NetworkId = int.Parse(objectData[3]);
                                }
                            }
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void OnInitializeHandshakeEventDelegate(object sender, EventArgs e);
        /// <summary>
        /// Calls at handshake initialization with server
        /// </summary>
        public static event OnInitializeHandshakeEventDelegate OnInitializeHandshake;

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
