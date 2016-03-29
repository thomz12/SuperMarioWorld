using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public abstract class Entity : GameObject
    {
        public Vector2 momentum;

        protected bool affectedByGravity;
        public bool grounded;

        public bool lookRight;

        protected float acceleration;

        public bool dead;

        protected float maxSpeed;
        protected float terminalVelocity;

        public bool hasTurned;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position"></param>
        public Entity(Point position) : base (position)
        {
            affectedByGravity = true;
            lookRight = true;
            grounded = false;
            maxSpeed = 256.0f;
            terminalVelocity = 128.0f;
            acceleration = 16f;
            hasTurned = false;
        }

        public override void Update(GameTime gameTime)
        {
            hasTurned = false;

            //Flip sprite when looking left/right
            if (!dead)
            {
                if (lookRight)
                    sprite.effect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                else
                    sprite.effect = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }

            //move
            Movement(gameTime);

            //Move the bounding box of the object
            boundingBox.X = (int)Math.Round(position.X) - boundingBox.Width / 2;
            boundingBox.Y = (int)Math.Round(position.Y) - boundingBox.Height;

            //update
            base.Update(gameTime);

            //If the object is affected by gravity set the grounded bool to false every update (needs a change)
            grounded = false;
        }

        /// <summary>
        /// Initiate the death animation for the entity
        /// </summary>
        /// <param name="cause">GameObject that caused this entities death</param>
        public virtual void Death(GameObject cause)
        {
            if (!dead)
            {
                dead = true;
                momentum.Y = -200;
                momentum.X /= 4;
                boundingBox.Height = 0;
                boundingBox.Width = 0;
                sprite.effect |= Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically;
            }
        }

        public override void OnCollision(GameObject collider)
        {
            
        }

        /// <summary>
        /// General movement for all entities
        /// </summary>
        protected virtual void Movement(GameTime gameTime)
        {
            //This now applies to all entities except the player and the smart koopa who override this function.
            if (!dead)
            {
                if (lookRight)
                    momentum = new Vector2(momentum.X + acceleration, momentum.Y);
                else
                    momentum = new Vector2(momentum.X - acceleration, momentum.Y);
            }

            //Add gravity
            if(!grounded && affectedByGravity)
                momentum.Y += 1000 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            //Limit the momentum for the object
            if (momentum.X > maxSpeed)
                momentum = new Vector2(maxSpeed, momentum.Y);
            if (momentum.X < -maxSpeed)
                momentum = new Vector2(-maxSpeed, momentum.Y);

            if (momentum.Y > terminalVelocity)
                momentum.Y = terminalVelocity;
            if (momentum.Y < -terminalVelocity)
                momentum.Y = -terminalVelocity;

            //add momentum to position
            position += momentum * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }
    }
}
