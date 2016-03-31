using System;
using SuperMarioWorld;
using Microsoft.Xna.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Test
{
    [TestClass]
    public class UTBuilder
    {
        [TestMethod]
        public void UTBuilderConstructor()
        {
            Builder b = new Builder(new Point(0,0), new Point(4,4), 16);

            Assert.IsNotNull(b);   
        }

        public void DoNothing(GameObject obj)
        {
            return;
        }

        [TestMethod]
        public void UTBuilderUpdate()
        {
            Builder b = new Builder(new Point(0, 0), new Point(4, 4), 16);
            b.velocity = new Vector2(5, 5);
            b.create = new GameObject.CreateObject(DoNothing);
            b.destroy = new GameObject.DestroyObject(DoNothing);

            TimeSpan totalGameTime = new TimeSpan(0, 0, 0, 0, 10000);
            TimeSpan elapsedGameTime = new TimeSpan(0, 0, 0, 0, 100);
            GameTime gameTime = new GameTime(totalGameTime, elapsedGameTime);

            b.Update(gameTime);

            Assert.IsTrue(b.position.X > -1);
        }
    }
}
