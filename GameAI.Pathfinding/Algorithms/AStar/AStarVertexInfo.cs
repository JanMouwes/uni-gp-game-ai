using GameAI.Pathfinding.Algorithms.Dijkstra;
using GameAI.Pathfinding.Graph;

namespace GameAI.Pathfinding.Algorithms.AStar
{
    public class AStarVertexInfo : DijkstraVertexInfo
    {
        public AStarVertexInfo(Vertex vertex) : base(vertex) { }

        public double HeuristicValue { get; set; }

        public int CompareTo(AStarVertexInfo other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            return this.Distance.CompareTo(other.Distance + other.HeuristicValue);
        }
    }
}