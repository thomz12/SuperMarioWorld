using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    class InputManager
    {

        private static InputManager inputManager;

        KeyboardState priorKeyboardState;
        KeyboardState curKeyboardState;

        GamePadState priorGamepadState;
        GamePadState curGamepadState;

        public InputManager()
        {
            curKeyboardState = Keyboard.GetState();

            curGamepadState = GamePad.GetState(PlayerIndex.One);
        }

        public static InputManager Instance
        {
            get
            {
                if (inputManager == null)
                    inputManager = new InputManager();
                return inputManager;
            }
        }

        public void Update()
        {
            priorKeyboardState = curKeyboardState;
            priorGamepadState = curGamepadState;

            curKeyboardState = Keyboard.GetState();
            curGamepadState = GamePad.GetState(PlayerIndex.One);
        }

        public bool IsPressed(Keys key)
        {
            if (curKeyboardState.IsKeyDown(key))
                return true;
            return false;
        }

        public bool IsReleased(Keys key)
        {
            if (curKeyboardState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool OnRelease(Keys key)
        {
            if (curKeyboardState.IsKeyUp(key) && priorKeyboardState.IsKeyDown(key))
                return true;
            return false;
        }

        public bool OnPress(Keys key)
        {
            if (curKeyboardState.IsKeyDown(key) && priorKeyboardState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool GamePadIsReleased(Buttons button)
        {
            if (curGamepadState.IsButtonUp(button))
                return true;
            return false;
        }

        public float GamePadAnalogX()
        {
            return curGamepadState.ThumbSticks.Left.X;
        }

        public bool GamePadIsPressed(Buttons button)
        {
            if (curGamepadState.IsButtonDown(button))
                return true;
            return false;
        }

        public bool GamePadOnRelease(Buttons button)
        {
            if (curGamepadState.IsButtonUp(button) && priorGamepadState.IsButtonDown(button))
                return true;
            return false;
        }

        public bool GamePadOnPress(Buttons button)
        {
            if (curGamepadState.IsButtonDown(button) && priorGamepadState.IsButtonUp(button))
                return true;
            return false;
        }
    }
}
