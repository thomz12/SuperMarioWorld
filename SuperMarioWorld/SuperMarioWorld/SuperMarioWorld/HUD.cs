using Microsoft.Xna.Framework;
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
        private const string _HUDMarioLuigiNameSource = "HUDMarioLuigiName";
        private const string _HUDLargeNumbersSource = "HUDLargeNumbers";
        private const string _HUDOutlineSource = "HUDOutline";
        private const string _HUDPowerUpsSource = "HUDPowerUps";
        private const string _HUDSmallNumbersSource = "HUDSmallNumbers";

        public Sprite marioLuigiName;
        public Sprite largeNumbers;
        public Sprite outline;
        public Sprite powerUps;
        public Sprite smallNumbers;

        public int score = 1337;
        public int lives = 11;
        public int starPoints = 69;
        public int coins = 42;
        public int time = 500;

        public Player.PowerSate powerUpHUD = Player.PowerSate.normal;

        
        public HUD()
        {
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

        public void DrawHUD(SpriteBatch batch)
        {
            //Draw the hud background or whatever
            outline.DrawSprite(batch, new Vector2(0, 0));

            //Draw the name of the character
            marioLuigiName.AddFrame(0, 0);
            marioLuigiName.DrawSprite(batch, new Vector2(15, 15));

            //Draw reserve powerup
            switch (powerUpHUD)
            {
                case Player.PowerSate.normal:
                    powerUps.NewAnimation(0, 0);
                    powerUps.DrawSprite(batch, new Vector2(119, 14));
                    break;
                case Player.PowerSate.fire:
                    powerUps.NewAnimation(1, 0);
                    powerUps.DrawSprite(batch, new Vector2(119, 14));
                    break;
                case Player.PowerSate.feather:
                    powerUps.NewAnimation(2, 0);
                    powerUps.DrawSprite(batch, new Vector2(119, 14));
                    break;
                default:
                    powerUps.NewAnimation();
                    break;
            }

            //Draw score
            for (int i = score.ToString().Length - 1; i >= 0; i--)
            {
                string s = score.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                smallNumbers.NewAnimation(num, 0);
                smallNumbers.DrawSprite(batch, new Vector2(240 - (i * smallNumbers.xSize), 25));
            }

            //Draw coins
            for (int i = coins.ToString().Length - 1; i >= 0; i--)
            {
                string s = coins.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                smallNumbers.NewAnimation(num, 0);
                smallNumbers.DrawSprite(batch, new Vector2(240 - (i * smallNumbers.xSize), 15));
            }

            //Draw time
            for (int i = time.ToString().Length - 1; i >= 0; i--)
            {
                string s = time.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                smallNumbers.NewAnimation(num, 1);
                smallNumbers.DrawSprite(batch, new Vector2(168 - (i * smallNumbers.xSize), 25));
            }

            //Draw lives
            for (int i = 0; i < lives.ToString().Length; i++)
            {
                string s = lives.ToString();
                int num = int.Parse(s[i].ToString());
                smallNumbers.NewAnimation(num, 0);
                smallNumbers.DrawSprite(batch, new Vector2(33 + (i * smallNumbers.xSize), 24));
            }

            //Draw Starpoints
            for (int i = starPoints.ToString().Length - 1; i >= 0; i--)
            {
                string s = starPoints.ToString();
                char[] array = s.ToCharArray();
                Array.Reverse(array);
                int num = int.Parse(array[i].ToString());
                largeNumbers.NewAnimation(num, 0);
                largeNumbers.DrawSprite(batch, new Vector2(100 - (i * largeNumbers.xSize), 17));
            }
        }
    }
}
