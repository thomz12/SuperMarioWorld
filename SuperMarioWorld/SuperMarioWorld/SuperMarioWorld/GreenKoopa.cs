using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class GreenKoopa : Enemy
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">position the object should be created on</param>
        public GreenKoopa(Point position) : base (position)
        {
            // Set this objects texture
            sprite.sourceName = @"Enemies\Koopas";
            sprite.xSize = 16;
            sprite.ySize = 32;

            // Add an animation
            sprite.AddFrame(1, 0);
            sprite.AddFrame(2, 0);

            // Set movement animations
            acceleration = 128f;
            maxSpeed = 32;
            lookRight = false;

            // Generate a bounding box
            boundingBox = new Rectangle(0, 0, 12, 24); 
        }

        /// <summary>
        /// Execute this when the object dies
        /// </summary>
        /// <param name="cause"></param>
        public override void Death(GameObject cause)
        {
            // Call the death function of Enemy
            base.Death(cause);
            // Don't drop a shell when this koopa gets killed by a shell
            if (!(cause is EmptyShell))
                create(new EmptyShell(new Point((int)position.X, (int)position.Y), EmptyShell.KoopaType.green));
        }
    }
}
