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
        //Non moving
        private bool _moveable;

        //Zoom of the camera
        public float Zoom { get; set; }

        public Matrix transformMatrix;

        //Current position of the camera
        public Vector2 Position { get; set; }

        //Current rotation of the camera
        public float Rotation { get; set; }

        //The player the camera should track
        private GameObject _target;

        /// <summary>
        /// Height of the default SNES resolution (in px)
        /// </summary>
        public int GameHeight { get; set; }
        /// <summary>
        /// Width of the default SNES resolution (in px)
        /// </summary>
        public int GameWidth { get; set; }

        private float _smoothness = 4f;

        private float _xDeadZone = 32.0f;

        private Point _levelSize;
        private int _gridSize;

        /// <summary>
        /// Constructor for a camera that follows a target
        /// </summary>
        /// <param name="target"></param>
        /// <param name="size"></param>
        /// <param name="grid"></param>
        public Camera2D(GameObject target, Point size, int grid)
        {
            _target = target;
            _levelSize = size;
            _gridSize = grid;

            Zoom = 1.0f;
            Rotation = 0;

            if(target != null)
                Position = target.position;

            _moveable = true;
        }

        /// <summary>
        /// default update function
        /// </summary>
        public void Update(GameTime gameTime)
        {
            //check if the target exists
            if (_target != null)
            {
                //Check if the camera is allowed to move
                if (_moveable)
                {
                    //Difference between camera and target position
                    Vector2 delta = _target.position - Position;

                    //The camera uses smooth damping.
                    float smoothDamping = _smoothness * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000);

                    //X axis
                    //If the player moves out of the dead zone, move the camera to the player
                    if (delta.X < -_xDeadZone || delta.X > _xDeadZone)
                    {
                        Position = new Vector2(Position.X + (_target.position.X - Position.X) * smoothDamping, Position.Y);
                    }

                    //Limit the camera to the left edge of the map
                    if (Position.X < GameWidth / 2 - _gridSize / 2)
                        Position = new Vector2(GameWidth / 2 - _gridSize / 2, Position.Y);
                    //Limit the camera to the right edge of the map
                    if (Position.X > _levelSize.X * _gridSize - GameWidth / 2 - _gridSize / 2)
                        Position = new Vector2(_levelSize.X * _gridSize - GameWidth / 2 - _gridSize / 2, Position.Y);

                    //Y axis
                    //if the lower bounds of the camera vieuwport is lower than the lowest pixel in the level (y+ == lower)
                    //if (Position.Y + GameHeight / 2 >= _levelSize.Y * _gridSize)
                    //{
                    //    //set the camera position to snap to the bottom edge (use levelSize for Y coord instead of player Y)
                    //    Position = new Vector2(Position.X, _levelSize.Y * _gridSize - GameHeight / 2);

                    //    //If the player is in the top 1/4th half of the screen, the camera breaks free from the snap.
                    //    if (delta.Y < -(GameHeight / 4))
                    //    {
                    //        Position = new Vector2(Position.X, Position.Y + (_target.position.Y - Position.Y) * smoothDamping);
                    //    }
                    //}
                    //else
                    //{
                    //    //If the camera not beneath the lower bounds of the level, the position of the camera is the position of the player.
                    //    Position = new Vector2(Position.X, Position.Y + (_target.position.Y - Position.Y) * smoothDamping);
                    //}
                }
            }
            else
            {
                //Set the position of the camera to the center of the level
                Position = new Vector2(_levelSize.X * _gridSize / 2 - _gridSize / 2, _levelSize.Y * _gridSize / 2);
            }
        }

        /// <summary>
        /// Change the position of the cameraby a specific amount.
        /// </summary>
        /// <param name="amount">The amount which a camera should move.</param>
        public void Move(Vector2 amount)
        {
            if(_moveable)
            Position += amount;
        }

        /// <summary>
        /// Gets the transformation matrix of the camera position/rotation/zoom
        /// </summary>
        /// <param name="device">Graphics device that the camera uses.</param>
        /// <returns>Returns a Matrix type.</returns>
        public Matrix GetTransformation(GraphicsDevice device)
        {
            transformMatrix = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * Matrix.CreateTranslation(new Vector3(device.Viewport.Width * 0.5f, device.Viewport.Height * 0.5f, 0));
            return transformMatrix;
        }
    }
}
