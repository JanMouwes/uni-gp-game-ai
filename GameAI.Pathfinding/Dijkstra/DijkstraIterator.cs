using System.Collections;
using System.Collections.Generic;
using Graph;
using PriorityQueue;

namespace GameAI.Pathfinding.Dijkstra
{
    public class DijkstraIterator<TValue> : IEnumerator<(Vertex<TValue>, (Vertex<TValue>, double))>
    {
        private readonly Vertex<TValue> origin;
        private readonly Dictionary<int, VertexDijkstraData> vertexMap = new Dictionary<int, VertexDijkstraData>();
        private readonly Dictionary<int, Vertex<TValue>> vertices = new Dictionary<int, Vertex<TValue>>();

        private readonly PriorityQueue<VertexDijkstraData> queue = new PriorityQueue<VertexDijkstraData>();

        private int currentVertexId;

        public (Vertex<TValue>, (Vertex<TValue>, double)) Current =>
            (this.vertices[currentVertexId],
             (
                 this.vertices[this.vertexMap[currentVertexId].PreviousId],
                 this.vertexMap[currentVertexId].TravelledDistance
             ));

        object IEnumerator.Current => this.Current;

        public DijkstraIterator(Graph<TValue> graph, Vertex<TValue> origin)
        {
            this.origin = origin;

            foreach (Vertex<TValue> vertex in graph.Vertices) { this.vertices[vertex.Id] = vertex; }

            this.Init(this.origin);
        }

        private VertexDijkstraData GetVertexInfo(Vertex<TValue> vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex.Id)) { this.vertexMap[vertex.Id] = new VertexDijkstraData(vertex.Id); }

            return this.vertexMap[vertex.Id];
        }

        public bool MoveNext()
        {
            if (this.queue.Size == 0) { return false; }

            VertexDijkstraData currentVertex = this.queue.Remove();

            if (currentVertex.Known) { return MoveNext(); }

            currentVertex.Known = true;

            this.vertexMap[currentVertex.VertexId] = currentVertex;

            foreach (Edge<TValue> edge in this.vertices[currentVertex.VertexId].Edges)
            {
                VertexDijkstraData data = new VertexDijkstraData(edge.Dest.Id)
                {
                    TravelledDistance = currentVertex.TravelledDistance + edge.Cost,
                    PreviousId = currentVertex.VertexId
                };

                this.queue.Add(data);
            }

            this.currentVertexId = currentVertex.VertexId;

            return true;
        }

        public void Reset()
        {
            this.queue.Clear();
            this.Init(this.origin);
        }

        private void Init(Vertex<TValue> withOrigin)
        {
            VertexDijkstraData originData = GetVertexInfo(withOrigin);
            originData.TravelledDistance = 0;
            this.queue.Add(originData);
        }

        public void Dispose() { }
    }
}