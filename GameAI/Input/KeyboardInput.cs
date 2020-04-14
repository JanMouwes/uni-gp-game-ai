using Microsoft.Xna.Framework.Input;

namespace GameAI.Input
{
    //    TODO think of better name
    public class KeyboardInput : Input<Keys>
    {
        public KeyboardState KeyboardState { get; set; }

        public void Update(KeyboardState keyboardState)
        {
            KeyboardState = keyboardState;
            base.Update(keyboardState.GetPressedKeys());
        }
    }
}