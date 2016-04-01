using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class MysteryBlock : StaticObject
    {
        // The object that the mystery block contains
        public GameObject content;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">Object position</param>
        /// <param name="contents">What the object contains</param>
        public MysteryBlock(Point position, GameObject contents) : base (position)
        {
            // Sets the contents of the mysteryblock
            if (contents != null)
                content = contents;
            else
                content = new Coin(position, true);

            content.position = new Vector2(position.X, position.Y - 16);
            // Generates a boundingbox around the block
            boundingBox = new Rectangle((int)position.X - 8, (int)position.Y - 16, 16, 16);
            
            // Sets the sizes of the sprite
            sprite.xSize = 16;
            sprite.ySize = 16;

            // Call sprite class to load the texture
            sprite.sourceName = @"Blocks\MysteryBlock";

            // Sprite animation
            sprite.NewAnimation();
            sprite.animationSpeed = 128.0f;

            // Add all sprites from the file to the list
            for (int i = 0; i < 4; i++)
            {
                sprite.AddFrame(i, 0);
            }
        }

        /// <summary>
        /// This is called when something collides with the mystery block
        /// </summary>
        /// <param name="collider"></param>
        public override void OnCollision(GameObject collider)
        {
            // If the collider is an entity
            if (collider is Entity)
            {
                // Cast the entity
                Entity entity = (Entity)collider;

                // Create a rectangle for overlap
                Rectangle overlap;
                // Set the overlap rectanble to be as big as the overlap between the two colliding bounding boxes
                Rectangle.Intersect(ref collider.boundingBox, ref boundingBox, out overlap);

                // IF the width of the overlap is bigger than the hight the colliding object comes from the top
                if (overlap.Width > overlap.Height)
                {
                    // Check if the colliding entity is on the top of bottom
                    if (entity.position.Y < position.Y)
                    {
                        // Check if the entity is coming from above
                        if (entity.velocity.Y > 0)
                        {
                            // Set the entity outside of the block
                            entity.position.Y = position.Y - boundingBox.Height + 1;
                            // Set the entities grounded value to true
                            entity.grounded = true;
                            // Set the entities Y velcity to 0
                            entity.velocity.Y = 0;
                        }
                    }
                    else
                    {
                        // The entity comes from below
                        // Set the entity outside of the box
                        entity.position.Y = position.Y + entity.boundingBox.Height;

                        // Push the entity back down
                        entity.velocity.Y = 16;

                        // Spawn the content of the box
                        create(content);

                        // Create a new used block on this blocks position
                        create(new StaticBlock(new Point((int)position.X, (int)position.Y), StaticBlock.BlockType.used, 0.5f));

                        // Destroy this object
                        destroy(this);
                    }
                }
                else
                {
                    // The entity comes from the side
                    if (Math.Abs(entity.boundingBox.Bottom - boundingBox.Top) > 2)
                    {
                        // If it comes from the right
                        if (entity.position.X < position.X)
                        {
                            // Push the entity out of the box
                            entity.position.X = position.X - boundingBox.Width / 2 - entity.boundingBox.Width / 2 - 1;
                        }

                        // If it comes from the left
                        if (entity.position.X > position.X)
                        {
                            // Pus the entity out of th ebox
                            entity.position.X = position.X + boundingBox.Width / 2 + entity.boundingBox.Width / 2;
                        }

                        // Make the entity turn around when it is not a player
                        if (!(entity is Player))
                            entity.lookRight = !entity.lookRight;

                        // Reset the entities velocity
                        entity.velocity.X = 0;
                    }
                }
            }
        }
    }
}
