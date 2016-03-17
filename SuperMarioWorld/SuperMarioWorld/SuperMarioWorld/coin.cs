using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class Coin : Entity
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">Position the object is created on</param>
        /// <param name="moving">Is the coin a moving coin or a static one?</param>
        public Coin(Vector2 position, bool moving) : base (position)
        {
            maxSpeed = 16;

            if (moving)
            {
                acceleration = 16;
            }
            else
            {
                acceleration = 0;
                grounded = true;
                affectedByGravity = false;
            }

            lookRight = false;

            //Generates a boundingbox around the coin
            boundingBox = new Rectangle((int)position.X - 4, (int)position.Y - 16, 8, 16);

            //Set the sizes of the sprite
            sprite.xSize = 16;
            sprite.ySize = 16;

            //Call sprite class to load the texture
            sprite.sourceName = @"Misc\Coin";

            //Add animation
            sprite.NewAnimation();
            sprite.animationSpeed = 128.0f;

            //Add all sprites from the file to the animation list
            for (int i = 0; i < 4; i++)
            {
                sprite.AddFrame(i, 0);
            }
        }

        public override void OnCollision(GameObject collider)
        {

        }
    }
}
