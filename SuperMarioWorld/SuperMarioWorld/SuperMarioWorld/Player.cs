using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            boundingBox = new Rectangle(0, 0, 9, 15);

            sprite.xSize = 16;
            sprite.ySize = 32;
            sprite.AddFrame(0, 0);

            acceleration = 300.0f;
            maxSpeed = 64;

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

        /// <summary>
        /// called every frame
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //If the button D is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                lookRight = true;
                if(grounded)
                    momentum.X += acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                else
                    momentum.X += acceleration / 3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            }
            //If button A is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                lookRight = false;
                if (grounded)
                    momentum.X -= acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                else
                    momentum.X -= acceleration / 3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
            }
            if(Keyboard.GetState().IsKeyDown(Keys.Space) && grounded)
            {
                momentum.Y = -128;
                grounded = false;
            }

            //Handle animations
            //if player is moving
            sprite.animationSpeed = 150.0f;
            if(Math.Abs(momentum.X) > 0.5f && grounded)
            {
                sprite.animationSpeed = -3 * Math.Abs(momentum.X) + 300;
                SetAnimation(1);
            }
            //if up is pressed, and not moving
            else if(Keyboard.GetState().IsKeyDown(Keys.W) && Math.Abs(momentum.X) < 0.5f)
            {
                SetAnimation(4);
            }
            //if player is falling
            else if(grounded == false && momentum.Y > 0.5f)
            {
                SetAnimation(3);
            }
            else if(grounded == false && momentum.Y < 0.5f)
            {
                SetAnimation(2);
            }
            //player is doing nothing
            else
            {
                SetAnimation(0);
            }

            //Calculate player movement
            Movement(gameTime);

            base.Update(gameTime);
        }

        //Set current animation
        public void SetAnimation(int animation)
        {
            //If player is already in the animation, exit the method
            if (_animationState == animation)
                return;

            //Clear current animation
            sprite.NewAnimation();

            //Set new animation
            switch (animation)
            {
                //small mario standing
                case 0:
                    sprite.AddFrame(0, 0);
                    break;
                //small mario walk
                case 1:
                    sprite.AddFrame(0, 0);
                    sprite.AddFrame(1, 0);
                    break;
                //small mario jump
                case 2:
                    sprite.AddFrame(2, 0);
                    break;
                //small mario fall
                case 3:
                    sprite.AddFrame(3, 0);
                    break;
                //small mario look up
                case 4:
                    sprite.AddFrame(9, 0);
                    break;
            }
            _animationState = animation;
        }

        /// <summary>
        /// Handles all the movement of the player.
        /// </summary>
        protected override void Movement(GameTime gameTime)
        {

            //calculate friction
            if(gameTime.ElapsedGameTime.TotalMilliseconds != 0 && grounded)
                momentum.X /= 2.0f * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f) + 1;

            if (!grounded)
                momentum.Y += 100 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            //Limit the momentum for the object
            if (momentum.X > maxSpeed)
                momentum = new Vector2(maxSpeed, momentum.Y);
            if (momentum.X < -maxSpeed)
                momentum = new Vector2(-maxSpeed, momentum.Y);

            //add momentum to position
            position += momentum * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);

            //something something collision
            if (position.Y > 0)
            {
                grounded = true;
                position.Y = 0;
            }
        }
    }
}
