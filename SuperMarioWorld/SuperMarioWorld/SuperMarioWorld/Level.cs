using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace SuperMarioWorld
{
    class Level
    {
        //Size of the level.
        private Point _size;

        //Size of the grid
        private int _gridSize;

        //Camera object
        public Camera2D cam;

        /// <summary>
        /// List of all the GameObjects in the current level, loaded from a .sml file.
        /// </summary>
        public List<GameObject> objects = new List<GameObject>();

        //Variables for the background
        public string backgroundSourceName = "BushBackground";
        public Texture2D backgroundTexture;

        //Position of the background texture
        private int _backOffset;

        List<GameObject> collidables = new List<GameObject>();

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

            _backOffset = (int)cam.Position.X / 2;

        }

        /// <summary>
        /// Constructs the level from a chosen file
        /// </summary>
        /// <param name="fileName">Give the name of the file without extension</param>
        public Level(string fileName)
        {
            //Set the gridsize
            _gridSize = 16;

            //Create camera object
            cam = new Camera2D();

            _backOffset = 0;

            //Create a streamreader
            StreamReader sr;

            fileName = @"Content\\Levels\\" + fileName + ".sml";

            //LOAD LEVEL FROM FILE
            /*
            Appearantly loading the level from a file like this prevents us from generating contents in f.ex Mystery boxes or changing the way some objects behave
            We need to generate an XML file from the level maker to make this work.

            So basically all this code is useless. but it provides us with a working project for the demo.
            */
            try
            {
                sr = new StreamReader(fileName);
            }
            catch (Exception e)
            {
                throw new FileLoadException("Could not load file", e);
            }

            //Read information about the level from the file
            //Read level name
            string name = sr.ReadLine().Split('\"')[1].Split('\"')[0];
            //Read x size of level
            string xSize = sr.ReadLine();
            //Read y size of level
            string ySize = sr.ReadLine();
            
            //Convert x and y size to Vector2 _size
            _size.X = int.Parse(xSize.Substring(xSize.LastIndexOf("=") + 1));
            _size.Y = int.Parse(ySize.Substring(ySize.LastIndexOf("=") + 1));

            while (true)
            { 
                if (sr.ReadLine().Equals("[Level]"))
                {
                    break;
                }
            }

            //Loops through the read lines
            for (int y = 0; y < _size.Y; y++)
            {
                //Get the line that needs to be used right now
                string thisLine = sr.ReadLine();
                char[] objectChars = thisLine.ToCharArray();

                for (int x = 0; x < _size.X; x++)
                {
                    //Convert the line into objects on correct positions
                    if (objectChars[x].Equals('M')) //If the char is a MysteryBlock
                    {
                        objects.Add(new MysteryBlock(new Vector2((x * _gridSize), (y * _gridSize) + 32 - _gridSize * _size.Y), null));
                    }
                    else if (objectChars[x].Equals('1'))
                    {
                        objects.Add(new Player(new Vector2((x * _gridSize), (y * _gridSize) + 32 - _gridSize * _size.Y), Player.Character.Mario));
                    }
                    else if (objectChars[x].Equals('G'))
                    {
                        objects.Add(new Goomba(new Vector2((x * _gridSize), (y * _gridSize) + 32 - _gridSize * _size.Y)));
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            int camX = (int)cam.Position.X;
            int camY = (int)cam.Position.Y;

            collidables.Clear();

            //Call the update method for all gameobjects
            foreach(GameObject go in objects)
            {
                go.Update(gameTime);

                if (Math.Abs(camX - go.position.X) < 256)
                {
                    if(Math.Abs(camY - go.position.Y) < 224)
                    {
                        collidables.Add(go);
                    }
                }

                if (go is Player)
                {
                    cam.Position = go.position;
                }
            }

            CheckCollisions();
        }

        public void CheckCollisions()
        {
            for(int i  = 0; i < collidables.Count; i++)
            {
                for (int j = i + 1; j < collidables.Count; j++)
                {
                    if (collidables[i].boundingBox.Intersects(collidables[j].boundingBox))
                    {
                        collidables[i].OnCollision(collidables[j]);
                        collidables[j].OnCollision(collidables[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Calls the drawfunctions for each object in the current loaded level.
        /// </summary>
        public void DrawLevel(SpriteBatch batch)
        {
            //Draw background(s)
            batch.Draw(backgroundTexture, new Rectangle(_backOffset + (int)cam.Position.X / 2 - backgroundTexture.Width / 2, (int)cam.Position.Y / 2 - backgroundTexture.Height / 2, backgroundTexture.Width, backgroundTexture.Height), Color.White);
            
            if (cam.Position.X < _backOffset + (int)cam.Position.X / 2)
                _backOffset -= backgroundTexture.Width;
            else
                _backOffset += backgroundTexture.Width;

            batch.Draw(backgroundTexture, new Rectangle(_backOffset + (int)cam.Position.X / 2 - backgroundTexture.Width / 2, (int)cam.Position.Y / 2 - backgroundTexture.Height / 2, backgroundTexture.Width, backgroundTexture.Height), Color.White);

            //Draw every object
            foreach (GameObject go in objects)
            {
                //Call the draw method of gameobject
                go.DrawObject(batch);
            }
        }
    }
}
