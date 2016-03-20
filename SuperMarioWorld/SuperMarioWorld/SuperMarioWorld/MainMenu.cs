using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class MainMenu : GameObject
    {
        //The content manager
        private ContentManager _contentManager;

        //Font for the menu
        private SpriteFont _spriteFont;

        //Delegate to load a scene
        public LoadScene load;
        
        //What menu the player is in
        enum Menu
        {
            Main,
            Play,
            Edit
        }
        Menu curMenu;

        //The text scale
        private float _textScale;

        private List<string> menu;
        private int selected;

        /// <summary>
        /// default constructor
        /// </summary>
        /// <param name="position">position of menu</param>
        /// <param name="contentManager">the contentmanager</param>
        public MainMenu(Point position, ContentManager contentManager) : base (position)
        {
            //Set sprite info
            sprite.sourceName = @"TitleScreen\Title";
            sprite.xSize = 256;
            sprite.ySize = 224;
            sprite.AddFrame(0, 0);
            sprite.animated = false;
            sprite.layer = 1.0f;

            //Set index for selected item
            selected = 0;

            //Set current menu
            curMenu = Menu.Main;

            //Add options to list
            menu = new List<string>();
            menu.Add("Play Game");
            menu.Add("Level Editor");

            _textScale = 0.12f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.Instance.KeyboardOnRelease(Microsoft.Xna.Framework.Input.Keys.W))
                selected--;
            if (InputManager.Instance.KeyboardOnRelease(Microsoft.Xna.Framework.Input.Keys.S))
                selected++;

            if (InputManager.Instance.KeyboardOnRelease(Microsoft.Xna.Framework.Input.Keys.Space))
            {
                //if we are in main menu
                if (curMenu == Menu.Main)
                {
                    //and have "Play" selected
                    if (selected == 0)
                    {
                        selected = 0;
                        GetLevels();
                        curMenu = Menu.Play;
                    }
                    //or have "Edit selected"
                    else if(selected == 1)
                    {

                    }
                }
                else if (curMenu == Menu.Play)
                {
                    load(menu[selected]);
                }
            }

            if (selected >= menu.Count)
                selected = 0;
            if (selected < 0)
                selected = menu.Count - 1;
        }

        private void GetLevels()
        {
            string[] files = Directory.GetFiles(@"Content\Levels");
            menu.Clear();
            foreach(string s in files)
            { 
                string name = Path.GetFileName(s);
                string[] split = name.Split('.');

                if (split[1].Equals("sml"))
                {
                    menu.Add(split[0]);
                }
            }
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

            for (int i = 0; i < menu.Count; i++)
            {
                Color textColor = Color.Black;
                if (i == selected)
                    textColor = Color.White;

                text = (selected == i ? ")))  " : "") + menu[i] + (selected == i ? "   (((" : "");

                batch.DrawString(_spriteFont, text, new Vector2(sprite.xSize / 2, i * 5 + sprite.ySize / 2 + 4), textColor, 0, _spriteFont.MeasureString(text) * 0.5f, _textScale, SpriteEffects.None, sprite.layer);

            }

            sprite.DrawSprite(batch, new Vector2(0, 0));
        }
    }
}
