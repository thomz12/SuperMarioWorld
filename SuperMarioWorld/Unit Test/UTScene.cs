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
    }
}
