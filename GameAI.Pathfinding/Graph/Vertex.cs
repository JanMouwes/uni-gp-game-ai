using System.Collections.Generic;
using System.Linq;

namespace GameAI.Pathfinding.Graph
{
    public class Vertex : IVertex
    {
        public readonly int Id;

        public readonly LinkedList<Edge> Edges;

        #region Search-specific attributes

        public double Dist;
        public IVertex Prev;
        public bool Known = false; //    Scratch

        #endregion

        public Vertex(int id)
        {
            this.Id = id;

            this.Edges = new LinkedList<Edge>();
        }

        public void Reset()
        {
            this.Known = false;
            this.Dist = int.MaxValue;
        }

        public override string ToString()
        {
            string currentVertexString = this.Known ? $"{this.Id}({this.Dist})" : this.Id.ToString();
            string neighbourString = this.Edges.Any() ? $" [ {string.Join(" ", this.Edges.Select(edge => $"{edge.dest.Id}({edge.cost})"))} ] " : string.Empty;

            return currentVertexString + neighbourString;
        }
    }
}