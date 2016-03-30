using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorld;
using Microsoft.Xna.Framework;

namespace SMWUT
{
    [TestClass]
    public class UTEntity
    {
        Player player;
        ScoreHandler score;

        [TestInitialize]
        public void Initialize()
        {
            score = new ScoreHandler();
            player = new Player(new Point(0, 0), score, Player.Character.Mario);
        }

        [TestMethod]
        public void UTPlayerConstructor()
        {
            Player p = new Player(new Point(0, 0), null, Player.Character.Mario);
            Assert.IsInstanceOfType(p, typeof(Player));
        }

        [TestMethod]
        public void UTMushroomConstructor()
        {
            Mushroom m = new Mushroom(new Point(0, 0));
            Assert.IsInstanceOfType(m, typeof(Mushroom));
        }

        [TestMethod]
        public void UTCoinConstructor()
        {
            Coin c = new Coin(new Point(0, 0), false);
            Assert.IsInstanceOfType(c, typeof(Coin));
        }

        [TestMethod]
        public void UTGoombaConstructor()
        {
            Goomba g = new Goomba(new Point(0, 0));
            Assert.IsInstanceOfType(g, typeof(Goomba));
        }

        [TestMethod]
        public void UTGreenKoopaConstructor()
        {
            GreenKoopa gk = new GreenKoopa(new Point(0, 0));
            Assert.IsInstanceOfType(gk, typeof(GreenKoopa));
        }

        [TestMethod]
        public void UTRedKoopaConstructor()
        {
            RedKoopa rk = new RedKoopa(new Point(0, 0));
            Assert.IsInstanceOfType(rk, typeof(RedKoopa));
        }

        [TestMethod]
        public void UTEmptyShellConstructor()
        {
            EmptyShell es = new EmptyShell(new Point(0, 0), EmptyShell.KoopaType.green);
            Assert.IsInstanceOfType(es, typeof(EmptyShell));
        }

        [TestMethod]
        public void UTOneUpConstructor()
        {
            OneUp ou = new OneUp(new Point(0, 0));
            Assert.IsInstanceOfType(ou, typeof(OneUp));
        }

        [TestMethod]
        public void UTEntityDeath()
        {
            Mushroom m = new Mushroom(new Point(0, 0));
            m.Death(null);

            Assert.IsTrue(m.dead);
        }
    }
}
