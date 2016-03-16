using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    abstract class Enemy : Entity
    {
        public Enemy(Vector2 position) : base (position)
        {
            
        }

        public override void OnCollision(GameObject collider)
        {
            base.OnCollision(collider);

            //Also turn around if an enemy collides with an enemy.
            if (collider is Enemy)
                lookRight = !lookRight;

            if(collider is Player)
            {
                Player p = (Player)collider;
                if (p.momentum.Y > 3)
                {
                    p.momentum.Y = -110;
                    p.boundingBox.Y = (int)position.Y - boundingBox.Height - p.boundingBox.Height;
                    p.position.Y = position.Y - boundingBox.Height;
                    Death();
                }
                else if(!death)
                {
                    p.Death();
                }
            }
        }

        protected override void Movement(GameTime gameTime)
        {
            base.Movement(gameTime);
        }
    }
}
