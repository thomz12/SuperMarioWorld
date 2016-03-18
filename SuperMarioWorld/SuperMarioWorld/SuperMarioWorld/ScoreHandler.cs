using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    /// <summary>
    /// This class has every score that needs to be displayed int he HUD
    /// and tracks the maximum allowed time for a level
    /// </summary>
    class ScoreHandler
    {
        public int coins;
        public int lives = 3;
        public int maxTime;
        public int score;
        public int starPoints;

        private bool combo;
        private int comboPoints;
        private float comboTimer;

        public enum PowerUp
        {
            none,
            mushroom,
            fireFlower,
            feather
        }

        public PowerUp powerUp;

        private string _scoreFilePath = "score.sms";

        public ScoreHandler()
        {
            combo = false;
            comboPoints = 100;
            comboTimer = 0;
            //LoadScores()
        }

        public void Update(GameTime gameTime)
        {
            if (combo)
            {
                comboTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(comboTimer >= 1000)
                {
                    combo = false;
                    comboTimer = 0;
                    comboPoints = 100;
                }
            }
        }

        /// <summary>
        /// Adds additional score to the combo and checks if it is still running.
        /// Things like coins and hitting enemies can increase the combo.
        /// </summary>
        public void AddCombo()
        {
            if (!combo)
            {
                combo = true;
            }

            score += comboPoints;
            comboTimer = 0;

            //This ugly switch should do the trick for the irregular combo points rewarded.
            switch (comboPoints)
            {
                case 100:
                    comboPoints = 200;
                    break;
                case 200:
                    comboPoints = 400;
                    break;
                case 400:
                    comboPoints = 500;
                    break;
                case 500:
                    comboPoints = 800;
                    break;
                case 800:
                    comboPoints = 1000;
                    break;
                case 1000:
                    lives++;
                    break;
            }
        }

        /// <summary>
        /// Load scores from a file
        /// </summary>
        private void LoadScores()
        {
            //Load the scores from score.sms (only implemented when we are going to use savegames)
        }

        /// <summary>
        /// Save current scores to a file
        /// </summary>
        private void SaveScores()
        {
            //Saves coins, lives, score and starPoints in a score.sms file
        }
    }
}
