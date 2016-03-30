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
            Scene.LoadScene loadScene = new Scene.LoadScene(DoNothing);

            score = new ScoreHandler();
            scene = new Scene("Level01", score, loadScene, false);
        }

        public void DoNothing(string s, bool b)
        {
            return;
        }

        [TestMethod]
        public void UTSceneConstructor()
        {
            Scene.LoadScene loadScene = new Scene.LoadScene(DoNothing);

            Scene s = new Scene("Level01", score, loadScene, false);
            Assert.IsNotNull(s);
        }
    }
}
