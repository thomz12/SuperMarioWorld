using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
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
        private ContentManager _contentManager;

        private SpriteFont _spriteFont;

        private float _textScale;

        public MainMenu(Vector2 position, ContentManager contentManager) : base (position)
        {
            sprite.sourceName = @"TitleScreen\Title";
            sprite.xSize = 256;
            sprite.ySize = 224;
            sprite.AddFrame(0, 0);
            sprite.animated = false;
            sprite.layer = 1.0f;

            _textScale = 0.3f;
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

        public void LoadContent(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _spriteFont = _contentManager.Load<SpriteFont>(@"Fonts\MainMenuFont");
        }

        public override void DrawObject(SpriteBatch batch)
        {
            string text = "blljoawowaj";

            batch.DrawString(_spriteFont, text, new Vector2(sprite.xSize / 2, sprite.ySize / 2 + 4), Color.Black, 0, _spriteFont.MeasureString(text) * 0.5f, _textScale, SpriteEffects.None, sprite.layer);

            sprite.DrawSprite(batch, new Vector2(0, 0));
        }
    }
}
