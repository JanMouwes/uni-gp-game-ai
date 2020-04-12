using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.Util
{
    public class PathRenderer
    {
        private readonly IEnumerable<Vector2> path;
        private readonly Color colour;
        private readonly SpriteFont font;

        public PathRenderer(IEnumerable<Vector2> path, Color colour, SpriteFont font)
        {
            this.path = path;
            this.colour = colour;
            this.font = font;
        }

        public void Render(SpriteBatch spriteBatch) => RenderPath(spriteBatch, this.font, this.path, this.colour);

        public static void RenderPath(SpriteBatch spriteBatch, SpriteFont font, IEnumerable<Vector2> path, Color colour)
        {
            Vector2 previous = Vector2.Zero;
            bool hasPrevious = false;


            foreach (Vector2 current in path)
            {
                if (hasPrevious)
                {
                    Vector2 halfway = previous + ((current - previous) / 2);
                    int distance = (int) Vector2.Distance(previous, current);

                    spriteBatch.DrawLine(previous, current, colour);
                    spriteBatch.DrawString(font, distance.ToString(), halfway, colour);
                }

                hasPrevious = true;
                previous = current;
            }
        }

        public static void RenderPath(SpriteBatch spriteBatch, IEnumerable<Vector2> path, Color colour)
        {
            Vector2 previous = Vector2.Zero;
            bool hasPrevious = false;

            foreach (Vector2 current in path)
            {
                if (hasPrevious) { spriteBatch.DrawLine(previous, current, colour); }
                else { hasPrevious = true; }

                previous = current;
            }
        }
    }
}