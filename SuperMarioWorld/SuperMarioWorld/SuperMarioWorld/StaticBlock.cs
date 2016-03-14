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
    class StaticBlock : Object
    {
        public enum BlockType
        {
            grass,
            cloud,
            rock,
            used,
            help
        }

        private BlockType _blockType;

        public StaticBlock(Vector2 position, BlockType type) : base(position) 
        {
            //What type of static block is this (based on enum BlockType)
            _blockType = type;

            //Generates a boundingbox around the block
            boundingBox = new Rectangle((int)position.X - 8, (int)position.Y - 16, 16, 16);

            //Sets the sizes of the sprite
            sprite.xSize = 16;
            sprite.ySize = 16;
            sprite.animated = false;

            //Call sprite class to load the texture and add texture to the spriteclass for each block
            switch (_blockType)
            {
                case BlockType.grass:
                    sprite.sourceName = @"Blocks\Grass";
                    sprite.NewAnimation(0, 0);
                    isPlatform = true;
                    break;
                case BlockType.cloud:
                    sprite.sourceName = @"Blocks\StaticBlocks";
                    sprite.NewAnimation(0, 0);
                    isPlatform = true;
                    break;
                case BlockType.rock:
                    sprite.sourceName = @"Blocks\StaticBlocks";
                    sprite.NewAnimation(1, 0);
                    isPlatform = false;
                    break;
                case BlockType.used:
                    sprite.sourceName = @"Blocks\StaticBlocks";
                    sprite.NewAnimation(2, 0);
                    isPlatform = false;
                    break;
                case BlockType.help:
                    sprite.sourceName = @"Blocks\StaticBlocks";
                    sprite.NewAnimation(3, 0);
                    isPlatform = false;
                    break;
                default:
                    break;
            }
        }

        public override void OnCollision(GameObject collider)
        {
            base.OnCollision(collider);
        }
    }
}
