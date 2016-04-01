using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    public class Checkpoint : StaticObject
    {
        // Variable to keep track if the object is a finish
        public bool isFinish;

        public Checkpoint(Point position, bool isFinish) : base(position)
        {
            // A checkpoint does not block the player
            blocking = false;

            // Set the finish to the finish
            this.isFinish = isFinish;

            // Set the sprites size
            sprite.xSize = 32;
            sprite.ySize = 64;

            // Set the sprites bounding box
            boundingBox = new Rectangle((int)position.X - 8, (int)position.Y - sprite.ySize, 16, 64);

            // Set the animations for the sprite
            sprite.sourceName = "Blocks/checkpoint";
            sprite.AddFrame(0, 0);
            sprite.AddFrame(1, 0);
            sprite.AddFrame(2, 0);
            sprite.AddFrame(3, 0);

            // Set the animation speed
            sprite.animationSpeed = 75.0f;
        }

        public override void OnCollision(GameObject collider)
        {
            // Check if this checkpoint is a finish
            if (isFinish)
            {
                // Do something when the player hits this object
                if(collider is Player)
                {
                    SceneManager.Instance.LoadMainMenu();
                }
            }
        }
    }
}
