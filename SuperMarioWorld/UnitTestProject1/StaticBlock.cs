using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperMarioWorld;
using Microsoft.Xna.Framework;

namespace UnitTestProject1
{
    [TestClass]
    public class StaticBlock
    {
        Player player;
        StaticBlock block;

        [TestInitialize]
        public void Init()
        {
            player = new Player(Point.Zero, null, Player.Character.Mario);
            StaticBlock block = new StaticBlock();
        }
    }
}
