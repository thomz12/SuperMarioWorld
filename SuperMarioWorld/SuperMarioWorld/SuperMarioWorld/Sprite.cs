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
        private Texture2D _texture;
        public string sourceName;
        
        public Sprite(Texture2D texture, SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _texture = texture;
        }

        public void Draw(Vector2 pos)
        {

        }

        public void SetAnimation()
        {

        }
    }
}
