using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace GameAI.Input
{
    public abstract class Input<TInputElement>
    {
        public delegate void InputHandler(TInputElement input, InputState keyState);

        private Dictionary<TInputElement, bool> previousInputStates;
        private Dictionary<TInputElement, bool> inputStates;

        private readonly Dictionary<(TInputElement, InputState), ICollection<InputHandler>> handlers;

        public Input()
        {
            this.previousInputStates = new Dictionary<TInputElement, bool>();
            this.inputStates = new Dictionary<TInputElement, bool>();

            this.handlers = new Dictionary<(TInputElement, InputState), ICollection<InputHandler>>();
        }

        public void Update(IEnumerable<TInputElement> pressedInputElements)
        {
            SwapStates();

            // Add pressed keys
            foreach (TInputElement pressedKey in pressedInputElements) { this.inputStates.Add(pressedKey, true); }

            // Add released states
            foreach (KeyValuePair<TInputElement, bool> previousKeyState in this.previousInputStates)
            {
                if (!this.inputStates.ContainsKey(previousKeyState.Key)) { this.inputStates.Add(previousKeyState.Key, false); }
            }

            foreach ((TInputElement, InputState) inputState in GetInputStates())
            {
                if (!this.handlers.ContainsKey(inputState)) { continue; }

                foreach (InputHandler keyStateHandler in this.handlers[inputState]) { keyStateHandler(inputState.Item1, inputState.Item2); }
            }
        }

        public void AddEventListener(TInputElement button, InputState inputState, InputHandler handler)
        {
            if (!this.handlers.ContainsKey((button, inputState))) { this.handlers[(button, inputState)] = new LinkedList<InputHandler>(); }

            if (!this.inputStates.ContainsKey(button)) { this.inputStates.Add(button, false); }

            this.handlers[(button, inputState)].Add(handler);
        }

        public void OnKeyPress(TInputElement button, InputHandler handler) => AddEventListener(button, InputState.Pressed, handler);
        public void OnKeyUp(TInputElement button, InputHandler handler) => AddEventListener(button, InputState.Released, handler);

        private void SwapStates()
        {
            Dictionary<TInputElement, bool> tempKeyStates = this.previousInputStates;

            this.previousInputStates = this.inputStates;
            this.inputStates = tempKeyStates;

            this.inputStates.Clear();
        }

        private IEnumerable<(TInputElement, InputState)> GetInputStates()
        {
            foreach (KeyValuePair<TInputElement, bool> keyState in this.inputStates)
            {
                bool isPressed = keyState.Value;
                bool wasPressed = this.previousInputStates.ContainsKey(keyState.Key) && this.previousInputStates[keyState.Key];

                if (wasPressed) { yield return isPressed ? (keyState.Key, InputState.Held) : (keyState.Key, InputState.Released); }
                else { yield return isPressed ? (keyState.Key, InputState.Pressed) : (keyState.Key, InputState.Loose); }
            }
        }
    }
}