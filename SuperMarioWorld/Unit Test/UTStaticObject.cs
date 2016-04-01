using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorld;
using Microsoft.Xna.Framework;

namespace SMWUT
{
    [TestClass]
    public class UTStaticObject
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
        public void UTStaticBlockConstructor()
        {
            StaticBlock sb = new StaticBlock(new Point(0, 0), StaticBlock.BlockType.cloud, 0.5f);
            Assert.IsInstanceOfType(sb, typeof(StaticBlock));
        }

        [TestMethod]
        public void UTStaticBlock()
        {
            StaticBlock sb = new StaticBlock(new Point(0, 0), StaticBlock.BlockType.rock, 0.5f);
            Assert.IsTrue(sb.blockType == StaticBlock.BlockType.rock);
        }

        [TestMethod]
        public void UTStaticBlockOnCollision()
        {
            StaticBlock sb = new StaticBlock(new Point(0, 0), StaticBlock.BlockType.rock, 0.5f);
            Player player = new Player(new Point(0, -16), null, Player.Character.Mario);
            player.Update(gameTime);
            sb.OnCollision(player);
            Assert.IsFalse(player.boundingBox.Intersects(sb.boundingBox));
        }
    }
}
