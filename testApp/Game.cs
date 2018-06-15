﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.GameEngine.Input;
using craftersmine.GameEngine.System;
using craftersmine.GameEngine.Content;
using System.IO;

namespace testApp
{
    class Game : GameWindow
    {
        Scene scene = new Scene(0);
        public static ContentStorage cs;
        public static GameConfig config;

        public Game(string title = "TestGameApp", int width = 1280, int height = 720) : base(title, width, height)
        {
            this.SetBackgroundColor(Color.Tomato);
            cs = new ContentStorage("testassets");
            config = new GameConfig("Configs", "TestConfig");
            cs.ContentStorageCreated += Cs_ContentStorageCreated;
            cs.ContentLoading += Cs_ContentLoading;
        }

        private void Cs_ContentLoading(object sender, ContentLoadingEventArgs e)
        {
            GameApplication.Log(craftersmine.GameEngine.Utils.LogEntryType.Info, "Loading content with name \"" + e.ContentFileName + "\" type of " + e.ContentType.ToString());
        }

        private void Cs_ContentStorageCreated(object sender, EventArgs e)
        {
            GameApplication.Log(craftersmine.GameEngine.Utils.LogEntryType.Info, "Creating content storage...");
        }

        Obj1 obj1 = new Obj1();
        Obj2 obj2 = new Obj2();

        Label labelDebugKeyboard = new Label();

        public override void OnCreated()
        {
            Random rnd = new Random();
            scene.AddAudioChannel(new craftersmine.GameEngine.Objects.AudioChannel("aud", cs.LoadAudio("aud")));
            scene.SetAudioChannelVolume("aud", 0.4f);
            scene.SetAudioChannelRepeat("aud", true);
            scene.SetBackgroundColor(Color.Black);
            this.AddScene(scene);
            scene.SetBackgroundTexture(cs.LoadTexture("bg"));

            scene.Controls.Add(labelDebugKeyboard);

            scene.AddGameObject(obj1);
            scene.AddGameObject(obj2);
            obj1.ApplyTexture(cs.LoadTexture("obj1"));
            obj2.ApplyTexture(cs.LoadTexture("obj2"));

            obj2.AddAnimation("anim", cs.LoadAnimation("anim"));

            ShowScene(0);
            scene.PlayAudioChannel("aud");
        }

        public override void OnUpdate()
        {
            Random rnd = new Random();
            Program.labelDebug.BackColor = scene.BackColor;
            Buttons buttons = Gamepad.GetButtons(Player.First);

            Triggers triggers = Gamepad.GetTriggers(Player.First);
            Sticks sticks = Gamepad.GetSticks(Player.First);

            DPad dPad = Gamepad.GetDPad(Player.First);

            labelDebugKeyboard.Text = Keyboard.GetKeys().ToString();
            Program.labelDebug.Text = $"Current Game Tick: {this.Tick}\r\nLS_X: {sticks.LeftStickAxisX:F2} | LS_Y: {sticks.LeftStickAxisY:F2} | RS_X: {sticks.RightStickAxisX:F2} | RS_Y: {sticks.RightStickAxisY:F2} | LT: {triggers.LT:F2} | RT: {triggers.RT:F2}\r\nButtons: A: {buttons.A} | B: {buttons.B} | X: {buttons.X} | Y: {buttons.Y} | LB: {buttons.LB} | RB: {buttons.RB} | BACK: {buttons.Back} | START: {buttons.Menu} | LS: {buttons.LeftStick} | RS: {buttons.RightStick}\r\nDPad: Up {dPad.Up} | Right: {dPad.Right} | Down: {dPad.Down} | Left: {dPad.Left}\r\nKeyboard: {Keyboard.GetKeys().ToString()}";

            obj1.X += (int)(obj1.InitialSpeed * sticks.LeftStickAxisX);
            obj1.Y += (int)(obj1.InitialSpeed * sticks.LeftStickAxisY * -1);

            obj2.X += (int)(obj2.InitialSpeed * sticks.RightStickAxisX);
            obj2.Y += (int)(obj2.InitialSpeed * sticks.RightStickAxisY * -1);

            Keys mod = Keyboard.GetKeyModifiers();

            if (Keyboard.IsKeyDown(Keys.W))
            {
                if (!obj2.IsAnimated)
                    obj2.PlayAnimation("anim");
                if (Keyboard.IsKeyDown(Keys.ControlKey))
                    obj1.Y -= (int)(obj1.InitialSpeed * 0.5f);
                else obj1.Y -= obj1.InitialSpeed;
            }
            if (Keyboard.IsKeyDown(Keys.S))
            {
                if (!obj2.IsAnimated)
                    obj2.PlayAnimation("anim");
                if (Keyboard.IsKeyDown(Keys.ControlKey))
                    obj1.Y += (int)(obj1.InitialSpeed * 0.5f);
                else obj1.Y += obj1.InitialSpeed;
            }
            if (Keyboard.IsKeyDown(Keys.A))
            {
                if (!obj2.IsAnimated)
                    obj2.PlayAnimation("anim");
                if (Keyboard.IsKeyDown(Keys.ControlKey))
                    obj1.X -= (int)(obj1.InitialSpeed * 0.5f);
                else obj1.X -= obj1.InitialSpeed;
            }
            if (Keyboard.IsKeyDown(Keys.D))
            {
                if (!obj2.IsAnimated)
                    obj2.PlayAnimation("anim");
                if (Keyboard.IsKeyDown(Keys.ControlKey))
                    obj1.X += (int)(obj1.InitialSpeed * 0.5f);
                else obj1.X += obj1.InitialSpeed;
            }

            if (obj1.IsCollided)
                scene.RemoveGameObject(obj1);
        }
    }

    public class Obj1 : GameObject
    {
        public Obj1() : base(0, "obj1")
        {
            this.BackColor = Color.Transparent;
            this.Height = 32;
            this.Width = 32;
            this.X = 100;
            this.Y = 300;
            this.IsCollidable = true;
            this.SetCollider(4, 4, 24, 24);
        }

        public int InitialSpeed { get; } = 5;

        public override void OnCollide(string collidedObjectInternalName)
        {
            GameApplication.Log(craftersmine.GameEngine.Utils.LogEntryType.Info, this.InternalName + " Collided with " + collidedObjectInternalName, true);
            if (collidedObjectInternalName == "obj2")
                this.IsCollided = true;
        }
    }

    public class Obj2 : GameObject
    {
        public Obj2() : base(1, "obj2")
        {
            this.BackColor = Color.Transparent;
            this.Height = 32;
            this.Width = 32;
            this.X = 200;
            this.Y = 200;
            this.IsCollidable = true;
            this.SetCollider(0, 0, 32, 32);
        }

        public int InitialSpeed { get; } = 10;
    }
}
