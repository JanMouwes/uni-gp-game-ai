using System.Collections.Generic;
using GameAI.Pathfinding.Algorithms.Dijkstra;
using GameAI.Pathfinding.Graph;
using GameAI.Pathfinding.PriorityQueue;
using Microsoft.Xna.Framework;

namespace GameAI.Pathfinding.Algorithms.AStar
{
    public delegate double Heuristic(Vertex from, Vertex to);

    public class AStarRunner
    {
        private readonly Dictionary<Vertex, DijkstraVertexInfo> vertexMap = new Dictionary<Vertex, DijkstraVertexInfo>();

        private readonly PriorityQueue<DijkstraVertexInfo> queue = new PriorityQueue<DijkstraVertexInfo>();

        public DijkstraRunner.DijkstraResult Run(Vertex origin, Vertex target, Heuristic heuristic)
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
                    double heuristicDist = heuristic(edge.dest, target);

                    AStarVertexInfo vertexInfo = new AStarVertexInfo(edge.dest)
                    {
                        Distance = currentVertex.Distance + edge.cost,
                        HeuristicValue = heuristicDist,
                        Previous = currentVertex.Vertex
                    };

                    this.queue.Add(vertexInfo);
                }
            }

            Dictionary<Vertex, (Vertex, double)> results = new Dictionary<Vertex, (Vertex, double)>();

            foreach ((Vertex vertex, DijkstraVertexInfo vertexInfo) in this.vertexMap) { results[vertex] = (vertexInfo.Previous, vertexInfo.Distance); }

            return new DijkstraRunner.DijkstraResult(results);
        }

        private DijkstraVertexInfo GetDijkstraVertexInfo(Vertex vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex)) { this.vertexMap[vertex] = new DijkstraVertexInfo(vertex); }

            return this.vertexMap[vertex];
        }
    }
}