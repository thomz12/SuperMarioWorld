using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    //Yes we hit our hands with a hammer.
    public abstract class StaticObject : GameObject
    {
        //Does the object have a bounding box?
        public bool blocking;

        //Is the object a platform? (only collides from up)
        public bool isPlatform;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"></param>
        public StaticObject(Point position) : base(position)
        {
            blocking = true;
            isPlatform = false;
            sprite.layer = 0.4f;
        }

        public override void OnCollision(GameObject collider)
        {
            if (collider is Entity && blocking)
            {
                Entity e = (Entity)collider;
                //Overlap rect
                Rectangle overlap;
                //get overlap rect
                Rectangle.Intersect(ref collider.boundingBox, ref boundingBox, out overlap);

                //if overlap width is higher than overlap height
                if (overlap.Width > overlap.Height)
                {
                    if (e.position.Y < position.Y)
                    {
                        if (e.velocity.Y > 0)
                        {
                            //When entity collides from the top
                            e.position.Y = position.Y - boundingBox.Height + 1;
                            e.velocity.Y = 16;
                            e.grounded = true;
                            e.velocity.Y = 0;
                        }
                    }
                    else if(!isPlatform)
                    {
                        //When entity collides from the buttom
                        e.position.Y = position.Y + e.boundingBox.Height;

                        e.velocity.Y = 16;
                    }
                }
                else if(!isPlatform)
                {
                    if (Math.Abs(e.boundingBox.Bottom - boundingBox.Top) > 2)
                    {
                        //When entity collides from the left
                        if (e.position.X < position.X)
                        {
                            e.position.X = position.X - boundingBox.Width / 2 - e.boundingBox.Width / 2 - 1;
                        }

                        //When entity collides from the right
                        if (e.position.X > position.X)
                        {
                            e.position.X = position.X + boundingBox.Width / 2 + e.boundingBox.Width / 2;
                        }

                        if (!e.hasTurned && !(e is Player))
                        {
                            e.lookRight = !e.lookRight;
                            e.hasTurned = true;
                        }

                        e.velocity.X = 0;
                    }
                }
            }
        }
    }
}
