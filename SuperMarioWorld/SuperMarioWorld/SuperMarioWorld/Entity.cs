﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class Entity : GameObject
    {
        public Vector2 momentum;

        protected bool grounded;

        protected bool lookRight;

        protected float acceleration = 16f;

        protected float maxSpeed;

        public Entity(Vector2 position) : base (position)
        {
            lookRight = true;
            grounded = false;
            maxSpeed = 256.0f;
        }

        public override void Update(GameTime gameTime)
        {
            //Move the bounding box of the object
            boundingBox.X = (int)Math.Round(position.X) - boundingBox.Width / 2;
            boundingBox.Y = (int)Math.Round(position.Y) - boundingBox.Height;

            //Flip sprite when looking left/right
            if (lookRight)
                sprite.effect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
            else
                sprite.effect = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;


            Movement(gameTime);
            //Calls the update function from gameobject

            base.Update(gameTime);
        }

        /// <summary>
        /// General movement for all entities
        /// </summary>
        protected virtual void Movement(GameTime gameTime)
        {
            //Check for collision and turn around when colliding with something
            //TODO



            //This now applies to all entities except the player and the smart koopa who override this function.
            if (lookRight)
                momentum = new Vector2(momentum.X + acceleration, momentum.Y);
            else
                momentum = new Vector2(momentum.X - acceleration, momentum.Y);

            //Limit the momentum for the object (x-axis only)
            if (momentum.X > maxSpeed)
                momentum = new Vector2(maxSpeed, momentum.Y);
            if (momentum.X < -maxSpeed)
                momentum = new Vector2(-maxSpeed, momentum.Y);

            //add momentum to position
            position += momentum * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }
    }
}
