using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class Coin : Entity
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">Position the object is created on</param>
        /// <param name="moving">Is the coin a moving coin or a static one?</param>
        public Coin(Point position, bool moving) : base (position)
        {
            // Set the speed for a coin
            maxSpeed = 16;

            // Set movement related variables of the coin depending on if it moves or not.
            if (moving)
            {
                acceleration = 16;
                grounded = false;
                affectedByGravity = true;
            }
            else
            {
                acceleration = 0;
                grounded = true;
                affectedByGravity = false;
            }

            // Set the coin to go to the left
            lookRight = false;

            // Generates a boundingbox around the coin
            boundingBox = new Rectangle((int)position.X - 4, (int)position.Y - 16, 8, 16);

            // Set the sizes of the sprite
            sprite.xSize = 16;
            sprite.ySize = 16;

            // Call sprite class to load the texture
            sprite.sourceName = @"Misc\Coin";

            // Add animation
            sprite.NewAnimation();
            sprite.animationSpeed = 128.0f;

            // Add all sprites from the file to the animation list
            for (int i = 0; i < 4; i++)
            {
                sprite.AddFrame(i, 0);
            }
        }

        /// <summary>
        /// Coin does nothing when colliding with anything
        /// </summary>
        /// <param name="collider">gameobject that is being collided with</param>
        public override void OnCollision(GameObject collider)
        {

        }
    }
}
