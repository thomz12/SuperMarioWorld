using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class Mushroom : Entity
    {
        public Mushroom(Point position) : base (position)
        {
            sprite.sourceName = @"PowerUps\Mushroom";
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
