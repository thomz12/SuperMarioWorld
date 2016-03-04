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
        }

        /// <summary>
        /// Draws this object's sprite on the SpriteBatch.
        /// </summary>
        /// <param name="batch">The batch which should be drawn on.</param>
        public void Draw(SpriteBatch batch)
        {
            //Draw this object's sprite on the correct position on the SpriteBatch.
            batch.Draw(sprite.texture, new Rectangle((int)position.X, (int)position.Y, sprite.texture.Width, sprite.texture.Height), Color.White);
        }
    }
}
