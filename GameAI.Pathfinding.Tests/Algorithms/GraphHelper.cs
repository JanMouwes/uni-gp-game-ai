using GameAI.Pathfinding.Graph;

namespace GameAI.Pathfinding.Tests.Algorithms
{
    public static class GraphHelper
    {
        public static Graph.Graph CreateGraph_3Vertex_Simple()
        {
            Graph.Graph graph = new Graph.Graph();

            graph.AddVertex(new Vertex(1));
            graph.AddVertex(new Vertex(2));
            graph.AddVertex(new Vertex(3));
            
            graph.AddEdge(1, 2, 20);
            graph.AddEdge(2, 3, 10);
            graph.AddEdge(1, 3, 40);
            
            return graph;
        }
    }
}