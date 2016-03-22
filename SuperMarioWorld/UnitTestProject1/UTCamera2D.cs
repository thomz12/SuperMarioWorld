using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    [TestClass]
    public class UTCamera2D
    {
        //Camera Variables
        public Vector2 cameraPosition;
        public float xDeadZone;
        public Vector2 delta;
        public bool movingRight;

        //Target Variables
        public Vector2 targetPosition;
        float targetX;

        [TestInitialize]
        public void Initialize()
        {
            //Camera
            cameraPosition = new Vector2(1, 0);
            xDeadZone = 5f;

            //Target
            targetPosition = new Vector2(7, 0);
            targetX = targetPosition.X;

            //Calculate here
            delta = targetPosition - cameraPosition;
        }

        [TestMethod]
        public void Camera2DMovingLeft()
        {
            //Check is camera is outside of the left deadzone bounds
            Assert.IsFalse(delta.X < -xDeadZone);
        }

        [TestMethod]
        public void Camera2DMovingRight()
        {
            //Check if camera is outside of the right deadzone bounds
            Assert.IsTrue(delta.X > xDeadZone);
        }

        [TestMethod]
        public void Camera2DAdjustPositionIfoutOFLeftBounds()
        {
            targetPosition = new Vector2(-10, 0);
            cameraPosition = new Vector2(0, 0);
            //delta.x is -10
            delta = targetPosition - cameraPosition;

            //Deadzone is 5
            //delta.X < - xDeadZone returns true
            if (delta.X < -xDeadZone)
            {
                movingRight = false;
            }

            //moving right is false
            if (movingRight)
            {
                //this is not called
            }
            else
            {
                //This IS called
                //delta.X < 0 returns true
                Assert.IsTrue(delta.X < 0);
            }
        }

        [TestMethod]
        public void Camera2DAdjustPositionIfOutOfRightBounds()
        {
            targetPosition = new Vector2(1, 0);
            cameraPosition = new Vector2(7, 0);

            //delta.X = 6
            delta = targetPosition - cameraPosition;

            //xDeadZone is 5
            //delta.X > xDeadZone returns true
            if (delta.X > xDeadZone)
            {
                movingRight = true;
            }

            //movingRight = true;
            if (movingRight)
            {
                //This IS called
                Assert.IsTrue(delta.X > 0);
            }
        }

        [TestMethod]
        public void Camera2DAdjustPositionYAxisSnapToBottomOfLevel()
        {
            Point levelSize = new Point(10, 10);
            float gridSize = 32;

            targetPosition = new Vector2(5 * gridSize, 2 * gridSize);
            cameraPosition = new Vector2(5 * gridSize, 2 * gridSize);

            float cameraHeight = 3f * gridSize;

            //If 112 >= 320
            if (cameraPosition.Y + cameraHeight / 2 >= levelSize.Y * gridSize)
            {

            }
        }
    }
}
