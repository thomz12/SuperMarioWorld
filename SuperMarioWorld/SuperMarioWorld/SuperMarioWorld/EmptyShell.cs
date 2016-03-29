

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class EmptyShell : Entity
    {
        public enum KoopaType
        {
            green = 0,
            red = 1,
            blue = 2,
            yellow = 3
        }

        public KoopaType koopaType;

        private bool _moving;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">Position in the world</param>
        /// <param name="koopaType">What color was the koopa?</param>
        public EmptyShell(Point position, KoopaType koopaType) : base (position)
        {
            this.koopaType = koopaType;
            sprite.sourceName = @"Enemies\KoopaShells";
            sprite.xSize = 16;
            sprite.ySize = 16;
            sprite.animationSpeed = 100;

            sprite.NewAnimation(0, (int)this.koopaType);

            acceleration = 128.0f;
            maxSpeed = 160;
            lookRight = false;

            boundingBox = new Rectangle(0, 0, 15, 15);
        }

        protected override void Movement(GameTime gameTime)
        {
            //This now applies to all entities except the player and the smart koopa who override this function.
            if (!dead && _moving)
            {
                if (lookRight)
                    momentum = new Vector2(momentum.X + acceleration, momentum.Y);
                else
                    momentum = new Vector2(momentum.X - acceleration, momentum.Y);
            }

            //Add gravity
            if (!grounded)
                momentum.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            //Limit the momentum for the object
            if (momentum.X > maxSpeed)
                momentum = new Vector2(maxSpeed, momentum.Y);
            if (momentum.X < -maxSpeed)
                momentum = new Vector2(-maxSpeed, momentum.Y);

            if (momentum.Y > terminalVelocity)
                momentum.Y = terminalVelocity;
            if (momentum.Y < -terminalVelocity)
                momentum.Y = -terminalVelocity;

            //add momentum to position
            position += momentum * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }

        public override void OnCollision(GameObject collider)
        {
            if (collider is Player)
            {
                Player p = (Player)collider;
                //When shell is moving
                if (_moving)
                {
                    //Stop moving when player comes from above
                    if (p.momentum.Y > 3)
                    {
                        _moving = false;
                        p.momentum.Y = -140;
                        momentum.X = 0;
                        sprite.NewAnimation(0, (int)koopaType);
                    }
                    //else kill it!
                    else
                    {
                        p.Death(this);
                    }
                }
                else
                {
                    _moving = true;

                    for (int i = 0; i < 4; i++)
                    {
                        sprite.AddFrame(i, (int)koopaType);
                    }

                    if (p.position.X > position.X)
                    {
                        lookRight = false;
                        position.X -= 4;
                    }
                    else
                    {
                        lookRight = true;
                        position.X += 4;
                    }

                }
            }
            else if (collider is Entity && _moving)
            {
                Entity e = (Entity)collider;
                if(!(e is Coin) && !(e is Mushroom) && !(e is OneUp))
                    e.Death(this);
            }
        }
    }
}
