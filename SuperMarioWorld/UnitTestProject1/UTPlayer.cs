using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SMWUT
{
    [TestClass]
    public class UTPlayer
    {
        SuperMarioWorld.Player player;

        [TestInitialize]
        public void Initialize()
        {
            player = new SuperMarioWorld.Player(new Microsoft.Xna.Framework.Point(0, 0), null, SuperMarioWorld.Player.Character.Mario);
        }

        [TestMethod]
        public void UTDeath()
        {
            player.powerState = SuperMarioWorld.Player.PowerState.normal;

            player.Death(new SuperMarioWorld.Goomba(new Point(0, 0)));
            Assert.IsTrue(player.powerState == SuperMarioWorld.Player.PowerState.small);
        }
    }
}