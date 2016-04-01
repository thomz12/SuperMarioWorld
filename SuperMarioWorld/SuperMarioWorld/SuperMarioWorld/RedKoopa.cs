using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class RedKoopa : Enemy
    {
        // This box checks if the koopa is still on the platform
        public Rectangle checkPlatformBox;

        // This bool remembers if the koopa is on the platform
        private bool _onPlatform;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">Object position</param>
        public RedKoopa(Point position) : base (position)
        {
            // Set the texture for this object
            sprite.sourceName = @"Enemies\Koopas";
            sprite.xSize = 16;
            sprite.ySize = 32;

            // Add animation for this object
            sprite.AddFrame(1, 1);
            sprite.AddFrame(2, 1);

            // Set movement values
            acceleration = 128f;
            maxSpeed = 32;
            lookRight = false;

            // Generate a bounding box
            boundingBox = new Rectangle(0, 0, 12, 24);

            // Generate a checkplatform box
            checkPlatformBox = new Rectangle(0, 0, 4, 4);
        }

        /// <summary>
        /// This function is called every update
        /// </summary>
        /// <param name="gameTime">The gametime variable</param>
        public override void Update(GameTime gameTime)
        {
            // Check if the koopa was on the ground in the last update, then check if the variable _onPlatform is false (the koopa is about to walk off the platform)
            if (grounded)
            {
                if (!_onPlatform)
                    // Turn around because koopa is about to fall off the platform
                    lookRight = !lookRight;
                // Set the onplatform variable on false, this get set on true later this update if the box hits a platform
                _onPlatform = false;
            }

            // Set the position of the bounding box to the correct position.
            if (lookRight)
            {
                // When the koopa is looking to the right the bounding box is offset to the right
                checkPlatformBox.X = (int)Math.Round(position.X) + boundingBox.Width / 2 - checkPlatformBox.Width;
            }
            else
            {
                // When the koopa is looking to the left, the bounding box is offset to the left
                checkPlatformBox.X = (int)Math.Round(position.X) - boundingBox.Width / 2;
            }
            // Koopa's Y coord of the check collision box is at the koopas feet, the anchor point of the koopa is in the bottom center.
            checkPlatformBox.Y = (int)Math.Round(position.Y);

            // Execute enemy.Update()
            base.Update(gameTime);
        }

        /// <summary>
        /// This gets called when the koopa dies
        /// </summary>
        /// <param name="cause">Cause of the death</param>
        public override void Death(GameObject cause)
        {
            // Call enemy.Death()
            base.Death(cause);

            // If a koopa is not killed by an EmptyShell, it creates an EmptyShell.
            if(!(cause is EmptyShell))
                create(new EmptyShell(new Point((int)position.X, (int)position.Y), EmptyShell.KoopaType.red));
        }

        /// <summary>
        /// This gets called when the koopa collides with something
        /// </summary>
        /// <param name="collider">The object that is collided with</param>
        public override void OnCollision(GameObject collider)
        {
            // Check if the object that is collided is is a static block
            if(collider is StaticObject)
            {
                // Cast
                StaticObject o = (StaticObject)collider;

                // Check if the static blocks can block entities
                if (o.blocking)
                {
                    // Check if the position of the koopa is higher than the position of the block
                    if(position.Y < o.position.Y)
                    {
                        // Check if the checkplatform box intersects with the bounding box from the platform
                        if (checkPlatformBox.Intersects(o.boundingBox))
                        {
                            // Set on platform to true
                            _onPlatform = true;
                        }
                    }
                }
            }

            // Call the bases on collision function
            base.OnCollision(collider);
        }
    }
}
