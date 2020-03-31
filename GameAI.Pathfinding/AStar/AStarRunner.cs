using System.Collections.Generic;
using Graph;
using PriorityQueue;

namespace GameAI.Pathfinding.AStar
{
    public delegate double Heuristic<TValue>(Vertex<TValue> from, Vertex<TValue> to);

    public class AStarRunner<TValue>
    {
        private readonly Graph<TValue> graph;
        // private readonly Dictionary<Vertex<TValue>, VertexAStarData<TValue>> vertexMap = new Dictionary<Vertex<TValue>, VertexAStarData<TValue>>();

        /// <summary>
        /// Vertices by ID
        /// </summary>
        private readonly Dictionary<int, Vertex<TValue>> vertices;

        private readonly Dictionary<int, VertexAStarData> vertexData;

        private readonly PriorityQueue<VertexAStarData> queue = new PriorityQueue<VertexAStarData>();

        public AStarRunner(Graph<TValue> graph)
        {
            this.graph = graph;

            this.vertices = new Dictionary<int, Vertex<TValue>>();
            this.vertexData = new Dictionary<int, VertexAStarData>();

            foreach (Vertex<TValue> vertex in graph.Vertices)
            {
                this.vertices[vertex.Id] = vertex;
                this.vertexData.Add(vertex.Id, new VertexAStarData(vertex.Id));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="destination"></param>
        /// <param name="heuristic"></param>
        /// <returns>Path of vertices, including origin and destination</returns>
        public IEnumerable<Vertex<TValue>> Run(Vertex<TValue> origin, Vertex<TValue> destination, Heuristic<TValue> heuristic)
        {
            VertexAStarData originData = this.vertexData[origin.Id];
            originData.TravelledDistance = 0;
            this.queue.Add(originData);

            while (this.queue.Size > 0)
            {
                VertexAStarData currentVertex = this.queue.Remove();

                if (this.vertexData[currentVertex.VertexId].Known) { continue; }

                this.vertexData[currentVertex.VertexId] = currentVertex;

                this.vertexData[currentVertex.VertexId].Known = true;

                foreach (Edge<TValue> edge in this.vertices[currentVertex.VertexId].Edges)
                {
                    double heuristicDist = heuristic(edge.Dest, destination);

                    // Prevent adding to queue if already known
                    if (this.vertexData[edge.Dest.Id].Known) { continue; }

                    VertexAStarData data = new VertexAStarData(edge.Dest.Id)
                    {
                        TravelledDistance = currentVertex.TravelledDistance + edge.Cost,
                        HeuristicValue = heuristicDist,
                        PreviousId = currentVertex.VertexId
                    };

                    this.queue.Add(data);
                }
            }

            LinkedList<Vertex<TValue>> results = new LinkedList<Vertex<TValue>>();

            VertexAStarData currentNodeInfo = this.vertexData[destination.Id];

            while (currentNodeInfo.VertexId != origin.Id)
            {
                if (!currentNodeInfo.Known) { throw new PathNotFoundException(); }

                VertexAStarData previous = this.vertexData[currentNodeInfo.PreviousId];
                // Next node & cost to get there
                results.AddFirst(this.vertices[currentNodeInfo.VertexId]);

                currentNodeInfo = previous;
            }

            results.AddFirst(origin);

            return results;
        }
    }
}