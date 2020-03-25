using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace GameAI.Input
{
    public delegate void KeyHandler(Keys key, InputState keyState);

    //    TODO think of better name
    public class KeyboardInput
    {
        private Dictionary<Keys, bool> previousKeyStates;
        private Dictionary<Keys, bool> keyStates;

        private readonly Dictionary<(Keys, InputState), ICollection<KeyHandler>> handlers;

        public KeyboardInput()
        {
            this.previousKeyStates = new Dictionary<Keys, bool>();
            this.keyStates = new Dictionary<Keys, bool>();

            this.handlers = new Dictionary<(Keys, InputState), ICollection<KeyHandler>>();
        }

        public void Update(KeyboardState keyboardState)
        {
            SwapStates();

            // Add pressed keys
            foreach (Keys pressedKey in keyboardState.GetPressedKeys()) { this.keyStates.Add(pressedKey, true); }

            // Add released states
            foreach (KeyValuePair<Keys, bool> previousKeyState in this.previousKeyStates)
            {
                if (!this.keyStates.ContainsKey(previousKeyState.Key)) { this.keyStates.Add(previousKeyState.Key, false); }
            }

            foreach ((Keys, InputState) inputState in GetInputStates())
            {
                if (!this.handlers.ContainsKey(inputState)) { continue; }

                foreach (KeyHandler keyStateHandler in this.handlers[inputState]) { keyStateHandler(inputState.Item1, inputState.Item2); }
            }
        }

        public void AddEventListener(Keys key, InputState inputState, KeyHandler handler)
        {
            if (!this.handlers.ContainsKey((key, inputState))) { this.handlers[(key, inputState)] = new LinkedList<KeyHandler>(); }

            if (!this.keyStates.ContainsKey(key)) { this.keyStates.Add(key, false); }

            this.handlers[(key, inputState)].Add(handler);
        }

        public void OnKeyPress(Keys key, KeyHandler handler) => AddEventListener(key, InputState.Pressed, handler);
        public void OnKeyUp(Keys key, KeyHandler handler) => AddEventListener(key, InputState.Released, handler);

        private void SwapStates()
        {
            Dictionary<Keys, bool> tempKeyStates = this.previousKeyStates;

            this.previousKeyStates = this.keyStates;
            this.keyStates = tempKeyStates;

            this.keyStates.Clear();
        }


        private IEnumerable<(Keys, InputState)> GetInputStates()
        {
            foreach (KeyValuePair<Keys, bool> keyState in this.keyStates)
            {
                bool isPressed = keyState.Value;
                bool wasPressed = this.previousKeyStates.ContainsKey(keyState.Key) && this.previousKeyStates[keyState.Key];

                if (wasPressed) { yield return isPressed ? (keyState.Key, InputState.Held) : (keyState.Key, InputState.Released); }
                else { yield return isPressed ? (keyState.Key, InputState.Pressed) : (keyState.Key, InputState.Loose); }
            }
        }
    }

    public enum InputState
    {
        Pressed,
        Released,
        Held,
        Loose
    }
}