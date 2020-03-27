using System.Collections;
using System.Collections.Generic;
using Graph;
using PriorityQueue;

namespace GameAI.Pathfinding.Algorithms.Dijkstra
{
    public class DijkstraIterator<TValue> : IEnumerator<(Vertex<TValue>, (Vertex<TValue>, double))>
    {
        private readonly Vertex<TValue> origin;
        private readonly Dictionary<Vertex<TValue>, DijkstraVertexInfo<TValue>> vertexMap = new Dictionary<Vertex<TValue>, DijkstraVertexInfo<TValue>>();
        private readonly PriorityQueue<DijkstraVertexInfo<TValue>> queue = new PriorityQueue<DijkstraVertexInfo<TValue>>();

        private DijkstraVertexInfo<TValue> current;

        public (Vertex<TValue>, (Vertex<TValue>, double)) Current => (this.current.Vertex, (this.current.Previous, this.current.Distance));
        object IEnumerator.Current => this.Current;

        public DijkstraIterator(Vertex<TValue> origin)
        {
            this.origin = origin;
            this.Init(this.origin);
        }

        private DijkstraVertexInfo<TValue> GetVertexInfo(Vertex<TValue> vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex)) { this.vertexMap[vertex] = new DijkstraVertexInfo<TValue>(vertex); }

            return this.vertexMap[vertex];
        }

        public bool MoveNext()
        {
            if (this.queue.Size == 0) { return false; }

            DijkstraVertexInfo<TValue> currentVertex = this.queue.Remove();

            bool isFarther = currentVertex.Distance > GetVertexInfo(currentVertex.Vertex).Distance;

            if (currentVertex.Known || isFarther) { return MoveNext(); }

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

            this.current = currentVertex;

            return true;
        }

        public void Reset()
        {
            this.queue.Clear();
            this.Init(this.origin);
        }

        private void Init(Vertex<TValue> withOrigin)
        {
            DijkstraVertexInfo<TValue> originVertexInfo = GetVertexInfo(withOrigin);
            originVertexInfo.Distance = 0;
            this.queue.Add(originVertexInfo);
        }

        public void Dispose() { }
    }
}