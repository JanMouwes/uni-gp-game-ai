using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameAI.Pathfinding.Graph
{
    public class Graph : IGraph
    {
        public const double INFINITY = double.MaxValue;

        private Dictionary<int, Vertex> vertexMap;

        public Graph()
        {
            this.vertexMap = new Dictionary<int, Vertex>();
        }

        public Vertex GetVertex(int id)
        {
            if (!this.vertexMap.ContainsKey(id)) { this.vertexMap[id] = new Vertex(id); }

            return this.vertexMap[id];
        }

        public void AddVertex(Vertex vertex)
        {
            if (this.vertexMap.ContainsKey(vertex.Id)) { throw new ArgumentException(); }

            this.vertexMap[vertex.Id] = vertex;
        }

        public void AddEdge(int source, int dest, double cost)
        {
            Vertex sourceVertex = GetVertex(source);
            Vertex destVertex = GetVertex(dest);

            Edge edge = new Edge(destVertex, cost);

            sourceVertex.Edges.AddLast(edge);
        }

        public void ClearAll()
        {
            foreach ((int _, Vertex vertex) in this.vertexMap) { vertex.Edges.Clear(); }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach ((int _, Vertex vertex) in this.vertexMap)
            {
                stringBuilder.Append(vertex);
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}