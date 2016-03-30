﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public abstract class Enemy : Entity
    {
        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="position">The object position</param>
        public Enemy(Point position) : base (position)
        {
            
        }

        public override void OnCollision(GameObject collider)
        {
            //Also turn around if an enemy collides with an enemy.
            if (collider is Enemy)
            {
                lookRight = !lookRight;
                if (collider.position.X < position.X)
                    collider.position.X = position.X - (collider.boundingBox.Width / 2) - (boundingBox.Width / 2);
                if (collider.position.X > position.X)
                    collider.position.X = position.X + (collider.boundingBox.Width / 2) + (boundingBox.Width / 2);
            }

            if(collider is Player)
            {
                Player p = (Player)collider;
                if (p.velocity.Y > 3)
                {
                    p.velocity.Y = -110;
                    p.boundingBox.Y = (int)position.Y - boundingBox.Height - p.boundingBox.Height;
                    p.position.Y = position.Y - boundingBox.Height;
                    Death(collider);
                }
                else if(!dead)
                {
                    p.Death(collider);
                }
            }
        }
    }
}
