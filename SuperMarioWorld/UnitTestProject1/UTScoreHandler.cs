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
    public class UTScoreHandler
    {
        SuperMarioWorld.ScoreHandler score;

        [TestInitialize]
        public void Initialization()
        {
            score = new SuperMarioWorld.ScoreHandler();
        }

        [TestMethod]
        public void UTScoreHandlerConstructor()
        {
            ScoreHandler s = new ScoreHandler();
            Assert.IsNotNull(s);
        }

        [TestMethod]
        public void UTAddCombo1()
        {
            score.AddCombo();
            Assert.IsTrue(score.score == 100);
        }

        [TestMethod]
        public void UTAddCombo2()
        {
            score.AddCombo();
            score.AddCombo();
            Assert.IsTrue(score.score == 300);
        }

        [TestMethod]
        public void UTResetCombo()
        {
            score.AddCombo();
            score.ResetCombo();
            score.AddCombo();
            Assert.IsTrue(score.score == 200);
        }


    }
}
