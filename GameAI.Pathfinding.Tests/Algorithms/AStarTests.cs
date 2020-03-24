using GameAI.Pathfinding.AStar;
using GameAI.Pathfinding.Dijkstra;
using Graph;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace GameAI.Pathfinding.Tests.Algorithms
{
    public class AStarTests
    {
        [Test]
        public void Test_Algorithm_Simple3VertexGraph()
        {
            Graph<Vector2> graph = GraphHelper.CreateGraph_3Vertex_Simple();

            DijkstraRunner<Vector2>.DijkstraResult result = new AStarRunner<Vector2>().Run(graph.GetVertex(1), graph.GetVertex(3), Heuristics.Manhattan);

            Vertex<Vector2> vertex1 = graph.GetVertex(1);
            Vertex<Vector2> vertex2 = graph.GetVertex(2);
            Vertex<Vector2> vertex3 = graph.GetVertex(3);

            Assert.AreEqual(null, result.Results[vertex1].Item1);
            Assert.AreEqual(vertex1, result.Results[vertex2].Item1);
            Assert.AreEqual(vertex2, result.Results[vertex3].Item1);
        }

        [Test]
        public void Test_Algorithm_GridGraphAdjacent5by5()
        {
            Graph<Vector2> graph = GraphHelper.CreateGridGraph_AdjacentOnly(5, 5);

            Vertex<Vector2> vertex1 = graph.GetVertex(1);
            Vertex<Vector2> vertex25 = graph.GetVertex(25);

            DijkstraRunner<Vector2>.DijkstraResult result = new AStarRunner<Vector2>().Run(graph.GetVertex(1), graph.GetVertex(25), Heuristics.Manhattan);

            Assert.AreEqual(null, result.Results[vertex1].Item1);
            Assert.AreEqual(8d, result.Results[vertex25].Item2);
        }

        [Test]
        public void Test_Algorithm_GridGraphDiagonal5by5()
        {
            Graph<Vector2> graph = GraphHelper.CreateGridGraph_DiagonalOnly(5, 5);

            Vertex<Vector2> vertex1 = graph.GetVertex(1);
            Vertex<Vector2> vertex25 = graph.GetVertex(25);

            DijkstraRunner<Vector2>.DijkstraResult result = new AStarRunner<Vector2>().Run(graph.GetVertex(1), graph.GetVertex(25), Heuristics.Manhattan);

            Assert.AreEqual(null, result.Results[vertex1].Item1);
            Assert.AreEqual(4d, result.Results[vertex25].Item2);
        }

        [Test]
        public void Test_Algorithm_GridGraphAllNeighbours5by5()
        {
            Graph<Vector2> graph = GraphHelper.CreateGridGraph_AllNeighbours(5, 5);

            Vertex<Vector2> vertex1 = graph.GetVertex(1);
            Vertex<Vector2> vertex25 = graph.GetVertex(25);

            DijkstraRunner<Vector2>.DijkstraResult result = new AStarRunner<Vector2>().Run(graph.GetVertex(1), graph.GetVertex(25), Heuristics.Manhattan);

            Assert.AreEqual(null, result.Results[vertex1].Item1);
            Assert.AreEqual(4d, result.Results[vertex25].Item2);
        }
    }
}