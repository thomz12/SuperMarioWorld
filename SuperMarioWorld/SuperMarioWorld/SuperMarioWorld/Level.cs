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
        public SpriteBatch _spriteBatch;
        private Vector2 size;

        public Camera2D cam;
        public List<GameObject> objects = new List<GameObject>();

        public Level(SpriteBatch batch)
        {
            _spriteBatch = batch;
            objects.Add(new GameObject(new Vector2(0,0), batch));
            cam = new Camera2D();
        }

        public void DrawLevel()
        {
            foreach(GameObject go in objects)
            {
                go.Draw(_spriteBatch);
            }
        }
    }
}
