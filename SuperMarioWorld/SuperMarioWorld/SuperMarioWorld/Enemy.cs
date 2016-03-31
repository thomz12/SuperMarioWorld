using Microsoft.Xna.Framework;
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

        /// <summary>
        /// Override for the collision function, all enemies
        /// </summary>
        /// <param name="collider"></param>
        public override void OnCollision(GameObject collider)
        {
            // When this enemy collides with another enemy
            if (collider is Enemy)
            {
                // Turn around
                lookRight = !lookRight;

                // Set this enemy outside of the other enemy.
                if (collider.position.X < position.X)
                    collider.position.X = position.X - (collider.boundingBox.Width / 2) - (boundingBox.Width / 2);
                if (collider.position.X > position.X)
                    collider.position.X = position.X + (collider.boundingBox.Width / 2) + (boundingBox.Width / 2);
            }

            // When this enemy collides with the player
            if(collider is Player)
            {
                // Cast the collider to the player
                Player player = (Player)collider;

                // When the players Y velocity is high the player comes from above
                if (player.velocity.Y > 3)
                {
                    // Bounce the player back up
                    player.velocity.Y = -110;

                    // Set the players bounding box outside of this bounding box to prevent death when jumping on multiple enemies at once
                    player.boundingBox.Y = (int)position.Y - boundingBox.Height - player.boundingBox.Height;
                    
                    // Update the players position
                    player.position.Y = position.Y - boundingBox.Height;

                    // This Enemy is dead
                    Death(collider);
                }
                else if(!dead)
                {
                    // If the player is not coming from above the player is dead.
                    player.Death(collider);
                }
            }
        }
    }
}
