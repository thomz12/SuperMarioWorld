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

        //Is the object a platform? (only collides from up)
        public bool isPlatform;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"></param>
        public Object(Vector2 position) : base(position)
        {
            blocking = true;
            isPlatform = false;
            sprite.layer = 0.4f;
        }

        public override void OnCollision(GameObject collider)
        {
            if (collider is Entity && blocking)
            {
                Entity p = (Entity)collider;
                //Overlap rect
                Rectangle overlap;
                //get overlap rect
                Rectangle.Intersect(ref collider.boundingBox, ref boundingBox, out overlap);

                //if overlap width is higher than overlap height
                if (overlap.Width > overlap.Height)
                {
                    if (p.position.Y < position.Y)
                    {
                        if (p.momentum.Y > 0)
                        {
                            //When entity collides from the top
                            p.position.Y = position.Y - boundingBox.Height + 1;
                            p.momentum.Y = 16;
                            p.grounded = true;
                            p.momentum.Y = 0;
                        }
                    }
                    else if(!isPlatform)
                    {
                        //When entity collides from the buttom
                        p.position.Y = position.Y + p.boundingBox.Height;

                        p.momentum.Y = 16;
                    }
                }
                else if(!isPlatform)
                {
                    if (Math.Abs(p.boundingBox.Bottom - boundingBox.Top) > 2)
                    {
                        //When entity collides from the left
                        if (p.position.X < position.X)
                        {
                            p.position.X = position.X - boundingBox.Width / 2 - p.boundingBox.Width / 2 - 1;
                        }

                        //When entity collides from the right
                        if (p.position.X > position.X)
                        {
                            p.position.X = position.X + boundingBox.Width / 2 + p.boundingBox.Width / 2;
                        }

                        //Turn around enemies
                        if (!(p is Player))
                            p.lookRight = !p.lookRight;

                        p.momentum.X = 0;
                    }
                }
            }
        }
    }
}
