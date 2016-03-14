using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    abstract class GameObject
    {
        //Sprite (texture) of this object
        public Sprite sprite;
        //Collision box of this object
        public Rectangle boundingBox;
        //Current position of this object
        public Vector2 position;

        public bool destoryed = false;

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameObject()
        {
            this.position = Vector2.Zero;

            sprite = new Sprite();
            sprite.layer = 0.5f;
        }

        /// <summary>
        /// An object in the game with a position and a texture.
        /// </summary>
        /// <param name="position">Position of the object.</param>
        /// <param name="batch">Batch that the texture should be drawn on.</param>
        public GameObject(Vector2 position)
        {
            this.position = position;

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
            sprite.UpdateAnimation(gameTime);
        }

        /// <summary>
        /// Called to draw the game object
        /// </summary>
        /// <param name="batch">Spritebatch to draw to</param>
        public void DrawObject(SpriteBatch batch)
        {
            //Call the draw function of sprite
            sprite.DrawSpriteCentered(batch, position);
        }
    }
}
