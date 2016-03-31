using System;
using SuperMarioWorld;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Test
{
    [TestClass]
    public class UTCamera
    {
        [TestMethod]
        public void UTCameraConstructor()
        {
            Goomba g = new Goomba(new Point(0,0));
            Camera2D cam = new Camera2D(g, new Point(128,16), 16);

            Assert.IsNotNull(cam);
        }
        [TestMethod]
        public void UTCameraUpdate()
        {
            Goomba g = new Goomba(new Point(0, 0));

            TimeSpan totalGameTime = new TimeSpan(0, 0, 0, 0, 10000);
            TimeSpan elapsedGameTime = new TimeSpan(0, 0, 0, 0, 100);
            GameTime gameTime = new GameTime(totalGameTime, elapsedGameTime);

            Camera2D cam = new Camera2D(g, new Point(128, 16), 16);

            g.velocity = new Vector2(10, 10);
            g.Update(gameTime);
            g.Update(gameTime);

            cam.Update(gameTime);
            cam.Update(gameTime);


            Assert.IsTrue(cam.position.Y > 1);

        }
    }
}
