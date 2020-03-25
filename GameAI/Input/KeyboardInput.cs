using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace GameAI.Input
{
    //    TODO think of better name
    public class KeyboardInput : Input<Keys>
    {
        public void Update(KeyboardState keyboardState)
        {
            base.Update(keyboardState.GetPressedKeys());
        }
    }
}