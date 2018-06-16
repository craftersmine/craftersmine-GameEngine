using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using craftersmine.GameEngine.Content;
using craftersmine.GameEngine.Input;

namespace craftersmine.GameEngine.System
{
    /// <summary>
    /// Main part of game. Represents an entity, item or other object in game.
    /// </summary>
    public class GameObject : PictureBox
    {
        private Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
        private Dictionary<string, Animation> _animations = new Dictionary<string, Animation>();

        /// <summary>
        /// Game object identifier
        /// </summary>
        public int Id { get; private set; }
        /// <summary>
        /// Internal game object name
        /// </summary>
        public string InternalName { get; private set; }
        /// <summary>
        /// Position of object by X axis
        /// </summary>
        public int X { get { return Location.X; }
            set
            {
                UpdateCollider(value, this.Y, this.Width, this.Height);
                Location = new Point(value, this.Y);
            }
        }
        /// <summary>
        /// Position of object by Y axis. REMARK: Y axis is inverted! To move object down, you need to add value instead subtract
        /// </summary>
        public int Y { get { return Location.Y; }
            set
            {
                UpdateCollider(this.X, value, this.Width, this.Height);
                Location = new Point(this.X, value);
            }
        }

        internal int ColliderOffsetX { get; set; }
        internal int ColliderOffsetY { get; set; }

        /// <summary>
        /// <code>true</code> if object collided with other object, else <code>false</code>
        /// </summary>
        public bool IsCollided { get; set; }
        /// <summary>
        /// <code>true</code> if mouse clicked on this object, else <code>false</code>
        /// </summary>
        public bool IsClicked { get; set; }
        /// <summary>
        /// Sets or gets a value indicating whether the collision event should be handled with another object 
        /// </summary>
        public bool IsCollidable { get; set; }

        public bool IsAnimated { get; private set; }
        
        public Animation ObjectAnimation { get; set; }

        /// <summary>
        /// Game object collision bounding box
        /// </summary>
        public Rectangle BoundingBox { get; set; }

        /// <summary>
        /// Creates new <see cref="GameObject"/>
        /// </summary>
        /// <param name="id">Game object identifier</param>
        /// <param name="internalName">Internal game object name</param>
        public GameObject(int id, string internalName)
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            Id = id;
            InternalName = internalName;
            IsCollidable = true;
            this.DoubleBuffered = true;
            this.MouseClick += GameObject_MouseClick;
            this.MouseEnter += GameObject_MouseEnter;
            this.MouseHover += GameObject_MouseHover;
            this.MouseLeave += GameObject_MouseLeave;
            this.MouseMove += GameObject_MouseMove;
            this.MouseUp += GameObject_MouseUp;
        }
        
        private void GameObject_MouseUp(object sender, MouseEventArgs e) => OnMouseUp(e.Button);

        private void GameObject_MouseMove(object sender, MouseEventArgs e) => OnMouseMove(e.X, e.Y, e.Button);

        private void GameObject_MouseLeave(object sender, EventArgs e) => OnMouseLeave();

        private void GameObject_MouseHover(object sender, EventArgs e) => OnMouseHover();

        private void GameObject_MouseEnter(object sender, EventArgs e) => OnMouseEnter();

        private void GameObject_MouseClick(object sender, MouseEventArgs e) => OnMouseClick(e.Button);

        /// <summary>
        /// Applies <see cref="Texture"/> to this object
        /// </summary>
        /// <param name="texture"><see cref="Texture"/> data</param>
        /// <param name="isBackground">Use <paramref name="isBackground"/> = <code>true</code>, if game object is static or have another texture layer instead <code>false</code></param>
        public void ApplyTexture(Texture texture, bool isBackground = true)
        {
            if (isBackground)
                this.BackgroundImage = texture.TextureImage;
            else this.Image = texture.TextureImage;
        }

        /// <summary>
        /// Applies <see cref="Texture"/> to this object at foreground layer
        /// </summary>
        /// <param name="texture"><see cref="Texture"/> data</param>
        public void ApplyTexture(Texture texture)
        {
            ApplyTexture(texture, true);
        }
        /// <summary>
        /// Applies <see cref="Texture"/> from object texture storage to this object at foreground layer if <paramref name="isBackground"/> false, else at background
        /// </summary>
        /// <param name="name"><see cref="Texture"/> name</param>
        /// <param name="isBackground">If <code>true</code> applies to background, else to foreground</param>
        public void ApplyAddedTexture(string name, bool isBackground = true)
        {
            ApplyTexture(_textures[name], isBackground);
        }
        /// <summary>
        /// Adds <see cref="Texture"/> to object storage
        /// </summary>
        /// <param name="name">Name of adding <see cref="Texture"/></param>
        /// <param name="texture"><see cref="Texture"/> data</param>
        public void AddTexture(string name, Texture texture)
        {
            _textures.Add(name, texture);
        }
        /// <summary>
        /// Plays selected <see cref="Animation"/> from object animation storage
        /// </summary>
        /// <param name="name">Name of animation</param>
        public void PlayAnimation(string name)
        {
            ApplyAnimation(_animations[name]);
        }
        /// <summary>
        /// Stops currently playing <see cref="Animation"/>
        /// </summary>
        public void StopAnimation()
        {
            ResetCurrentAnimation();
        }
        /// <summary>
        /// Adds <see cref="Animation"/> to object <see cref="Animation"/> storage
        /// </summary>
        /// <param name="name"></param>
        /// <param name="animation"></param>
        public void AddAnimation(string name, Animation animation)
        {
            _animations.Add(name, animation);
        }
        /// <summary>
        /// Applies <see cref="Animation"/> to this object and starts playing this animation immediately
        /// </summary>
        /// <param name="animation"></param>
        public void ApplyAnimation(Animation animation)
        {
            ObjectAnimation = animation;
            IsAnimated = true;
        }
        /// <summary>
        /// Stops currently playing <see cref="Animation"/>
        /// </summary>
        public void ResetCurrentAnimation()
        {
            IsAnimated = false;
            if (ObjectAnimation != null)
            {
                ObjectAnimation.CurrentFrame = 0;
                if (ObjectAnimation.IsBackgroundAnimation)
                    this.BackgroundImage = ObjectAnimation.GetFrame(ObjectAnimation.CurrentFrame);
                else this.Image = ObjectAnimation.GetFrame(ObjectAnimation.CurrentFrame);
            }
        }
        /// <summary>
        /// Starts playing already applied <see cref="Animation"/> from specified frame
        /// </summary>
        /// <param name="startFrame">Frame number where animation starts</param>
        public void StartCurrentAnimation(int startFrame)
        {
            if (ObjectAnimation != null)
            {
                IsAnimated = true;
                if (startFrame < ObjectAnimation.AnimationFramesCount)
                    ObjectAnimation.CurrentFrame = startFrame;
                else if (startFrame == ObjectAnimation.AnimationFramesCount)
                    ObjectAnimation.CurrentFrame = ObjectAnimation.AnimationFramesCount - 1;
                else ObjectAnimation.CurrentFrame = 0;
            }
        }

        /// <summary>
        /// Starts playing already applied <see cref="Animation"/> from first frame
        /// </summary>
        public void StartCurrentAnimation()
        {
            StartCurrentAnimation(0);
        }

        /// <summary>
        /// Sets <see cref="GameObject"/> collider bounds
        /// </summary>
        /// <param name="x">Offset by X axis from upper left corner of <see cref="GameObject"/></param>
        /// <param name="y">Offset by Y axis from upper left corner of <see cref="GameObject"/></param>
        /// <param name="width">Width of collider</param>
        /// <param name="height">Height of collider</param>
        public void SetCollider(int x, int y, int width, int height)
        {
            ColliderOffsetX = x;
            ColliderOffsetY = y;
            UpdateCollider(this.X, this.Y, width, height);
        }

        internal void UpdateCollider(int x, int y, int width, int height)
        {
            BoundingBox = new Rectangle(x + ColliderOffsetX, y + ColliderOffsetY, width, height);
        }

        /// <summary>
        /// Calls at object creation
        /// </summary>
        public virtual void OnCreate()
        {

        }
        /// <summary>
        /// Calls at object destroy
        /// </summary>
        public virtual void OnDestroy()
        {

        }
        /// <summary>
        /// Calls if object collided with another object
        /// </summary>
        /// <param name="collidedObjectInternalName">Collided object name</param>
        public virtual void OnCollide(string collidedObjectInternalName)
        {

        }
        /// <summary>
        /// Calls at click on object
        /// </summary>
        /// <param name="mouseButtons">Clicked mouse button</param>
        public virtual void OnMouseClick(MouseButtons mouseButtons)
        {
            IsClicked = true;
        }
        /// <summary>
        /// Calls at click release on object
        /// </summary>
        /// <param name="mouseButtons">Released mouse button</param>
        public virtual void OnMouseUp(MouseButtons mouseButtons)
        {
            IsClicked = false;
        }
        /// <summary>
        /// Calls while mouse on object
        /// </summary>
        public virtual void OnMouseHover()
        {

        }
        /// <summary>
        /// Calls if mouse entered in object bounding box
        /// </summary>
        public virtual void OnMouseEnter()
        {

        }
        /// <summary>
        /// Calls if mouse leaved from object bounding box
        /// </summary>
        public virtual void OnMouseLeave()
        {

        }
        /// <summary>
        /// Calls while mouse moved in object bounding box
        /// </summary>
        /// <param name="xPos">Position of mouse in X axis</param>
        /// <param name="yPos">Position of mouse in Y axis</param>
        /// <param name="mouseButtons">Clicked mouse buttons</param>
        public virtual void OnMouseMove(int xPos, int yPos, MouseButtons mouseButtons)
        {

        }
        /// <summary>
        /// Calls at game update
        /// </summary>
        public virtual void OnUpdate()
        {

        }
        
        internal void InternalUpdate()
        {
            if (IsAnimated)
            {
                ObjectAnimation.CountedTicks++;
                if (ObjectAnimation.CountedTicks == ObjectAnimation.FrameTickTrigger)
                {
                    ObjectAnimation.CountedTicks = 0;
                    if (ObjectAnimation.CurrentFrame == ObjectAnimation.AnimationFramesCount - 1)
                        ObjectAnimation.CurrentFrame = 0;
                    else ObjectAnimation.CurrentFrame++;
                }
                if (ObjectAnimation.IsBackgroundAnimation)
                    this.BackgroundImage = ObjectAnimation.GetFrame(ObjectAnimation.CurrentFrame);
                else this.Image = ObjectAnimation.GetFrame(ObjectAnimation.CurrentFrame);
            }
        }
    }
}
