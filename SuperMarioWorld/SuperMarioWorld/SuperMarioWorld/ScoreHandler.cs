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

        public Player.PowerSate powerUp;

        private string _scoreFilePath = "score.sms";

        public ScoreHandler()
        {
            //LoadScores()
        }

        private void LoadScores()
        {
            //Load the scores from score.sms (only implemented when we are going to use savegames)
        }
    }
}
