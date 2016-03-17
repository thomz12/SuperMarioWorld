using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioWorld
{
    class Camera2D
    {
        //Zoom of the camera
        public float Zoom { get; set; }

        public Matrix transformMatrix;

        //Current position of the camera
        public Vector2 Position { get; set; }

        //Current rotation of the camera
        public float Rotation { get; set; }

        //The player the camera should track
        private GameObject _target;

        public int GameHeight { get; set; }
        public int GameWidth { get; set; }

        private float _smoothness = 4f;

        private float _xDeadZone = 32.0f;
        private bool movingRight;

        private Point _levelSize;
        private int _gridSize;

        public Camera2D(GameObject target, Point size, int grid)
        {
            _target = target;
            _levelSize = size;
            _gridSize = grid;

            Zoom = 1.0f;
            Rotation = 0;
            Position = target.position;
        }

        /// <summary>
        /// default update function
        /// </summary>
        public void Update(GameTime gameTime)
        {
            Vector2 delta = _target.position - Position;

            //X axis
            if (delta.X < -_xDeadZone)
                movingRight = false;
            if (delta.X > _xDeadZone)
                movingRight = true;

            float targetX = _target.position.X;

            if(movingRight)
            {
                if (delta.X > 0)
                    Position = new Vector2(Position.X + (targetX - Position.X) * _smoothness * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f), Position.Y);

            }
            else
            {
                if (delta.X < 0)
                    Position = new Vector2(Position.X + (targetX - Position.X) * _smoothness * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f), Position.Y);
            }

            //Y axis
            if (Position.Y + GameHeight / 2 >= _levelSize.Y * _gridSize)
            {
                Position = new Vector2(Position.X, _levelSize.Y * _gridSize - GameHeight / 2);
                if (delta.Y < -(GameHeight / 4))
                {
                    Position = new Vector2(Position.X, Position.Y + (_target.position.Y - Position.Y) * _smoothness * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f));
                }
            }
            else
            {
                Position = new Vector2(Position.X, Position.Y + (_target.position.Y - Position.Y) * _smoothness * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f));
            }

            if (Position.X < GameWidth / 2 - _gridSize / 2)
                Position = new Vector2(GameWidth / 2 - _gridSize / 2, Position.Y);

            if (Position.X > _levelSize.X * _gridSize - GameWidth / 2 - _gridSize / 2)
                Position = new Vector2(_levelSize.X * _gridSize - GameWidth / 2 - _gridSize / 2, Position.Y);
        }

        /// <summary>
        /// Change the position of the cameraby a specific amount.
        /// </summary>
        /// <param name="amount">The amount which a camera should move.</param>
        public void Move(Vector2 amount)
        {
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
