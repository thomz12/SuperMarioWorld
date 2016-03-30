using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public abstract class Entity : GameObject
    {
        //Boolean to see if the enemy is affected by gravity (flying keepo ignore the laws of physics)
        protected bool affectedByGravity;

        //Boolean to see if the Entity was on the ground in the last update
        public bool grounded;

        //Boolean to see if the Entity is looking to the right or to the left
        public bool lookRight;

        //Boolean to see if the Entity is dead, a dead entity will enter its "dead" animation (falling down)
        public bool dead;

        //Boolean to see if the object has already turned around this update (in order to prevent multiple turns per update when the OnCollision function is called twice)
        public bool hasTurned;

        //Value that is added to the position of the Entity
        public Vector2 velocity;

        //Maxiumum velocity that can be gained per frame.
        protected float acceleration;

        //The maximum velocity on the X axis
        protected float maxSpeed;

        //The maximum velocity on the Y axis
        protected float terminalVelocity;


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">X and Y position in Pixels</param>
        public Entity(Point position) : base (position)
        {
            //Default values, to prevent null values
            affectedByGravity = true;
            lookRight = true;
            grounded = false;
            hasTurned = false;
            maxSpeed = 256.0f;
            terminalVelocity = 128.0f;
            acceleration = 16f;
        }

        /// <summary>
        /// Update function that gets called every frame
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //reset the hasTurned boolean
            hasTurned = false;
            //reset the grounded value to false
            grounded = false;

            //Flip sprite when looking left/right
            if (!dead)
            {
                if (lookRight)
                    sprite.effect = Microsoft.Xna.Framework.Graphics.SpriteEffects.None;
                else
                    sprite.effect = Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipHorizontally;
            }

            //Call this entities movement function
            Movement(gameTime);

            //Move the bounding box of the object
            boundingBox.X = (int)Math.Round(position.X) - boundingBox.Width / 2;
            boundingBox.Y = (int)Math.Round(position.Y) - boundingBox.Height;

            //Call the GameObject.Update() function
            base.Update(gameTime);
        }

        /// <summary>
        /// Initiate the death animation for the entity
        /// </summary>
        /// <param name="cause">GameObject that caused this entities death</param>
        public virtual void Death(GameObject cause)
        {
            if (!dead)
            {
                //Set dead to true
                dead = true;
                //Launch the entity upwards
                velocity.Y = -200;
                //Cut the entities velocity
                velocity.X /= 4;
                //Disable the bounding box
                boundingBox.Height = 0;
                boundingBox.Width = 0;
                //Flip the entities sprite upside down
                sprite.effect |= Microsoft.Xna.Framework.Graphics.SpriteEffects.FlipVertically;
            }
        }

        /// <summary>
        /// All the children of entities need OnCollision functions
        /// </summary>
        /// <param name="collider">GameObject that is collided with</param>
        public override void OnCollision(GameObject collider)
        {
            //By default a collision between two object does nothing.
        }

        /// <summary>
        /// General movement for all entities
        /// </summary>
        protected virtual void Movement(GameTime gameTime)
        {
            //Check if entity is not dead
            if (!dead)
            {
                //add or substract the acceleration to the velocity depending on the Entities facing
                if (lookRight)
                    velocity = new Vector2(velocity.X + acceleration, velocity.Y);
                else
                    velocity = new Vector2(velocity.X - acceleration, velocity.Y);

                //Limit the momentum for the object on the X and Y axis
                if (velocity.X > maxSpeed)
                    velocity = new Vector2(maxSpeed, velocity.Y);
                if (velocity.X < -maxSpeed)
                    velocity = new Vector2(-maxSpeed, velocity.Y);

                if (velocity.Y > terminalVelocity)
                    velocity.Y = terminalVelocity;
                if (velocity.Y < -terminalVelocity)
                    velocity.Y = -terminalVelocity;
            }

            //Add gravity when the Entity is off the ground and is affected by gravity
            if(!grounded && affectedByGravity)
                velocity.Y += 1000 * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);

            //add momentum to position
            position += velocity * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }
    }
}
