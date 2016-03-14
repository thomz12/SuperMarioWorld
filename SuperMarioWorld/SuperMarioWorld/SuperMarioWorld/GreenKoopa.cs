﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class GreenKoopa : Enemy
    {
        public GreenKoopa(Vector2 position) : base (position)
        {
            sprite.sourceName = "Koopas";
            sprite.xSize = 16;
            sprite.ySize = 32;

            sprite.AddFrame(1, 0);
            sprite.AddFrame(2, 0);

            acceleration = 128f;
            maxSpeed = 32;
            lookRight = false;

            boundingBox = new Rectangle(0, 0, 12, 16); 
        }

        public override void OnCollision(GameObject collider)
        {
            //Call parent's OnCollision function
            base.OnCollision(collider);
        }
    }
}