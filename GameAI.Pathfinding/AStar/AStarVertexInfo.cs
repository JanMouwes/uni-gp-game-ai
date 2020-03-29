using GameAI.Pathfinding.Dijkstra;
using Graph;

namespace GameAI.Pathfinding.AStar
{
    public class AStarVertexInfo<TValue> : DijkstraVertexInfo<TValue>
    {
        public new double TotalDistance => TravelledDistance + HeuristicValue;

        public AStarVertexInfo(Vertex<TValue> vertex) : base(vertex) { }

        public double HeuristicValue { get; set; }

        public override string ToString()
        {
            return "(" + this.Vertex.Id + " <-" + ((int) this.TravelledDistance + (int) HeuristicValue) + "- " + this.Previous.Id + ")";
        }
    }
}