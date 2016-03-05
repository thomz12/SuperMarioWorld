using Microsoft.Xna.Framework;
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

        protected float acceleration;

        protected float maxSpeed;

        public Entity(Vector2 position) : base (position)
        {
            lookRight = true;
            grounded = false;
            acceleration = 16f;
            maxSpeed = 128.0f;
        }

        public override void Update(GameTime gameTime)
        {
            sprite.UpdateAnimation(gameTime);

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
                momentum = new Vector2(momentum.X - acceleration, momentum.Y);
            else
                momentum = new Vector2(momentum.X + acceleration, momentum.Y);

            //Limit the momentum for the object
            if (momentum.X > maxSpeed)
                momentum = new Vector2(maxSpeed, momentum.Y);
            if (momentum.X < -maxSpeed)
                momentum = new Vector2(-maxSpeed, momentum.Y);

            //add momentum to position
            position += momentum * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }
    }
}
