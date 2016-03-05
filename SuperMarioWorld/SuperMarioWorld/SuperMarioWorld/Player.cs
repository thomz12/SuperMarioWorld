using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class Player : Entity
    {
        /// <summary>
        /// Tracks if mario is small, big, empowered or something else
        /// </summary>
        private int _powerState { get; set; }

        /// <summary>
        /// Different powerstates that the player can have
        /// </summary>
        public enum PowerSate
        {
            small = 0,
            normal = 1,
            fire = 2
        }

        /// <summary>
        /// State the animation is in, changed with player's movement
        /// </summary>
        private int _animationState { get; set; }

        public Player (Vector2 position) : base (position)
        {

        }

        /// <summary>
        /// Handles all the movement of the player.
        /// </summary>
        protected override void Movement(GameTime gameTime)
        {
            //If the button D is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                _lookRight = true;
                //TEMP
                position += new Vector2((float)100 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f, 0);
            }
            //If button A is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                _lookRight = false;
                //TEMP
                position += new Vector2((float)-100 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000f, 0);
            }
        }
    }
}
