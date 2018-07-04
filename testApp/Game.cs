using System;
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
using craftersmine.GameEngine.Utils;
using craftersmine.GameEngine.Network;

namespace testApp
{
    class Game : GameWindow
    {
        Scene scene = new Scene() { Id = 0 };
        public static ContentStorage cs;
        public static ContentStorage gc1;
        GameObject[] bordersGrads;
        bool IsTinting = false;
        float TintVal = 0.0f;

        public Game(string title = "TestGameApp", int width = 1280, int height = 720) : base(title, width, height)
        {
            this.SetBackgroundColor(Color.Black);
            cs = new ContentStorage("testassets");
            gc1 = new ContentStorage("gamecontent1");
            //config = new GameConfig("Configs", "TestConfig");
            cs.ContentStorageCreated += Cs_ContentStorageCreated;
            cs.ContentLoading += Cs_ContentLoading;
        }

        private void Cs_ContentLoading(object sender, ContentLoadingEventArgs e)
        {
            GameApplication.Log(craftersmine.GameEngine.Utils.LogEntryType.Info, "Loading content from " + e.PackageName + " with name \"" + e.ContentFileName + "\" type of " + e.ContentType.ToString());
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
            GameApplication.SetLogger(new Logger(Path.Combine(GameApplication.AppDataGameRoot, "Logs"), "testGame"));
            GameApplication.Log(LogEntryType.Info, "Initializing Game...");
            //GameClient gameClient = new GameClient(this);
            //gameClient.Connect("127.0.0.1", 2000);
            //this.Controls.Add(Program.labelDebug);
            Random rnd = new Random();
            //scene.AddAudioChannel(new craftersmine.GameEngine.Objects.AudioChannel("aud", cs.LoadAudio("aud")));
            //Program.labelDebug.Font = cs.LoadFont("andy", 9);
            //scene.SetAudioChannelVolume("aud", 0.1f);
            //scene.SetAudioChannelRepeat("aud", true);
            Gamepad.SetDeadzone(Player.First, DeadZoneControl.LeftTrigger, 0.0f);
            scene.SetBackgroundColor(Color.Black);
            this.AddScene(scene);
            scene.SetBackgroundTexture(cs.LoadTexture("bg", TextureLayout.Stretch));

            scene.Controls.Add(labelDebugKeyboard);

            scene.AddGameObject(obj1);
            scene.AddGameObject(obj2);
            obj1.ApplyTexture(cs.LoadTexture("obj1", TextureLayout.Tile));
            obj2.ApplyTexture(cs.LoadTexture("obj2", TextureLayout.Stretch));

            obj2.AddAnimation("anim", cs.LoadAnimation("anim"));
            
            //obj1.ApplyAnimation(cs.LoadAnimation("anim"));

            ShowScene(0);
            bordersGrads = new GameObject[4];
            for (int c = 0; c < 4; c++)
            {
                switch (c)
                {
                    case 0:
                        bordersGrads[c] = new GameObject() { Height = this.Height, Width = 64, X = 0, Y = 0, IsCollidable = false };
                        bordersGrads[c].ApplyTexture(gc1.LoadTexture("bordergrad-left", TextureLayout.Tile));
                        break;
                    case 1:
                        bordersGrads[c] = new GameObject() { Height = 64, Width = this.Width, X = 0, Y = 0, IsCollidable = false };
                        bordersGrads[c].ApplyTexture(gc1.LoadTexture("bordergrad-up", TextureLayout.Tile));
                        break;
                    case 2:
                        bordersGrads[c] = new GameObject() { Height = this.Height, Width = 64, X = this.Width - 64, Y = 0, IsCollidable = false };
                        bordersGrads[c].ApplyTexture(gc1.LoadTexture("bordergrad-right", TextureLayout.Tile));
                        break;
                    case 3:
                        bordersGrads[c] = new GameObject() { Height = 64, Width = this.Width, X = 0, Y = this.Height - 64, IsCollidable = false };
                        bordersGrads[c].ApplyTexture(gc1.LoadTexture("bordergrad-down", TextureLayout.Tile));
                        break;
                }
                scene.AddGameObject(bordersGrads[c]);
            }
            //scene.PlayAudioChannel("aud");
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

            if (GameApplication.GetGameTick() == 100)
            {
                IsTinting = true;
            }
            if (IsTinting && GameApplication.GetGameTick() < 130 && GameApplication.GetGameTick() > 100)
            {
                TintVal += 0.05f;
                if (TintVal < 1.0f)
                    scene.TintScene(Color.Black, TintVal);
                else
                {
                    scene.TintScene(Color.Black, 1.0f);
                    IsTinting = false;
                }
            }
            if (!IsTinting && GameApplication.GetGameTick() > 130)
            {
                TintVal -= 0.05f;
                if (TintVal > 0.0f)
                    scene.TintScene(Color.Black, TintVal);
                else scene.TintScene(Color.Black, 0.0f);
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }

    public class Obj1 : NetworkGameObject
    {
        public Obj1()
        {
            this.InternalName = "obj1";
            this.BackColor = Color.Transparent;
            this.Height = 48;
            this.Width = 48;
            this.X = 100;
            this.Y = 300;
            this.IsCollidable = false;
            this.SetCollider(4, 4, 24, 24);
            this.IsTransmittingLocation = true;
        }

        public int InitialSpeed { get; } = 5;

        public override void OnCollide(GameObject collidedObject)
        {
            GameApplication.Log(LogEntryType.Info, this.InternalName + " Collided with " + collidedObject.InternalName, true);
            if (collidedObject.InternalName == "obj2")
                this.IsCollided = true;
        }

        public override void OnMouseClick(int xPos, int yPos, MouseButtons mouseButtons)
        {
            MessageBox.Show("Clicked on " + this.InternalName + " with " + mouseButtons.ToString() + " button");
        }

        public override void OnMouseMove(int xPos, int yPos, MouseButtons mouseButtons)
        {
            GameApplication.Log(LogEntryType.Info, "Mouse position X: " + xPos + " Y: " + yPos);
        }

        public override void OnMouseUp(MouseButtons mouseButtons)
        {
            GameApplication.Log(LogEntryType.Info, "Mouse button up event called!");
        }
    }

    public class Obj2 : GameObject
    {
        public Obj2()
        {
            this.InternalName = "obj2";
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
