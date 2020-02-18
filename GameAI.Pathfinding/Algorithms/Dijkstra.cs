using System;
using System.Collections.Generic;
using GameAI.Pathfinding.Graph;
using GameAI.Pathfinding.PriorityQueue;

namespace GameAI.Pathfinding.Algorithms
{
    public class Dijkstra
    {
        private class VertexInfo : IComparable<VertexInfo>
        {
            public double Distance { get; set; } = double.PositiveInfinity;

            public Vertex Previous { get; set; }

            public Vertex Vertex { get; set; }

            public bool Known { get; set; }

            public VertexInfo(Vertex vertex)
            {
                this.Vertex = vertex;
            }

            public int CompareTo(VertexInfo other)
            {
                if (ReferenceEquals(this, other)) return 0;
                if (ReferenceEquals(null, other)) return 1;

                return this.Distance.CompareTo(other.Distance);
            }
        }

        public struct DijkstraResult
        {
            public Dictionary<Vertex, (Vertex, double)> Results { get; }

            public DijkstraResult(Dictionary<Vertex, (Vertex, double)> results) => this.Results = results;
        }

        private readonly Dictionary<Vertex, VertexInfo> vertexMap = new Dictionary<Vertex, VertexInfo>();

        private VertexInfo GetVertexInfo(Vertex vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex)) { this.vertexMap[vertex] = new VertexInfo(vertex); }

            return this.vertexMap[vertex];
        }

        private PriorityQueue<VertexInfo> queue = new PriorityQueue<VertexInfo>();

        public DijkstraResult Run(Vertex origin)
        {
            VertexInfo originVertexInfo = GetVertexInfo(origin);
            originVertexInfo.Distance = 0;
            this.queue.Add(originVertexInfo);

            while (this.queue.Size > 0)
            {
                VertexInfo currentVertex = this.queue.Remove();

                bool isFarther = currentVertex.Distance > GetVertexInfo(currentVertex.Vertex).Distance;

                if (currentVertex.Known || isFarther) { continue; }

                currentVertex.Known = true;

                this.vertexMap[currentVertex.Vertex] = currentVertex;

                foreach (Edge edge in currentVertex.Vertex.Edges)
                {
                    VertexInfo vertexInfo = new VertexInfo(edge.dest)
                    {
                        Distance = currentVertex.Distance + edge.cost,
                        Previous = currentVertex.Vertex
                    };

                    this.queue.Add(vertexInfo);
                }
            }

            Dictionary<Vertex, (Vertex, double)> results = new Dictionary<Vertex, (Vertex, double)>();

            foreach ((Vertex vertex, VertexInfo vertexInfo) in this.vertexMap) { results[vertex] = (vertexInfo.Previous, vertexInfo.Distance); }

            return new DijkstraResult(results);
        }
    }
}