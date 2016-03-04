using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    class Sprite
    {
        //Speed of the animation of the sprite.
        public float animationSpeed = 250.0f;
        private float _animationProgress = 250.0f;

        /// <summary>
        /// Name of the source of the texture. (without extension!)
        /// </summary>
        public string sourceName;

        /// <summary>
        /// Texture which is loaded in from a file.
        /// </summary>
        public Texture2D texture;

        //X & Y size of a single sprite in sprite sheet
        public int xSize, ySize;

        //TODO MAKE IT LESS UGLY
        public List<Vector2> animationPositions;
        int texCoordX, texCoordY;
        private int animIndex = 0;
               
        /// <summary>
        /// default constructor
        /// </summary>
        public Sprite()
        {
            animationPositions = new List<Vector2>();
        }

        /// <summary>
        /// Updates the sprite, if time has come
        /// </summary>
        public void UpdateAnimation(GameTime gameTime)
        {
            _animationProgress -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(_animationProgress < 0)
            {
                texCoordX = (int)animationPositions[animIndex].X * xSize;
                texCoordY = (int)animationPositions[animIndex].Y * ySize;

                animIndex++;

                if (animIndex == animationPositions.Count)
                    animIndex = 0;

                _animationProgress = animationSpeed;
            }
        }

        public void Draw(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(texture, new Rectangle((int)position.X, (int)position.Y, xSize, ySize), new Rectangle(texCoordX, texCoordY, xSize, ySize), Color.White);
        }
    }
}
