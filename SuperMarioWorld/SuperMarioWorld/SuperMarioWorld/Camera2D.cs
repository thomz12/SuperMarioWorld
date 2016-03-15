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

        private Vector2 _position;

        //Current rotation of the camera
        public float Rotation { get; set; }

        //The player the camera should track
        private Player _player;

        private const int _gameHeight = 224;
        private const int _gameWidth = 256;

        private int _boundaryRight;
        private int _boundaryLeft;

        private int xOffset = -64;

        private bool movingRight;

        private Point _levelSize;
        private int _gridSize;

        public Camera2D(Player player, Point size, int grid)
        {
            _player = player;
            _levelSize = size;
            _gridSize = grid;

            Zoom = 1.0f;
            Rotation = 0;
            Position = player.position;
        }

        /// <summary>
        /// default update function
        /// </summary>
        public void Update(GameTime gameTime)
        {
            Vector2 delta = _player.position - Position;

            //X axis
            if (delta.X < 16)
                movingRight = false;
            if (delta.X > 16)
                movingRight = true;

            if(movingRight)
            {
                if (delta.X > 0)
                    Position = new Vector2(Position.X + (_player.position.X - Position.X) * 0.1f, Position.Y);
            }
            else
            {
                if (delta.X < 0)
                    Position = new Vector2(Position.X + (_player.position.X - Position.X) * 0.1f, Position.Y);
            }

            //Y axis
            if (Position.Y + _gameHeight / 2 >= _levelSize.Y * _gridSize)
            {
                Position = new Vector2(Position.X, _levelSize.Y * _gridSize - _gameHeight / 2);
                if (delta.Y < -(_gameHeight / 4))
                {
                    Position = new Vector2(Position.X, Position.Y + (_player.position.Y - Position.Y) * 0.1f);
                }
            }
            else
            {
                Position = new Vector2(Position.X, Position.Y + (_player.position.Y - Position.Y) * 0.1f);
            }
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
