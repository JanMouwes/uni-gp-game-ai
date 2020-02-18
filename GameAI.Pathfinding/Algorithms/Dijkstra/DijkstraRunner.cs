using System.Collections.Generic;
using GameAI.Pathfinding.Graph;
using GameAI.Pathfinding.PriorityQueue;

namespace GameAI.Pathfinding.Algorithms.Dijkstra
{
    public class DijkstraRunner
    {
        public struct DijkstraResult
        {
            public Dictionary<Vertex, (Vertex, double)> Results { get; }

            public DijkstraResult(Dictionary<Vertex, (Vertex, double)> results) => this.Results = results;
        }

        private readonly Dictionary<Vertex, DijkstraVertexInfo> vertexMap = new Dictionary<Vertex, DijkstraVertexInfo>();

        private readonly PriorityQueue<DijkstraVertexInfo> queue = new PriorityQueue<DijkstraVertexInfo>();

        public DijkstraResult Run(Vertex origin)
        {
            DijkstraVertexInfo originDijkstraVertexInfo = GetDijkstraVertexInfo(origin);
            originDijkstraVertexInfo.Distance = 0;
            this.queue.Add(originDijkstraVertexInfo);

            while (this.queue.Size > 0)
            {
                DijkstraVertexInfo currentVertex = this.queue.Remove();

                bool isFarther = currentVertex.Distance > GetDijkstraVertexInfo(currentVertex.Vertex).Distance;

                if (currentVertex.Known || isFarther) { continue; }

                currentVertex.Known = true;

                this.vertexMap[currentVertex.Vertex] = currentVertex;

                foreach (Edge edge in currentVertex.Vertex.Edges)
                {
                    DijkstraVertexInfo vertexInfo = new DijkstraVertexInfo(edge.dest)
                    {
                        Distance = currentVertex.Distance + edge.cost,
                        Previous = currentVertex.Vertex
                    };

                    this.queue.Add(vertexInfo);
                }
            }

            Dictionary<Vertex, (Vertex, double)> results = new Dictionary<Vertex, (Vertex, double)>();

            foreach ((Vertex vertex, DijkstraVertexInfo vertexInfo) in this.vertexMap) { results[vertex] = (vertexInfo.Previous, vertexInfo.Distance); }

            return new DijkstraResult(results);
        }

        private DijkstraVertexInfo GetDijkstraVertexInfo(Vertex vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex)) { this.vertexMap[vertex] = new DijkstraVertexInfo(vertex); }

            return this.vertexMap[vertex];
        }
    }
}