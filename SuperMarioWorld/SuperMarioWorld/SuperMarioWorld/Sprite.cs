using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    class Sprite
    {
        //Speed of the animation of the sprite.
        public float animationSpeed;

        /// <summary>
        /// Name of the source of the texture. (without extension!)
        /// </summary>
        public string sourceName;

        /// <summary>
        /// Texture which is loaded in from a file.
        /// </summary>
        public Texture2D texture;
                
        public Sprite()
        {
            
        }

        /// <summary>
        /// Changes animation based on the state of the object.
        /// </summary>
        public void SetAnimation()
        {

        }
    }
}
