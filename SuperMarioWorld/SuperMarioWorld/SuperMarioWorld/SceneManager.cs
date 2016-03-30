using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioWorld
{
    /// <summary>
    /// Singleton class for managing scenes
    /// </summary>
    public class SceneManager
    {

        //Singleton instance
        private static SceneManager instance;
        public static SceneManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SceneManager();
                return instance;
            }
        }

        public SMWGame game;

        /// <summary>
        /// Default constructor
        /// </summary>
        public SceneManager()
        {

        }

        /// <summary>
        /// Load a scene
        /// </summary>
        /// <param name="scene">scene to be loaded</param>
        /// <param name="edit">in edit mode</param>
        public void LoadScene(string scene, bool edit)
        {
            //Create new scene
            Scene s = new Scene(scene, game.scores, edit);
            s.cam.Zoom = game.scale;
            s.cam.GameHeight = game.gameHeight;
            s.cam.GameWidth = game.gameWidth;         
            s.LoadContent(game.Content);

            //Set the new scene
            game.scene = s;
            game.currentGameState = SMWGame.GameState.Playing;
        }

        /// <summary>
        /// Load the main menu
        /// </summary>
        public void LoadMainMenu()
        {
            LoadScene("Main_Menu.sml", false);
            game.currentGameState = SMWGame.GameState.MainMenu;
        }
    }
}
