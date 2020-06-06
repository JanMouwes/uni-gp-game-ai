using System;

namespace GameAI.Pathfinding.AStar
{
    public class VertexAStarData : IComparable<VertexAStarData>
    {
        private double TotalDistance => TravelledDistance + HeuristicValue;

        public readonly int VertexId;
        public int PreviousId { get; set; }

        public double TravelledDistance { get; set; }
        public double HeuristicValue { get; set; }

        public bool Known { get; set; }

        public VertexAStarData(int vertexId)
        {
            this.VertexId = vertexId;
        }

        public int CompareTo(VertexAStarData other)
        {
            if (Equals(null, other)) return 1;
            if (this.VertexId == other.VertexId) return 0;

            return this.TotalDistance.CompareTo(other.TotalDistance);
        }


        public override string ToString()
        {
            return "(" + this.VertexId + " <-" + ((int) this.TravelledDistance + (int) HeuristicValue) + "- " + this.PreviousId + ")";
        }
    }
}