using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class OneUp : Entity
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"></param>
        public OneUp(Point position) : base (position)
        {
            // Set this objects sprite
            sprite.sourceName = @"PowerUps\OneUp";
            sprite.xSize = 16;
            sprite.ySize = 16;

            // Add an animation
            sprite.AddFrame(0, 0);
            // Set the animation to not be an animation (usefull!)
            sprite.animated = false;

            // Do movement things
            acceleration = 128.0f;
            maxSpeed = 32.0f;
            lookRight = true;

            // Generate bounding box
            boundingBox = new Rectangle(0, 0, 14, 14);
        }
    }
}
