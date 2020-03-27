using System;
using Graph;

namespace GameAI.Pathfinding.Algorithms.Dijkstra
{
    public class DijkstraVertexInfo<TValue> : IComparable<DijkstraVertexInfo<TValue>>
    {
        public double Distance { get; set; } = double.PositiveInfinity;

        public Vertex<TValue> Previous { get; set; }

        public Vertex<TValue> Vertex { get; set; }

        public bool Known { get; set; }

        public DijkstraVertexInfo(Vertex<TValue> vertex)
        {
            this.Vertex = vertex;
        }

        public int CompareTo(DijkstraVertexInfo<TValue> other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            return this.Distance.CompareTo(other.Distance);
        }
    }
}