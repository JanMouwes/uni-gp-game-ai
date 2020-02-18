using GameAI.Pathfinding.Algorithms.AStar;
using GameAI.Pathfinding.Algorithms.Dijkstra;
using GameAI.Pathfinding.Graph;
using NUnit.Framework;

namespace GameAI.Pathfinding.Tests.Algorithms
{
    public class AStarTests
    {
        [Test]
        public void Test_Algorithm_Simple3VertexGraph()
        {
            Graph.Graph graph = GraphHelper.CreateGraph_3Vertex_Simple();

            DijkstraRunner.DijkstraResult result = new AStarRunner().Run(graph.GetVertex(1), graph.GetVertex(3), Heuristics.Manhattan);

            Vertex vertex1 = graph.GetVertex(1);
            Vertex vertex2 = graph.GetVertex(2);
            Vertex vertex3 = graph.GetVertex(3);

            Assert.AreEqual(null, result.Results[vertex1].Item1);
            Assert.AreEqual(vertex1, result.Results[vertex2].Item1);
            Assert.AreEqual(vertex2, result.Results[vertex3].Item1);
        }

        [Test]
        public void Test_Algorithm_GridGraphAdjacent5by5()
        {
            Graph.Graph graph = GraphHelper.CreateGridGraph_AdjacentOnly(5, 5);

            Vertex vertex1 = graph.GetVertex(1);
            Vertex vertex25 = graph.GetVertex(25);

            DijkstraRunner.DijkstraResult result = new AStarRunner().Run(graph.GetVertex(1), graph.GetVertex(25), Heuristics.Manhattan);

            Assert.AreEqual(null, result.Results[vertex1].Item1);
            Assert.AreEqual(8d, result.Results[vertex25].Item2);
        }
        
        [Test]
        public void Test_Algorithm_GridGraphDiagonal5by5()
        {
            Graph.Graph graph = GraphHelper.CreateGridGraph_DiagonalOnly(5, 5);

            Vertex vertex1 = graph.GetVertex(1);
            Vertex vertex25 = graph.GetVertex(25);

            DijkstraRunner.DijkstraResult result = new AStarRunner().Run(graph.GetVertex(1), graph.GetVertex(25), Heuristics.Manhattan);

            Assert.AreEqual(null, result.Results[vertex1].Item1);
            Assert.AreEqual(4d, result.Results[vertex25].Item2);
        }
        
        [Test]
        public void Test_Algorithm_GridGraphAllNeighbours5by5()
        {
            Graph.Graph graph = GraphHelper.CreateGridGraph_AllNeighbours(5, 5);

            Vertex vertex1 = graph.GetVertex(1);
            Vertex vertex25 = graph.GetVertex(25);

            DijkstraRunner.DijkstraResult result = new AStarRunner().Run(graph.GetVertex(1), graph.GetVertex(25), Heuristics.Manhattan);

            Assert.AreEqual(null, result.Results[vertex1].Item1);
            Assert.AreEqual(4d, result.Results[vertex25].Item2);
        }
    }
}