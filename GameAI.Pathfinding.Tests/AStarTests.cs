using System.Linq;
using GameAI.Pathfinding.AStar;
using Graph;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace GameAI.Pathfinding.Tests
{
    public class AStarTests
    {
        [Test]
        public void Test_Algorithm_Simple3VertexGraph()
        {
            Graph<Vector2> graph = GraphHelper.CreateGraph_3Vertex_Simple();
            AStarRunner<Vector2> runner = new AStarRunner<Vector2>(graph);

            Vertex<Vector2>[] result = runner.Run(graph.GetVertex(1), graph.GetVertex(3), Heuristics.Manhattan)
                                             .ToArray();

            Vertex<Vector2> vertex1 = graph.GetVertex(1);
            Vertex<Vector2> vertex2 = graph.GetVertex(2);

            Assert.AreEqual(vertex1, result[0]);
            Assert.AreEqual(vertex2, result[1]);
        }

        [TestCase(1, 2, 2)]
        [TestCase(1, 7, 3)]
        [TestCase(1, 5, 5)]
        [TestCase(1, 15, 7)]
        public void Test_Algorithm_GridGraphAdjacent5by5(int sourceId, int destId, int expectedLength)
        {
            Graph<Vector2> graph = GraphHelper.CreateGridGraph_AdjacentOnly(5, 5);

            Vertex<Vector2> source = graph.GetVertex(sourceId);
            Vertex<Vector2> dest = graph.GetVertex(destId);

            Vertex<Vector2>[] result = new AStarRunner<Vector2>(graph).Run(source, dest, Heuristics.Manhattan).ToArray();

            int pathLength = result.Length;

            Assert.AreEqual(expectedLength, pathLength);
        }

        [TestCase(1, 7, 2)]
        [TestCase(1, 5, 5)]
        [TestCase(1, 15, 5)]
        public void Test_Algorithm_GridGraphDiagonal5by5(int sourceId, int destId, int expectedLength)
        {
            Graph<Vector2> graph = GraphHelper.CreateGridGraph_DiagonalOnly(5, 5);

            Vertex<Vector2> source = graph.GetVertex(sourceId);
            Vertex<Vector2> dest = graph.GetVertex(destId);

            Vertex<Vector2>[] result = new AStarRunner<Vector2>(graph).Run(source, dest, Heuristics.Manhattan).ToArray();

            int pathLength = result.Length;

            Assert.AreEqual(expectedLength, pathLength);
        }

        [TestCase(1, 2, 2)]
        [TestCase(1, 7, 2)]
        [TestCase(1, 5, 5)]
        [TestCase(1, 15, 5)]
        public void Test_Algorithm_GridGraphAllNeighbours5by5(int sourceId, int destId, int expectedLength)
        {
            Graph<Vector2> graph = GraphHelper.CreateGridGraph_AllNeighbours(5, 5);

            Vertex<Vector2> source = graph.GetVertex(sourceId);
            Vertex<Vector2> dest = graph.GetVertex(destId);

            Vertex<Vector2>[] result = new AStarRunner<Vector2>(graph).Run(source, dest, Heuristics.Manhattan).ToArray();

            int pathLength = result.Length;

            Assert.AreEqual(expectedLength, pathLength);
        }
    }
}