using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioWorld
{
    delegate void LoadScene(string level);

    class Scene
    {
        //Content manager
        private ContentManager _contentManager;

        //HUD
        private HUD _hud;

        //Score handler
        private ScoreHandler _scores;

        //Size of the level.
        private Point _size;
        private string _levelName;

        //Size of the grid
        private int _gridSize;

        //Camera object
        public Camera2D cam;
        //player object
        private GameObject _player;

        /// <summary>
        /// List of all the GameObjects in the current level, loaded from a .sml file.
        /// </summary>
        public List<GameObject> objects = new List<GameObject>();

        //Dictionary of all the sprites that the gameobjects use, so they dont have to be loaded in multiple times
        private Dictionary<string, Texture2D> loadedSprites = new Dictionary<string, Texture2D>();

        public LoadScene load;

        //Variables for the background
        private string _backgroundSourceName;
        private Texture2D _backgroundTexture;

        //Position of the background texture
        private int _backOffset;

        private List<GameObject> _collidables = new List<GameObject>();

        /// <summary>
        /// Constructs the level from a chosen file
        /// </summary>
        /// <param name="fileName">Give the name of the file without extension</param>
        public Scene(string fileName, ScoreHandler scoreHandler, LoadScene loadScene, bool edit)
        {
            #region oldStuff
            /*
            //Create the scorehandler so the level file can put information in it.
            _scores = scoreHandler;

            _backgroundSourceName = @"Background\BushBackground";

            //Set the gridsize
            _gridSize = 16;

            _backOffset = 0;
            
            //Create a streamreader
            StreamReader sr;

            fileName = @"Content\Levels\" + fileName;
            
            //LOAD LEVEL FROM FILE
            /*
            Appearantly loading the level from a file like this prevents us from generating contents in f.ex Mystery boxes or changing the way some objects behave
            We need to generate an XML file from the level maker to make this work.

            So basically all this code is useless. but it provides us with a working project for the demo.
            */
            /*
            try
            {
                sr = new StreamReader(@"Content\Levels\Edit.sml");
            }
            catch (Exception e)
            {
                throw new FileLoadException("Could not load file " + fileName, e);
            }
            
            //Read information about the level from the file
            //Read level name
            _levelName = sr.ReadLine().Split('\"')[1].Split('\"')[0];
            //Read x size of level
            string xSize = sr.ReadLine();
            //Read y size of level
            string ySize = sr.ReadLine();
            
            //Convert x and y size to Point _size
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
                    GameObject obj = null;

                    //Checks the file for a character and creates the corresponding object
                    #region CreateObjects
                    //Convert the line into objects on correct positions
                    if (objectChars[x].Equals('M')) //If the char represents a MysteryBlock
                    {
                        obj = new MysteryBlock(new Point((x * _gridSize), (y * _gridSize)), new Mushroom(Point.Zero));
                    }
                    else if (objectChars[x].Equals('1')) //If the char represents a Player
                    {
                        Random r = new Random();
                        obj = new Player(new Point((x * _gridSize), (y * _gridSize)), _scores, (Player.Character)r.Next(0, Enum.GetNames(typeof(Player.Character)).Length));
                        _player = (Player)obj;
                    }
                    else if (objectChars[x].Equals('G')) //If the char represents a Goomba
                    {
                        obj = new Goomba(new Point((x * _gridSize), (y * _gridSize)));
                    }
                    else if (objectChars[x].Equals('C')) //If the char represents a coin which moves
                    {
                        obj = new Coin(new Point((x * _gridSize), (y * _gridSize)), true);
                    }
                    else if (objectChars[x].Equals('c')) //If the char represents a coin which doesnt move
                    {
                        obj = new Coin(new Point((x * _gridSize), (y * _gridSize)), false);
                    }
                    else if(objectChars[x].Equals('R')) //If the char represents a Grass
                    {
                        StaticBlock grass = new StaticBlock(new Point((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.grassMiddle, 0.4f);
                        obj = grass;
                        for (int i = (int)((grass.position.Y + 1) / _gridSize); i < _size.Y; i++)
                        {
                            objects.Add(new StaticBlock(new Point((int)grass.position.X, (i + 1) * _gridSize), StaticBlock.BlockType.dirtMiddle, 0.1f + (float)y * (float) _gridSize / 1000f));
                        }
                    }
                    else if (objectChars[x].Equals('>')) //If the char represents a Grass right block
                    {
                        StaticBlock grass = new StaticBlock(new Point((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.grassRight, 0.4f);
                        obj = grass;
                        for (int i = (int)((grass.position.Y + 1) / _gridSize); i < _size.Y; i++)
                        {
                            objects.Add(new StaticBlock(new Point((int)grass.position.X, (i + 1) * _gridSize), StaticBlock.BlockType.dirtRight, 0.1f + (float)y * (float)_gridSize / 1000f));
                        }
                    }
                    else if (objectChars[x].Equals('<')) //If the char represents a Grass left block
                    {
                        StaticBlock grass = new StaticBlock(new Point((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.grassLeft, 0.4f);
                        obj = grass;
                        for (int i = (int)((grass.position.Y + 1) / _gridSize); i < _size.Y; i++)
                        {
                            objects.Add(new StaticBlock(new Point((int)grass.position.X, (i + 1) * _gridSize), StaticBlock.BlockType.dirtLeft, 0.1f + (float)y * (float)_gridSize / 1000f));
                        }
                    }
                    else if (objectChars[x].Equals('K')) //If the char represents a Koopa
                    {
                        obj = new GreenKoopa(new Point((x * _gridSize), (y * _gridSize)));
                    }
                    else if (objectChars[x].Equals('S')) //If the char represents a Shell
                    {
                        obj = new EmptyShell(new Point((x * _gridSize), (y * _gridSize)), EmptyShell.KoopaType.green);
                    }
                    else if (objectChars[x].Equals('L')) //If the char represents a cloud
                    {
                        obj = new StaticBlock(new Point((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.cloud, 0.4f);
                    }
                    else if (objectChars[x].Equals('O')) //If the char represents a rock
                    {
                        obj = new StaticBlock(new Point((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.rock, 0.4f);
                    }
                    else if (objectChars[x].Equals('H')) //If the char represents a help
                    {
                        obj = new StaticBlock(new Point((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.help, 0.4f);
                    }
                    else if (objectChars[x].Equals('U')) //If the char represents a used block (brown one)
                    {
                        obj = new StaticBlock(new Point((x * _gridSize), (y * _gridSize)), StaticBlock.BlockType.used, 0.4f);
                    }
                    else if (objectChars[x].Equals('Y')) //If the char represents a mushroom
                    {
                        obj = new Mushroom(new Point((x * _gridSize), (y * _gridSize)));
                    }
                    else if (objectChars[x].Equals('P')) //If the char represents a one up
                    {
                        obj = new OneUp(new Point((x * _gridSize), (y * _gridSize)));
                    }
                    else if (objectChars[x].Equals('r')) //If the char represents a red koopa
                    {
                        obj = new RedKoopa(new Point(x * _gridSize, y * _gridSize));
                    }
                    else if(objectChars[x].Equals('f')) //If the char represents a checkpoint
                    {
                        obj = new Checkpoint(new Point(x * _gridSize, y * _gridSize));
                    }
                    else if (objectChars[x].Equals('+')) //If the char represents a menu
                    {
                        obj = new MainMenu(new Point(0, 0), _contentManager);
                    }
                    #endregion

                    if (obj != null)
                    {
                        obj.create = new CreateObject(CreateObject);
                        obj.destory = new DestoryObject(DestroyObject);
                        objects.Add(obj);
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
            if (_player != null)
            {
                cam = new Camera2D(_player, _size, _gridSize);
            }
            else
            {
                cam = new Camera2D(null, _size, _gridSize);
            }*/
            #endregion
            
            #region new stuff
            //Set score handler
            _scores = scoreHandler;

            load = loadScene;

            //Set background texture
            _backgroundSourceName = @"Background\BushBackground";

            //Set the gridsize
            _gridSize = 16;
            _backOffset = 0;

            fileName = @"Content\Levels\" + fileName;
            if (!fileName.Contains(".sml"))
                fileName += ".sml";

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                XmlNode root = xmlDoc.ChildNodes[1];
                _size.X = int.Parse(root.Attributes[0].Value);
                _size.Y = int.Parse(root.Attributes[1].Value);

                GameObject obj = null;
                Point pos = new Point();

                foreach (XmlNode node in root.ChildNodes)
                {
                    pos.X = int.Parse(node.LastChild.Attributes["xPos"].Value);
                    pos.Y = int.Parse(node.LastChild.Attributes["yPos"].Value);

                    if (node.Name.Equals("MainMenu"))
                    {
                        obj = new MainMenu(pos, _contentManager);
                        MainMenu mm = (MainMenu)obj;
                        mm.load = loadScene;
                    }
                    else if (node.Name.Equals("Player"))
                    {
                        if (!edit)
                        {
                            Random r = new Random();
                            obj = new Player(pos, _scores, (Player.Character)r.Next(0, Enum.GetNames(typeof(Player.Character)).Length));
                            _player = (Player)obj;
                        }
                        else
                        {
                            obj = new Builder(pos);
                            _player = obj;
                        }
                    }
                    else if (node.Name.Equals("Goomba"))
                    {
                        obj = new Goomba(pos);
                    }
                    else if (node.Name.Equals("GreenKoopa"))
                    {
                        obj = new GreenKoopa(pos);
                    }
                    else if (node.Name.Equals("RedKoopa"))
                    {
                        obj = new RedKoopa(pos);
                    }
                    else if (node.Name.Equals("MysteryBlock"))
                    {
                        obj = new MysteryBlock(pos, null);
                    }
                    else if (node.Name.Equals("StaticBlock"))
                    {
                        obj = new StaticBlock(pos, (StaticBlock.BlockType)Enum.Parse(typeof(StaticBlock.BlockType), node.FirstChild.InnerText, true), float.Parse(node.Attributes["layer"].Value));
                    }
                    else if (node.Name.Equals("Coin"))
                    {
                        obj = new Coin(pos, false);
                    }
                    else if (node.Name.Equals("EmptyShell"))
                    {
                        obj = new EmptyShell(pos, (EmptyShell.KoopaType)Enum.Parse(typeof(EmptyShell.KoopaType), node.FirstChild.InnerText, true));
                    }
                    else if (node.Name.Equals("Mushroom"))
                    {
                        obj = new Mushroom(pos);
                    }
                    else if (node.Name.Equals("OneUp"))
                    {
                        obj = new OneUp(pos);
                    }

                    if (obj != null)
                    {
                        obj.create = new CreateObject(CreateObject);
                        obj.destory = new DestoryObject(DestroyObject);
                        objects.Add(obj);
                    }
                }

                //Create HUD object
                _hud = new HUD(_scores);

                //Create camera object
                if (_player != null)
                {
                    cam = new Camera2D(_player, _size, _gridSize);
                }
                else
                {
                    cam = new Camera2D(null, _size, _gridSize);
                }
            }
            //something went wrong, we have to do something here
            catch(Exception e)
            {
                throw e;
            }
            #endregion
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

                if(go is MainMenu)
                {
                    MainMenu mainMenu = (MainMenu)go;
                    mainMenu.LoadContent(contentManager);
                }

                go.sprite.texture = loadedSprites[go.sprite.sourceName];
            }

            //Tell HUD to load its content too
            _hud.LoadContent(contentManager);
        }

        /// <summary>
        /// Creates an GameObject
        /// </summary>
        /// <param name="gameObject">GameObject that should be created</param>
        public void CreateObject(GameObject gameObject)
        {
            //Create a temp object
            GameObject obj = gameObject;

            //load in the sprites (if nesecarry)
            if (loadedSprites.ContainsKey(obj.sprite.sourceName))
                obj.sprite.texture = loadedSprites[obj.sprite.sourceName];
            else
               obj.sprite.texture = _contentManager.Load<Texture2D>(obj.sprite.sourceName);

            //Add a create and destroy function
            obj.create = new CreateObject(CreateObject);
            obj.destory = new DestoryObject(DestroyObject);

            objects.Add(obj);
        }

        /// <summary>
        /// Destorys an GameObject
        /// </summary>
        /// <param name="gameObject">Destroy this GameObject</param>
        public void DestroyObject(GameObject gameObject)
        {
            objects.Remove(gameObject);
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
                //Check if the game object is within the screen
                if (Math.Abs(camX - objects[i].position.X) < cam.GameWidth)
                {
                    //Add the objects that are in the frame to the collision list to be checked for collisions
                    if(objects[i] is StaticBlock)
                        _collidables.Add(objects[i]);
                    if (Math.Abs(camX - objects[i].position.X) < cam.GameWidth / 1.5f)
                    {
                        objects[i].Update(gameTime);
                        _collidables.Add(objects[i]);
                    }

                    //Check if the object has fallen out of the map (downwards)
                    if(objects[i].position.Y > _size.Y * _gridSize + 50)
                    {
                        //Destroy the object if it is still alive.
                        if(objects[i] != null)
                        {
                            objects[i].destory(objects[i]);
                        }
                    }
                }
            }
        
            //Check collisions
            CheckCollisions();

            //Update the camera position
            cam.Update(gameTime);

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

        /// <summary>
        /// Draw the hud
        /// </summary>
        /// <param name="batch"></param>
        public void DrawHUD(SpriteBatch batch)
        {
            //Tell HUD to draw itself
            _hud.DrawHUD(batch);
        }

        /// <summary>
        /// Save the level to a .sml file (xml format)
        /// </summary>
        public void SaveLevel()
        {
            XmlWriter writer = XmlWriter.Create(@"Content\Levels\Template.sml");
            writer.WriteStartDocument();

            writer.WriteStartElement("Level");
            writer.WriteAttributeString("xSize", _size.X.ToString());
            writer.WriteAttributeString("ySize", _size.Y.ToString());

            foreach(GameObject obj in objects)
            {
                AddGameObjectXML(writer, obj);
            }

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }

        /// <summary>
        /// Add object to XML file
        /// </summary>
        /// <param name="writer">the xmlWriter</param>
        /// <param name="obj">the object to add</param>
        private void AddGameObjectXML(XmlWriter writer, GameObject obj)
        {
            if (obj is MysteryBlock)
            {
                writer.WriteStartElement(obj.GetType().Name);

                MysteryBlock block = (MysteryBlock)obj;

                writer.WriteStartElement("Content");

                writer.WriteAttributeString("gameobject", block.content.GetType().Name);

                writer.WriteEndElement();
            }
            else if (obj is StaticBlock)
            {
                StaticBlock block = (StaticBlock)obj;

                writer.WriteStartElement(obj.GetType().Name);
                writer.WriteAttributeString("layer", obj.sprite.layer.ToString());
                writer.WriteStartElement("BlockType");
                writer.WriteString(block.blockType.ToString());
                writer.WriteEndElement();
            }
            else if (obj is EmptyShell)
            {
                EmptyShell t = (EmptyShell)obj;
                writer.WriteStartElement(obj.GetType().Name);
                writer.WriteStartElement("Type");
                writer.WriteString(t.koopaType.ToString());
                writer.WriteEndElement();
            }
            else
                writer.WriteStartElement(obj.GetType().Name);

            writer.WriteStartElement("Position");
            writer.WriteAttributeString("xPos", ((int)Math.Round(obj.position.X)).ToString());
            writer.WriteAttributeString("yPos", ((int)Math.Round(obj.position.Y)).ToString());
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
    }
}
