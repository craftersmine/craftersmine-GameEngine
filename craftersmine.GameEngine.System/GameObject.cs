﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
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
    public class GameObject : Control
    {
        private Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
        private Dictionary<string, Animation> _animations = new Dictionary<string, Animation>();

        /// <summary>
        /// Game object identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Internal game object name
        /// </summary>
        public string InternalName { get; set; }
        /// <summary>
        /// Position of object by X axis
        /// </summary>
        public int X { get { return Location.X; }
            set
            {
                UpdateCollider();
                Location = new Point(value, this.Y);
            }
        }
        /// <summary>
        /// Position of object by Y axis. REMARK: Y axis is inverted! To move object down, you need to add value instead subtract
        /// </summary>
        public int Y { get { return Location.Y; }
            set
            {
                UpdateCollider();
                Location = new Point(this.X, value);
            }
        }

        internal int ColliderOffsetX { get; set; }
        internal int ColliderOffsetY { get; set; }
        internal int ColliderWidth { get; set; }
        internal int ColliderHeight { get; set; }

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

        /// <summary>
        /// Gets value indicating that object is animated
        /// </summary>
        public bool IsAnimated { get; private set; }
        
        /// <summary>
        /// Game object current animation
        /// </summary>
        public Animation ObjectAnimation { get; set; }

        /// <summary>
        /// Game object collision bounding box
        /// </summary>
        public Rectangle BoundingBox { get; set; }

        /// <summary>
        /// Creates new <see cref="GameObject"/> instance
        /// </summary>
        public GameObject()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
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
        public void ApplyTexture(Texture texture)
        {
            this.BackgroundImage = texture.TextureImage;
        }
        /// <summary>
        /// Applies <see cref="Texture"/> from object texture storage to this object at foreground layer if <paramref name="isBackground"/> false, else at background
        /// </summary>
        /// <param name="name"><see cref="Texture"/> name</param>
        public void ApplyAddedTexture(string name)
        {
            ApplyTexture(_textures[name]);
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
                this.BackgroundImage = ObjectAnimation.GetFrame(ObjectAnimation.CurrentFrame);
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
            ColliderWidth = width;
            ColliderHeight = height;
            UpdateCollider();
        }

        internal void UpdateCollider()
        {
            BoundingBox = new Rectangle(this.X + ColliderOffsetX, this.Y + ColliderOffsetY, ColliderWidth, ColliderHeight);
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
        /// <param name="collidedObject">Collided object data</param>
        public virtual void OnCollide(GameObject collidedObject)
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
            UpdateCollider();
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
                this.BackgroundImage = ObjectAnimation.GetFrame(ObjectAnimation.CurrentFrame);
            }
            if (IsTinted)
            {
                if (TintTickCounter == TintDuration)
                {
                    switch (TintTarget)
                    {
                        case TintTargets.Background:
                            this.BackgroundImage = UntintedImage;
                            this.IsTinted = false;
                            break;
                    }
                }
                TintTickCounter++;
            }
        }

        internal int TintTickCounter { get; set; }
        internal int TintDuration { get; set; }
        internal Image UntintedImage { get; set; }

        /// <summary>
        /// Gets tinting game object texture layer
        /// </summary>
        public TintTargets TintTarget { get; internal set; }
        /// <summary>
        /// Gets true if game object texture layer is currently tinted
        /// </summary>
        public bool IsTinted { get; internal set; }

        /// <summary>
        /// Tints game object texture with RGB color for <paramref name="tickDuration"/> game ticks count
        /// </summary>
        /// <param name="r">Red color component (0.0d - 1.0d)</param>
        /// <param name="g">Green color component (0.0d - 1.0d)</param>
        /// <param name="b">Blue color component (0.0d - 1.0d)</param>
        /// <param name="target">Tinting game object texture layer</param>
        /// <param name="tickDuration">Duration of tint in game ticks</param>
        public void Tint(double r, double g, double b, TintTargets target, int tickDuration)
        {
            this.TintTickCounter = 0;
            TintTarget = target;
            this.TintDuration = tickDuration;
            switch (target)
            {
                case TintTargets.Background:
                    Bitmap bitmap = new Bitmap(this.BackgroundImage);
                    UntintedImage = this.BackgroundImage;
                    for (int imageX = 0; imageX < bitmap.Width; imageX++)
                    {
                        for (int imageY = 0; imageY < bitmap.Height; imageY++)
                        {
                            Color targetColor = bitmap.GetPixel(imageX, imageY);
                            if (targetColor.A != 0)
                            {
                                double rMixed = targetColor.R + (1 - targetColor.R / 255.0d) * (r * 255.0d);
                                double gMixed = targetColor.G + (1 - targetColor.G / 255.0d) * (g * 255.0d);
                                double bMixed = targetColor.B + (1 - targetColor.B / 255.0d) * (b * 255.0d);
                                bitmap.SetPixel(imageX, imageY, Color.FromArgb(targetColor.A, (int)rMixed, (int)gMixed, (int)bMixed));
                            }
                        }
                    }
                    this.BackgroundImage = bitmap;
                    this.IsTinted = true;
                    break;
            }
        }

        /// <summary>
        /// Target of tint
        /// </summary>
        public enum TintTargets
        {
            /// <summary>
            /// Background layer tint
            /// </summary>
            Background,
            /// <summary>
            /// Foreground layer tint
            /// </summary>
            Foreground
        }
    }
}
