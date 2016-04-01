using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class Goomba : Enemy
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">Position the goomba should be on.</param>
        public Goomba(Point position) : base (position)
        {
            // Set this objects sprite
            sprite.sourceName = @"Enemies\Goomba";
            sprite.xSize = 16;
            sprite.ySize = 16;
            sprite.AddFrame(0, 0);
            sprite.AddFrame(1, 0);

            // Set this objects movement variables
            acceleration = 128.0f;
            maxSpeed = 16;
            lookRight = false;

            // Create this objects bounding box
            boundingBox = new Rectangle(0, 0, 14, 14);
        }
    }
}
