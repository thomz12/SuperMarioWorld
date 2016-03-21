using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioWorld
{
    class Builder : Entity
    {
        private GameObject _objectToPlace;    
        private Dictionary<string, GameObject> _placeableObjects;
        private int selected;

        public Builder(Point position) : base(position)
        {
            sprite.layer = 0.9f;
            boundingBox = new Rectangle(0, 0, 0, 0);

            sprite.xSize = 16;
            sprite.ySize = 32;
            sprite.AddFrame(0, 0);
            sprite.sourceName = @"Players\Mario";

            acceleration = 500.0f;
            terminalVelocity = 180;
            maxSpeed = 180;

            LoadObjects();
        }

        private void LoadObjects()
        {
            _placeableObjects = new Dictionary<string, GameObject>();

            Point pos = Point.Zero;

            _placeableObjects.Add("Used Block", new StaticBlock(pos, StaticBlock.BlockType.used, 0.2f));
            _placeableObjects.Add("MysteryBlock", new MysteryBlock(pos, null));
            _placeableObjects.Add("Stone", new StaticBlock(pos, StaticBlock.BlockType.rock, 0.2f));
            _placeableObjects.Add("Coin", new Coin(pos, false));
        }

        public override void Death(GameObject cause)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            momentum = Vector2.Zero;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.W))
                momentum.Y = -terminalVelocity;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.S))
                momentum.Y = terminalVelocity;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.A))
                momentum.X = -maxSpeed;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.D))
                momentum.X = maxSpeed;

            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.Q))
                selected--;
            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.E))
                selected++;

            if (selected < 0)
                selected = _placeableObjects.Count - 1;
            if (selected > _placeableObjects.Count - 1)
                selected = 0;

            _objectToPlace = _placeableObjects[_placeableObjects.Keys.ElementAt(selected)];
            _objectToPlace.position = new Vector2((float)Math.Round(position.X / 16f) * 16, (float)Math.Round(position.Y / 16) * 16f);

            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.Space))
                create(_objectToPlace);
            else
            {
                create(_objectToPlace);
                destory(_objectToPlace);
            }
            Movement(gameTime);
        }

        public override void DrawObject(SpriteBatch batch)
        {
            base.DrawObject(batch);

            if(_objectToPlace != null)
                _objectToPlace.DrawObject(batch);
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
