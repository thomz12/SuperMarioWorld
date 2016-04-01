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
    public class ScoreHandler
    {
        // Variables for different scores that should be tracked
        public int coins;
        public int lives = 3;
        public int maxTime;
        public int score;
        public int starPoints;

        // Variables for the combo
        private bool _combo;
        private int _comboPoints;

        /// <summary>
        /// The different powerups that can be in the powerup slot
        /// </summary>
        public enum PowerUp
        {
            none,
            mushroom,
            fireFlower,
            feather
        }
        public PowerUp powerUp;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ScoreHandler()
        {
            _combo = false;
            _comboPoints = 100;
        }

        /// <summary>
        /// Adds additional score to the combo and checks if it is still running.
        /// Things like coins and hitting enemies can increase the combo.
        /// </summary>
        public void AddCombo()
        {
            // If the combo hasnt been started yet, start it
            if (!_combo)
            {
                _combo = true;
            }

            // Add the points to score
            score += _comboPoints;

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
        
        /// <summary>
        /// Reset the current combo
        /// </summary>
        public void ResetCombo()
        {
            _comboPoints = 100;
            _combo = false;
        }
    }
}
