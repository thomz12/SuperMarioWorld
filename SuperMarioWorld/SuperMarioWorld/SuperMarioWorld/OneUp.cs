using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class OneUp : Entity
    {
        public OneUp(Point position) : base (position)
        {
            sprite.sourceName = @"PowerUps\OneUp";
            sprite.xSize = 16;
            sprite.ySize = 16;
            sprite.AddFrame(0, 0);
            sprite.animated = false;

            acceleration = 128.0f;
            maxSpeed = 32.0f;
            lookRight = true;

            boundingBox = new Rectangle(0, 0, 14, 14);
        }
    }
}
