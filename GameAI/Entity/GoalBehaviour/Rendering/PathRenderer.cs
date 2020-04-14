using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.Entity.GoalBehaviour.Rendering
{
    public class PathRenderer : IGoalRenderer
    {
        private readonly IEnumerable<(Vector2 from, Vector2 to)> otherConsiderations;
        private readonly IEnumerable<Vector2> path;

        private readonly Color colour;

        public PathRenderer(IEnumerable<Vector2> path, IEnumerable<(Vector2 from, Vector2 to)> otherConsiderations) : this(path, otherConsiderations, Color.Green) { }

        public PathRenderer(IEnumerable<Vector2> path, IEnumerable<(Vector2 from, Vector2 to)> otherConsiderations, Color colour)
        {
            this.path = path;
            this.colour = colour;
            this.otherConsiderations = otherConsiderations;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            foreach ((Vector2 from, Vector2 to) in this.otherConsiderations)
            {
                spriteBatch.DrawLine(from, to, Color.Aquamarine);
            }

            Util.PathRenderer.RenderPath(spriteBatch, this.path, this.colour);
        }
    }
}