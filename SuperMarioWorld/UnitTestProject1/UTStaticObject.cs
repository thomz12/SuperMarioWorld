using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorld;
using Microsoft.Xna.Framework;

namespace SMWUT
{
    [TestClass]
    public class UTStaticObject
    {
        [TestInitialize]
        public void Initialize()
        {

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
    }
}
