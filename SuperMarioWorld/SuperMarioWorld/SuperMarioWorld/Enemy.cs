﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    abstract class Enemy : Entity
    {
        protected bool flying;
        protected bool huge;

        protected GameObject drop;

        public Enemy(Vector2 position) : base (position)
        {

        }

        public void Death()
        {

        }

        public override void OnCollision(GameObject collider)
        {
            //Also turn around if an enemy collides with an enemy.
            if (collider is Enemy)
                lookRight = !lookRight;

            if(collider is Player)
            {
                Player p = (Player)collider;
                p.momentum.Y = -110;
            }
        }

        protected override void Movement(GameTime gameTime)
        {
            base.Movement(gameTime);
        }
    }
}
