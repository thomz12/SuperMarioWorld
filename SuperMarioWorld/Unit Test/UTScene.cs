using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorld;
using Microsoft.Xna.Framework;

namespace SMWUT
{
    [TestClass]
    public class UTScene
    {
        private Scene scene;
        private ScoreHandler score;

        [TestInitialize]
        public void Initialize()
        {
            score = new ScoreHandler();
            scene = new Scene("Level01", score, false);
        }

        [TestMethod]
        public void UTSceneConstructor()
        {
            Scene s = new Scene("Level01", score, false);
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void UTSceneUpdate()
        {
            TimeSpan totalGameTime = new TimeSpan(0, 0, 0, 0, 10000);
            TimeSpan elapsedGameTime = new TimeSpan(0, 0, 0, 0, 100);
            GameTime gameTime = new GameTime(totalGameTime, elapsedGameTime);

            Scene s = new Scene("Level01", score, false);
            Goomba g = new Goomba(new Point(0, 0));
            s.objects.Add(g);
            g.velocity = new Vector2(3, 3);
            g.Update(gameTime);
            s.Update(gameTime);
            Vector2 pos = g.position;
            Assert.IsTrue(g.position.X < -1);
        }
    }
}
