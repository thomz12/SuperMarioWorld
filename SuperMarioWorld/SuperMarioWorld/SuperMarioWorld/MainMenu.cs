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
    public class MainMenu : GameObject
    {
        // The content manager
        private ContentManager _contentManager;

        // Font for the menu
        private SpriteFont _spriteFont;
        
        // What menu the player is in
        private enum Menu
        {
            Main,
            Play,
            Edit
        }
        private Menu curMenu;

        // The text scale
        private float _textScale;

        // All the names of the levels in th menu
        private List<string> _menuContent;

        // Current selected name in levels
        private int _selected;

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
            _selected = 0;

            //Set current menu
            curMenu = Menu.Main;

            //Add options to list
            _menuContent = new List<string>();
            _menuContent.Add("Play Game");
            _menuContent.Add("Level Editor");

            _textScale = 0.12f;
        }

        /// <summary>
        /// This gets called every frame
        /// </summary>
        /// <param name="gameTime">Time variable</param>
        public override void Update(GameTime gameTime)
        {
            // Call the GameObject update
            base.Update(gameTime);

            // Check if a key is released
            if (InputManager.Instance.KeyboardOnRelease(Microsoft.Xna.Framework.Input.Keys.W) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.DPadUp))
                _selected--;
            if (InputManager.Instance.KeyboardOnRelease(Microsoft.Xna.Framework.Input.Keys.S) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.DPadDown))
                _selected++;

            // Check if spacebar or A button was pressed
            if (InputManager.Instance.KeyboardOnRelease(Microsoft.Xna.Framework.Input.Keys.Space) || InputManager.Instance.GamePadOnPress(Microsoft.Xna.Framework.Input.Buttons.A))
            {
                // If we are in main menu
                if (curMenu == Menu.Main)
                {
                    // And have "Play" selected
                    if (_selected == 0)
                    {
                        // Reset selected to 0 (for first entry)
                        _selected = 0;
                        // Get all the levels that can be played
                        GetLevels();
                        // Set the menu the player is in to the Play menu
                        curMenu = Menu.Play;
                    }
                    // Or have "Edit selected"
                    else if(_selected == 1)
                    {
                        // Reset selected to 0 (for first entry)
                        _selected = 0;
                        // Get all the levels that can be edited
                        GetLevels();
                        // Set the menu the player is in to the Edit menu
                        curMenu = Menu.Edit;
                    }
                }
                else if (curMenu == Menu.Play)
                {
                    // if we are in the play menu play the selected level
                    SceneManager.Instance.LoadScene(_menuContent[_selected], false);
                }
                else if(curMenu == Menu.Edit)
                {
                    // if we are in the edit menu, edit the selected level
                    SceneManager.Instance.LoadScene(_menuContent[_selected], true);
                }
            }

            // Make sure the selected does not go out of range
            if (_selected >= _menuContent.Count)
                _selected = 0;
            if (_selected < 0)
                _selected = _menuContent.Count - 1;
        }

        /// <summary>
        /// A menu cannot collide, so this is empty
        /// </summary>
        /// <param name="collider">stuff that is ot collided with</param>
        public override void OnCollision(GameObject collider)
        {
            return;
        }

        /// <summary>
        /// Gets all the levels that are saved in the Content\Levels\ folder
        /// </summary>
        private void GetLevels()
        {
            // Gets all the names of the files in the Content\Levels directory
            string[] files = Directory.GetFiles(@"Content\Levels");

            // Clears all the strings in the menu content list
            _menuContent.Clear();

            // For each name in the files array
            foreach(string s in files)
            { 
                // Get the file's name
                string name = Path.GetFileName(s);
                string[] split = name.Split('.');

                // Check if the extension of the file is .sml (our level file extension)
                if (split[1].Equals("sml"))
                {
                    // Add the name of the level to the content
                    _menuContent.Add(split[0]);
                }
            }
        }

        /// <summary>
        /// Loads the content of the main menu
        /// </summary>
        /// <param name="contentManager">The content manager that loads content</param>
        public void LoadContent(ContentManager contentManager)
        {
            _contentManager = contentManager;
            _spriteFont = _contentManager.Load<SpriteFont>(@"Fonts\MainMenuFont");
        }
        
        /// <summary>
        /// Draw the sprite and menu entries
        /// </summary>
        /// <param name="batch">The batch that should be drawn on</param>
        public override void DrawObject(SpriteBatch batch)
        {
            // Empty the text string
            string text = String.Empty;

            // Cycle through the menu content
            for (int i = 0; i < _menuContent.Count; i++)
            {
                // Set the text color to black
                Color textColor = Color.Black;

                // When this item is selected it is highlighted in white
                if (i == _selected)
                    textColor = Color.White;

                // add ))) to the text for extra visibility
                text = (_selected == i ? ")))  " : "") + _menuContent[i] + (_selected == i ? "   (((" : "");

                // Draw the text
                batch.DrawString(_spriteFont, text, new Vector2(sprite.xSize / 2, i * 5 + sprite.ySize / 2 + 4), textColor, 0, _spriteFont.MeasureString(text) * 0.5f, _textScale, SpriteEffects.None, sprite.layer);
            }

            // Draw the menu sprite
            sprite.DrawSprite(batch, new Vector2(0, 0));
        }
    }
}
