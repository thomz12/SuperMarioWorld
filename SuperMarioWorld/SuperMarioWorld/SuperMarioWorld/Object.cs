using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    abstract class Object : GameObject
    {
        //Does the object have a bounding box?
        public bool blocking;

        public bool isPlatform;

        public Object(Vector2 position) : base(position)
        {
            blocking = true;
        }

        public override void OnCollision(GameObject collider)
        {
            if (collider is Entity)
            {
                Entity p = (Entity)collider;

                Rectangle overlap;
                Rectangle.Intersect(ref collider.boundingBox, ref boundingBox, out overlap);

                if (overlap.Width > overlap.Height)
                {
                    if (p.position.Y < position.Y)
                    {
                        p.position.Y = position.Y - boundingBox.Height / 2 - p.boundingBox.Height / 2;
                        p.momentum.Y = 16;
                        p.grounded = true;
                        p.momentum.Y = 0;
                    }
                    else
                    {
                        p.position.Y = position.Y + boundingBox.Height / 2 + p.boundingBox.Height / 2;
                        if (p is Player)
                        {
                            sprite.NewAnimation();
                            sprite.AddFrame(4, 0);
                        }

                        p.momentum.Y = 16;
                    }
                }
                else
                {
                    if (Math.Abs(p.boundingBox.Bottom - boundingBox.Top) > 2)
                    {
                        if (p.position.X < position.X)
                        {
                            p.position.X = position.X - boundingBox.Width / 2 - p.boundingBox.Width / 2 - 1;
                        }

                        if (p.position.X > position.X)
                        {
                            p.position.X = position.X + boundingBox.Width / 2 + p.boundingBox.Width / 2;
                        }

                        if (!(p is Player))
                            p.lookRight = !p.lookRight;

                        p.momentum.X = 0;
                    }
                }
            }
        }
    }
}
