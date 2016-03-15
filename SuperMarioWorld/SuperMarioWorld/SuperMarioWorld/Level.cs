using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioWorld
{
    class Level
    {
        //Content manager
        private ContentManager _contentManager;

        //HUD
        private HUD _hud;

        //Score handler
        private ScoreHandler _scores;

        //Size of the level.
        private Point _size;

        //Size of the grid
        private int _gridSize;

        //Camera object
        public Camera2D cam;
        //player object
        private Player _player;

        /// <summary>
        /// List of all the GameObjects in the current level, loaded from a .sml file.
        /// </summary>
        public List<GameObject> objects = new List<GameObject>();

        //Dictionary of all the sprites that the gameobjects use, so they dont have to be loaded in multiple times
        private Dictionary<string, Texture2D> loadedSprites = new Dictionary<string, Texture2D>();


        //Variables for the background
        private string _backgroundSourceName = "BushBackground";
        private Texture2D _backgroundTexture;

        //Position of the background texture
        private int _backOffset;

        private List<GameObject> _collidables = new List<GameObject>();

        /// <summary>
        /// Display a testlevel
        /// </summary>
        /// <param name="batch">Give the batch that the sprites should be drawn in.</param>
        public Level(ScoreHandler scoreHandler)
        {
            _hud = new HUD(scoreHandler);
            _scores = scoreHandler;

            //Set a max time
            _scores.maxTime = 420;

            //TEMP, Add a object to the level
            _player = new Player(new Vector2(0.0f, 0.0f), _scores, Player.Character.Mario);
            objects.Add(_player);
            objects.Add( new MysteryBlock(new Vector2(0.0f, -32.0f), null));
            objects.Add( new Goomba(new Vector2(64.0f, 0)));

            //add a floor
            for(int i = -512; i <= 512; i+= 16)
            {
                objects.Add(new MysteryBlock(new Vector2(i, 16.0f), null));
            }

            //Create camera object
            cam = new Camera2D(_player, _size, _gridSize);

            _backOffset = (int)cam.Position.X / 2;
        }

        /// <summary>
        /// Constructs the level from a chosen file
        /// </summary>
        /// <param name="fileName">Give the name of the file without extension</param>
        public Level(string fileName, ScoreHandler scoreHandler)
        {
            //Create the scorehandler so the level file can put information in it.
            _scores = scoreHandler;


            //Set the gridsize
            _gridSize = 16;

            _backOffset = 0;

            //Create a streamreader
            StreamReader sr;

            fileName = @"Content\Levels\" + fileName + ".sml";

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
                    if (objectChars[x].Equals('M')) //If the char represents a MysteryBlock
                    {
                        objects.Add(new MysteryBlock(new Vector2((x * _gridSize), (y * _gridSize)), null));
                    }
                    else if (objectChars[x].Equals('1')) //If the char represents a Player
                    {
                        Random r = new Random();
                        _player = new Player(new Vector2((x * _gridSize), (y * _gridSize)), _scores, (Player.Character)r.Next(0, 4));
                        objects.Add(_player);
                    }
                    else if (objectChars[x].Equals('G')) //If the char represents a Goomba
                    {
                        objects.Add(new Goomba(new Vector2((x * _gridSize), (y * _gridSize))));
                    }
                    else if (objectChars[x].Equals('C')) //If the char represents a coin which moves
                    {
                        objects.Add(new Coin(new Vector2((x * _gridSize), (y * _gridSize)), true));
                    }
                    else if (objectChars[x].Equals('c')) //If the char represents a coin which doesnt move
                    {
                        objects.Add(new Coin(new Vector2((x * _gridSize), (y * _gridSize)), false));
                    }
                    else if(objectChars[x].Equals('R')) //If the char represents a Grass
                    {
                        StaticBlock grass = new StaticBlock(new Vector2((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.grass);
                        objects.Add(grass);
                        for (int i = (int)((grass.position.Y + 1) / _gridSize); i < _size.Y; i++)
                        {
                            objects.Add(new StaticBlock(new Vector2((int)grass.position.X, (i + 1) * _gridSize), StaticBlock.BlockType.dirt));
                        }
                    }
                    else if(objectChars[x].Equals('D')) //If the char represents a Dirt
                    {
                        objects.Add(new StaticBlock(new Vector2((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.dirt));
                    }
                    else if (objectChars[x].Equals('K')) //If the char represents a Koopa
                    {
                        objects.Add(new GreenKoopa(new Vector2((x * _gridSize), (y * _gridSize))));
                    }
                    else if (objectChars[x].Equals('S')) //If the char represents a Shell
                    {
                        objects.Add(new EmptyShell(new Vector2((x * _gridSize), (y * _gridSize)), EmptyShell.KoopaType.green));
                    }
                    else if (objectChars[x].Equals('L')) //If the char represents a cloud
                    {
                        objects.Add(new StaticBlock(new Vector2((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.cloud));
                    }
                    else if (objectChars[x].Equals('O')) //If the char represents a rock
                    {
                        objects.Add(new StaticBlock(new Vector2((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.rock));
                    }
                    else if (objectChars[x].Equals('H')) //If the char represents a help
                    {
                        objects.Add(new StaticBlock(new Vector2((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.help));
                    }
                    else if (objectChars[x].Equals('U')) //If the char represents a used block (brown one)
                    {
                        objects.Add(new StaticBlock(new Vector2((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.used));
                    }
                }
            }

            //Load in formation from the level file
            //TODO add a time to load from the level file
            _scores.maxTime = 420;

            //TODO load information from a savefile

            //Create a new HUD for this level
            _hud = new HUD(scoreHandler);


            //Create camera object
            cam = new Camera2D(_player, _size, _gridSize);
        }

        /// <summary>
        /// Load sprites of the content.
        /// </summary>
        /// <param name="contentManager"></param>
        public void LoadContent(ContentManager contentManager)
        {
            //Set a content manager so new items can be spawned in.
            _contentManager = contentManager;

            //Load all the assets that belong to level
            _backgroundTexture = contentManager.Load<Texture2D>(_backgroundSourceName);

            foreach (GameObject go in objects)
            {
                if (!loadedSprites.ContainsKey(go.sprite.sourceName))
                {
                    loadedSprites.Add(go.sprite.sourceName, contentManager.Load<Texture2D>(go.sprite.sourceName));
                }

                go.sprite.texture = loadedSprites[go.sprite.sourceName];
            }

            //Tell HUD to load its content too
            _hud.LoadContent(contentManager);
        }

        /// <summary>
        /// Call update for every object in the level, check collisions and call oncollisions
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            int camX = (int)cam.Position.X;
            int camY = (int)cam.Position.Y;

            _collidables.Clear();

            //Call the update method for all gameobjects
            for (int i = 0; i < objects.Count; i++)
            {
                if(objects[i].destoryed)
                {
                    objects.RemoveAt(i);
                    continue;
                }

                //Check if the game object is within the screen
                if (Math.Abs(camX - objects[i].position.X) < 256)
                {
                    if (Math.Abs(camY - objects[i].position.Y) < 224)
                    {
                        //Update the object and add it to the list of collidable objects.
                        objects[i].Update(gameTime);
                        _collidables.Add(objects[i]);
                    }
                }
            }
        
            //Check collisions
            CheckCollisions();

            //Update the camera position
            cam.Update();

            //Tell HUD to update
            _hud.Update(gameTime);
        }

        /// <summary>
        /// Check if something collides in the camera area and call the two colliding object's OnCollision functions
        /// </summary>
        public void CheckCollisions()
        {
            //Cycle through every GameObject in _collidables
            for(int i  = 0; i < _collidables.Count; i++)
            {
                if (_collidables[i] is Entity)
                {
                    Entity e = (Entity)_collidables[i];
                    if (e.death)
                        continue;
                }

                for (int j = i + 1; j < _collidables.Count; j++)
                {
                    //Check if two objects collide
                    if (_collidables[i].boundingBox.Intersects(_collidables[j].boundingBox))
                    {
                        //Call the OnCollision functions of both objects.
                        _collidables[i].OnCollision(_collidables[j]);
                        _collidables[j].OnCollision(_collidables[i]);
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
            int xPos = (int)Math.Round(cam.Position.X / 2);
            int yPos = (int)Math.Round(cam.Position.Y / 2);

            batch.Draw(_backgroundTexture, new Rectangle(_backOffset + xPos - _backgroundTexture.Width / 2, yPos - _backgroundTexture.Height / 2, _backgroundTexture.Width, _backgroundTexture.Height), Color.White);

            if (cam.Position.X < _backOffset + xPos)
                _backOffset -= _backgroundTexture.Width;
            else
                _backOffset += _backgroundTexture.Width;

            batch.Draw(_backgroundTexture, new Rectangle(_backOffset + xPos - _backgroundTexture.Width / 2, yPos - _backgroundTexture.Height / 2, _backgroundTexture.Width, _backgroundTexture.Height), Color.White);

            //Draw every object
            foreach (GameObject go in objects)
            {
                //Call the draw method of gameobject
                go.DrawObject(batch);
            }
        }

        public void DrawHUD(SpriteBatch batch)
        {
            //Tell HUD to draw itself
            _hud.DrawHUD(batch);
        }
    }
}
