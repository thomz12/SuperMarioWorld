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
        private GameObject objectToPlace;    
        private Dictionary<string, GameObject> placeableObjects;
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
            thermalVelocity = 180;
            maxSpeed = 180;

            LoadObjects();
        }

        public void LoadObjects()
        {
            placeableObjects = new Dictionary<string, GameObject>();

            Point pos = Point.Zero;

            placeableObjects.Add("Used Block", new StaticBlock(pos, StaticBlock.BlockType.used, 0.2f));
            placeableObjects.Add("MysteryBlock", new MysteryBlock(pos, null));
            placeableObjects.Add("Stone", new StaticBlock(pos, StaticBlock.BlockType.rock, 0.2f));
            placeableObjects.Add("Coin", new Coin(pos, false));
        }

        public override void Death(GameObject cause)
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            momentum = Vector2.Zero;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.W))
                momentum.Y = -thermalVelocity;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.S))
                momentum.Y = thermalVelocity;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.A))
                momentum.X = -maxSpeed;
            if (InputManager.Instance.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.D))
                momentum.X = maxSpeed;

            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.Q))
                selected--;
            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.E))
                selected++;

            if (selected < 0)
                selected = placeableObjects.Count - 1;
            if (selected > placeableObjects.Count - 1)
                selected = 0;

            objectToPlace = placeableObjects[placeableObjects.Keys.ElementAt(selected)];
            objectToPlace.position = new Vector2((float)Math.Round(position.X / 16f) * 16, (float)Math.Round(position.Y / 16) * 16f);

            if (InputManager.Instance.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.Space))
                create(objectToPlace);
            else
            {
                create(objectToPlace);
                destory(objectToPlace);
            }
            Movement(gameTime);
        }

        public override void DrawObject(SpriteBatch batch)
        {
            base.DrawObject(batch);

            if(objectToPlace != null)
                objectToPlace.DrawObject(batch);
        }

        protected override void Movement(GameTime gameTime)
        {
            if (momentum.X > maxSpeed)
                momentum = new Vector2(maxSpeed, momentum.Y);
            if (momentum.X < -maxSpeed)
                momentum = new Vector2(-maxSpeed, momentum.Y);

            if (momentum.Y > thermalVelocity)
                momentum.Y = thermalVelocity;
            if (momentum.Y < -thermalVelocity)
                momentum.Y = -thermalVelocity;

            //add momentum to position
            position += momentum * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f);
        }

    }
}
