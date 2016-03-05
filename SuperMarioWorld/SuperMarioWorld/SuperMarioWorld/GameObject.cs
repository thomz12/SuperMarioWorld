using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    class GameObject
    {
        //Sprite (texture) of this object
        public Sprite sprite;
        //Width and height of objects bounding box
        public int boundingWidth, boundingHeight;
        //Collision box of this object
        public Rectangle boundingBox;
        //Current position of this object
        public Vector2 position;

        /// <summary>
        /// An object in the game with a position and a texture.
        /// </summary>
        /// <param name="position">Position of the object.</param>
        /// <param name="batch">Batch that the texture should be drawn on.</param>
        public GameObject(Vector2 position)
        { 
            this.position = position;

            sprite = new Sprite();
            sprite.AnimationPositions.Add(new Vector2(0, 0));
        }

        /// <summary>
        /// Called every frame
        /// </summary>
        /// <param name="gameTime">Gametime that has passed since prev. frame</param>
        public virtual void Update(GameTime gameTime)
        {
            sprite.UpdateAnimation(gameTime);

            //Create bounding box for the object
            boundingBox.X = (int)position.X - boundingWidth / 2;
            boundingBox.Y = (int)position.Y - boundingHeight;
        }

        /// <summary>
        /// Called to draw the game object
        /// </summary>
        /// <param name="batch">Spritebatch to draw to</param>
        public void DrawObject(SpriteBatch batch)
        {
            //Call the draw function of sprite
            sprite.DrawSprite(batch, position);
        }
    }
}
