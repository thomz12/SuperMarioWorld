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

        private int _boundaryRight;
        private int _boundaryLeft;

        private int xOffset = -64;

        private bool movingRight;

        public Camera2D(Player player)
        {
            _player = player;
            Zoom = 1.0f;
            Rotation = 0;
            Position = player.position;
        }

        /// <summary>
        /// default update function
        /// </summary>
        public void Update()
        {
            Vector2 delta = _player.position - Position;
            System.Diagnostics.Debug.WriteLine(delta.ToString());

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
