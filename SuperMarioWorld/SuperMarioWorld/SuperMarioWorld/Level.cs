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
        public Level()
        {
            //TEMP, Add a object to the level
            objects.Add( new Player(new Vector2(0.0f,0.0f), Player.Character.Mario));
            objects.Add( new MysteryBlock(new Vector2(0.0f, -32.0f), null));
            objects.Add( new Goomba(new Vector2(64.0f, 0)));

            //add a floor
            for(int i = -512; i <= 512; i+= 16)
            {
                objects.Add(new MysteryBlock(new Vector2(i, 16.0f), null));
            }

            //Create camera object
            cam = new Camera2D();
        }

        public Level(string path)
        {
            throw new NotImplementedException();
            //LOAD LEVEL FROM FILE
        }

        public void Update(GameTime gameTime)
        {
            //Call the update method for all gameobjects
            foreach(GameObject go in objects)
            {
                go.Update(gameTime);
            }
            cam.Position = objects[0].position;
        }

        /// <summary>
        /// Calls the drawfunctions for each object in the current loaded level.
        /// </summary>
        public void DrawLevel(SpriteBatch batch)
        {
            //TODO: culling

            //Draw every object
            foreach(GameObject go in objects)
            {
                //Call the draw method of gameobject
                go.DrawObject(batch);
            }
        }
    }
}
