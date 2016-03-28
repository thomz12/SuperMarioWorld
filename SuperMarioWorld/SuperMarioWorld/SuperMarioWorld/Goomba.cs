using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class Goomba : Enemy
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">Position the goomba should be on.</param>
        public Goomba(Point position) : base (position)
        {
            sprite.sourceName = @"Enemies\Goomba";
            sprite.xSize = 16;
            sprite.ySize = 16;
            sprite.AddFrame(0, 0);
            sprite.AddFrame(1, 0);

            acceleration = 128.0f;
            maxSpeed = 16;
            lookRight = false;

            boundingBox = new Rectangle(0, 0, 14, 14);
        }

        /// <summary>
        /// Override of the default OnCollision function.
        /// Called when two things collide.
        /// </summary>
        /// <param name="collider">The other thing that is being collided with.</param>
        public override void OnCollision(GameObject collider)
        {
            //Call parent's OnCollision function.
            base.OnCollision(collider);
        }
    }
}
