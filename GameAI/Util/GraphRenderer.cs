using Microsoft.Xna.Framework.Graphics;
using Graph;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.Util
{
    public class GraphRenderer
    {
        private Graph<Vector2> graph;
        private readonly SpriteFont font;
        private readonly Color colour;

        public bool Enabled { get; set; }

        public GraphRenderer(Graph<Vector2> graph, SpriteFont font, Color colour)
        {
            this.graph = graph;
            this.font = font;
            this.colour = colour;
        }

        public void ToggleEnabled() => this.Enabled = !this.Enabled;

        public void Render(SpriteBatch spriteBatch)
        {
            if (!Enabled) { return; }

            foreach (Vertex<Vector2> graphVertex in this.graph.Vertices)
            {
                spriteBatch.DrawPoint(graphVertex.Value, Color.Aqua, 3f);
                // spriteBatch.DrawString(this.font, graphVertex.Id.ToString(), graphVertex.Value, this.colour);

                foreach (Edge<Vector2> edge in graphVertex.Edges)
                {
                    // Vector2 from = edge.Source.Value;
                    // Vector2 to = edge.Dest.Value;
                    // Vector2 halfway = edge.Source.Value + ((to - from) / 2);
                    // int distance = (int)Vector2.Distance(from, to);
                    // spriteBatch.DrawString(font, distance.ToString(), halfway, colour);

                    Vector2 towardsDest = edge.Source.Value + (edge.Dest.Value - edge.Source.Value).NormalizedCopy() * 15;

                    spriteBatch.DrawLine(edge.Source.Value, edge.Dest.Value, this.colour);
                    // spriteBatch.DrawPoint(towardsDest, Color.DarkRed, 5f);
                    spriteBatch.DrawString(this.font, edge.Dest.Id.ToString(), towardsDest, Color.DarkRed);
                }
            }
        }
    }
}