using GameAI.Pathfinding.Algorithms;
using GameAI.Pathfinding.Graph;
using NUnit.Framework;

namespace GameAI.Pathfinding.Tests.Algorithms
{
    public class DijkstraTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void Test_Simple3VertexGraph()
        {
            Graph.Graph graph = GraphHelper.CreateGraph_3Vertex_Simple();

            Dijkstra.DijkstraResult result = new Dijkstra().Run(graph.GetVertex(1));

            Vertex vertex1 = graph.GetVertex(1);
            Vertex vertex2 = graph.GetVertex(2);
            Vertex vertex3 = graph.GetVertex(3);
            
            Assert.AreEqual(null, result.Results[vertex1].Item1);
            Assert.AreEqual(vertex1, result.Results[vertex2].Item1);
            Assert.AreEqual(vertex2, result.Results[vertex3].Item1);
        }
    }
}