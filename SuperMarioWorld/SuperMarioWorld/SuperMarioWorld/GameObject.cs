using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{ 
    public abstract class GameObject
    {
        // Delegate for creating and destroying objects while in game
        public delegate void DestroyObject(GameObject gameObject);
        public delegate void CreateObject(GameObject gameObject);

        public CreateObject create;
        public DestroyObject destroy;

        // Sprite (texture) of this object
        public Sprite sprite;
        // Collision box of this object
        public Rectangle boundingBox;
        // Current position of this object
        public Vector2 position;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameObject()
        {
            position = Vector2.Zero;

            sprite = new Sprite();
            sprite.layer = 0.5f;  
        }

        /// <summary>
        /// An object in the game with a position and a texture.
        /// </summary>
        /// <param name="position">Position of the object.</param>
        /// <param name="batch">Batch that the texture should be drawn on.</param>
        public GameObject(Point position)
        {
            this.position.X = position.X;
            this.position.Y = position.Y;

            sprite = new Sprite();
            sprite.layer = 0.5f;
        }

        /// <summary>
        /// Execute this function when colliding with something.
        /// </summary>
        /// <param name="collider">the other thing that is being collided with.</param>
        public abstract void OnCollision(GameObject collider);

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="gameTime">Gametime that has passed since prev. frame</param>
        public virtual void Update(GameTime gameTime)
        {
            // Call this objects sprite to update its animation
            sprite.UpdateAnimation(gameTime);
        }

        /// <summary>
        /// Called to draw the game object
        /// </summary>
        /// <param name="batch">Spritebatch to draw to</param>
        public virtual void DrawObject(SpriteBatch batch)
        {
            // Call the draw function of sprite
            sprite.DrawSpriteCentered(batch, position);
        }
    }
}
