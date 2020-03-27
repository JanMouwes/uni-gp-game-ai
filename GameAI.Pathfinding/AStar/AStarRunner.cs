using System.Collections.Generic;
using GameAI.Pathfinding.Algorithms.Dijkstra;
using GameAI.Pathfinding.Dijkstra;
using Graph;
using PriorityQueue;

namespace GameAI.Pathfinding.AStar
{
    public delegate double Heuristic<TValue>(Vertex<TValue> from, Vertex<TValue> to);

    public class AStarRunner<TValue>
    {
        private readonly Dictionary<Vertex<TValue>, DijkstraVertexInfo<TValue>> vertexMap = new Dictionary<Vertex<TValue>, DijkstraVertexInfo<TValue>>();

        private readonly PriorityQueue<DijkstraVertexInfo<TValue>> queue = new PriorityQueue<DijkstraVertexInfo<TValue>>();

        public DijkstraRunner<TValue>.DijkstraResult Run(Vertex<TValue> origin, Vertex<TValue> target, Heuristic<TValue> heuristic)
        {
            DijkstraVertexInfo<TValue> originDijkstraVertexInfo = GetDijkstraVertexInfo(origin);
            originDijkstraVertexInfo.Distance = 0;
            this.queue.Add(originDijkstraVertexInfo);

            while (this.queue.Size > 0)
            {
                DijkstraVertexInfo<TValue> currentVertex = this.queue.Remove();

                bool isFarther = currentVertex.Distance > GetDijkstraVertexInfo(currentVertex.Vertex).Distance;

                if (currentVertex.Known || isFarther) { continue; }

                currentVertex.Known = true;

                this.vertexMap[currentVertex.Vertex] = currentVertex;

                foreach (Edge<TValue> edge in currentVertex.Vertex.Edges)
                {
                    double heuristicDist = heuristic(edge.Dest, target);

                    AStarVertexInfo<TValue> vertexInfo = new AStarVertexInfo<TValue>(edge.Dest)
                    {
                        Distance = currentVertex.Distance + edge.Cost,
                        HeuristicValue = heuristicDist,
                        Previous = currentVertex.Vertex
                    };

                    this.queue.Add(vertexInfo);
                }
            }

            Dictionary<Vertex<TValue>, (Vertex<TValue>, double)> results = new Dictionary<Vertex<TValue>, (Vertex<TValue>, double)>();

            foreach (KeyValuePair<Vertex<TValue>, DijkstraVertexInfo<TValue>> pair in this.vertexMap)
            {
                results[pair.Key] = (pair.Value.Previous, pair.Value.Distance);
            }

            return new DijkstraRunner<TValue>.DijkstraResult(results);
        }

        private DijkstraVertexInfo<TValue> GetDijkstraVertexInfo(Vertex<TValue> vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex)) { this.vertexMap[vertex] = new DijkstraVertexInfo<TValue>(vertex); }

            return this.vertexMap[vertex];
        }
    }
}