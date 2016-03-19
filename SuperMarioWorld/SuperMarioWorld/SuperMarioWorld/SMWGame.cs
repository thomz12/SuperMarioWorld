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
    public class SMWGame : Microsoft.Xna.Framework.Game
    {
        //Graphics
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        /// <summary>
        /// Default SNES height in pixels.
        /// </summary>
        private const int _gameHeight = 224;
        /// <summary>
        /// Default SNES width in pixels.
        /// </summary>
        private const int _gameWidth = 256;

#if DEBUG
        private Texture2D _debugTexture;
        private SpriteFont _debugFont;
#endif
        //Gamestate and menus
        public enum GameState
        {
            MainMenu,
            Playing,
            Pause
        }
        public GameState currentGameState;

        private string levelPath;

        //A Level
        private Scene _scene;

        //Create a score tracker
        private ScoreHandler _scores;

        private bool _vSync;
        private const int _scale = 3;

#if DEBUG
        //FPS Counter
        struct FPSCounter
        {
            public int totalFrames;
            public float elapsedTime;
            public int fps;
        }
        private FPSCounter _counter;
#endif
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SMWGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            //Set default resolution (SNES resolution) and scale it
            _graphics.PreferredBackBufferHeight = _gameHeight * _scale;
            _graphics.PreferredBackBufferWidth = _gameWidth * _scale;

            _graphics.SynchronizeWithVerticalRetrace = _vSync;

            //Make game fullscreen
            _graphics.IsFullScreen = false;

            IsFixedTimeStep = true;
            _vSync = true;
            levelPath = "DemoLevel.sml";
            currentGameState = GameState.MainMenu;

            //Make sure mouse is visable
            IsMouseVisible = true;

            //Create the new ScoreHandler
            _scores = new ScoreHandler();
            
            //TODO load from a savefile

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
            // TODO: Add your initialization logic here

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
            LoadScene();
        }

        /// <summary>
        /// Changes the scene to the scene of the given file path
        /// </summary>
        /// <param name="sceneSourceFile">Name of the scene in the \Content\Levels folder, including extension.</param>
        public void LoadScene()
        {
            _scene = new Scene(levelPath, _scores);
            _scene.cam.Zoom = _scale;
            _scene.cam.GameHeight = _gameHeight;
            _scene.cam.GameWidth = _gameWidth;
            _scene.LoadContent(this.Content);
            _scene.load = new LoadScene(LoadScene);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _scores.Update(gameTime);
            InputManager.Instance.Update();
#if DEBUG
            //FPS Counter
            _counter.elapsedTime += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (_counter.elapsedTime > 1000.0f)
            {
                _counter.fps = _counter.totalFrames;
                _counter.totalFrames = 0;
                _counter.elapsedTime = 0;
            }
#endif
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            //Toggle Fullscreen when F11 is pressed
            if (Keyboard.GetState().IsKeyDown(Keys.F11))
            {
                _graphics.ToggleFullScreen();
            }

            _scene.Update(gameTime);

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
            _counter.totalFrames++;
#endif

            //Set clear color
            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (currentGameState == GameState.MainMenu)
            {
                _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _scene.cam.GetTransformation(GraphicsDevice));
                _scene.DrawLevel(_spriteBatch);

                _spriteBatch.End();

            }
            //Only do this when the game is in a level
            else if (currentGameState == GameState.Playing)
            {
                //Draw level
                _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _scene.cam.GetTransformation(GraphicsDevice));
#if DEBUG
                //Draw each object that is in the level
                for (int i = 0; i < _scene.objects.Count; i++)
                {
                    _spriteBatch.Draw(_debugTexture, _scene.objects[i].boundingBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                    if (_scene.objects[i] is RedKoopa)
                    {
                        RedKoopa r = (RedKoopa)_scene.objects[i];
                        _spriteBatch.Draw(_debugTexture, r.checkPlatformBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
                    }
                }
#endif
                _scene.DrawLevel(_spriteBatch);
                _spriteBatch.End();

                //Draw HUD
                Matrix HUDMatrix = Matrix.CreateScale(new Vector3(_scale, _scale, 1));
                _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, HUDMatrix);
                _scene.DrawHUD(_spriteBatch);
                _spriteBatch.End();
            }
#if DEBUG
            //Draw debug
            _spriteBatch.Begin();
            _spriteBatch.DrawString(_debugFont, "fps: " + _counter.fps, new Vector2(10, 0), Color.Black);
            _spriteBatch.DrawString(_debugFont, "Session time: " + gameTime.TotalGameTime, new Vector2(10, 20), Color.Black);
            _spriteBatch.DrawString(_debugFont, "Update time: " + gameTime.ElapsedGameTime.TotalMilliseconds, new Vector2(10, 40), Color.Black);
            _spriteBatch.DrawString(_debugFont, GraphicsDevice.Adapter.Description, new Vector2(10, 60), Color.Black);
            _spriteBatch.End();
#endif
            base.Draw(gameTime);
        }
    }
}
