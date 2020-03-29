using System.Collections.Generic;
using Graph;
using PriorityQueue;

namespace GameAI.Pathfinding.Dijkstra
{
    public class DijkstraRunner<TValue>
    {
        private readonly Dictionary<Vertex<TValue>, DijkstraVertexInfo<TValue>> vertexMap = new Dictionary<Vertex<TValue>, DijkstraVertexInfo<TValue>>();

        private readonly PriorityQueue<DijkstraVertexInfo<TValue>> queue = new PriorityQueue<DijkstraVertexInfo<TValue>>();

        public IEnumerable<(Vertex<TValue>dest, double cost)> Run(Vertex<TValue> origin)
        {
            DijkstraVertexInfo<TValue> originDijkstraVertexInfo = GetDijkstraVertexInfo(origin);
            originDijkstraVertexInfo.TravelledDistance = 0;
            this.queue.Add(originDijkstraVertexInfo);

            while (this.queue.Size > 0)
            {
                DijkstraVertexInfo<TValue> currentVertex = this.queue.Remove();

                bool isFarther = currentVertex.TravelledDistance > GetDijkstraVertexInfo(currentVertex.Vertex).TravelledDistance;

                if (currentVertex.Known || isFarther) { continue; }

                currentVertex.Known = true;

                this.vertexMap[currentVertex.Vertex] = currentVertex;

                foreach (Edge<TValue> edge in currentVertex.Vertex.Edges)
                {
                    DijkstraVertexInfo<TValue> vertexInfo = new DijkstraVertexInfo<TValue>(edge.Dest)
                    {
                        TravelledDistance = currentVertex.TravelledDistance + edge.Cost,
                        Previous = currentVertex.Vertex
                    };

                    this.queue.Add(vertexInfo);
                }
            }

            LinkedList<(Vertex<TValue>, double)> results = new LinkedList<(Vertex<TValue>, double)>();

            foreach (KeyValuePair<Vertex<TValue>, DijkstraVertexInfo<TValue>> pair in this.vertexMap)
            {
                // Next node & cost to get there
                results.AddLast((pair.Key, pair.Value.TravelledDistance));
            }

            return results;
        }

        private DijkstraVertexInfo<TValue> GetDijkstraVertexInfo(Vertex<TValue> vertex)
        {
            if (!this.vertexMap.ContainsKey(vertex)) { this.vertexMap[vertex] = new DijkstraVertexInfo<TValue>(vertex); }

            return this.vertexMap[vertex];
        }
    }
}