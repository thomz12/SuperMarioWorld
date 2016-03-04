using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioWorld
{
    class Level
    {
        private SpriteBatch _spriteBatch;
        private Vector2 size;
        public List<GameObject> objects = new List<GameObject>();

        public Level(SpriteBatch batch)
        {
            _spriteBatch = batch;
        }

        public void Draw()
        {
            //Draw objects
        }
    }
}
