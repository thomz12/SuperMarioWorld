using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarioWorld;
using Microsoft.Xna.Framework;

namespace SMWUT
{
    [TestClass]
    public class UTStaticBlock
    {
        Player player;
        StaticBlock block;

        [TestInitialize]
        public void Init()
        {
            player = new Player(Point.Zero, null, Player.Character.Mario);
            block = new StaticBlock(new Point(0, 0), StaticBlock.BlockType.used, 0.5f);
        }

        [TestMethod]
        public void TestCollision()
        {
            player.position = new Vector2(0, 0.1f);
            block.position = new Vector2(0, 0);

            if (player.boundingBox.Intersects(block.boundingBox))
            {
                block.OnCollision(player);
            }
        }
    }
}
