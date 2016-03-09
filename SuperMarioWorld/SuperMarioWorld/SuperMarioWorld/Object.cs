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
                Entity entity = (Entity)collider;

                if (collider.position.Y > position.Y + sprite.ySize / 2)
                {
                    entity.momentum.Y = 16;
                }
                else if (collider.position.Y < position.Y - sprite.ySize / 2)
                {
                    entity.grounded = true;
                    entity.momentum.Y = 0;
                }
            }
        }

    }
}
