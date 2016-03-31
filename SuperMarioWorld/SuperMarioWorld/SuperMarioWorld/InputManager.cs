using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorld
{
    public class InputManager
    {
        // Input manager instance
        private static InputManager _instance;

        // Create itself (singleton)
        public static InputManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new InputManager();
                return _instance;
            }
        }

        // Keyboard states
        private KeyboardState _priorKeyboardState;
        private KeyboardState _curKeyboardState;

        // Gamepad states
        private GamePadState _priorGamepadState;
        private GamePadState _curGamepadState;

        /// <summary>
        /// Default constructor
        /// </summary>
        public InputManager()
        {
            _curKeyboardState = Keyboard.GetState();
            _curGamepadState = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// This function gets called every update of the game
        /// </summary>
        public void Update()
        {
            _priorKeyboardState = _curKeyboardState;
            _priorGamepadState = _curGamepadState;

            _curKeyboardState = Keyboard.GetState();
            _curGamepadState = GamePad.GetState(PlayerIndex.One);
        }

        /// <summary>
        /// Return true if key is pressed
        /// </summary>
        /// <param name="key">The key you want to know</param>
        /// <returns></returns>
        public bool KeyboardIsPressed(Keys key)
        {
            if (_curKeyboardState.IsKeyDown(key))
                return true;
            return false;
        }

        /// <summary>
        /// Return strue is the key is released
        /// </summary>
        /// <param name="key">The key you want to know</param>
        /// <returns></returns>
        public bool KeyboardIsReleased(Keys key)
        {
            if (_curKeyboardState.IsKeyUp(key))
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the key is pressed
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        public bool KeyboardOnPress(Keys key)
        {
            if (_curKeyboardState.IsKeyUp(key) && _priorKeyboardState.IsKeyDown(key))
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if the key is released
        /// </summary>
        /// <param name="key">The key</param>
        /// <returns></returns>
        public bool KeyboardOnRelease(Keys key)
        {
            if (_curKeyboardState.IsKeyDown(key) && _priorKeyboardState.IsKeyUp(key))
                return true;
            return false;
        }

        /// <summary>
        /// Returns true if button is released
        /// </summary>
        /// <param name="button">The button</param>
        /// <returns></returns>
        public bool GamePadIsReleased(Buttons button)
        {
            if (_curGamepadState.IsButtonUp(button))
                return true;
            return false;
        }

        /// <summary>
        /// Get the left joystick value of the controller on the X-axis
        /// </summary>
        /// <returns>-1.0 - 1.0</returns>
        public float GamePadAnalogLeftX()
        {
            return _curGamepadState.ThumbSticks.Left.X;
        }

        /// <summary>
        /// Get the left joystick value of the controller on the Y-axis
        /// </summary>
        /// <returns></returns>
        public float GamePadAnalogLeftY()
        {
            return _curGamepadState.ThumbSticks.Left.Y;
        }

        /// <summary>
        /// Is button pressed
        /// </summary>
        /// <param name="button">The button</param>
        /// <returns></returns>
        public bool GamePadIsPressed(Buttons button)
        {
            if (_curGamepadState.IsButtonDown(button))
                return true;
            return false;
        }

        /// <summary>
        /// Returns true on release of the button
        /// </summary>
        /// <param name="button">The button you want to know</param>
        /// <returns></returns>
        public bool GamePadOnRelease(Buttons button)
        {
            if (_curGamepadState.IsButtonUp(button) && _priorGamepadState.IsButtonDown(button))
                return true;
            return false;
        }

        /// <summary>
        /// Returns true on press of the button
        /// </summary>
        /// <param name="button">The button you want to know</param>
        /// <returns></returns>
        public bool GamePadOnPress(Buttons button)
        {
            if (_curGamepadState.IsButtonDown(button) && _priorGamepadState.IsButtonUp(button))
                return true;
            return false;
        }
    }
}
