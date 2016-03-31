

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class EmptyShell : Entity
    {
        /// <summary>
        /// Different types of shells that are (not all) in the game
        /// Shell's type depends on its color
        /// </summary>
        public enum KoopaType
        {
            green = 0,
            red = 1,
            blue = 2,
            yellow = 3
        }

        /// <summary>
        /// The type of this shell
        /// </summary>
        public KoopaType koopaType;

        /// <summary>
        /// Boolean to track if the shell is moving or not
        /// </summary>
        private bool _moving;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">Position in the world</param>
        /// <param name="koopaType">What color was the koopa?</param>
        public EmptyShell(Point position, KoopaType koopaType) : base (position)
        {
            // Set this shells type to the type that is given
            this.koopaType = koopaType;

            // Set the sprite
            sprite.sourceName = @"Enemies\KoopaShells";
            sprite.xSize = 16;
            sprite.ySize = 16;
            sprite.animationSpeed = 100;

            // Set a animation
            sprite.NewAnimation(0, (int)this.koopaType);

            // Set the acceleration
            acceleration = 128.0f;
            // Set the max speed of the shell
            maxSpeed = 160;
            // Set the shell to go to the left (default)
            lookRight = false;

            // Generate a bounding box around the shell
            boundingBox = new Rectangle(0, 0, 15, 15);
        }

        protected override void Movement(GameTime gameTime)
        { 
            // Only do moving on the x axis when not dead and can move
            if (!dead && _moving)
            {
                if (lookRight)
                    velocity = new Vector2(velocity.X + acceleration, velocity.Y);
                else
                    velocity = new Vector2(velocity.X - acceleration, velocity.Y);
            }

            // Add gravity (even when dead) for animation purposes
            if (!grounded)
                velocity.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            // Limit the momentum for the object
            if (velocity.X > maxSpeed)
                velocity = new Vector2(maxSpeed, velocity.Y);
            if (velocity.X < -maxSpeed)
                velocity = new Vector2(-maxSpeed, velocity.Y);

            if (velocity.Y > terminalVelocity)
                velocity.Y = terminalVelocity;
            if (velocity.Y < -terminalVelocity)
                velocity.Y = -terminalVelocity;

            // Add momentum to position
            position += velocity * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }

        /// <summary>
        /// Is called when this objects bounding box overlaps with another bounding box
        /// </summary>
        /// <param name="collider">The object that is collided with</param>
        public override void OnCollision(GameObject collider)
        {
            // When this shell collides with a player
            if (collider is Player)
            {
                // Cast the object
                Player player = (Player)collider;
                // When shell is moving
                if (_moving)
                {
                    // Stop moving when player comes from above
                    if (player.velocity.Y > 3)
                    {
                        _moving = false;
                        player.velocity.Y = -140;
                        velocity.X = 0;
                        sprite.NewAnimation(0, (int)koopaType);
                    }
                    // Else kill player!!!!!
                    else
                    {
                        player.Death(this);
                    }
                }
                else
                {
                    // The shell is not moving
                    _moving = true;

                    // Set a new animation
                    for (int i = 0; i < 4; i++)
                    {
                        sprite.AddFrame(i, (int)koopaType);
                    }

                    // Check if the players x position relative to own position
                    if (player.position.X > position.X)
                    {
                        // Position is bigger so the shell is kicked to the left 
                        lookRight = false;
                        // Put the shell outside of the player
                        position.X -= 4;
                    }
                    else
                    {
                        // Position is smaller so shell is kicked to the right
                        lookRight = true;
                        // Put the shell outside of the player
                        position.X += 4;
                    }

                }
            }
            // If this shell collides with an entity and it is moving
            else if (collider is Entity && _moving)
            {
                // Cast the entity
                Entity entity = (Entity)collider;
                // Check if the entity is not a coin or a mushroom or a oneup
                if(!(entity is Coin) && !(entity is Mushroom) && !(entity is OneUp))
                    // KILL IT!
                    entity.Death(this);
            }
        }
    }
}
