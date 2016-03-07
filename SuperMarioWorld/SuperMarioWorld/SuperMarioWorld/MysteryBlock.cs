using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class MysteryBlock : Object
    {
        private GameObject _content;

        public MysteryBlock(Vector2 position, GameObject contents) : base (position)
        {
            //Sets the contents of the mysteryblock
            _content = contents;

            //Dimensions for the bounding box
            boundingWidth = 16;
            boundingHeight = 16;

            //Generates a boundingbox around the block
            boundingBox = new Rectangle((int)position.X - boundingWidth / 2, (int)position.Y - boundingHeight, boundingWidth, boundingHeight);
            
            //Sets the sizes of the sprite
            sprite.xSize = 16;
            sprite.ySize = 16;

            //Call sprite class to load the texture
            sprite.sourceName = "MysteryBlock";

            //Sprite animation
            sprite.NewAnimation();
            sprite.animationSpeed = 128.0f;

            //Add all sprites from the file to the list
            for (int i = 0; i < 4; i++)
            {
                sprite.AddFrame(i, 0);
            }
        }
    }
}
