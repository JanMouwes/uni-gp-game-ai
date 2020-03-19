using GameAI.Pathfinding.Algorithms.Dijkstra;
using Graph;

namespace GameAI.Pathfinding.Algorithms.AStar
{
    public class AStarVertexInfo<TValue> : DijkstraVertexInfo<TValue>
    {
        public AStarVertexInfo(Vertex<TValue> vertex) : base(vertex) { }

        public double HeuristicValue { get; set; }

        public int CompareTo(AStarVertexInfo<TValue> other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;

            return this.Distance.CompareTo(other.Distance + other.HeuristicValue);
        }
    }
}