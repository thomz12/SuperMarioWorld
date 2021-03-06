﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    public class Sprite
    {
        // Speed of the animation of the sprite. (ms per frame)
        public float animationSpeed;
        private float _animationProgress;

        /// <summary>
        /// Name of the source of the texture. (without extension!)
        /// </summary>
        public string sourceName;

        /// <summary>
        /// Texture which is loaded in from a file.
        /// </summary>
        public Texture2D texture { get; set; }

        // X & Y size of a single sprite in sprite sheet
        public int xSize;
        public int ySize;

        // Vars for cycling and displaying animated sprite sheets
        private List<Point> animationPositions;
        public bool animated;

        // Coords of frame (top left)
        private int _texCoordX;
        private int _texCoordY;

        // The frame the animation is at
        private int _animIndex;
        // Effect to flip the sprite
        public SpriteEffects effect;
        // Set the layer of this sprite to be drawn in.
        public float layer;

        /// <summary>
        /// default constructor
        /// </summary>
        public Sprite()
        {
            _animIndex = 0;
            animationPositions = new List<Point>();
            animationSpeed = 250.0f;
            _animationProgress = animationSpeed;
            animated = true;
            effect = SpriteEffects.None;
        }

        /// <summary>
        /// Constructor with an sourcename
        /// </summary>
        /// <param name="sourceName">Name of the sprite that should be loaded from the Content folder without extension.</param>
        public Sprite(string sourceName)
        {
            _animIndex = 0;
            this.sourceName = sourceName;
            animationPositions = new List<Point>();
            animationSpeed = 250.0f;
            _animationProgress = animationSpeed;
            animated = true;
            effect = SpriteEffects.None;
        }

        /// <summary>
        /// Updates the sprite, if time has come
        /// </summary>
        public void UpdateAnimation(GameTime gameTime)
        {
            // If there are animation frames
            if (animationPositions.Count != 0 && animated)
            {
                // Take update time (ms) from animation progress
                _animationProgress += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

                // When it reaches 0 or less, set next frame.
                if (_animationProgress >= animationSpeed)
                {
                    _animIndex++;

                    // If index is higher then the amount of frames, reset it
                    if (_animIndex >= animationPositions.Count)
                        _animIndex = 0;

                    // Reset progress
                    _animationProgress = 0;

                    // Set frame coords
                    _texCoordX = (int)animationPositions[_animIndex].X * xSize;
                    _texCoordY = (int)animationPositions[_animIndex].Y * ySize;
                }
            }
            // Else, display entire image
            else if(animationPositions.Count == 0)
            {
                xSize = texture.Width;
                ySize = texture.Height;
            }
        }

        /// <summary>
        /// clear current animation frames & add frames
        /// </summary>
        /// <param name="animation">New frames</param>
        public void NewAnimation(int x, int y)
        {
            NewAnimation();
            animationPositions.Add(new Point(x, y));

            _texCoordX = animationPositions[_animIndex].X * xSize;
            _texCoordY = animationPositions[_animIndex].Y * ySize;
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
            animationPositions.Add(new Point(x, y));
            _texCoordX = animationPositions[0].X * xSize;
            _texCoordY = animationPositions[0].Y * ySize;
        }

        /// <summary>
        /// Draws this object's sprite on the SpriteBatch. The sprite is centered.
        /// </summary>
        /// <param name="batch">The batch which should be drawn on.</param>
        /// /// <param name="position">The world position of the sprite</param>
        public void DrawSpriteCentered(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(texture, new Rectangle((int)Math.Round(position.X) -(xSize / 2), (int)Math.Round(position.Y) - ySize, xSize, ySize), new Rectangle(_texCoordX, _texCoordY, xSize, ySize), Color.White, 0, Vector2.Zero, effect, layer);
        }

        /// <summary>
        /// Draws this object's sprite ont he SpriteBatch. The sprite is drawn from the left-top corner.
        /// </summary>
        /// <param name="batch">The batch which should be drawn on.</param>
        /// <param name="position">the world position of the sprite.</param>
        public void DrawSprite(SpriteBatch batch, Vector2 position)
        {
            batch.Draw(texture, new Rectangle((int)Math.Round(position.X), (int)Math.Round(position.Y), xSize, ySize), new Rectangle(_texCoordX, _texCoordY, xSize, ySize), Color.White, 0, Vector2.Zero, effect, layer);
        }
    }
}
