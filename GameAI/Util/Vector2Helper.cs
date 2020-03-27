using System;
using System.Collections.Generic;
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

        public static IEnumerable<Vector2> NeighboursAdjacent(float x, float y)
        {
            yield return new Vector2(x - 1, y); /* Left */
            yield return new Vector2(x + 1, y); /* Right */

            yield return new Vector2(x, y - 1); /* Top */
            yield return new Vector2(x, y + 1); /* Bottom */
        }

        public static IEnumerable<Vector2> NeighboursDiagonal(float x, float y)
        {
            yield return new Vector2(x - 1, y - 1); /* LeftTop */
            yield return new Vector2(x + 1, y - 1); /* RightTop */
            yield return new Vector2(x - 1, y + 1); /* LeftBottom */
            yield return new Vector2(x + 1, y + 1); /* RightBottom */
        }

        public static IEnumerable<Vector2> Neighbours(float x, float y)
        {
            foreach (Vector2 vector2 in NeighboursAdjacent(x, y)) { yield return vector2; }

            foreach (Vector2 vector2 in NeighboursDiagonal(x, y)) { yield return vector2; }
        }
    }
}