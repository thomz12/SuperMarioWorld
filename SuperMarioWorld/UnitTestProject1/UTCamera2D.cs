using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using SuperMarioWorld;

namespace SuperMarioWorldTEST
{
    [TestClass]
    public class UTCamera2D
    {
        Point size = new Point(10, 10);
        int grid = 16;

        Camera2D cam;

        [TestInitialize]
        public void Initialize()
        {
            cam = new Camera2D(null, size, grid);
        }

        [TestMethod]
        public void k()
        {

        }
    }
}
