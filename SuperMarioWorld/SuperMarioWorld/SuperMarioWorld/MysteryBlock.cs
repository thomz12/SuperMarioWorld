using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            _content.position = new Vector2(position.X, position.Y - 16);
            //Generates a boundingbox around the block
            boundingBox = new Rectangle((int)position.X - 8, (int)position.Y - 16, 16, 16);
            
            //Sets the sizes of the sprite
            sprite.xSize = 16;
            sprite.ySize = 16;

            //Call sprite class to load the texture
            sprite.sourceName = @"Blocks\MysteryBlock";

            //Sprite animation
            sprite.NewAnimation();
            sprite.animationSpeed = 128.0f;

            //Add all sprites from the file to the list
            for (int i = 0; i < 4; i++)
            {
                sprite.AddFrame(i, 0);
            }
        }

        public override void OnCollision(GameObject collider)
        {
            if (collider is Entity)
            {
                Entity p = (Entity)collider;

                Rectangle overlap;
                Rectangle.Intersect(ref collider.boundingBox, ref boundingBox, out overlap);
                if (overlap.Width > overlap.Height)
                {
                    if (p.position.Y < position.Y)
                    {
                        if (p.momentum.Y > 0)
                        {
                            //When entity collides from the top
                            p.position.Y = position.Y - boundingBox.Height + 1;
                            p.momentum.Y = 16;
                            p.grounded = true;
                            p.momentum.Y = 0;
                        }
                    }
                    else
                    {
                        p.position.Y = position.Y + p.boundingBox.Height;
                        if (p is Player)
                        {
                            sprite.NewAnimation();
                            sprite.AddFrame(4, 0);
                            if (_content != null)
                            {
                                create(_content);
                                _content = null;
                            }
                        }

                        p.momentum.Y = 16;
                    }
                }
                else
                {
                    if (Math.Abs(p.boundingBox.Bottom - boundingBox.Top) > 2)
                    {
                        if (p.position.X < position.X)
                        {
                            p.position.X = position.X - boundingBox.Width / 2 - p.boundingBox.Width / 2 - 1;
                        }

                        if (p.position.X > position.X)
                        {
                            p.position.X = position.X + boundingBox.Width / 2 + p.boundingBox.Width / 2;
                        }

                        if (!(p is Player))
                            p.lookRight = !p.lookRight;

                        p.momentum.X = 0;
                    }
                }
            }
        }
    }
}
