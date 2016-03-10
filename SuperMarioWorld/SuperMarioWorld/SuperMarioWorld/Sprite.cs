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

        //Coords of frame (top left)
        private int _texCoordX, _texCoordY;
        //The frame the animation is at
        private int _animIndex = 0;
        //effect to flip the sprite
        public SpriteEffects effect = SpriteEffects.None;
        //set the layer of this sprite to be drawn in.
        public float layer;

        /// <summary>
        /// default constructor
        /// </summary>
        public Sprite()
        {
            animationPositions = new List<Vector2>();
            _animationProgress = animationSpeed;
        }

        /// <summary>
        /// Updates the sprite, if time has come
        /// </summary>
        public void UpdateAnimation(GameTime gameTime)
        {
            //if there are animation frames
            if (animationPositions.Count != 0)
            {
                //take update time (ms) from animation progress
                _animationProgress += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                //When it reaches 0 or less, set next frame.
                if (_animationProgress >= animationSpeed)
                {
                    _animIndex++;

                    //if index is higher then the amount of frames, reset it
                    if (_animIndex >= animationPositions.Count)
                        _animIndex = 0;

                    //reset progress
                    _animationProgress = 0;

                    //Set frame coords
                    _texCoordX = (int)animationPositions[_animIndex].X * xSize;
                    _texCoordY = (int)animationPositions[_animIndex].Y * ySize;
                }
            }
            //else, display entire image
            else
            {
                xSize = texture.Width;
                ySize = texture.Height;
            }
        }

        /// <summary>
        /// clear current animation frames & add frames
        /// </summary>
        /// <param name="animation">New frames</param>
        public void NewAnimation(List<Vector2> animation)
        {
            NewAnimation();
            foreach (Vector2 v in animation)
                animationPositions.Add(v);
        }

        /// <summary>
        /// Clear current animation frames
        /// </summary>
        public void NewAnimation()
        {
            _animIndex = 0;
            _animationProgress = animationSpeed;
            animationPositions.Clear();
        }

        /// <summary>
        /// Add frame to the array of frames
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddFrame(int x, int y)
        {
            animationPositions.Add(new Vector2(x, y));
        }

        /// <summary>
        /// Draws this object's sprite on the SpriteBatch.
        /// </summary>
        /// <param name="batch">The batch which should be drawn on.</param>
        /// /// <param name="position">The world position of the sprite</param>
        public void DrawSprite(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(texture, new Rectangle((int)Math.Round(position.X) -(xSize / 2), (int)Math.Round(position.Y) - ySize, xSize, ySize), new Rectangle(_texCoordX, _texCoordY, xSize, ySize), Color.White, 0, Vector2.Zero, effect, layer);
        }
    }
}
