using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using SuperMarioWorld;

namespace SMWUT
{
    [TestClass]
    public class UTRedKoopa
    {
        GameTime gameTime;
        TimeSpan totalGameTime;
        TimeSpan elapsedGameTime;

        RedKoopa redKoopa;
        StaticBlock solidBlock1;

        [TestInitialize]
        public void Initialize()
        {
            redKoopa = new RedKoopa(new Point(0, -16));
            solidBlock1 = new StaticBlock(new Point(0, 0), StaticBlock.BlockType.rock, 0.5f);

            totalGameTime = new TimeSpan(0, 0, 0, 0, 16);
            elapsedGameTime = new TimeSpan(0, 0, 0, 0, 16);
            gameTime = new GameTime(totalGameTime, elapsedGameTime);
        }

        [TestMethod]
        public void UTRedKoopaUpdate()
        {
            redKoopa.Update(gameTime);
            Assert.IsTrue(redKoopa.checkPlatformBox.Intersects(solidBlock1.boundingBox));
        }
    }
}
