using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity.GoalBehaviour.Rendering
{
    public class PathRenderer : IGoalRenderer
    {
        private readonly IEnumerable<Vector2> path;

        private readonly Color colour;

        public PathRenderer(IEnumerable<Vector2> path) : this(path, Color.Green) { }

        public PathRenderer(IEnumerable<Vector2> path, Color colour)
        {
            this.path = path;
            this.colour = colour;
        }

        public void Render(SpriteBatch spriteBatch)
        {
            Util.PathRenderer.RenderPath(spriteBatch, this.path, this.colour);
        }
    }
}