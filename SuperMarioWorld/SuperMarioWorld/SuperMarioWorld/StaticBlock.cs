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
    public class StaticBlock : StaticObject
    {
        public enum BlockType
        {
            grassMiddle,
            grassLeft,
            grassRight,
            dirtMiddle,
            dirtLeft,
            dirtRight,
            cloud,
            rock,
            used,
            help
        }

        public BlockType blockType;

        public StaticBlock(Point position, BlockType type, float layer) : base(position) 
        {
            //What type of static block is this (based on enum BlockType)
            blockType = type;

            //Generates a boundingbox around the block
            boundingBox = new Rectangle(position.X - 8, position.Y - 16, 16, 16);

            //Sets the sizes of the sprite
            sprite.xSize = 16;
            sprite.ySize = 16;
            sprite.animated = false;
            sprite.layer = layer;

            //Call sprite class to load the texture and add texture to the spriteclass for each block
            switch (blockType)
            {
                case BlockType.grassMiddle:
                    sprite.sourceName = @"Blocks\Grass";
                    sprite.NewAnimation(0, 0);
                    isPlatform = true;
                    break;
                case BlockType.grassLeft:
                    sprite.sourceName = @"Blocks\Grass";
                    sprite.NewAnimation(1, 0);
                    isPlatform = true;
                    break;
                case BlockType.grassRight:
                    sprite.sourceName = @"Blocks\Grass";
                    sprite.NewAnimation(2, 0);
                    isPlatform = true;
                    break;
                case BlockType.dirtMiddle:
                    sprite.sourceName = @"Blocks\Grass";
                    sprite.NewAnimation(3, 0);
                    isPlatform = false;
                    blocking = false;
                    sprite.layer = layer;
                    break;
                case BlockType.dirtLeft:
                    sprite.sourceName = @"Blocks\Grass";
                    sprite.NewAnimation(4, 0);
                    isPlatform = false;
                    blocking = false;
                    sprite.layer = layer;
                    break;
                case BlockType.dirtRight:
                    sprite.sourceName = @"Blocks\Grass";
                    sprite.NewAnimation(5, 0);
                    isPlatform = false;
                    blocking = false;
                    sprite.layer = layer;
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
    }
}
