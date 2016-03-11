using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class Goomba : Enemy
    {
        public Goomba(Vector2 position) : base(position)
        {
            sprite.sourceName = "Goomba";
            sprite.xSize = 16;
            sprite.ySize = 16;
            sprite.AddFrame(0, 0);
            sprite.AddFrame(1, 0);

            acceleration = 128.0f;
            maxSpeed = 16;
            lookRight = false;

            boundingBox = new Rectangle(0, 0, 14, 14);
        }

        public override void OnCollision(GameObject collider)
        {
            if (collider is Enemy)
                lookRight = !lookRight;
            base.OnCollision(collider);
        }

        protected override void Movement(GameTime gameTime)
        {
            base.Movement(gameTime);
        }
    }
}
