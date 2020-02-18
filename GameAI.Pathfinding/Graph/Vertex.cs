using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace GameAI.Pathfinding.Graph
{
    public class Vertex : IVertex
    {
        public readonly int Id;

        public readonly LinkedList<Edge> Edges;

        public Vector2 Position { get; set; }

        public Vertex(int id)
        {
            this.Id = id;

            this.Edges = new LinkedList<Edge>();
        }

        public override string ToString()
        {
            string currentVertexString = this.Id.ToString();
            string neighbourString = this.Edges.Any() ? $" [ {string.Join(" ", this.Edges.Select(edge => $"{edge.dest.Id}({edge.cost})"))} ] " : string.Empty;

            return currentVertexString + neighbourString;
        }
    }
}