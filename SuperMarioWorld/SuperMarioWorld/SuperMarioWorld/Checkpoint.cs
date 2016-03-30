using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    public class Checkpoint : StaticObject
    {
        public bool isFinish;

        public Checkpoint(Point position, bool isFinish) : base(position)
        {
            blocking = false;
            this.isFinish = isFinish;

            sprite.xSize = 32;
            sprite.ySize = 64;

            boundingBox = new Rectangle((int)position.X - 8, (int)position.Y - sprite.ySize, 16, 64);

            sprite.sourceName = "Blocks/checkpoint";
            sprite.AddFrame(0, 0);
            sprite.AddFrame(1, 0);
            sprite.AddFrame(2, 0);
            sprite.AddFrame(3, 0);

            sprite.animationSpeed = 75.0f;
        }

        public override void OnCollision(GameObject collider)
        {
            //check if this checkpoint is a finish
            if (isFinish)
            {
                if(collider is Player)
                {
                    SceneManager.Instance.LoadMainMenu();
                }
            }
        }
    }
}
