using System;
using SuperMarioWorld;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Test
{
    [TestClass]
    public class UTSMWGame
    {
        [TestMethod]
        public void UTSMWGameCreate()
        {
            SMWGame game = new SMWGame();
            Assert.IsNotNull(game);
        }
    }
}
