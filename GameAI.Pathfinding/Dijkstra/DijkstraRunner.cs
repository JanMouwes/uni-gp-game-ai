using System.Collections.Generic;
using Graph;
using PriorityQueue;

namespace GameAI.Pathfinding.Dijkstra
{
    public class DijkstraRunner<TValue>
    {
        // private readonly Dictionary<Vertex<TValue>, VertexDijkstraData> vertexMap = new Dictionary<Vertex<TValue>, VertexDijkstraData>();
        private readonly Dictionary<int, VertexDijkstraData> vertexMap = new Dictionary<int, VertexDijkstraData>();
        private readonly Dictionary<int, Vertex<TValue>> vertices = new Dictionary<int, Vertex<TValue>>();

        private readonly PriorityQueue<VertexDijkstraData> queue = new PriorityQueue<VertexDijkstraData>();

        public DijkstraRunner(Graph<TValue> graph)
        {
            foreach (Vertex<TValue> vertex in graph.Vertices) { this.vertices[vertex.Id] = vertex; }
        }

        public IEnumerable<(Vertex<TValue>dest, double cost)> Run(Vertex<TValue> origin)
        {
            VertexDijkstraData originVertexDijkstraData = GetVertexInfo(origin.Id);
            originVertexDijkstraData.TravelledDistance = 0;
            this.queue.Add(originVertexDijkstraData);

            while (this.queue.Size > 0)
            {
                VertexDijkstraData currentVertex = this.queue.Remove();

                if (currentVertex.Known) { continue; }

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
            }

            LinkedList<(Vertex<TValue>, double)> results = new LinkedList<(Vertex<TValue>, double)>();

            foreach (KeyValuePair<int, VertexDijkstraData> pair in this.vertexMap)
            {
                // Next node & cost to get there
                results.AddLast((this.vertices[pair.Key], pair.Value.TravelledDistance));
            }

            return results;
        }

        private VertexDijkstraData GetVertexInfo(int vertexId)
        {
            if (!this.vertexMap.ContainsKey(vertexId)) { this.vertexMap[vertexId] = new VertexDijkstraData(vertexId); }

            return this.vertexMap[vertexId];
        }
    }
}