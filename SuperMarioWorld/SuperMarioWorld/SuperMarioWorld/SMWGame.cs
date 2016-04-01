using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SuperMarioWorld
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class SMWGame : Game
    {
        // Graphics
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        /// <summary>
        /// Default SNES height in pixels.
        /// </summary>
        public int gameHeight = 224;
        /// <summary>
        /// Default SNES width in pixels.
        /// </summary>
        public int gameWidth = 256;

#if DEBUG
        private Texture2D _debugTexture;
        private SpriteFont _debugFont;
#endif
        // Gamestate and menus
        public enum GameState
        {
            MainMenu,
            Playing
        }
        public GameState currentGameState;

        // A Level
        public Scene scene;

        // Create a score tracker
        public ScoreHandler scores;

        // Enables or disables VSync
        private bool _vSync;

        // Sets the scale of the game, the larger the scale the larger the game
        public int scale = 4;

#if DEBUG
        private int _totalFrames;
        private float _elapsedTime;
        private int _fps;
#endif
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMWGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            // Set default resolution (SNES resolution) and scale it
            _graphics.PreferredBackBufferHeight = gameHeight * scale;
            _graphics.PreferredBackBufferWidth = gameWidth * scale;

            _graphics.SynchronizeWithVerticalRetrace = _vSync;

            // Make game fullscreen
            _graphics.IsFullScreen = false;

            IsFixedTimeStep = true;
            _vSync = true;


            // Make sure mouse is visable
            IsMouseVisible = true;

            // Create the new ScoreHandler
            scores = new ScoreHandler();

            SceneManager.Instance.game = this;

            // TODO load from a savefile

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize game
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
#if DEBUG
            _debugTexture = Content.Load<Texture2D>("DebugTexture");
            _debugFont = Content.Load<SpriteFont>(@"Fonts\DefaultFont");
#endif

            SceneManager.Instance.LoadMainMenu();
            currentGameState = GameState.MainMenu;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // No content needs to be unloaded
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Call the update function from the input manager
            InputManager.Instance.Update();
#if DEBUG
            //FPS Counter
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_elapsedTime > 1000.0f)
            {
                _fps = _totalFrames;
                _totalFrames = 0;
                _elapsedTime = 0;
            }
#endif
            // Allows the game to exit
            if (InputManager.Instance.KeyboardOnPress(Keys.Escape) || InputManager.Instance.GamePadOnPress(Buttons.Back))
            {
                if (currentGameState == GameState.MainMenu)
                    this.Exit();
                else
                    SceneManager.Instance.LoadMainMenu();
            }

            // Toggle Fullscreen when F11 is pressed
            if (InputManager.Instance.KeyboardOnPress(Keys.F11))
            {
                _graphics.ToggleFullScreen();
            }

            // Calls the update function from scene
            scene.Update(gameTime);

            // Calls the update function from parent
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
#if DEBUG
            //Only count frames we actualy draw
            _totalFrames++;
#endif

            // Set clear color
            GraphicsDevice.Clear(Color.CornflowerBlue);

                _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, scene.cam.GetTransformation(GraphicsDevice));

#if DEBUG
            if (InputManager.Instance.KeyboardIsPressed(Keys.LeftShift) || InputManager.Instance.GamePadIsPressed(Buttons.X))
            {
                //Draw each object that is in the level (bounding box
                for (int i = 0; i < scene.objects.Count; i++)
                {
                    _spriteBatch.Draw(_debugTexture, scene.objects[i].boundingBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                    if (scene.objects[i] is RedKoopa)
                    {
                        RedKoopa r = (RedKoopa)scene.objects[i];
                        _spriteBatch.Draw(_debugTexture, r.checkPlatformBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                    }
                }
            }
#endif
                scene.DrawLevel(_spriteBatch);
                _spriteBatch.End();

            //Draw HUD
            if (currentGameState != GameState.MainMenu)
            {
                Matrix HUDMatrix = Matrix.CreateScale(new Vector3(scale, scale, 1));
                _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, HUDMatrix);
                scene.DrawHUD(_spriteBatch);

                _spriteBatch.End();
            }
#if DEBUG
            //Draw debug
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_debugFont, "fps: " + _fps, new Vector2(10, 0), Color.Black);
            _spriteBatch.DrawString(_debugFont, "Session time: " + gameTime.TotalGameTime, new Vector2(10, 20), Color.Black);
            _spriteBatch.DrawString(_debugFont, "Update time: " + gameTime.ElapsedGameTime.TotalMilliseconds, new Vector2(10, 40), Color.Black);
            _spriteBatch.DrawString(_debugFont, GraphicsDevice.Adapter.Description, new Vector2(10, 60), Color.Black);
            _spriteBatch.DrawString(_debugFont, "Preferred Backbuffer: " + _graphics.PreferredBackBufferWidth + "x" + _graphics.PreferredBackBufferHeight, new Vector2(10, 80), Color.Black);
            _spriteBatch.DrawString(_debugFont, "Actual Backbuffer: " + GraphicsDevice.Viewport.Width + "x" + GraphicsDevice.Viewport.Height, new Vector2(10, 100), Color.Black);

            _spriteBatch.End();
#endif
            // Calls the draw from the parent
            base.Draw(gameTime);
        }
    }
}
