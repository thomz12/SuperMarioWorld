using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    class Checkpoint : StaticObject
    {
        public Checkpoint(Point position) : base(position)
        {
            blocking = false;

            sprite.xSize = 32;
            sprite.ySize = 64;

            boundingBox = new Rectangle((int)position.X - 8, (int)position.Y - sprite.ySize, 16, 64);

            sprite.sourceName = "Blocks/checkpoint";
            sprite.AddFrame(0, 0);
            sprite.AddFrame(1, 0);
            sprite.AddFrame(2, 0);
            sprite.AddFrame(3, 0);

            sprite.animationSpeed = 75.0f;
        }
    }
}
