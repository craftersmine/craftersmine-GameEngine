using System;
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
    public class GameObject
    {
        private Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
        private Dictionary<string, Animation> _animations = new Dictionary<string, Animation>();
        private TextureLayout textureLayout { get; set; }

        /// <summary>
        /// Gets current <see cref="GameObject"/> texture
        /// </summary>
        public Texture CurrentTexture { get; internal set; }

        /// <summary>
        /// Gets is <see cref="GameObject"/> tiled texture cached in memory
        /// </summary>
        public bool IsTiledTextureCached { get; internal set; }
        /// <summary>
        /// Gets current cached tiled texture from memory
        /// </summary>
        public Texture TiledTextureCache { get; internal set; }

        /// <summary>
        /// Gets or sets game object identifier
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Gets or sets internal game object name
        /// </summary>
        public string InternalName { get; set; }

        private int xCoord = 0;
        private int yCoord = 0;

        /// <summary>
        /// Gets or sets position of object by X axis
        /// </summary>
        public int X { get { return xCoord; }
            set
            {
                xCoord = value;
                UpdateCollider();
            }
        }
        /// <summary>
        /// Gets or sets position of object by Y axis. REMARK: Y axis is inverted! To move object down, you need to add value instead subtract
        /// </summary>
        public int Y { get { return yCoord; }
            set
            {
                yCoord = value;
                UpdateCollider();
            }
        }

        /// <summary>
        /// Gets or sets width of game object
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets height of game object
        /// </summary>
        public int Height { get; set; }

        internal int ColliderOffsetX { get; set; }
        internal int ColliderOffsetY { get; set; }
        internal int ColliderWidth { get; set; }
        internal int ColliderHeight { get; set; }

        /// <summary>
        /// Gets <code>true</code> if object collided with other object, else <code>false</code>
        /// </summary>
        public bool IsCollided { get; internal set; }
        /// <summary>
        /// Gets <code>true</code> if mouse clicked on this object, else <code>false</code>
        /// </summary>
        public bool IsClicked { get; internal set; }
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
        /// Gets is mouse pointer in <see cref="GameObject"/> collision bounding box
        /// </summary>
        public bool IsMouseOnObject { get; internal set; }

        /// <summary>
        /// Creates new <see cref="GameObject"/> instance
        /// </summary>
        public GameObject()
        {
            IsCollidable = true;
        }

        /// <summary>
        /// Applies <see cref="Texture"/> to this object
        /// </summary>
        /// <param name="texture"><see cref="Texture"/> data</param>
        public void ApplyTexture(Texture texture)
        {
            if (texture.TextureLayout == TextureLayout.Tile)
                this.IsTiledTextureCached = false;
            this.CurrentTexture = texture;
            this.textureLayout = texture.TextureLayout;
        }
        /// <summary>
        /// Applies <see cref="Texture"/> from object texture storage to this object
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
                this.CurrentTexture = new Texture(ObjectAnimation.GetFrame(ObjectAnimation.CurrentFrame), TextureLayout.Stretch);
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
        /// Calls at click on object collision bounding box
        /// </summary>
        /// <param name="mouseButtons">Clicked mouse button</param>
        /// <param name="xPos">Position of mouse click in global X axis</param>
        /// <param name="yPos">Position of mouse click in global Y axis</param>
        public virtual void OnMouseClick(int xPos, int yPos, MouseButtons mouseButtons)
        {

        }
        /// <summary>
        /// Calls at click release on object
        /// </summary>
        /// <param name="mouseButtons">Released mouse button</param>
        public virtual void OnMouseUp(MouseButtons mouseButtons)
        {

        }
        /// <summary>
        /// Calls while mouse moved in object collision bounding box
        /// </summary>
        /// <param name="xPos">Position of mouse in global X axis</param>
        /// <param name="yPos">Position of mouse in global Y axis</param>
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
        /// <summary>
        /// Calls at mouse pointer is in object collision bounding box
        /// </summary>
        public virtual void OnMouseLeave()
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
                this.CurrentTexture = new Texture(ObjectAnimation.GetFrame(ObjectAnimation.CurrentFrame), TextureLayout.Stretch);
            }
        }

        internal void CheckCollisions(List<GameObject> gameObjects)
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (this != gameObjects[i])
                {
                    if (this.BoundingBox.IntersectsWith(gameObjects[i].BoundingBox))
                    {
                        this.IsCollided = true;
                        this.OnCollide(gameObjects[i]);
                    }
                }
            }
        }
    }
}
