using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioWorld
{
    public class Camera2D
    {
        // Non moving
        private bool _moveable;

        // Zoom of the camera
        public float zoom;

        // A matrix to scale, rotate and translate the spritebatch
        public Matrix transformMatrix;

        // Current position of the camera
        public Vector2 position;

        // Current rotation of the camera
        public float rotation;

        // The player the camera should track
        private GameObject _target;

        /// <summary>Height of the default SNES resolution (in px)</summary>
        public int gameHeight;
        /// <summary>Width of the default SNES resolution (in px)</summary>
        public int gameWidth;

        // A value for smooth damping, the higher the value the faster the camera moves to its target position.
        private float _smoothness = 4f;

        // A value for the deadzone measured in pixels, in that zone the camera does not move
        private float _xDeadZone = 32.0f;

        // Size of the level in pixels
        private Point _levelSize;

        // Size of the grid in pixels
        private int _gridSize;

        /// <summary>
        /// Constructor for a camera that follows a target
        /// </summary>
        /// <param name="target">The target that the camera should focus on</param>
        /// <param name="size">The size of the level</param>
        /// <param name="grid">The size of the grid</param>
        public Camera2D(GameObject target, Point size, int grid)
        {
            // Set private variables to the corresponding parameters
            _target = target;
            _levelSize = size;
            _gridSize = grid;

            // Default zoom
            zoom = 1.0f;

            // Default rotation
            rotation = 0;

            // If there is a target for the camera, move the camera to its position
            if(target != null)
                position = target.position;

            // Default value of movable
            _moveable = true;
        }

        /// <summary>
        /// Default update function
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // Check if the target exists
            if (_target != null)
            {
                // Check if the camera is allowed to move
                if (_moveable)
                {
                    // Difference between camera and target position
                    Vector2 delta = _target.position - position;

                    // The camera uses smooth damping.
                    float smoothDamping = _smoothness * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);

                    // X axis
                    // If the player moves out of the dead zone, move the camera to the player
                    if (delta.X < -_xDeadZone || delta.X > _xDeadZone)
                    {
                        position = new Vector2(position.X + (_target.position.X - position.X) * smoothDamping, position.Y);
                    }

                    //Limit the camera to the left edge of the map
                    if (position.X < (gameWidth / 2) - (_gridSize / 2))
                        position = new Vector2((gameWidth / 2) - (_gridSize / 2), position.Y);
                    //Limit the camera to the right edge of the map
                    if (position.X > (_levelSize.X * _gridSize) - (gameWidth / 2) - (_gridSize / 2))
                        position = new Vector2((_levelSize.X * _gridSize) - (gameWidth / 2) - (_gridSize / 2), position.Y);

                    //Y axis
                    //if the lower bounds of the camera vieuwport is lower than the lowest pixel in the level (y+ == lower)
                    if (position.Y + (gameHeight / 2) >= (_levelSize.Y * _gridSize))
                    {
                        //set the camera position to snap to the bottom edge (use levelSize for Y coord instead of player Y)
                        position = new Vector2(position.X, (_levelSize.Y * _gridSize) - (gameHeight / 2));

                        // If the player is in the top 1/4th half of the screen, the camera breaks free from the snap.
                        if (delta.Y < -(gameHeight / 4))
                        {
                            position = new Vector2(position.X, position.Y + (_target.position.Y - position.Y) * smoothDamping);
                        }
                    }
                    else
                    {
                        // If the camera not beneath the lower bounds of the level, the position of the camera is the position of the player.
                        position = new Vector2(position.X, position.Y + (_target.position.Y - position.Y) * smoothDamping);
                    }
                }
            }
            else
            {
                //Set the position of the camera to the center of the level
                position = new Vector2((_levelSize.X * _gridSize / 2) - (_gridSize / 2), (_levelSize.Y * _gridSize / 2));
            }
        }

        /// <summary>
        /// Gets the transformation matrix of the camera position/rotation/zoom
        /// </summary>
        /// <param name="device">Graphics device that the camera uses.</param>
        /// <returns>Returns a Matrix type.</returns>
        public Matrix GetTransformation(GraphicsDevice device)
        {
            transformMatrix = Matrix.CreateTranslation(new Vector3(-position.X, -position.Y, 0)) * Matrix.CreateRotationZ(rotation) * Matrix.CreateScale(new Vector3(zoom, zoom, 1)) * Matrix.CreateTranslation(new Vector3(device.Viewport.Width * 0.5f, device.Viewport.Height * 0.5f, 0));
            return transformMatrix;
        }
    }
}
