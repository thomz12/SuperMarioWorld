using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class Mushroom : Entity
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"></param>
        public Mushroom(Point position) : base (position)
        {
            // Set this objects sprite
            sprite.sourceName = @"PowerUps\Mushroom";
            sprite.xSize = 16;
            sprite.ySize = 16;

            // Add an animation
            sprite.AddFrame(0, 0);

            // This animation is not animated
            sprite.animated = false;

            // Movement variables
            acceleration = 128.0f;
            maxSpeed = 32.0f;
            lookRight = true;

            // Generate the bounding box
            boundingBox = new Rectangle(0, 0, 14, 14);
        }
    }
}
