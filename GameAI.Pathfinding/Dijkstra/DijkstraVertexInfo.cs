using System;
using Graph;

namespace GameAI.Pathfinding.Dijkstra
{
    public class DijkstraVertexInfo<TValue> : IComparable<DijkstraVertexInfo<TValue>>
    {
        public double TravelledDistance { get; set; }

        public double TotalDistance => TravelledDistance;

        public Vertex<TValue> Previous { get; set; }

        public Vertex<TValue> Vertex { get; }

        public bool Known { get; set; }

        public DijkstraVertexInfo(Vertex<TValue> vertex)
        {
            this.Vertex = vertex;
            this.TravelledDistance = double.PositiveInfinity;
            this.Known = false;
        }

        public virtual int CompareTo(DijkstraVertexInfo<TValue> other)
        {
            if (Equals(null, other)) return 1;
            if (this.Vertex == other.Vertex) return 0;

            return this.TotalDistance.CompareTo(other.TotalDistance);
        }

        public override string ToString()
        {
            return "(" + this.Vertex.Id + " <-" + (int) this.TravelledDistance + "- " + this.Previous.Id + ")";
        }
    }
}