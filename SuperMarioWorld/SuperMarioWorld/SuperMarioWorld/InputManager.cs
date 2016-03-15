using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace SuperMarioWorld
{
    class InputManager
    {

        KeyboardState priorKeyboardState;
        KeyboardState curKeyboardState;

        Dictionary<PlayerIndex, GamePadState> priorGamepadState;
        Dictionary<PlayerIndex, GamePadState> curGamepadState;

        public InputManager()
        {
            curKeyboardState = Keyboard.GetState();

            priorGamepadState = new Dictionary<PlayerIndex, GamePadState>();
            curGamepadState = new Dictionary<PlayerIndex, GamePadState>();

            foreach (PlayerIndex i in Enum.GetValues(typeof(PlayerIndex)))
            {
                if(GamePad.GetState(i).IsConnected)
                    curGamepadState[i] = GamePad.GetState(i);
            }
        }

        public void Update()
        {
            priorKeyboardState = curKeyboardState;
            priorGamepadState = curGamepadState;

            curKeyboardState = Keyboard.GetState();
            
            for(int i = 0; i < curGamepadState.Count; i++)
            {
                curGamepadState[(PlayerIndex)i] = GamePad.GetState((PlayerIndex)i);
            }
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

        public bool GamePadIsReleased(PlayerIndex index, Buttons button)
        {
            if (curGamepadState.Count != 0)
                if (curGamepadState[index].IsButtonUp(button))
                return true;
            return false;
        }

        public bool GamePadIsPressed(PlayerIndex index, Buttons button)
        {
            if(curGamepadState.Count != 0)
                if (curGamepadState[index].IsButtonDown(button))
                    return true;
            return false;
        }

        public bool GamePadOnRelease(PlayerIndex index, Buttons button)
        {
            if (curGamepadState.Count != 0)
                if (curGamepadState[index].IsButtonUp(button) && priorGamepadState[index].IsButtonDown(button))
                return true;
            return false;
        }

        public bool GamePadOnPress(PlayerIndex index, Buttons button)
        {
            if (curGamepadState.Count != 0)
                if (curGamepadState[index].IsButtonDown(button) && priorGamepadState[index].IsButtonUp(button))
                return true;
            return false;
        }
    }
}
