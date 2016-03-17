using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class MainMenu : GameObject
    {
        public SpriteFont font;

        public MainMenu(Vector2 position) : base (position)
        {
            sprite.sourceName = @"TitleScreen\Title";
            sprite.xSize = 256;
            sprite.ySize = 224;
            sprite.AddFrame(0, 0);
            sprite.animated = false;
            sprite.layer = 1.0f;


        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.Instance.GamePadIsPressed(Microsoft.Xna.Framework.Input.Buttons.A) || InputManager.Instance.IsPressed(Microsoft.Xna.Framework.Input.Keys.Space))
                ;
        }

        public override void OnCollision(GameObject collider)
        {
            return;
        }

        public override void DrawObject(SpriteBatch batch)
        {
            sprite.DrawSprite(batch, new Vector2(0, 0));
        }
    }
}
