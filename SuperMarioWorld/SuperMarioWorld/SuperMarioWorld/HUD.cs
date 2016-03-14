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
    class HUD
    {
        //Score handler to pull information from
        private ScoreHandler _scores;

        //Sources of the sprites
        private const string _HUDMarioLuigiNameSource = "HUDMarioLuigiName";
        private const string _HUDLargeNumbersSource = "HUDLargeNumbers";
        private const string _HUDOutlineSource = "HUDOutline";
        private const string _HUDPowerUpsSource = "HUDPowerUps";
        private const string _HUDSmallNumbersSource = "HUDSmallNumbers";

        //Draw these sprites
        private Sprite marioLuigiName;
        private Sprite largeNumbers;
        private Sprite outline;
        private Sprite powerUps;
        private Sprite smallNumbers;

        //Variables for tracking the time that is left in the level
        private int _timeLeft;
        private int _elapsedMiliseconds;

        public HUD(ScoreHandler scoreHandler)
        {
            _scores = scoreHandler;

            //Set the time left in current level tot the maximum time allowed by level
            _timeLeft = _scores.maxTime;

            //Initialize the player character name sprite
            marioLuigiName = new Sprite(_HUDMarioLuigiNameSource);
            marioLuigiName.xSize = 40;
            marioLuigiName.ySize = 8;
            marioLuigiName.layer = 0.6f;

            //Initialize the large number name sprite
            largeNumbers = new Sprite(_HUDLargeNumbersSource);
            largeNumbers.xSize = 8;
            largeNumbers.ySize = 14;
            
            
            //Initialize the background/outline/standard hud texture sprite
            outline = new Sprite(_HUDOutlineSource);
            outline.xSize = 256;
            outline.ySize = 38;
            outline.AddFrame(0, 0);
            outline.layer = 0.5f;

            //Initialize the powerup sprite
            powerUps = new Sprite(_HUDPowerUpsSource);
            powerUps.xSize = 18;
            powerUps.ySize = 18;

            //Initialize the small number sprite
            smallNumbers = new Sprite(_HUDSmallNumbersSource);
            smallNumbers.xSize = 8;
            smallNumbers.ySize = 7;
        }

        public void LoadContent(ContentManager c)
        {
            //Load all the content that is needed for the HUD
            largeNumbers.texture = c.Load<Texture2D>(largeNumbers.sourceName);
            marioLuigiName.texture = c.Load<Texture2D>(marioLuigiName.sourceName);
            outline.texture = c.Load<Texture2D>(outline.sourceName);
            powerUps.texture = c.Load<Texture2D>(powerUps.sourceName);
            smallNumbers.texture = c.Load<Texture2D>(smallNumbers.sourceName);
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
                //TODO: Death
                _timeLeft = 0;
            }
        }

        public void DrawHUD(SpriteBatch batch)
        {
            //Draw the hud background or whatever
            outline.DrawSprite(batch, new Vector2(0, 0));

            //Draw the name of the character
            marioLuigiName.AddFrame(0, 0);
            marioLuigiName.DrawSprite(batch, new Vector2(15, 15));

            //Draw reserve powerup
            switch (_scores.powerUp)
            {
                case Player.PowerState.normal:
                    powerUps.NewAnimation(0, 0);
                    powerUps.DrawSprite(batch, new Vector2(119, 14));
                    break;
                case Player.PowerState.fire:
                    powerUps.NewAnimation(1, 0);
                    powerUps.DrawSprite(batch, new Vector2(119, 14));
                    break;
                case Player.PowerState.feather:
                    powerUps.NewAnimation(2, 0);
                    powerUps.DrawSprite(batch, new Vector2(119, 14));
                    break;
                default:
                    //No powerup so nothing to add to animation
                    powerUps.NewAnimation();
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
                smallNumbers.NewAnimation(num, 0);
                //Draw the number
                smallNumbers.DrawSprite(batch, new Vector2(240 - (i * smallNumbers.xSize), 25));
            }

            //Draw coins, coins is lined out on the right
            for (int i = _scores.coins.ToString().Length - 1; i >= 0; i--)
            {
                string s = _scores.coins.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                smallNumbers.NewAnimation(num, 0);
                smallNumbers.DrawSprite(batch, new Vector2(240 - (i * smallNumbers.xSize), 16));
            }

            //Draw time, time is lined out on the right
            for (int i = _timeLeft.ToString().Length - 1; i >= 0; i--)
            {
                string s = _timeLeft.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                smallNumbers.NewAnimation(num, 1);
                smallNumbers.DrawSprite(batch, new Vector2(168 - (i * smallNumbers.xSize), 25));
            }

            //Draw lives, lives are lined out on the left.
            for (int i = 0; i < _scores.lives.ToString().Length; i++)
            {
                string s = _scores.lives.ToString();
                int num = int.Parse(s[i].ToString());
                smallNumbers.NewAnimation(num, 0);
                smallNumbers.DrawSprite(batch, new Vector2(33 + (i * smallNumbers.xSize), 24));
            }

            //Draw Starpoints, starpoints are lined out on the right
            for (int i = _scores.starPoints.ToString().Length - 1; i >= 0; i--)
            {
                string s = _scores.starPoints.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                largeNumbers.NewAnimation(num, 0);
                largeNumbers.DrawSprite(batch, new Vector2(100 - (i * largeNumbers.xSize), 17));
            }
        }
    }
}
