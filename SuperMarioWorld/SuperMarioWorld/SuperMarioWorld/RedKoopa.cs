using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class RedKoopa : Enemy
    {
        public Rectangle checkPlatformBox;

        private bool _onPlatform;

        public RedKoopa(Vector2 position) : base (position)
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
            if (!_onPlatform)
                lookRight = !lookRight;
            _onPlatform = false;

            if (lookRight)
            {
                checkPlatformBox.X = (int)Math.Round(position.X) + boundingBox.Width / 2 - checkPlatformBox.Width;
            }
            else
            {
                checkPlatformBox.X = (int)Math.Round(position.X) - boundingBox.Width / 2;
            }
            checkPlatformBox.Y = (int)Math.Round(position.Y);



            base.Update(gameTime);
        }

        public override void Death()
        {
            base.Death();
            create(new EmptyShell(position, EmptyShell.KoopaType.red));
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
