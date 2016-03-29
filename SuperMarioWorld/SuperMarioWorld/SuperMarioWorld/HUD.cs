using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class HUD
    {
        //Score handler to pull information from
        private ScoreHandler _scores;

        //Sources of the sprites
        private const string _HUDMarioLuigiNameSource = @"HUD\HUDMarioLuigiName";
        private const string _HUDLargeNumbersSource = @"HUD\HUDLargeNumbers";
        private const string _HUDOutlineSource = @"HUD\HUDOutline";
        private const string _HUDPowerUpsSource = @"HUD\HUDPowerUps";
        private const string _HUDSmallNumbersSource = @"HUD\HUDSmallNumbers";

        //Draw these sprites
        private Sprite _marioLuigiName;
        private Sprite _largeNumbers;
        private Sprite _outline;
        private Sprite _powerUps;
        private Sprite _smallNumbers;

        //Variables for tracking the time that is left in the level
        private int _timeLeft;
        private int _elapsedMiliseconds;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="scoreHandler">Score handler to read  out information</param>
        public HUD(ScoreHandler scoreHandler)
        {
            _scores = scoreHandler;

            //Set the time left in current level tot the maximum time allowed by level
            _timeLeft = _scores.maxTime;

            //Initialize the player character name sprite
            _marioLuigiName = new Sprite(_HUDMarioLuigiNameSource);
            _marioLuigiName.xSize = 40;
            _marioLuigiName.ySize = 8;
            _marioLuigiName.layer = 0.6f;

            //Initialize the large number name sprite
            _largeNumbers = new Sprite(_HUDLargeNumbersSource);
            _largeNumbers.xSize = 8;
            _largeNumbers.ySize = 14;
            
            
            //Initialize the background/outline/standard hud texture sprite
            _outline = new Sprite(_HUDOutlineSource);
            _outline.xSize = 256;
            _outline.ySize = 38;
            _outline.AddFrame(0, 0);
            _outline.layer = 0.5f;

            //Initialize the powerup sprite
            _powerUps = new Sprite(_HUDPowerUpsSource);
            _powerUps.xSize = 18;
            _powerUps.ySize = 18;

            //Initialize the small number sprite
            _smallNumbers = new Sprite(_HUDSmallNumbersSource);
            _smallNumbers.xSize = 8;
            _smallNumbers.ySize = 7;
        }

        public void LoadContent(ContentManager c)
        {
            //Load all the content that is needed for the HUD
            _largeNumbers.texture = c.Load<Texture2D>(_largeNumbers.sourceName);
            _marioLuigiName.texture = c.Load<Texture2D>(_marioLuigiName.sourceName);
            _outline.texture = c.Load<Texture2D>(_outline.sourceName);
            _powerUps.texture = c.Load<Texture2D>(_powerUps.sourceName);
            _smallNumbers.texture = c.Load<Texture2D>(_smallNumbers.sourceName);
        }

        public void Update(GameTime gameTime)
        {
            //Add the elapsed gameTime to the elapsed miliseconds every update
            _elapsedMiliseconds += gameTime.ElapsedGameTime.Milliseconds;

            //check if more than 1000 miliseconds (1s) have elapsed
            if(_elapsedMiliseconds >= 1000f)
            {
                //Take time of the clock
                _timeLeft--;
                //Reset elapsed miliseconds to 0
                _elapsedMiliseconds = 0;
            }
            if(_timeLeft < 0)
            {
                //GAME OVER
                _timeLeft = 0;
            }
            if(_scores.coins >= 100)
            {
                _scores.coins = _scores.coins - 100;
                _scores.lives++;
            }
            if(_scores.lives < 0)
            {
                _scores.lives = 0;
            }
        }

        public void DrawHUD(SpriteBatch batch)
        {
            //Draw the hud background or whatever
            _outline.DrawSprite(batch, new Vector2(0, 0));

            //Draw the name of the character
            _marioLuigiName.AddFrame(0, 0);
            _marioLuigiName.DrawSprite(batch, new Vector2(15, 15));

            //Draw reserve powerup
            switch (_scores.powerUp)
            {
                case ScoreHandler.PowerUp.mushroom:
                    _powerUps.NewAnimation(0, 0);
                    _powerUps.DrawSprite(batch, new Vector2(119, 14));
                    break;
                case ScoreHandler.PowerUp.fireFlower:
                    _powerUps.NewAnimation(1, 0);
                    _powerUps.DrawSprite(batch, new Vector2(119, 14));
                    break;
                case ScoreHandler.PowerUp.feather:
                    _powerUps.NewAnimation(2, 0);
                    _powerUps.DrawSprite(batch, new Vector2(119, 14));
                    break;
                default:
                    //No powerup so nothing to add to animation
                    _powerUps.NewAnimation();
                    break;
            }

            //Draw score, score is lined out on the right.
            for (int i = _scores.score.ToString().Length - 1; i >= 0; i--)
            {
                //Changes the score variable from ScoreHandler to an string
                string s = _scores.score.ToString();
                //Creates a char[] from the string
                char[] array = s.ToCharArray();
                //Reverse the char array
                Array.Reverse(array);
                //get a number and parse it to an int
                int num = int.Parse(array[i].ToString());
                //Set the number's sprite equivalent in the animation list
                _smallNumbers.NewAnimation(num, 0);
                //Draw the number
                _smallNumbers.DrawSprite(batch, new Vector2(240 - (i * _smallNumbers.xSize), 25));
            }

            //Draw coins, coins is lined out on the right
            for (int i = _scores.coins.ToString().Length - 1; i >= 0; i--)
            {
                string s = _scores.coins.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                _smallNumbers.NewAnimation(num, 0);
                _smallNumbers.DrawSprite(batch, new Vector2(240 - (i * _smallNumbers.xSize), 16));
            }

            //Draw time, time is lined out on the right
            for (int i = _timeLeft.ToString().Length - 1; i >= 0; i--)
            {
                string s = _timeLeft.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                _smallNumbers.NewAnimation(num, 1);
                _smallNumbers.DrawSprite(batch, new Vector2(168 - (i * _smallNumbers.xSize), 25));
            }

            //Draw lives, lives are lined out on the left.
            for (int i = 0; i < _scores.lives.ToString().Length; i++)
            {
                string s = _scores.lives.ToString();
                int num = int.Parse(s[i].ToString());
                _smallNumbers.NewAnimation(num, 0);
                _smallNumbers.DrawSprite(batch, new Vector2(33 + (i * _smallNumbers.xSize), 24));
            }

            //Draw Starpoints, starpoints are lined out on the right
            for (int i = _scores.starPoints.ToString().Length - 1; i >= 0; i--)
            {
                string s = _scores.starPoints.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                _largeNumbers.NewAnimation(num, 0);
                _largeNumbers.DrawSprite(batch, new Vector2(100 - (i * _largeNumbers.xSize), 17));
            }
        }
    }
}
