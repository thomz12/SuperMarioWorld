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

        protected bool _grounded;

        protected bool _lookRight;

        protected float _maxSpeed;

        public Entity(Vector2 position) : base (position)
        {
            _lookRight = true;
            _grounded = false;
            _maxSpeed = 64.0f;
        }

        public override void Update(GameTime gameTime)
        {
            sprite.UpdateAnimation(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// General movement for objects
        /// </summary>
        protected virtual void Movement(GameTime gameTime)
        {
            //Limit the momentum for the object
            if (momentum.X > _maxSpeed)
                momentum = new Vector2(_maxSpeed, momentum.Y);
            if (momentum.X < -_maxSpeed)
                momentum = new Vector2(-_maxSpeed, momentum.Y);
            if (momentum.Y > _maxSpeed)
                momentum = new Vector2(momentum.X, _maxSpeed);
            if (momentum.Y < -_maxSpeed)
                momentum = new Vector2(momentum.X, -_maxSpeed);

            //add momentum to position
            position += momentum * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }
    }
}
