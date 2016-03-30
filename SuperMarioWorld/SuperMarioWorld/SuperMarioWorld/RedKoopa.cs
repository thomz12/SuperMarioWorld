using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class RedKoopa : Enemy
    {
        public Rectangle checkPlatformBox;

        private bool _onPlatform;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">Object position</param>
        public RedKoopa(Point position) : base (position)
        {
            sprite.sourceName = @"Enemies\Koopas";
            sprite.xSize = 16;
            sprite.ySize = 32;

            sprite.AddFrame(1, 1);
            sprite.AddFrame(2, 1);

            acceleration = 128f;
            maxSpeed = 32;
            lookRight = false;

            boundingBox = new Rectangle(0, 0, 12, 24);

            checkPlatformBox = new Rectangle(0, 0, 4, 4);
        }

        public override void Update(GameTime gameTime)
        {
            //Check if the koopa was on the ground in the last update, then check if the variable _onPlatform is false (the koopa is about to walk off the platform)
            if (grounded)
            {
                if (!_onPlatform)
                    //Turn around because koopa is about to fall off the platform
                    lookRight = !lookRight;
                _onPlatform = false;
            }

            //Set the position of the bounding box to the correct position.
            if (lookRight)
            {
                //When the koopa is looking to the right the bounding box is offset to the right
                checkPlatformBox.X = (int)Math.Round(position.X) + boundingBox.Width / 2 - checkPlatformBox.Width;
            }
            else
            {
                //when the koopa is looking to the left, the bounding box is offset to the left
                checkPlatformBox.X = (int)Math.Round(position.X) - boundingBox.Width / 2;
            }
            //koopa's Y coord of the check collision box is at the koopas feet, the anchor point of the koopa is in the bottom center.
            checkPlatformBox.Y = (int)Math.Round(position.Y);

            //execute enemy.Update()
            base.Update(gameTime);
        }

        public override void Death(GameObject cause)
        {
            //Call enemy.Death()
            base.Death(cause);

            //If a koopa is not killed by an EmptyShell, it creates an EmptyShell.
            if(!(cause is EmptyShell))
                create(new EmptyShell(new Point((int)position.X, (int)position.Y), EmptyShell.KoopaType.red));
        }

        public override void OnCollision(GameObject collider)
        {
            if(collider is StaticObject)
            {
                StaticObject o = (StaticObject)collider;
                if (o.blocking)
                {
                    if(position.Y < o.position.Y)
                    {
                        if (checkPlatformBox.Intersects(o.boundingBox))
                        {
                            _onPlatform = true;
                        }
                    }
                }
            }
            base.OnCollision(collider);
        }
    }
}
