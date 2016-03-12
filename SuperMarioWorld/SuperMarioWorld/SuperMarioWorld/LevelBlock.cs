using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    /// <summary>
    /// level block (Grass, cave, Mud, leaves, dark grass)
    /// </summary>
    class LevelBlock : Object
    {
        public LevelBlock(Vector2 position) : base(position) 
        {
            //Generates a boundingbox around the block
            boundingBox = new Rectangle((int)position.X - 8, (int)position.Y - 16, 16, 16);

            //Sets the sizes of the sprite
            sprite.xSize = 16;
            sprite.ySize = 16;

            //Call sprite class to load the texture
            sprite.sourceName = "Grass";

            //Sprite animation
            sprite.NewAnimation(0, 0);
        }

        public override void OnCollision(GameObject collider)
        {
            base.OnCollision(collider);
        }
    }
}
