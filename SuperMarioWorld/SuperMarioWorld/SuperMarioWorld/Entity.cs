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
        }

        public override void Update(GameTime gameTime)
        {
            Movement(gameTime);
            sprite.UpdateAnimation(gameTime);
        }

        /// <summary>
        /// General movement for objects
        /// </summary>
        protected virtual void Movement(GameTime gameTime)
        {
            //Moves forward untill it hits a wall, then turns around
            //Falls of platforms
            if (_lookRight)
            {
                position += new Vector2(1, 0);
            }
            else
            {
                position += new Vector2(-1, 0);
            }
        }
    }
}
