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
        public float Zoom { get; set; }

        public Matrix transform;

        public Vector2 Position { get; set; }
        public float Rotation { get; set; }

        public Camera2D()
        {
            Zoom = 1.0f;
            Rotation = 0;
            Position = Vector2.Zero;
        }

        public void Move(Vector2 amount)
        {
            Position += amount;
        }

        public Matrix GetTransformation(GraphicsDevice device)
        {
            transform = Matrix.CreateTranslation(new Vector3(-Position.X, -Position.Y, 0)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) * Matrix.CreateTranslation(new Vector3(device.Viewport.Width * 0.5f, device.Viewport.Height * 0.5f, 0));
            return transform;
        }
    }
}
