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
    public class Player : Entity
    {
        /// <summary>Tracks if mario is small, big, empowered or something else</summary>
        public PowerState powerState = PowerState.small;

        // Tracks if the player is invunerable or not
        private bool _invunerable;
        private float _invunerableTimer;
        private float _invunerableTime = 2;

        /// <summary>Different powerstates that the player can have</summary>
        public enum PowerState
        {
            small = 0,
            normal = 1,
        }

        /// <summary>State the animation is in, changed with player's movement</summary>
        private PlayerAnimationState _animationState { get; set; }

        /// <summary>Different characters have different aspects</summary>
        public enum Character
        {
            Mario,
            Luigi,
            Wario,
            Waluigi,
            Peach
        }
        private Character _character;

        /// <summary>Different animation states for every type of state</summary>
        public enum PlayerAnimationState
        {
            idle,
            walking,
            jumping,
            falling,
            lookup,
            running,
            dead
        }

        // Higher value means less controll in the air
        private float _airControl;

        // Higher value makes the player slow down more
        private float _friction;

        // Higher value makes the player jump higher
        private float _jumpForce;

        // Higher value makes the player fall faster
        private float _fallVelocity;

        // The score handler so player can access it.
        private ScoreHandler _scores;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="position">start position</param>
        /// <param name="score">score handler</param>
        /// <param name="character">what character is the player?</param>
        public Player (Point position, ScoreHandler score, Character character) : base (position)
        {
            // Set a score handler, all the interactions that require a score change go through the player object.
            _scores = score;
            _character = character;

            // Set a layer for the player to be drawn
            sprite.layer = 0.9f;

            // Set the sprites size
            sprite.xSize = 16;
            sprite.ySize = 32;

            // Add a standard animation
            sprite.AddFrame(0, 0);

            // Generate a bounding box
            boundingBox = new Rectangle(0, 0, 9, 15);

            // Accelertion of the player
            acceleration = 500.0f;
            // Max vertical velocity
            terminalVelocity = 150;
            // Max velocity when falling (different for peach)
            _fallVelocity = terminalVelocity;
            // How much the player slows when grounded
            _friction = 8.0f;
            // Maximum horizontal velocity
            maxSpeed = 64;
            // Jump force
            _jumpForce = -140;
            // How much control in the air (lower is better)
            _airControl = 3.0f;

            // Switch characteristics on different characters
            switch (character)
            {
                // Mario, keeps default values
                case Character.Mario:
                    sprite.sourceName = @"Players\Mario";
                    break;
                // Luigi, jumps higher, less friction
                case Character.Luigi:
                    sprite.sourceName = @"Players\Luigi";
                    _friction = 4.0f;               
                    _jumpForce = -200;
                    break;
                // Wario slower, but more air control
                case Character.Wario:
                    sprite.sourceName = @"Players\Wario";
                    _airControl = 2.0f;
                    acceleration = 420.0f;
                    sprite.xSize = 24;
                    break;
                // Waluigi runs faster
                case Character.Waluigi:
                    sprite.sourceName = @"Players\Waluigi";
                    sprite.xSize = 24;
                    sprite.ySize = 40;
                    _friction = 4.0f;
                    _airControl = 4.0f;
                    maxSpeed = 90;
                    break;
                // Peach falls slower to the ground
                case Character.Peach:
                    sprite.sourceName = @"Players\Peach";
                    terminalVelocity = 150;
                    _fallVelocity = terminalVelocity / 2;
                    _jumpForce = -150;
                    _airControl = 4.0f;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Handles all the interactions that require one or more variables from _scores to be changed
        /// </summary>
        /// <param name="collider">Collision with the collider object</param>
        public override void OnCollision(GameObject collider)
        {
            // All the scoring is done by the player.
            if(collider is Coin)
            {
                // Destroy the coin
                destroy(collider);
                // If player is Wario, collect bonus coins
                if (_character == Character.Wario)
                    _scores.coins += 2;
                else
                    _scores.coins++;
            }
            else if (collider is OneUp)
            {
                // Destroy the OneUP
                destroy(collider);
                // Add a live
                _scores.lives++;
                // Add a bunch of score
                _scores.score += 1000;
            }
            else if (collider is Mushroom)
            {
                // Destroy the Mushroom
                destroy(collider);
                // If the player is not
                if(powerState != PowerState.small)
                {
                    // Put the mushroom in the powerup slot
                    _scores.powerUp = ScoreHandler.PowerUp.mushroom;
                    // Add a bunch of score
                    _scores.score += 1000;
                }
                else
                {
                    // The player is small
                    // Reset the animation
                    SetAnimation(PlayerAnimationState.dead);
                    // Set the powerstate to nromal
                    powerState = PowerState.normal;
                    // Change the height of the bounding box
                    boundingBox.Height = 24;
                }
            }
            else if (collider is Enemy)
            {
                // If the player lands on top of the enemies head
                if(velocity.Y > 3)
                {
                    // Add a combo to score
                    _scores.AddCombo();
                }
            }
            else if (collider is EmptyShell)
            {
                // If the player lands on top of the shell
                if (velocity.Y > 3)
                {
                    // Add a combo to score
                    _scores.AddCombo();
                }
            }
        }

        /// <summary>
        /// called every frame
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            // When the player is on the ground
            if (grounded)
            {
                // Reset the combo
                _scores.ResetCombo();
            }
            
            // When the player is invunerable
            if (_invunerable)
            {
                // Remove some invunerability time
                _invunerableTimer -= (float)gameTime.ElapsedGameTime.Milliseconds / 1000.0f;

                // If the invunerablility time is smaller than 0 the player is no longer invunerable
                if (_invunerableTimer < 0)
                    _invunerable = false;
            }

            // Check if th eplayer is not dead, so it can move
            if (!dead)
            {
                // If the button D is pressed
                if (InputManager.Instance.KeyboardIsPressed(Keys.D) || InputManager.Instance.GamePadIsPressed(Buttons.DPadRight) || InputManager.Instance.GamePadAnalogLeftX() > 0.1f)
                {
                    lookRight = true;
                    if (grounded)
                        velocity.X += acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                    else
                        velocity.X += acceleration / 3 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                }
                // If button A is pressed
                if (InputManager.Instance.KeyboardIsPressed(Keys.A) || InputManager.Instance.GamePadIsPressed(Buttons.DPadLeft) || InputManager.Instance.GamePadAnalogLeftX() < -0.1f)
                {
                    lookRight = false;
                    if (grounded)
                        velocity.X -= acceleration * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                    else
                        velocity.X -= acceleration / _airControl * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;
                }
                // IF the spacebar is pressed
                if ((InputManager.Instance.KeyboardOnRelease(Keys.Space) || InputManager.Instance.GamePadOnPress(Buttons.A)) && grounded)
                {
                    velocity.Y = _jumpForce;
                    grounded = false;
                }

                // Handle animations
                // When the player is moving
                sprite.animationSpeed = 150.0f;

                // Check if the player is moving on the ground
                if (Math.Abs(velocity.X) > 0.5f && grounded)
                {
                    // Set the animation speed to be relative to the players movement speed
                    sprite.animationSpeed = -3 * Math.Abs(velocity.X) + 300;
                    // Set the animation to be the walking animation
                    SetAnimation(PlayerAnimationState.walking);
                }
                // If up is pressed, and not moving
                else if (Keyboard.GetState().IsKeyDown(Keys.W) && Math.Abs(velocity.X) < 0.5f && grounded)
                {
                    // Set the animation to be the player looking up
                    SetAnimation(PlayerAnimationState.lookup);
                }
                // If player is falling
                else if (grounded == false && velocity.Y > 0.5f)
                {
                    // Set the players animation to be falling animation
                    SetAnimation(PlayerAnimationState.falling);
                }
                // If player is jumping
                else if (grounded == false && velocity.Y < 0.5f)
                {
                    // Set the player animtaion to be jumping animation
                    SetAnimation(PlayerAnimationState.jumping);
                }
                // Player is doing nothing
                else
                {
                    // Set the animation to be the idle animation
                    SetAnimation(PlayerAnimationState.idle);
                }
            }
            else
            {
                // Set the animation to be ded player animation
                SetAnimation(PlayerAnimationState.dead);
            }

            // Call the movement function
            Movement(gameTime);

            // Set the grounded value to false
            grounded = false;

            // Call the entities update function
            base.Update(gameTime);
        }

        /// <summary>
        /// Sets an animation to be shown
        /// </summary>
        /// <param name="animation">The state that says which animation should be shown</param>
        public void SetAnimation(PlayerAnimationState animation)
        {
            // If player is already in the animation, exit the method
            if (_animationState == animation)
                return;

            // Clear current animation
            sprite.NewAnimation();

            // Check if the powerstate is small
            if (powerState == PowerState.small)
            {
                // Set new animation
                switch (animation)
                {
                    // Small player standing
                    case PlayerAnimationState.idle:
                        sprite.AddFrame(0, 0);
                        break;
                    // Small player walk
                    case PlayerAnimationState.walking:
                        sprite.AddFrame(0, 0);
                        sprite.AddFrame(1, 0);
                        break;
                    // Small player jump
                    case PlayerAnimationState.jumping:
                        sprite.AddFrame(2, 0);
                        break;
                    // Small player fall
                    case PlayerAnimationState.falling:
                        sprite.AddFrame(3, 0);
                        break;
                    // Small player look up
                    case PlayerAnimationState.lookup:
                        sprite.AddFrame(9, 0);
                        break;
                    // Small player dead
                    case PlayerAnimationState.dead:
                        sprite.AddFrame(10, 0);
                        sprite.AddFrame(11, 0);
                        break;
                }
            }
            // Check if the powerstate is normal
            else if (powerState == Player.PowerState.normal)
            {
                switch (animation)
                {
                    // Normal player standing
                    case PlayerAnimationState.idle:
                        sprite.AddFrame(0, 1);
                        break;
                    // Normal player walk
                    case PlayerAnimationState.walking:
                        sprite.AddFrame(2, 1);
                        sprite.AddFrame(1, 1);
                        sprite.AddFrame(0, 1);
                        break;
                    // Normal player jump
                    case PlayerAnimationState.jumping:
                        sprite.AddFrame(3, 1);
                        break;
                    // Normal player fall
                    case PlayerAnimationState.falling:
                        sprite.AddFrame(4, 1);
                        break;
                    // Normal player look up
                    case PlayerAnimationState.lookup:
                        sprite.AddFrame(10, 1);
                        break;
                }
            }

            // Set the animation state to be the current animation state
            _animationState = animation;
        }

        /// <summary>
        /// Call this when the player should die
        /// </summary>
        /// <param name="cause">The cause of the death</param>
        public override void Death(GameObject cause)
        {
            // Checks if player is vunerable
            if (!_invunerable)
            {
                // If the player is anything else than small
                if (powerState != PowerState.small)
                {
                    // Reset the animation
                    SetAnimation(PlayerAnimationState.idle);
                    
                    // Set the power state to small
                    powerState = PowerState.small;

                    // Set invunerable to true
                    _invunerable = true;

                    // Set invunerable time
                    _invunerableTimer = _invunerableTime;

                    // Set the small bounding box height
                    boundingBox.Height = 14;

                    // If the score is is availabe
                    if (_scores != null)
                    {
                        // Drop a mushroom when it is available
                        if (_scores.powerUp == ScoreHandler.PowerUp.mushroom)
                        {
                            // Create a new mushroom
                            create(new Mushroom(new Point((int)position.X, (int)position.Y - 128)));

                            // Remove the mushroom powerup from the score powerup
                            _scores.powerUp = ScoreHandler.PowerUp.none;
                        }
                    }
                }
                else if (powerState == PowerState.small)
                {
                    // Powerstate is small so the player dies
                    dead = true;

                    // Set x velocity to 0 and y velocity to 500 so the player goes up
                    velocity.X = 0;
                    velocity.Y = -500;

                    // Remove the bounding box
                    boundingBox.Width = 0;
                    boundingBox.Height = 0;

                    // Remove a live from the score
                    _scores.lives--;
                    // Reset the combo the player is on
                    _scores.ResetCombo();
                }
            }
        }

        /// <summary>
        /// This is called to draw the object
        /// </summary>
        /// <param name="batch">The batch that should be drawn on</param>
        public override void DrawObject(SpriteBatch batch)
        {
            // If the player is invunerable
            if(_invunerable)
            {
                // Make the player blink
                if ((int)Math.Round(_invunerableTimer * 100) % 2 == 0)
                    return;
            }
            // Call the draw function of sprite
            sprite.DrawSpriteCentered(batch, position);
        }

        /// <summary>
        /// Handles all the movement of the player.
        /// </summary>
        protected override void Movement(GameTime gameTime)
        {
            // Calculate friction
            if(gameTime.ElapsedGameTime.TotalMilliseconds != 0 && grounded)
                velocity.X /= _friction * ((float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f) + 1;

            // Add gravity
            if (!grounded)
                velocity.Y += 150 * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            // Limit the momentum for the object
            if (velocity.X > maxSpeed)
                velocity = new Vector2(maxSpeed, velocity.Y);
            if (velocity.X < -maxSpeed)
                velocity = new Vector2(-maxSpeed, velocity.Y);

            if (velocity.Y > _fallVelocity)
                velocity.Y = _fallVelocity;
            if (velocity.Y < -terminalVelocity)
                velocity.Y = -terminalVelocity;

            // Add momentum to position
            position += velocity * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }
    }
}
