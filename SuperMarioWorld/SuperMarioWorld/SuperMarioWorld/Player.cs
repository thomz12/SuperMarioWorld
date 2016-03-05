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

        public enum Character
        {
            Mario,
            Luigi,
            Wario,
            Waluigi
        }

        public Player (Vector2 position, Character character) : base (position)
        {
            boundingWidth = 9;
            boundingHeight = 15;

            boundingBox = new Rectangle((int)position.X - boundingWidth / 2, (int)position.Y - boundingHeight, boundingWidth, boundingHeight);

            sprite.xSize = 16;
            sprite.ySize = 32;

            switch (character)
            {
                case Character.Mario:
                    sprite.sourceName = "Mario";
                    break;
                case Character.Luigi:
                    sprite.sourceName = "Luigi";
                    break;
                case Character.Wario:
                    sprite.sourceName = "Wario";
                    sprite.xSize = 24;
                    boundingWidth = 12;
                    break;
                case Character.Waluigi:
                    sprite.sourceName = "Waluigi";
                    sprite.xSize = 24;
                    sprite.ySize = 40;
                    break;
                default:
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            sprite.AnimationPositions.Clear();
            if (lookRight) 
                sprite.AnimationPositions.Add(new Vector2(0, 0));
            else
                sprite.AnimationPositions.Add(new Vector2(0, 1));

            //If the button D is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                lookRight = true;
                momentum = new Vector2(momentum.X + acceleration, momentum.Y);

                sprite.AnimationPositions.Clear();
                sprite.AnimationPositions.Add(new Vector2(0, 0));
                sprite.AnimationPositions.Add(new Vector2(1, 0));
            }
            //If button A is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                lookRight = false;
                momentum = new Vector2(momentum.X - acceleration, momentum.Y);

                sprite.AnimationPositions.Clear();
                sprite.AnimationPositions.Add(new Vector2(0, 1));
                sprite.AnimationPositions.Add(new Vector2(1, 1));
            }
            if(Keyboard.GetState().IsKeyDown(Keys.W) && Math.Abs(momentum.X) < 0.5f)
            {
                sprite.AnimationPositions.Clear();
                if (lookRight)
                    sprite.AnimationPositions.Add(new Vector2(9, 0));
                else
                    sprite.AnimationPositions.Add(new Vector2(9, 1));
            }

            Movement(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Handles all the movement of the player.
        /// </summary>
        protected override void Movement(GameTime gameTime)
        {
            //calculate friction
            if (gameTime.ElapsedGameTime.TotalMilliseconds != 0.0f)
                momentum = new Vector2(momentum.X * 0.8f, momentum.Y);

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
