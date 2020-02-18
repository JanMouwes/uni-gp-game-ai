using System;
using System.Collections;
using System.Collections.Generic;
using GameAI.Pathfinding.Graph;
using GameAI.Pathfinding.PriorityQueue;

namespace GameAI.Pathfinding.Algorithms
{
    public class DijkstraIterator : IEnumerator<(Vertex, (Vertex, double))>
    {
        private readonly Vertex origin;
        private readonly Dictionary<Vertex, DijkstraVertexInfo> vertexMap = new Dictionary<Vertex, DijkstraVertexInfo>();
        private readonly PriorityQueue<DijkstraVertexInfo> queue = new PriorityQueue<DijkstraVertexInfo>();

        private DijkstraVertexInfo current;

        public (Vertex, (Vertex, double)) Current => (this.current.Vertex, (this.current.Previous, this.current.Distance));
        object IEnumerator.Current => this.Current;

        public DijkstraIterator(Vertex origin)
        {
            this.origin = origin;
            this.Init(this.origin);
        }

        private DijkstraVertexInfo GetVertexInfo(Vertex vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex)) { this.vertexMap[vertex] = new DijkstraVertexInfo(vertex); }

            return this.vertexMap[vertex];
        }

        public bool MoveNext()
        {
            if (this.queue.Size == 0) { return false; }

            DijkstraVertexInfo currentVertex = this.queue.Remove();

            bool isFarther = currentVertex.Distance > GetVertexInfo(currentVertex.Vertex).Distance;

            if (currentVertex.Known || isFarther) { return MoveNext(); }

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

            this.current = currentVertex;

            return true;
        }

        public void Reset()
        {
            this.queue.Clear();
            this.Init(this.origin);
        }

        private void Init(Vertex withOrigin)
        {
            DijkstraVertexInfo originVertexInfo = GetVertexInfo(withOrigin);
            originVertexInfo.Distance = 0;
            this.queue.Add(originVertexInfo);
        }

        public void Dispose() { }
    }
}