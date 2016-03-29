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
        private GameObject _objectToPlace;    
        private Dictionary<string, GameObject> _placeableObjects;
        private int _selected;

        private Point _size;
        public List<GameObject> allObjects;
        SpriteFont font;

        public Builder(Point position, Point levelSize) : base(position)
        {
            sprite.layer = 0.9f;
            boundingBox = new Rectangle(0, 0, 0, 0);

            sprite.xSize = 16;
            sprite.ySize = 32;
            sprite.AddFrame(11, 1);
            sprite.sourceName = @"Players\Mario";

            acceleration = 500.0f;
            terminalVelocity = 180;
            maxSpeed = 180;

            _size = levelSize;

            LoadObjects();
        }

        private void LoadObjects()
        {
            _placeableObjects = new Dictionary<string, GameObject>();

            Point pos = Point.Zero;

            //Add objects to placeable objects list
            _placeableObjects.Add("Used Block", new StaticBlock(pos, StaticBlock.BlockType.used, 0.2f));
            _placeableObjects.Add("Stone", new StaticBlock(pos, StaticBlock.BlockType.rock, 0.2f));
            _placeableObjects.Add("Cloud", new StaticBlock(pos, StaticBlock.BlockType.cloud, 0.2f));
            _placeableObjects.Add("Help", new StaticBlock(pos, StaticBlock.BlockType.help, 0.2f));
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
        /// <param name="manager"></param>
        public void LoadContent(ContentManager manager)
        {
            font = manager.Load<SpriteFont>(@"Fonts\MainMenuFont");
        }

        /// <summary>
        /// When builder dies (spoiler: he doesn't!)
        /// </summary>
        /// <param name="cause"></param>
        public override void Death(GameObject cause)
        {
            
        }

        /// <summary>
        /// Update function (called every frame)
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            momentum = Vector2.Zero;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.W) || InputManager.Instance.GamePadAnalogLeftY() > 0.1f)
                momentum.Y = -terminalVelocity;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.S) || InputManager.Instance.GamePadAnalogLeftY() < -0.1f)
                momentum.Y = terminalVelocity;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.A) || InputManager.Instance.GamePadAnalogLeftX() < -0.1f)
                momentum.X = -maxSpeed;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.D) || InputManager.Instance.GamePadAnalogLeftX() > 0.1f)
                momentum.X = maxSpeed;

            if(InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.F) || InputManager.Instance.GamePadIsPressed(Microsoft.Xna.Framework.Input.Buttons.B))
            {
                for (int i = 0; i < allObjects.Count; i++)
                {
                    if (Vector2.Distance(_objectToPlace.position, allObjects[i].position) < 8 && !(allObjects[i] is Builder)) 
                        destroy(allObjects[i]);
                }
            }

            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.Q) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.LeftShoulder))
                _selected--;
            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.E) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.RightShoulder))
                _selected++;

            if (_selected < 0)
                _selected = _placeableObjects.Count - 1;
            if (_selected > _placeableObjects.Count - 1)
                _selected = 0;

            _objectToPlace = _placeableObjects[_placeableObjects.Keys.ElementAt(_selected)];

            _objectToPlace.position = new Vector2((float)Math.Round(position.X / 16f) * 16 + 16, (float)Math.Round(position.Y / 16) * 16f);

            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.Space) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.A))
            {
                PlaceBlock();
            }
            else
            {
                create(_objectToPlace);
                destroy(_objectToPlace);
            }
            Movement(gameTime);
        }

        /// <summary>
        /// Place the selected block
        /// </summary>
        private void PlaceBlock()
        {
            for (int i = 0; i < allObjects.Count; i++)
            {
                if (Vector2.Distance(_objectToPlace.position, allObjects[i].position) < 8)
                    if (allObjects[i] is StaticBlock)
                    {
                        StaticBlock block = (StaticBlock)allObjects[i];
                        if(block.blockType != StaticBlock.BlockType.dirtMiddle && block.blockType != StaticBlock.BlockType.dirtRight && block.blockType != StaticBlock.BlockType.dirtLeft)
                            destroy(allObjects[i]);
                    }
            }

            Point pos = new Point((int)_objectToPlace.position.X, (int)_objectToPlace.position.Y);

            if (_objectToPlace is StaticBlock)
            {
                StaticBlock block = (StaticBlock)_objectToPlace;
                create(new StaticBlock(pos, block.blockType, block.sprite.layer));

                if(block.blockType == StaticBlock.BlockType.grassLeft || block.blockType == StaticBlock.BlockType.grassMiddle || block.blockType == StaticBlock.BlockType.grassRight)
                {
                    StaticBlock.BlockType type;
                    if (block.blockType == StaticBlock.BlockType.grassLeft)
                        type = StaticBlock.BlockType.dirtLeft;
                    else if (block.blockType == StaticBlock.BlockType.grassMiddle)
                        type = StaticBlock.BlockType.dirtMiddle;
                    else
                        type = StaticBlock.BlockType.dirtRight;

                    for (int i = (int)((block.position.Y ) / 16) + 1; i <= _size.Y; i++)
                    {
                        create(new StaticBlock(new Point((int)block.position.X, i * 16), type, 0.1f + (block.position.Y / 16 / 100)));
                    }
                }
            }
            else if (_objectToPlace is MysteryBlock)
            {
                MysteryBlock block = (MysteryBlock)_objectToPlace;
                create(new MysteryBlock(pos, block.content));
            }
            else if (_objectToPlace is Coin)
                create(new Coin(pos, false));
            else if (_objectToPlace is RedKoopa)
                create(new RedKoopa(pos));
            else if (_objectToPlace is GreenKoopa)
                create(new GreenKoopa(pos));
            else if (_objectToPlace is Goomba)
                create(new Goomba(pos));
            else if (_objectToPlace is Checkpoint)
                create(new Checkpoint(pos, true));
        }

        public override void DrawObject(SpriteBatch batch)
        {
            base.DrawObject(batch);

            if(_objectToPlace != null)
                _objectToPlace.DrawObject(batch);

            batch.DrawString(font, _placeableObjects.ElementAt(_selected).Key, new Vector2(position.X, position.Y + 8), Color.Black, 0, Vector2.Zero, 0.1f, SpriteEffects.None, 1);
        }

        protected override void Movement(GameTime gameTime)
        {
            if (momentum.X > maxSpeed)
                momentum = new Vector2(maxSpeed, momentum.Y);
            if (momentum.X < -maxSpeed)
                momentum = new Vector2(-maxSpeed, momentum.Y);

            if (momentum.Y > terminalVelocity)
                momentum.Y = terminalVelocity;
            if (momentum.Y < -terminalVelocity)
                momentum.Y = -terminalVelocity;

            //add momentum to position
            position += momentum * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }

    }
}
