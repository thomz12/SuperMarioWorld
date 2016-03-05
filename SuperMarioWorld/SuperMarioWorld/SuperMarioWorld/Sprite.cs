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
        private List<Vector2> animationPositions;
        public List<Vector2> AnimationPositions
        {
            get
            {
                return animationPositions;
            }
            set
            {
                _animationProgress = animationSpeed;
                _animIndex = 0;
                animationPositions = value;
            }
        }

        private int _texCoordX, _texCoordY;
        private int _animIndex = 0;

        /// <summary>
        /// default constructor
        /// </summary>
        public Sprite()
        {
            animationPositions = new List<Vector2>();
            _animationProgress = animationSpeed;
            xSize = 16;
            ySize = 16;
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
                    _animIndex++;

                    if (_animIndex >= animationPositions.Count)
                        _animIndex = 0;

                    _animationProgress = 0;

                    _texCoordX = (int)animationPositions[_animIndex].X * xSize;
                    _texCoordY = (int)animationPositions[_animIndex].Y * ySize;
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
        public void DrawSprite(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(texture, new Rectangle((int)position.X -(xSize / 2), (int)position.Y - ySize, xSize, ySize), new Rectangle(_texCoordX, _texCoordY, xSize, ySize), Color.White);
        }
    }
}
