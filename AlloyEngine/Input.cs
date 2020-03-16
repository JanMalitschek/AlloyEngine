using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Input;
using OpenTK;

namespace Alloy
{
    public static class Input
    {
        private struct InputContainer
        {
            public bool pressed;
            public bool held;
        }

        private static KeyboardState keyboardState;
        private static InputContainer[] keyboardInput;
        private static List<int> pressedKeys = new List<int>();

        private static MouseState mouseState;
        private static InputContainer[] mouseInput;
        private static List<int> pressedButtons = new List<int>();

        private static Vector2 lastMousePos;
        public static Vector2 MouseDelta { get; private set; }
        public static Vector2 MousePosition { get; private set; }

        public static void Init(GameWindow window)
        {
            keyboardInput = new InputContainer[Enum.GetNames(typeof(Key)).Length];
            mouseInput = new InputContainer[Enum.GetNames(typeof(MouseButton)).Length];
            window.MouseMove += OnMouseMove;
        }

        public static void Update()
        {
            #region Keyboard Input
            keyboardState = Keyboard.GetState();
            bool keysReleased = false;
            for(int i = 0; i < pressedKeys.Count; i++)
            {
                if (keyboardState.IsKeyDown((Key)pressedKeys[i]))
                    keyboardInput[pressedKeys[i]].held = true;
                else
                {
                    keyboardInput[pressedKeys[i]].pressed = false;
                    keyboardInput[pressedKeys[i]].held = false;
                    pressedKeys[i] = -1;
                    keysReleased = true;
                }
            }
            if (keysReleased)
            {
                pressedKeys.RemoveAll(x => x == -1);
                keysReleased = false;
            }
            #endregion
            #region Mouse Input
            mouseState = Mouse.GetState();
            bool buttonsReleased = false;
            for (int i = 0; i < pressedButtons.Count; i++)
            {
                if (mouseState.IsButtonDown((MouseButton)pressedButtons[i]))
                    mouseInput[pressedButtons[i]].held = true;
                else
                {
                    mouseInput[pressedButtons[i]].pressed = false;
                    mouseInput[pressedButtons[i]].held = false;
                    pressedButtons[i] = -1;
                    buttonsReleased = true;
                }
            }
            if (buttonsReleased)
            {
                pressedButtons.RemoveAll(x => x == -1);
                buttonsReleased = false;
            }

            var mousePos = new Vector2(mouseState.X, mouseState.Y);
            MouseDelta = mousePos - lastMousePos;
            lastMousePos = mousePos;
            #endregion
        }

        public static bool GetKeyDown(Key key)
        {
            if (keyboardState.IsKeyDown(key) && !keyboardInput[(int)key].pressed)
            {
                keyboardInput[(int)key].pressed = true;
                pressedKeys.Add((int)key);
            }
            return keyboardInput[(int)key].pressed && !keyboardInput[(int)key].held;
        }
        public static bool GetKey(Key key)
        {
            return keyboardState.IsKeyDown(key);
        }

        public static bool GetMouseButtonDown(MouseButton button)
        {
            if(mouseState.IsButtonDown(button) && !mouseInput[(int)button].pressed)
            {
                mouseInput[(int)button].pressed = true;
                pressedButtons.Add((int)button);
            }
            return mouseInput[(int)button].pressed && !mouseInput[(int)button].held;
        }
        public static bool GetMouseButton(MouseButton button)
        {
            return mouseState.IsButtonDown(button);
        }
        static void OnMouseMove(object sender, MouseMoveEventArgs e)
        {
            MousePosition = new Vector2(e.X, e.Y);
        }
    }
}
