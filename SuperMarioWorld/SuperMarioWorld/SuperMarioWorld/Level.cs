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
        //Spritebatch that is currently used.
        public SpriteBatch _spriteBatch;

        //Size of the level.
        private Vector2 size;

        //Camera object
        public Camera2D cam;

        /// <summary>
        /// List of all the GameObjects in the current level, loaded from a .sml file.
        /// </summary>
        public List<GameObject> objects = new List<GameObject>();

        /// <summary>
        /// When loading the level this function will add a camera and all objects from the file.
        /// </summary>
        /// <param name="batch">Give the batch that the sprites should be drawn in.</param>
        public Level(SpriteBatch batch)
        {
            _spriteBatch = batch;
            objects.Add(new GameObject(new Vector2(0,0), batch));
            cam = new Camera2D();
        }

        /// <summary>
        /// Calls the drawfunctions for each object in the current loaded level.
        /// </summary>
        public void DrawLevel()
        {
            foreach(GameObject go in objects)
            {
                go.Draw(_spriteBatch);
            }
        }
    }
}
