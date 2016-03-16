using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        public PowerState powerState = PowerState.small;
        private bool _invunerable;
        private float _invunerableTimer;
        private float _invunerableTime = 2;

        /// <summary>
        /// Different powerstates that the player can have
        /// </summary>
        public enum PowerState
        {
            small = 0,
            normal = 1,
            fire = 2,
            feather = 3
        }

        /// <summary>
        /// State the animation is in, changed with player's movement
        /// </summary>
        private PlayerAnimationState _animationState { get; set; }

        public enum Character
        {
            Mario,
            Luigi,
            Wario,
            Waluigi,
            Peach
        }

        public enum PlayerAnimationState
        {
            idle,
            walking,
            jumping,
            falling,
            lookup,
            running,
            Dead
        }

        //Higher value -> less controll in the air
        private float _airControl = 3.0f;

        //Scorehandler
        private ScoreHandler _scores;

        InputManager _input;

        public Player (Vector2 position, ScoreHandler score, Character character) : base (position)
        {
            //Set a score handler, all the interactions that require a score change go through the player object.
            _scores = score;

            sprite.layer = 0.9f;
            boundingBox = new Rectangle(0, 0, 9, 15);

            sprite.xSize = 16;
            sprite.ySize = 32;
            sprite.AddFrame(0, 0);

            acceleration = 500.0f;
            thermalVelocity = 150;
            maxSpeed = 64;

            switch (character)
            {
                case Character.Mario:
                    sprite.sourceName = @"Players\Mario";
                    break;
                case Character.Luigi:
                    sprite.sourceName = @"Players\Luigi";
                    break;
                case Character.Wario:
                    sprite.sourceName = @"Players\Wario";
                    sprite.xSize = 24;
                    break;
                case Character.Waluigi:
                    sprite.sourceName = @"Players\Waluigi";
                    sprite.xSize = 24;
                    sprite.ySize = 40;
                    break;
                case Character.Peach:
                    sprite.sourceName = @"Players\Peach";
                    break;
                default:
                    break;
            }

            _input = new InputManager();
        }

        /// <summary>
        /// Handles all the interactions that require one or more variables from _scores to be changed
        /// </summary>
        /// <param name="collider">Collision with the collider object</param>
        public override void OnCollision(GameObject collider)
        {
            if(collider is Coin)
            {
                collider.destoryed = true;
                _scores.coins++;
            }
            else if (collider is OneUp)
            {
                collider.destoryed = true;
                _scores.lives++;
            }
            else if (collider is Mushroom)
            {
                collider.destoryed = true;
                if(powerState != PowerState.small)
                {
                    _scores.powerUp = ScoreHandler.PowerUp.mushroom;
                }
                else
                {
                    SetAnimation(PlayerAnimationState.Dead);
                    powerState = PowerState.normal;
                    boundingBox.Height = 24;
                }
            }
        }

        /// <summary>
        /// called every frame
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if(_invunerable)
            {
                _invunerableTimer -= (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

                if (_invunerableTimer < 0)
                    _invunerable = false;
            }

            _input.Update();
            if (!death)
            {
                //If the button D is pressed
                if (_input.IsPressed(Keys.D) || _input.GamePadIsPressed(PlayerIndex.One, Buttons.DPadRight))
                {
                    lookRight = true;
                    if (grounded)
                        momentum.X += acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                    else
                        momentum.X += acceleration / 3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                }
                //If button A is pressed
                if (_input.IsPressed(Keys.A) || _input.GamePadIsPressed(PlayerIndex.One, Buttons.DPadLeft))
                {
                    lookRight = false;
                    if (grounded)
                        momentum.X -= acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                    else
                        momentum.X -= acceleration / _airControl * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                }
                if ((_input.OnPress(Keys.Space) || _input.GamePadOnPress(PlayerIndex.One, Buttons.A)) && grounded)
                {
                    momentum.Y = -140;
                    grounded = false;
                }

                //Handle animations
                //if player is moving
                sprite.animationSpeed = 150.0f;
                if (Math.Abs(momentum.X) > 0.5f && grounded)
                {
                    sprite.animationSpeed = -3 * Math.Abs(momentum.X) + 300;
                    SetAnimation(PlayerAnimationState.walking);
                }
                //if up is pressed, and not moving
                else if (Keyboard.GetState().IsKeyDown(Keys.W) && Math.Abs(momentum.X) < 0.5f && grounded)
                {
                    SetAnimation(PlayerAnimationState.lookup);
                }
                //if player is falling
                else if (grounded == false && momentum.Y > 0.5f)
                {
                    SetAnimation(PlayerAnimationState.falling);
                }
                //If player is jumping
                else if (grounded == false && momentum.Y < 0.5f)
                {
                    SetAnimation(PlayerAnimationState.jumping);
                }
                //player is doing nothing
                else
                {
                    SetAnimation(PlayerAnimationState.idle);
                }
            }
            else
            {
                SetAnimation(PlayerAnimationState.Dead);
            }
            //Calculate player movement
            Movement(gameTime);
            grounded = false;
            base.Update(gameTime);
        }

        //Set current animation
        public void SetAnimation(PlayerAnimationState animation)
        {
            //If player is already in the animation, exit the method
            if (_animationState == animation)
                return;

            //Clear current animation
            sprite.NewAnimation();

            if (powerState == PowerState.small)
            {
                //Set new animation
                switch (animation)
                {
                    //small mario standing
                    case PlayerAnimationState.idle:
                        sprite.AddFrame(0, 0);
                        break;
                    //small mario walk
                    case PlayerAnimationState.walking:
                        sprite.AddFrame(0, 0);
                        sprite.AddFrame(1, 0);
                        break;
                    //small mario jump
                    case PlayerAnimationState.jumping:
                        sprite.AddFrame(2, 0);
                        break;
                    //small mario fall
                    case PlayerAnimationState.falling:
                        sprite.AddFrame(3, 0);
                        break;
                    //small mario look up
                    case PlayerAnimationState.lookup:
                        sprite.AddFrame(9, 0);
                        break;
                    case PlayerAnimationState.Dead:
                        sprite.AddFrame(10, 0);
                        sprite.AddFrame(11, 0);
                        break;
                }
            }
            else
            {
                switch (animation)
                {
                    //mario standing
                    case PlayerAnimationState.idle:
                        sprite.AddFrame(0, 2);
                        break;
                    //mario walk
                    case PlayerAnimationState.walking:
                        sprite.AddFrame(2, 2);
                        sprite.AddFrame(1, 2);
                        sprite.AddFrame(0, 2);
                        break;
                    //mario jump
                    case PlayerAnimationState.jumping:
                        sprite.AddFrame(3, 2);
                        break;
                    //mario fall
                    case PlayerAnimationState.falling:
                        sprite.AddFrame(4, 2);
                        break;
                    //mario look up
                    case PlayerAnimationState.lookup:
                        sprite.AddFrame(10, 2);
                        break;
                }
            }
            _animationState = animation;
        }

        public override void Death()
        {
            if (!_invunerable)
            {
                if (powerState != PowerState.small)
                {
                    SetAnimation(PlayerAnimationState.idle);
                    powerState = PowerState.small;
                    _invunerable = true;
                    _invunerableTimer = _invunerableTime;
                    boundingBox.Height = 14;
                }
                else if (powerState == PowerState.small)
                {
                    death = true;
                    momentum.X = 0;
                    momentum.Y = -500;
                    _scores.lives--;
                }
            }
        }

        public override void DrawObject(SpriteBatch batch)
        {
            if(_invunerable)
            {
                if ((int)Math.Round(_invunerableTimer * 100) % 2 == 0)
                    return;
            }
            //Call the draw function of sprite
            sprite.DrawSpriteCentered(batch, position);
        }

        /// <summary>
        /// Handles all the movement of the player.
        /// </summary>
        protected override void Movement(GameTime gameTime)
        {
            //calculate friction
            if(gameTime.ElapsedGameTime.TotalMilliseconds != 0 && grounded)
                momentum.X /= 8.0f * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f) + 1;

            if (!grounded)
                momentum.Y += 150 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            //Limit the momentum for the object
            if (momentum.X > maxSpeed)
                momentum = new Vector2(maxSpeed, momentum.Y);
            if (momentum.X < -maxSpeed)
                momentum = new Vector2(-maxSpeed, momentum.Y);

            if (momentum.Y > thermalVelocity)
                momentum.Y = thermalVelocity;
            if (momentum.Y < -thermalVelocity)
                momentum.Y = -thermalVelocity;

            //add momentum to position
            position += momentum * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }
    }
}
