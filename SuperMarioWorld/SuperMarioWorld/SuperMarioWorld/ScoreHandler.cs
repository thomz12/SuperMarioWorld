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

        private bool _combo;
        private int _comboPoints;
        private float _comboTimer;

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
            _combo = false;
            _comboPoints = 100;
            _comboTimer = 0;
        }

        public void Update(GameTime gameTime)
        {
            if (_combo)
            {
                _comboTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if(_comboTimer >= 1000)
                {
                    _combo = false;
                    _comboTimer = 0;
                    _comboPoints = 100;
                }
            }
        }

        /// <summary>
        /// Adds additional score to the combo and checks if it is still running.
        /// Things like coins and hitting enemies can increase the combo.
        /// </summary>
        public void AddCombo()
        {
            if (!_combo)
            {
                _combo = true;
            }

            score += _comboPoints;
            _comboTimer = 0;

            //This ugly switch should do the trick for the irregular combo points rewarded.
            switch (_comboPoints)
            {
                case 100:
                    _comboPoints = 200;
                    break;
                case 200:
                    _comboPoints = 400;
                    break;
                case 400:
                    _comboPoints = 500;
                    break;
                case 500:
                    _comboPoints = 800;
                    break;
                case 800:
                    _comboPoints = 1000;
                    break;
                case 1000:
                    lives++;
                    break;
            }
        }
    }
}
