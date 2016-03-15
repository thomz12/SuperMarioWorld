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
        public int lives;
        public int maxTime;
        public int score;
        public int starPoints;

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
            //LoadScores()
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
