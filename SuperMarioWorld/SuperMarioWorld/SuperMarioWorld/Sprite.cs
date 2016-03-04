using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    class Sprite
    {
        public float animationSpeed;
        private SpriteBatch _spriteBatch;
        public Texture2D texture;
        public string sourceName;
        
        public Sprite(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        public void SetAnimation()
        {

        }
    }
}
