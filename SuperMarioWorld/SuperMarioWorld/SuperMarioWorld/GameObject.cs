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

            //TEMP
            sprite = new Sprite();
            sprite.xSize = 16;
            sprite.ySize = 32;
            sprite.animationSpeed = 150.0f;
            sprite.animationPositions.Add(new Vector2(0, 0));
            sprite.animationPositions.Add(new Vector2(1, 0));
            //TEMP
        }

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
            sprite.DrawSprite(batch, position);
        }
    }
}
