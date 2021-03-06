﻿using System;
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
    public class Scene
    {
        // Content manager
        private ContentManager _contentManager;

        // HUD
        private HUD _hud;

        // Score handler
        private ScoreHandler _scores;

        // Size of the level.
        private Point _size;

        // Size of the grid
        private int _gridSize;

        // Camera object
        public Camera2D cam;
        // Player object
        private GameObject _player;

        // Is the level in edit mode?
        private bool _edit;
        private string _name;

        /// <summary>
        /// List of all the GameObjects in the current level, loaded from a .sml file.
        /// </summary>
        public List<GameObject> objects = new List<GameObject>();

        // Dictionary of all the sprites that the gameobjects use, so they dont have to be loaded in multiple times
        private Dictionary<string, Texture2D> loadedSprites = new Dictionary<string, Texture2D>();

        // Variables for the background
        private string _backgroundSourceName;
        private Texture2D _backgroundTexture;

        // Position of the background texture
        private int _backOffset;

        // This list contains all the gameobjects in the current scene
        private List<GameObject> _collidables = new List<GameObject>();

        /// <summary>
        /// Constructs the level from a chosen file
        /// </summary>
        /// <param name="fileName">Give the name of the file without extension</param>
        public Scene(string fileName, ScoreHandler scoreHandler, bool edit)
        { 
            // Set score handler
            _scores = scoreHandler;
            _scores.maxTime = 200;

            // Set if the scene is in edit mode
            _edit = edit;

            // Set background texture
            _backgroundSourceName = @"Background\BushBackground";

            // Set the gridsize
            _gridSize = 16;
            _backOffset = 0;

            // Add file path to lvl name
            fileName = @"Content\Levels\" + fileName;
            if (!fileName.Contains(".sml"))
                fileName += ".sml";

            _name = fileName;

            //Create all objects and add them to the objects array
            try
            {
                //Create new xml document (to read XML)
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(fileName);

                //get root node
                XmlNode root = xmlDoc.ChildNodes[1];
                _size.X = int.Parse(root.Attributes[0].Value);
                _size.Y = int.Parse(root.Attributes[1].Value);

                //Gameobject to add
                GameObject obj = null;
                //At position
                Point pos = new Point();

                //Go through all elements
                foreach (XmlNode node in root.ChildNodes)
                {
                    //Get the position first
                    pos.X = int.Parse(node.LastChild.Attributes["xPos"].Value);
                    pos.Y = int.Parse(node.LastChild.Attributes["yPos"].Value);

                    //Check element name, and create object corresponding to that name
                    if (node.Name.Equals("MainMenu"))
                    {
                        obj = new MainMenu(pos, _contentManager);
                        MainMenu mm = (MainMenu)obj;                  
                    }
                    else if (node.Name.Equals("Player"))
                    {
                        // Check if the game is not in edit mode
                        if (!edit)
                        {
                            // Generate a random number for a random player
                            Random r = new Random();
                            // Create a random player
                            obj = new Player(pos, _scores, (Player.Character)r.Next(0, Enum.GetNames(typeof(Player.Character)).Length));
                            _player = (Player)obj;
                        }
                        else
                        {
                            // Create a builder if the game is in edit mode
                            obj = new Builder(pos, _size, _gridSize);
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
                        // Create new gameobject
                        GameObject content;

                        // Get the node for content
                        if (node.FirstChild.Attributes[0].Value.Equals("Mushroom"))
                            content = new Mushroom(Point.Zero);
                        else if (node.FirstChild.Attributes[0].Value.Equals("OneUp"))
                            content = new OneUp(Point.Zero);
                        else
                            content = new Coin(Point.Zero, true);

                        // Create the mystery block with correct content
                        obj = new MysteryBlock(pos, content);
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
                    else if(node.Name.Equals("Checkpoint"))
                    {
                        obj = new Checkpoint(pos, bool.Parse(node.FirstChild.InnerText));
                    }

                    if (obj != null)
                    {
                        // Add the delecates to the gameobjects
                        obj.create = new GameObject.CreateObject(CreateObject);
                        obj.destroy = new GameObject.DestroyObject(DestroyObject);
                        // Add the object to the objects array
                        objects.Add(obj);
                    }
                }

                // Create HUD object when a player is present
                if (_player is Player)
                {
                    Player player = (Player)_player;
                    _hud = new HUD(_scores, player);
                }
                else
                {
                    // Create an hud without a player
                    _hud = new HUD(_scores, null);
                }

                // Create camera object
                cam = new Camera2D(_player, _size, _gridSize);
            }
            //something went wrong, we have to do something here
            catch(Exception e)
            {
                // Go to Thom for help (cuz he needs to fix his shit then)
                throw e;
            }
        }

        /// <summary>
        /// Load sprites of the content.
        /// </summary>
        /// <param name="contentManager"></param>
        public void LoadContent(ContentManager contentManager)
        {
            // Set a content manager so new items can be spawned in.
            _contentManager = contentManager;

            // Load all the assets that belong to level
            _backgroundTexture = contentManager.Load<Texture2D>(_backgroundSourceName);

            // Load all the content for each gameobject
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
                if(go is Builder)
                {
                    Builder builder = (Builder)go;
                    builder.LoadContent(contentManager);
                }

                go.sprite.texture = loadedSprites[go.sprite.sourceName];
            }

            // Tell HUD to load its content too
            _hud.LoadContent(contentManager);
        }

        /// <summary>
        /// Creates an GameObject
        /// </summary>
        /// <param name="gameObject">GameObject that should be created</param>
        public void CreateObject(GameObject gameObject)
        {
            // Create a temp object
            GameObject obj = gameObject;

            // Load in the sprites (if nesecarry)
            if (loadedSprites.ContainsKey(obj.sprite.sourceName))
                obj.sprite.texture = loadedSprites[obj.sprite.sourceName];
            else
               obj.sprite.texture = _contentManager.Load<Texture2D>(obj.sprite.sourceName);

            // Add a create and destroy function
            obj.create = new GameObject.CreateObject(CreateObject);
            obj.destroy = new GameObject.DestroyObject(DestroyObject);

            // Add the object to the objects list
            objects.Add(obj);
        }

        /// <summary>
        /// Destorys an GameObject
        /// </summary>
        /// <param name="gameObject">Destroy this GameObject</param>
        public void DestroyObject(GameObject gameObject)
        {
            // Remove the object
            objects.Remove(gameObject);
        }

        /// <summary>
        /// Call update for every object in the level, check collisions and call oncollisions
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Get the current camera position 
            int camX = (int)cam.position.X;
            int camY = (int)cam.position.Y;

            // Clear the array with objects that should have their collision checked
            _collidables.Clear();

            // Give Builder object all the level information
            if (_player is Builder)
            {
                Builder builder = (Builder)_player;
                builder.allObjects = objects;
            }

            // Kill the player when it falls out of the scene
            if(_player is Player)
            {
                // Cast player
                Player player = (Player)_player;
                // Check if player is already dead
                if (player.dead)
                {
                    // Check if the player is outside of the map, then load the main menu
                    if (player.position.Y >= _size.Y * _gridSize + 30)
                        SceneManager.Instance.LoadMainMenu();
                }
                else if(player.position.Y >= _size.Y * _gridSize + 20)
                {
                    // Kill the player instantly
                    player.powerState = Player.PowerState.small;
                    player.Death(null);
                }
            }

            // Call the update method for all gameobjects
            for (int i = 0; i < objects.Count; i++)
            {
                // Check if the game object is within the screen
                if (Math.Abs(camX - objects[i].position.X) < cam.gameWidth)
                {
                    // Add the objects that are in the frame to the collision list to be checked for collisions
                    if(objects[i] is StaticBlock)
                        _collidables.Add(objects[i]);
                    else if (Math.Abs(camX - objects[i].position.X) < cam.gameWidth / 1.5f)
                    {
                        if (_edit && objects[i] is Enemy)
                            continue;

                        objects[i].Update(gameTime);

                        _collidables.Add(objects[i]);
                    }

                    // Check if the object has fallen out of the map (downwards)
                    if(objects[i].position.Y > _size.Y * _gridSize + 50)
                    {
                        // Destroy the object if it is present in the objects array.
                        if(objects[i] != null)
                        {
                            objects[i].destroy(objects[i]);
                        }
                    }
                }
            }

            // Check collisions
            CheckCollisions();

            // Update the camera position
            cam.Update(gameTime);

            // Tell HUD to update
            _hud.Update(gameTime);

            // Save level when <enter>  is pressed
            if (_edit && ( InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.Enter) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.Start)))
                SaveLevel();
        }

        /// <summary>
        /// Check if something collides in the camera area and call the two colliding object's OnCollision functions
        /// </summary>
        public void CheckCollisions()
        {
            // Cycle through every GameObject in _collidables
            for(int i  = 0; i < _collidables.Count; i++)
            {
                // Cycle through every gameobject in _collidables that comes after the first object (optimalisation)
                for (int j = i + 1; j < _collidables.Count; j++)
                {
                    // Check if two objects collide
                    if (_collidables[i].boundingBox.Intersects(_collidables[j].boundingBox))
                    {
                        // Call the OnCollision functions of both objects.
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
            // Draw background(s)
            int xPos = (int)Math.Round(cam.position.X / 2);
            int yPos = (int)Math.Round(cam.position.Y / 2);

            // Draw the first background with parallax scrolling
            batch.Draw(_backgroundTexture, new Rectangle(_backOffset + xPos - _backgroundTexture.Width / 2, yPos - _backgroundTexture.Height / 2, _backgroundTexture.Width, _backgroundTexture.Height), Color.White);

            if (cam.position.X < _backOffset + xPos)
                _backOffset -= _backgroundTexture.Width;
            else
                _backOffset += _backgroundTexture.Width;

            // Draw the second background with parallax scrolling
            batch.Draw(_backgroundTexture, new Rectangle(_backOffset + xPos - _backgroundTexture.Width / 2, yPos - _backgroundTexture.Height / 2, _backgroundTexture.Width, _backgroundTexture.Height), Color.White);

            // Draw every object
            foreach (GameObject go in objects)
            {
                // Call the draw method of gameobject
                go.DrawObject(batch);
            }
        }

        /// <summary>
        /// Draw the hud
        /// </summary>
        /// <param name="batch"></param>
        public void DrawHUD(SpriteBatch batch)
        {
            // Tell HUD to draw itself
            _hud.DrawHUD(batch);
        }

        /// <summary>
        /// Save the level to a .sml file (xml format)
        /// </summary>
        public void SaveLevel()
        {
            // Create a new XML Writer to write in a document
            XmlWriter writer = XmlWriter.Create(_name);

            // Start with writing in a document
            writer.WriteStartDocument();

            // Create a new element
            writer.WriteStartElement("Level");

            // Give the new level an attribute xSize and ySize
            writer.WriteAttributeString("xSize", _size.X.ToString());
            writer.WriteAttributeString("ySize", _size.Y.ToString());

            // Add all the objects in the level to the xml file
            foreach(GameObject obj in objects)
            {
                // Call function that adds a gameobject to xml
                AddGameObjectXML(writer, obj);
            }

            // End
            writer.WriteEndElement();
            // End
            writer.WriteEndDocument();
            // End
            writer.Close();
        }

        /// <summary>
        /// Add object to XML file
        /// </summary>
        /// <param name="writer">the xmlWriter</param>
        /// <param name="obj">the object to add</param>
        private void AddGameObjectXML(XmlWriter writer, GameObject obj)
        { 
            // Checks which block is currently being writtten and writes it on the file
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
            else if (obj is Checkpoint)
            {
                Checkpoint c = (Checkpoint)obj;
                writer.WriteStartElement(obj.GetType().Name);
                writer.WriteStartElement("Finish");
                if (c.isFinish)
                    writer.WriteString("true");
                else
                    writer.WriteString("false");
                writer.WriteEndElement();
            }
            else if(obj is Builder)
            {
                writer.WriteStartElement("Player");
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
