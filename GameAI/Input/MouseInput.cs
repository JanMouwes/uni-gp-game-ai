using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;

namespace GameAI.Input
{
    public class MouseInput : Input<MouseButtons>
    {
        public void Update(MouseState mouseState)
        {
            this.Update(GetPressedMouseButtons(mouseState));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static IEnumerable<MouseButtons> GetPressedMouseButtons(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed) { yield return MouseButtons.Left; }

            if (mouseState.RightButton == ButtonState.Pressed) { yield return MouseButtons.Right; }

            if (mouseState.MiddleButton == ButtonState.Pressed) { yield return MouseButtons.Middle; }
        }
    }

    public enum MouseButtons
    {
        Left,
        Right,
        Middle
    }
}