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
        public GameObject(Vector2 position, SpriteBatch batch)
        { 
            this.position = position;

            //TEMP
            sprite = new Sprite();
            sprite.sourceName = "Mario";
            sprite.xSize = 16;
            sprite.ySize = 32;
            sprite.animationSpeed = 150.0f;
            sprite.animationPositions.Add(new Vector2(0, 0));
            sprite.animationPositions.Add(new Vector2(1, 0));
            sprite.animationPositions.Add(new Vector2(0, 0));
            sprite.animationPositions.Add(new Vector2(1, 0));
            sprite.animationPositions.Add(new Vector2(0, 0));
            sprite.animationPositions.Add(new Vector2(1, 0));
            sprite.animationPositions.Add(new Vector2(0, 1));
            sprite.animationPositions.Add(new Vector2(1, 1));
            sprite.animationPositions.Add(new Vector2(0, 1));
            sprite.animationPositions.Add(new Vector2(1, 1));
            sprite.animationPositions.Add(new Vector2(0, 1));
            sprite.animationPositions.Add(new Vector2(1, 1));
            sprite.animationPositions.Add(new Vector2(2, 2));
            sprite.animationPositions.Add(new Vector2(1, 2));
            sprite.animationPositions.Add(new Vector2(0, 2));
            sprite.animationPositions.Add(new Vector2(2, 2));
            sprite.animationPositions.Add(new Vector2(1, 2));
            sprite.animationPositions.Add(new Vector2(0, 2));
            sprite.animationPositions.Add(new Vector2(2, 3));
            sprite.animationPositions.Add(new Vector2(1, 3));
            sprite.animationPositions.Add(new Vector2(0, 3));
            sprite.animationPositions.Add(new Vector2(2, 3));
            sprite.animationPositions.Add(new Vector2(1, 3));
            sprite.animationPositions.Add(new Vector2(0, 3));
            //TEMP
        }

        public void Update(GameTime gameTime)
        {
            sprite.UpdateAnimation(gameTime);
        }

        /// <summary>
        /// Draws this object's sprite on the SpriteBatch.
        /// </summary>
        /// <param name="batch">The batch which should be drawn on.</param>
        public void Draw(SpriteBatch batch)
        {
            //Draw this object's sprite on the correct position on the SpriteBatch.
            sprite.Draw(batch, position);
        }
    }
}
