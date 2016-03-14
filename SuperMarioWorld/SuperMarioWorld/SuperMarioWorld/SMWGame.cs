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

        private const int _gameHeight = 224;
        private const int _gameWidth = 256;

#if DEBUG
        private Texture2D _debugTexture;
        private SpriteFont _debugFont;
#endif
        //A level
        private Level _level;
        //Create a score tracker
        private ScoreHandler _scores;

        private bool _vSync = true;
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

            //Make sure mouse is visable
            IsMouseVisible = true;

            //Load a level
            //TODO load specific levels from main menu/world map
            _scores = new ScoreHandler();
            _level = new Level("0", _scores);
            _level.cam.Zoom = _scale;
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

            //Tell level to load its content
            _level.LoadContent(this.Content);
            
#if DEBUG
            _debugTexture = Content.Load<Texture2D>("DebugTexture");
            _debugFont = Content.Load<SpriteFont>("DefaultFont");
#endif
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

            _level.Update(gameTime);

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

            //Draw level
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _level.cam.GetTransformation(GraphicsDevice));
#if DEBUG
            //Draw each object that is in the level
            for (int i = 0; i < _level.objects.Count; i++)
            {
                _spriteBatch.Draw(_debugTexture, _level.objects[i].boundingBox, null, Color.White, 0, Vector2.Zero, SpriteEffects.None, 1);
            }
#endif
            _level.DrawLevel(_spriteBatch);
            _spriteBatch.End();

            //Draw HUD
            Matrix HUDMatrix = Matrix.CreateScale(new Vector3(_scale, _scale, 1));
            _spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, HUDMatrix);
            _level.DrawHUD(_spriteBatch);
            _spriteBatch.End();
#if DEBUG
            //Draw HUD
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
