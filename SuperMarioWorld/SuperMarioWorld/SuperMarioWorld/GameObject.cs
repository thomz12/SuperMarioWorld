using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    class GameObject
    {
        public Sprite sprite;
        public Rectangle boundingBox;
        public Vector2 position;

        public GameObject(Vector2 position, SpriteBatch batch)
        {
            this.position = position;

            sprite = new Sprite(batch);
            sprite.sourceName = "Mario";
        }

        public void Draw(SpriteBatch batch)
        {
            batch.Draw(sprite.texture, new Rectangle((int)position.X, (int)position.Y, sprite.texture.Width, sprite.texture.Height), Color.White);
        }
    }
}
