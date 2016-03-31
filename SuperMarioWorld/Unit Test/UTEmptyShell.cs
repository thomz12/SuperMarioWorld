using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SuperMarioWorld;

namespace SMWUT
{
    [TestClass]
    public class UTEmptyShell
    {
        TimeSpan totalGameTime;
        TimeSpan elapsedGameTime;
        GameTime gameTime;

        [TestInitialize]
        public void Initialize()
        {
            totalGameTime = new TimeSpan(0, 0, 0, 0, 2910);
            elapsedGameTime = new TimeSpan(0, 0, 0, 0, 16);
            gameTime = new GameTime(totalGameTime, elapsedGameTime);
        }

        [TestMethod]
        public void UTEmptyShellStoppedCollisionPlayer()
        {
            EmptyShell shell = new EmptyShell(new Point(0, 0), EmptyShell.KoopaType.green);
            ScoreHandler score = new ScoreHandler();
            Player player = new Player(new Point(10, 10), score, Player.Character.Mario);
            shell.OnCollision(player);
            shell.Update(gameTime);
            Assert.IsTrue(shell.velocity.X != 0);
        }

        [TestMethod]
        public void UTEmptyShellMovingCollisionPlayer()
        {
            EmptyShell shell = new EmptyShell(new Point(0, 0), EmptyShell.KoopaType.green);
            ScoreHandler score = new ScoreHandler();
            Player player = new Player(new Point(10, 10), score, Player.Character.Mario);
            shell.OnCollision(player);
            shell.Update(gameTime);
            player.velocity.Y = 4;
            shell.OnCollision(player);
            Assert.IsTrue(shell.velocity.X == 0);
        }

        [TestMethod]
        public void UTEmptyShellMovingCollisionEnemy()
        {
            EmptyShell shell = new EmptyShell(new Point(0, 0), EmptyShell.KoopaType.green);
            ScoreHandler score = new ScoreHandler();
            Player player = new Player(new Point(10, 10), score, Player.Character.Mario);
            shell.OnCollision(player);
            shell.Update(gameTime);
            GreenKoopa koopa = new GreenKoopa(new Point(0, 0));
            shell.OnCollision(koopa);
            Assert.IsTrue(koopa.dead);
        }
    }
}
