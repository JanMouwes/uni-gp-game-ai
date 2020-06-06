using System;

namespace GameAI.Pathfinding.Dijkstra
{
    public class VertexDijkstraData : IComparable<VertexDijkstraData>
    {
        public double TravelledDistance { get; set; }

        /// <summary>
        /// ID of previous vertex
        /// </summary>
        public int PreviousId { get; set; }

        /// <summary>
        /// ID of previous vertex 
        /// </summary>
        public int VertexId { get; }

        public bool Known { get; set; }

        public VertexDijkstraData(int vertexId)
        {
            this.VertexId = vertexId;
            this.TravelledDistance = double.PositiveInfinity;
            this.Known = false;
        }

        public int CompareTo(VertexDijkstraData other)
        {
            if (Equals(null, other)) return 1;
            if (this.VertexId == other.VertexId) return 0;

            return this.TravelledDistance.CompareTo(other.TravelledDistance);
        }

        public override string ToString()
        {
            return "(" + this.VertexId + " <-" + (int) this.TravelledDistance + "- " + this.PreviousId + ")";
        }
    }
}