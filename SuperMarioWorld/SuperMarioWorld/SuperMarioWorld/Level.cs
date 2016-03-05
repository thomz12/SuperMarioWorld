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
        //Size of the level.
        private Vector2 _size;

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
            //TEMP, Add a object to the level
            GameObject mario = new GameObject(new Vector2(-20.0f,0.0f));
            mario.sprite.sourceName = "Mario";
            objects.Add(mario);
            GameObject luigi = new GameObject(new Vector2(0.0f, 0.0f));
            luigi.sprite.sourceName = "Luigi";
            objects.Add(luigi);
            GameObject wario = new GameObject(new Vector2(16.0f, 0.0f));
            objects.Add(wario);
            wario.sprite.xSize = 24;
            wario.sprite.sourceName = "Wario";
            
            //Create camera object
            cam = new Camera2D();
        }

        public void Update(GameTime gameTime)
        {
            //Call the update method for all gameobjects
            foreach(GameObject go in objects)
            {
                go.Update(gameTime);
            }
        }

        /// <summary>
        /// Calls the drawfunctions for each object in the current loaded level.
        /// </summary>
        public void DrawLevel(SpriteBatch batch)
        {
            //Draw every sprite
            foreach(GameObject go in objects)
            {
                //Call the draw method of gameobject
                go.Draw(batch);
            }
        }
    }
}
