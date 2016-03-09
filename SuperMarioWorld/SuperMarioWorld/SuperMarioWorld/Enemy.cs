using Microsoft.Xna.Framework;
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

        }

        protected override void Movement(GameTime gameTime)
        {
            base.Movement(gameTime);
        }
    }
}
