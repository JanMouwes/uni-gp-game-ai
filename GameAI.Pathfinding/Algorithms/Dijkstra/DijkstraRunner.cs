using System.Collections.Generic;
using GameAI.Pathfinding.PriorityQueue;
using Graph;

namespace GameAI.Pathfinding.Algorithms.Dijkstra
{
    public class DijkstraRunner<TValue>
    {
        public struct DijkstraResult
        {
            public Dictionary<Vertex<TValue>, (Vertex<TValue>, double)> Results { get; }

            public DijkstraResult(Dictionary<Vertex<TValue>, (Vertex<TValue>, double)> results) => this.Results = results;
        }

        private readonly Dictionary<Vertex<TValue>, DijkstraVertexInfo<TValue>> vertexMap = new Dictionary<Vertex<TValue>, DijkstraVertexInfo<TValue>>();

        private readonly PriorityQueue<DijkstraVertexInfo<TValue>> queue = new PriorityQueue<DijkstraVertexInfo<TValue>>();

        public DijkstraResult Run(Vertex<TValue> origin)
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
                    DijkstraVertexInfo<TValue> vertexInfo = new DijkstraVertexInfo<TValue>(edge.Dest)
                    {
                        Distance = currentVertex.Distance + edge.Cost,
                        Previous = currentVertex.Vertex
                    };

                    this.queue.Add(vertexInfo);
                }
            }

            Dictionary<Vertex<TValue>, (Vertex<TValue>, double)> results = new Dictionary<Vertex<TValue>, (Vertex<TValue>, double)>();

            foreach ((Vertex<TValue> vertex, DijkstraVertexInfo<TValue> vertexInfo) in this.vertexMap) { results[vertex] = (vertexInfo.Previous, vertexInfo.Distance); }

            return new DijkstraResult(results);
        }

        private DijkstraVertexInfo<TValue> GetDijkstraVertexInfo(Vertex<TValue> vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex)) { this.vertexMap[vertex] = new DijkstraVertexInfo<TValue>(vertex); }

            return this.vertexMap[vertex];
        }
    }
}