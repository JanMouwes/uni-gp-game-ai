using System;
using GameAI.Pathfinding.Graph;

namespace GameAI.Pathfinding.Algorithms
{
    public class DijkstraVertexInfo : IComparable<DijkstraVertexInfo>
    {
        public double Distance { get; set; } = double.PositiveInfinity;

        public Vertex Previous { get; set; }

        public Vertex Vertex { get; set; }

        public bool Known { get; set; }

        public DijkstraVertexInfo(Vertex vertex)
        {
            this.Vertex = vertex;
        }

        public int CompareTo(DijkstraVertexInfo other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            return this.Distance.CompareTo(other.Distance);
        }
    }
}