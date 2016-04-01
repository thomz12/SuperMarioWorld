using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    // Yes we hit our hands with a hammer.
    public abstract class StaticObject : GameObject
    {
        // Does the object have a bounding box?
        public bool blocking;

        // Is the object a platform? (only collides from up)
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

        /// <summary>
        /// This is called when the object collides with something
        /// </summary>
        /// <param name="collider"></param>
        public override void OnCollision(GameObject collider)
        {
            if (collider is Entity && blocking)
            {
                // Cast
                Entity entity = (Entity)collider;
                // Overlap rect
                Rectangle overlap;
                // Get overlap rect
                Rectangle.Intersect(ref collider.boundingBox, ref boundingBox, out overlap);

                //if overlap width is higher than overlap height
                if (overlap.Width > overlap.Height)
                {
                    if (entity.position.Y < position.Y)
                    {
                        if (entity.velocity.Y > 0)
                        {
                            //When entity collides from the top
                            entity.position.Y = position.Y - boundingBox.Height + 1;
                            entity.velocity.Y = 16;
                            entity.grounded = true;
                            entity.velocity.Y = 0;
                        }
                    }
                    else if(!isPlatform)
                    {
                        //When entity collides from the buttom
                        entity.position.Y = position.Y + entity.boundingBox.Height;

                        entity.velocity.Y = 16;
                    }
                }
                else if(!isPlatform)
                {
                    if (Math.Abs(entity.boundingBox.Bottom - boundingBox.Top) > 2)
                    {
                        //When entity collides from the left
                        if (entity.position.X < position.X)
                        {
                            entity.position.X = position.X - boundingBox.Width / 2 - entity.boundingBox.Width / 2 - 1;
                        }

                        //When entity collides from the right
                        if (entity.position.X > position.X)
                        {
                            entity.position.X = position.X + boundingBox.Width / 2 + entity.boundingBox.Width / 2;
                        }

                        if (!entity.hasTurned && !(entity is Player))
                        {
                            entity.lookRight = !entity.lookRight;
                            entity.hasTurned = true;
                        }

                        entity.velocity.X = 0;
                    }
                }
            }
        }
    }
}
