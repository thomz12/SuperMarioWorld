using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SuperMarioWorld;

namespace SMWUT
{
    [TestClass]
    public class UTPlayer
    {
        Player player;
        ScoreHandler score;

        [TestInitialize]
        public void Initialize()
        {
            score = new ScoreHandler();
            player = new Player(new Point(0, 0), score, Player.Character.Mario);
            player.destroy = new GameObject.DestroyObject(DoNothing);
        }

        public void DoNothing(GameObject k)
        {
            return;
        }

        [TestMethod]
        public void UTMario()
        {
            score = new ScoreHandler();
            player = new Player(new Point(0, 0), score, Player.Character.Mario);

            Assert.IsNotNull(player);
        }

        [TestMethod]
        public void UTLuigi()
        {
            score = new ScoreHandler();
            player = new Player(new Point(0, 0), score, Player.Character.Luigi);

            Assert.IsNotNull(player);
        }

        [TestMethod]
        public void UTWario()
        {
            score = new ScoreHandler();
            player = new Player(new Point(0, 0), score, Player.Character.Wario);

            Assert.IsNotNull(player);
        }

        [TestMethod]
        public void UTWaluigi()
        {
            score = new ScoreHandler();
            player = new Player(new Point(0, 0), score, Player.Character.Waluigi);

            Assert.IsNotNull(player);
        }

        [TestMethod]
        public void UTPeach()
        {
            score = new ScoreHandler();
            player = new Player(new Point(0, 0), score, Player.Character.Peach);

            Assert.IsNotNull(player);
        }

        [TestMethod]
        public void UTDieing()
        {
            player.powerState = Player.PowerState.normal;

            player.Death(null);
            Assert.IsTrue(player.powerState == Player.PowerState.small);
        }

        [TestMethod]
        public void UTDeath()
        {
            player.powerState = Player.PowerState.small;
            player.Death(null);
            Assert.IsTrue(player.dead);
        }

        [TestMethod]
        public void UTAnimationSmallIdle()
        {
            player.SetAnimation(Player.PlayerAnimationState.idle);
            
        }

        [TestMethod]
        public void UTOnCollisionCoin()
        {
            score.coins = 0;
            player.OnCollision(new Coin(new Point(0, 0), false));
            Assert.IsTrue(score.coins == 1);
        }

        [TestMethod]
        public void UTOnCollisionOneUp()
        {
            score.lives = 0;
            player.OnCollision(new OneUp(new Point(0, 0)));
            Assert.IsTrue(score.lives == 1);
        }

        [TestMethod]
        public void UTOnCollisionMushroomSmall()
        {
            player.powerState = Player.PowerState.small;
            player.OnCollision(new Mushroom(new Point(0, 0)));
            Assert.IsTrue(player.powerState == Player.PowerState.normal);
        }

        [TestMethod]
        public void UTOnCollisionMushroomNormal()
        {
            player.powerState = Player.PowerState.normal;
            player.OnCollision(new Mushroom(new Point(0, 0)));
            Assert.IsTrue(score.powerUp == ScoreHandler.PowerUp.mushroom);
        }
    }
}