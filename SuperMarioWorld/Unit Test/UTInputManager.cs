using System;
using SuperMarioWorld;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Unit_Test
{
    [TestClass]
    public class UTInputManager
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void UTKeyboardUp()
        {
            InputManager mngr = new InputManager();
            //We dont release key, so it should be false
            Assert.IsFalse(mngr.KeyboardOnRelease(Microsoft.Xna.Framework.Input.Keys.A));
        }

        [TestMethod]
        public void UTKeyboardDown()
        {
            InputManager mngr = new InputManager();
            //We dont press key, so it should be false
            Assert.IsFalse(mngr.KeyboardOnPress(Microsoft.Xna.Framework.Input.Keys.A));
        }

        [TestMethod]
        public void UTKeyboardPressed()
        {
            InputManager mngr = new InputManager();
            //We dont press any key, so it should be false
            Assert.IsFalse(mngr.KeyboardIsPressed(Microsoft.Xna.Framework.Input.Keys.A));
        }

        [TestMethod]
        public void UTKeyboardReleased()
        {
            InputManager mngr = new InputManager();
            //No key is pressed, it should be true
            Assert.IsTrue(mngr.KeyboardIsReleased(Microsoft.Xna.Framework.Input.Keys.A));
        }
    }
}