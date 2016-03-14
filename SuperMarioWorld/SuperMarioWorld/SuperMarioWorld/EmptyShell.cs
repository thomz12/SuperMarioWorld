﻿

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class EmptyShell : Entity
    {
        public enum KoopaType
        {
            green = 0,
            red = 1,
            blue = 2,
            yellow = 3
        }

        private KoopaType _koopaType;

        public EmptyShell(Vector2 position, KoopaType koopaType) : base (position)
        {
            _koopaType = koopaType;
            sprite.sourceName = "KoopaShells";
            sprite.xSize = 16;
            sprite.ySize = 16;
            sprite.animationSpeed = 100;

            for (int i = 0; i < 4; i++)
            {
                sprite.AddFrame(i, (int)_koopaType);
            }

            acceleration = 128.0f;
            maxSpeed = 160;
            lookRight = false;

            boundingBox = new Rectangle(0, 0, 15, 15);
        }

        public override void OnCollision(GameObject collider)
        {
            //Does things
        }
    }
}
