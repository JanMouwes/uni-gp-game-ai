using System.Collections.Generic;
using Graph;
using PriorityQueue;

namespace GameAI.Pathfinding.AStar
{
    public delegate double Heuristic<TValue>(Vertex<TValue> from, Vertex<TValue> to);

    public class AStarRunner<TValue>
    {
        private readonly Dictionary<Vertex<TValue>, AStarVertexInfo<TValue>> vertexMap = new Dictionary<Vertex<TValue>, AStarVertexInfo<TValue>>();

        private readonly PriorityQueue<AStarVertexInfo<TValue>> queue = new PriorityQueue<AStarVertexInfo<TValue>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="heuristic"></param>
        /// <returns>Path of vertices, including origin and destination</returns>
        public IEnumerable<Vertex<TValue>> Run(Vertex<TValue> origin, Vertex<TValue> destination, Heuristic<TValue> heuristic)
        {
            AStarVertexInfo<TValue> originVertexInfo = GetVertexInfo(origin);
            originVertexInfo.TravelledDistance = 0;
            this.queue.Add(originVertexInfo);

            while (this.queue.Size > 0)
            {
                AStarVertexInfo<TValue> currentVertex = this.queue.Remove();

                bool isFarther = currentVertex.TravelledDistance > GetVertexInfo(currentVertex.Vertex).TravelledDistance;

                if (currentVertex.Known || isFarther) { continue; }

                currentVertex.Known = true;

                this.vertexMap[currentVertex.Vertex] = currentVertex;

                foreach (Edge<TValue> edge in currentVertex.Vertex.Edges)
                {
                    double heuristicDist = heuristic(edge.Dest, destination);

                    AStarVertexInfo<TValue> vertexInfo = new AStarVertexInfo<TValue>(edge.Dest)
                    {
                        TravelledDistance = currentVertex.TravelledDistance + edge.Cost,
                        HeuristicValue = heuristicDist,
                        Previous = currentVertex.Vertex
                    };

                    this.queue.Add(vertexInfo);
                }
            }

            LinkedList<Vertex<TValue>> results = new LinkedList<Vertex<TValue>>();

            AStarVertexInfo<TValue> currentNodeInfo = GetVertexInfo(destination);

            while (currentNodeInfo.Vertex != origin)
            {
                if (currentNodeInfo.Previous == null) { throw new PathNotFoundException(); }

                AStarVertexInfo<TValue> previous = GetVertexInfo(currentNodeInfo.Previous);
                // Next node & cost to get there
                results.AddFirst(currentNodeInfo.Vertex);

                currentNodeInfo = previous;
            }

            results.AddFirst(origin);

            return results;
        }

        private AStarVertexInfo<TValue> GetVertexInfo(Vertex<TValue> vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex)) { this.vertexMap[vertex] = new AStarVertexInfo<TValue>(vertex); }

            return this.vertexMap[vertex];
        }
    }
}