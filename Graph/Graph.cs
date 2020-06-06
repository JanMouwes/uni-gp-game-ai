using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Graph
{
    public class Graph<TValue> : IGraph<TValue>
    {
        private readonly Dictionary<int, Vertex<TValue>> vertexMap;

        public IEnumerable<Vertex<TValue>> Vertices => this.vertexMap.Values;

        public Graph()
        {
            this.vertexMap = new Dictionary<int, Vertex<TValue>>();
        }

        public Vertex<TValue> GetVertex(int id)
        {
            if (!this.vertexMap.ContainsKey(id)) { this.vertexMap[id] = new Vertex<TValue>(id); }

            return this.vertexMap[id];
        }

        public void AddVertex(Vertex<TValue> vertex)
        {
            if (this.vertexMap.ContainsKey(vertex.Id)) { throw new ArgumentException(); }

            this.vertexMap[vertex.Id] = vertex;
        }

        /// <summary>
        /// Removes vertex and all edges to this vertex. Very expensive, don't use if you don't have to
        /// </summary>
        /// <param name="vertex">Vertex to remove</param>
        public void RemoveVertex(Vertex<TValue> vertex)
        {
            RemoveVertex(vertex.Id);
        }

        /// <summary>
        /// Removes vertex and all edges to this vertex. Very expensive, don't use if you don't have to
        /// </summary>
        /// <param name="vertexId">Vertex Id</param>
        public void RemoveVertex(int vertexId)
        {
            this.vertexMap.Remove(vertexId);

            // Vertices that have an edge to current vertex
            IEnumerable<Vertex<TValue>> vertices = Vertices.Where(vertex => vertex.Edges.Any(edge => edge.Dest.Id == vertexId));

            foreach (Vertex<TValue> vertex in vertices) { vertex.Edges.RemoveWhere(edge => edge.Dest.Id == vertexId); }
        }

        public void AddEdge(int source, int dest, double cost)
        {
            Vertex<TValue> sourceVertex = GetVertex(source);
            Vertex<TValue> destVertex = GetVertex(dest);

            sourceVertex.AddEdge(destVertex, cost);
        }

        public void ClearAll()
        {
            foreach (KeyValuePair<int, Vertex<TValue>> pair in this.vertexMap) { pair.Value.Edges.Clear(); }

            this.vertexMap.Clear();
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (KeyValuePair<int, Vertex<TValue>> pair in this.vertexMap)
            {
                stringBuilder.Append(pair.Value);
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}