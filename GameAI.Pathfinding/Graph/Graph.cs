using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GameAI.Pathfinding.Graph
{
    public class Graph : IGraph
    {
        public static readonly double INFINITY = System.Double.MaxValue;

        private Dictionary<string, Vertex> vertexMap;

        public Graph()
        {
            this.vertexMap = new Dictionary<string, Vertex>();
        }

        public Vertex GetVertex(string name)
        {
            if (!this.vertexMap.ContainsKey(name)) { this.vertexMap[name] = new Vertex(name); }

            return this.vertexMap[name];
        }

        public void AddEdge(string source, string dest, double cost)
        {
            Vertex sourceVertex = GetVertex(source);
            Vertex destVertex = GetVertex(dest);

            Edge edge = new Edge(destVertex, cost);

            sourceVertex.Edges.AddLast(edge);
        }

        public void ClearAll()
        {
            foreach ((string _, Vertex vertex) in this.vertexMap) { vertex.Edges.Clear(); }
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach ((string _, Vertex vertex) in this.vertexMap)
            {
                stringBuilder.Append(vertex);
                stringBuilder.Append("\n");
            }

            return stringBuilder.ToString();
        }
    }
}