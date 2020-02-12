using System;
using Microsoft.Xna.Framework;

namespace GameAI.Util
{
    public static class Vector2Helper
    {
        public static Vector2 FromAngle(float angle)
        {
            return new Vector2
            {
                X = (float) Math.Cos(angle),
                Y = (float) Math.Sin(angle)
            };
        }
    }
}