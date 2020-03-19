using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public class Vertex<TValue>
    {
        public readonly int Id;

        public readonly SortedSet<Edge<TValue>> Edges;

        public TValue Value;

        public Vertex(int id, TValue value = default)
        {
            this.Id = id;
            this.Value = value;

            this.Edges = new SortedSet<Edge<TValue>>();
        }

        public void AddEdge(Vertex<TValue> dest, double cost)
        {
            Edge<TValue> edge = new Edge<TValue>(this, dest, cost);
            this.Edges.Add(edge);
        }

        public override string ToString()
        {
            string currentVertexString = this.Id.ToString();
            string neighbourString = this.Edges.Any() ? $" [ {string.Join(" ", this.Edges.Select(edge => $"{edge.Dest.Id}({edge.Cost})"))} ] " : string.Empty;

            return currentVertexString + neighbourString;
        }
    }
}