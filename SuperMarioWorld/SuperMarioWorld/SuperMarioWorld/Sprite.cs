using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    class Sprite
    {
        //Speed of the animation of the sprite. (ms per frame)
        public float animationSpeed = 250.0f;
        private float _animationProgress;

        /// <summary>
        /// Name of the source of the texture. (without extension!)
        /// </summary>
        public string sourceName;

        /// <summary>
        /// Texture which is loaded in from a file.
        /// </summary>
        public Texture2D texture { get; set; }

        //X & Y size of a single sprite in sprite sheet
        public int xSize, ySize;

        //Vars for cycling and displaying animated sprite sheets
        public List<Vector2> animationPositions;
        private int texCoordX, texCoordY;
        private int animIndex = 0;

        /// <summary>
        /// default constructor
        /// </summary>
        public Sprite()
        {
            animationPositions = new List<Vector2>();
            _animationProgress = animationSpeed;
            xSize = 1;
            ySize = 1;
        }

        /// <summary>
        /// Updates the sprite, if time has come
        /// </summary>
        public void UpdateAnimation(GameTime gameTime)
        {
            if (animationPositions.Count != 0)
            {
                _animationProgress += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_animationProgress >= animationSpeed)
                {
                    animIndex++;

                    if (animIndex == animationPositions.Count)
                        animIndex = 0;

                    _animationProgress = 0;

                    texCoordX = (int)animationPositions[animIndex].X * xSize;
                    texCoordY = (int)animationPositions[animIndex].Y * ySize;
                }
            }
            else
            {
                xSize = texture.Width;
                ySize = texture.Height;
            }
        }

        /// <summary>
        /// Draws this object's sprite on the SpriteBatch.
        /// </summary>
        /// <param name="batch">The batch which should be drawn on.</param>
        /// /// <param name="position">The world position of the sprite</param>
        public void Draw(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(texture, new Rectangle((int)position.X -(xSize / 2), (int)position.Y - ySize, xSize, ySize), new Rectangle(texCoordX, texCoordY, xSize, ySize), Color.White);
        }
    }
}
