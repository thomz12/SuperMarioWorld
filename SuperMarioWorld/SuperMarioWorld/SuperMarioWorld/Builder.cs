using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioWorld
{
    public class Builder : Entity
    {
        /// <summary>The object to place (preview it too)</summary>
        private GameObject _objectToPlace;

        /// <summary>All objects the builder can place. Key is the item name, Gameobject is the object</summary>
        private Dictionary<string, GameObject> _placeableObjects;

        /// <summary>Selected item in the dictionary</summary>
        private int _selected;

        /// <summary>Size of the level</summary>
        private Point _size;

        /// <summary>Size of the grid in pixels</summary>
        private int _gridSize;

        /// <summary>List containing all objects in the level</summary>
        public List<GameObject> allObjects;

        /// <summary>The font to display selected block</summary>
        SpriteFont font;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="position">The builder position</param>
        /// <param name="levelSize">The size of the level</param>
        public Builder(Point position, Point levelSize, int gridSize) : base(position)
        {
            // Set layer
            sprite.layer = 0.9f;
            // Set the bounding box (no collision)
            boundingBox = new Rectangle(0, 0, 0, 0);

            // Set sprite values
            sprite.xSize = 16;
            sprite.ySize = 32;
            sprite.AddFrame(11, 1);
            sprite.sourceName = @"Players\Mario";

            // Set the max Horizontal and vertical speed
            terminalVelocity = 180;
            maxSpeed = 180;

            // Set level size
            _size = levelSize;

            // Set the grid size to match the game's grid size
            _gridSize = gridSize;

            // Load all the objects in the dictionary
            LoadObjects();
        }

        /// <summary>
        /// Loads all placeable objects in an dictionary
        /// </summary>
        private void LoadObjects()
        {
            //create a new instance of the dictionary to hold all the objects
            _placeableObjects = new Dictionary<string, GameObject>();

            Point pos = Point.Zero;

            //Add objects to placeable objects list
            _placeableObjects.Add("Used Block", new StaticBlock(pos, StaticBlock.BlockType.used, 0.3f));
            _placeableObjects.Add("Stone", new StaticBlock(pos, StaticBlock.BlockType.rock, 0.3f));
            _placeableObjects.Add("Cloud", new StaticBlock(pos, StaticBlock.BlockType.cloud, 0.3f));
            _placeableObjects.Add("Help", new StaticBlock(pos, StaticBlock.BlockType.help, 0.3f));
            _placeableObjects.Add("Grass Left", new StaticBlock(pos, StaticBlock.BlockType.grassLeft, 0.4f));
            _placeableObjects.Add("Grass", new StaticBlock(pos, StaticBlock.BlockType.grassMiddle, 0.4f));
            _placeableObjects.Add("Grass Right", new StaticBlock(pos, StaticBlock.BlockType.grassRight, 0.4f));
            _placeableObjects.Add("MysteryBlock Coin", new MysteryBlock(pos, new Coin(pos, false)));
            _placeableObjects.Add("MysteryBlock Mushroom", new MysteryBlock(pos, new Mushroom(pos)));
            _placeableObjects.Add("MysteryBlock OneUp", new MysteryBlock(pos, new OneUp(pos)));
            _placeableObjects.Add("Coin", new Coin(pos, false));
            _placeableObjects.Add("Red Koopa", new RedKoopa(pos));
            _placeableObjects.Add("Green Koopa", new GreenKoopa(pos));
            _placeableObjects.Add("Goomba", new Goomba(pos));
            _placeableObjects.Add("Finish", new Checkpoint(pos, true));
        }

        /// <summary>
        /// Load in font
        /// </summary>
        /// <param name="manager">the content manager</param>
        public void LoadContent(ContentManager manager)
        {
            font = manager.Load<SpriteFont>(@"Fonts\MainMenuFont");
        }

        /// <summary>
        /// When builder dies (spoiler: he won't!)
        /// </summary>
        /// <param name="cause">Object cause</param>
        public override void Death(GameObject cause)
        {
            
        }

        /// <summary>
        /// Update function (called every frame)
        /// </summary>
        /// <param name="gameTime">The game time</param>
        public override void Update(GameTime gameTime)
        {
            // Reset velocity
            velocity = Vector2.Zero;

            // Take input and move
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.W) || InputManager.Instance.GamePadAnalogLeftY() > 0.1f)
                velocity.Y = -terminalVelocity;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.S) || InputManager.Instance.GamePadAnalogLeftY() < -0.1f)
                velocity.Y = terminalVelocity;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.A) || InputManager.Instance.GamePadAnalogLeftX() < -0.1f)
                velocity.X = -maxSpeed;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.D) || InputManager.Instance.GamePadAnalogLeftX() > 0.1f)
                velocity.X = maxSpeed;

            // If F OR B is pressed, destory the objects on the selected position
            if(InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.F) || InputManager.Instance.GamePadIsPressed(Microsoft.Xna.Framework.Input.Buttons.B))
            {
                // For each object in the allObjects list
                for (int i = 0; i < allObjects.Count; i++)
                {
                    // Check if the distance between the object to place (in the builders hand) and the object in the scene is smaller than one grid square.
                    // The object must not be the builder
                    if (Vector2.Distance(_objectToPlace.position, allObjects[i].position) < 8 && !(allObjects[i] is Builder)) 
                        // Destroy object(s) if conditions are met
                        destroy(allObjects[i]);
                }
            }

            // Cycle the dictionary with Q/E or shoulder buttons
            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.Q) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.LeftShoulder))
                _selected--;
            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.E) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.RightShoulder))
                _selected++;

            // Make sure selected cant go out of range
            if (_selected < 0)
                _selected = _placeableObjects.Count - 1;
            if (_selected > _placeableObjects.Count - 1)
                _selected = 0;

            // Set the current object to place to the selected object
            _objectToPlace = _placeableObjects[_placeableObjects.Keys.ElementAt(_selected)];

            // Change the position of the object to place to match the Builders position, but with a snap to the grid
            _objectToPlace.position = new Vector2((float)Math.Round(position.X / _gridSize) * _gridSize + _gridSize, (float)Math.Round(position.Y / _gridSize) * _gridSize);

            //When player presses SPACE or A, place the block
            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.Space) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.A))
            {
                //Place object to place
                PlaceBlock();
            }
            else
            {
                //Create and destroy the object to place to load in its sprite
                if (_objectToPlace.sprite.texture == null)
                {
                    create(_objectToPlace);
                    destroy(_objectToPlace);
                }
            }

            //Apply the movement
            Movement(gameTime);
        }

        /// <summary>
        /// Place the selected block
        /// </summary>
        private void PlaceBlock()
        {
            // Destroy block on the position of the new block
            for (int i = 0; i < allObjects.Count; i++)
            {
                // Get the object that has been placed already on the position of the object to place
                if (Vector2.Distance(_objectToPlace.position, allObjects[i].position) < 8)
                {
                    // Check if the object is a StaticBlock for casting purposes
                    if (allObjects[i] is StaticBlock)
                    {
                        StaticBlock block = (StaticBlock)allObjects[i];
                        // Do not destroy dirt when placing a new block in front of it.
                        if (block.blockType != StaticBlock.BlockType.dirtMiddle && block.blockType != StaticBlock.BlockType.dirtRight && block.blockType != StaticBlock.BlockType.dirtLeft)
                            // If the object on the position is not a dirt block destroy it before placing a new block.
                            destroy(allObjects[i]);
                    }
                    else
                    {
                        // If the object on the position of the object to place is anything else than a StaticBlock remove it
                        destroy(allObjects[i]);
                    }
                }
            }

            // Create a variable pos for later use
            Point pos = new Point((int)_objectToPlace.position.X, (int)_objectToPlace.position.Y);

            // Place a staticblock
            if (_objectToPlace is StaticBlock)
            {
                // Cast to static block
                StaticBlock block = (StaticBlock)_objectToPlace;

                // Create a new instance of the static block
                create(new StaticBlock(pos, block.blockType, block.sprite.layer));

                // When a grass, add dirt from the grass to the bottom of the level
                if(block.blockType == StaticBlock.BlockType.grassLeft || block.blockType == StaticBlock.BlockType.grassMiddle || block.blockType == StaticBlock.BlockType.grassRight)
                {
                    // Make a variable for type
                    StaticBlock.BlockType type;

                    // Set the type variable to the type of dirt corresponding to the type of grass block that has been placed
                    if (block.blockType == StaticBlock.BlockType.grassLeft)
                        type = StaticBlock.BlockType.dirtLeft;
                    else if (block.blockType == StaticBlock.BlockType.grassMiddle)
                        type = StaticBlock.BlockType.dirtMiddle;
                    else
                        type = StaticBlock.BlockType.dirtRight;

                    // Cycle through a loop untill the end of the level is reached
                    for (int i = (int)((block.position.Y ) / _gridSize) + 1; i <= _size.Y; i++)
                    {
                        // Create a new dirt block on the position and set its layer to be lower the higher the parent (grass block) is placed in the level.
                        create(new StaticBlock(new Point((int)block.position.X, i * _gridSize), type, 0.1f + (block.position.Y / _gridSize / 100)));
                    }
                }
            }
            // Place mysteryblock
            else if (_objectToPlace is MysteryBlock)
            {
                MysteryBlock block = (MysteryBlock)_objectToPlace;
                create(new MysteryBlock(pos, block.content));
            }
            // Place coin
            else if (_objectToPlace is Coin)
                create(new Coin(pos, false));
            // Place red koopa
            else if (_objectToPlace is RedKoopa)
                create(new RedKoopa(pos));
            // Place green koopa
            else if (_objectToPlace is GreenKoopa)
                create(new GreenKoopa(pos));
            // Place goomba
            else if (_objectToPlace is Goomba)
                create(new Goomba(pos));
            // Place checkpoint
            else if (_objectToPlace is Checkpoint)
                create(new Checkpoint(pos, true));
        }

        /// <summary>
        /// Draw this object and preview block
        /// </summary>
        /// <param name="batch"></param>
        public override void DrawObject(SpriteBatch batch)
        {
            // Draw the builder sprite
            base.DrawObject(batch);

            // If there is a preview block, draw it
            if(_objectToPlace != null)
                _objectToPlace.DrawObject(batch);

            // Draw the name of the block as a string
            batch.DrawString(font, _placeableObjects.ElementAt(_selected).Key, new Vector2(position.X, position.Y + 8), Color.Black, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 1);
        }

        /// <summary>
        /// Movement function
        /// </summary>
        /// <param name="gameTime">The gametime</param>
        protected override void Movement(GameTime gameTime)
        {
            // Limit velocity
            if (velocity.X > maxSpeed)
                velocity = new Vector2(maxSpeed, velocity.Y);
            if (velocity.X < -maxSpeed)
                velocity = new Vector2(-maxSpeed, velocity.Y);

            if (velocity.Y > terminalVelocity)
                velocity.Y = terminalVelocity;
            if (velocity.Y < -terminalVelocity)
                velocity.Y = -terminalVelocity;

            // Add momentum to position
            position += velocity * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }

        // The builder does not interact with anything.
        public override void OnCollision(GameObject collider)
        {
            return;
        }
    }
}
