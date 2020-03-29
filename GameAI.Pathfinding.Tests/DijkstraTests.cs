using System.Collections.Generic;
using System.Linq;
using GameAI.Pathfinding.Dijkstra;
using Graph;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace GameAI.Pathfinding.Tests
{
    public class DijkstraTests
    {
        [SetUp]
        public void Setup() { }

        [Test]
        public void Test_Algorithm_Simple3VertexGraph()
        {
            Graph<Vector2> graph = GraphHelper.CreateGraph_3Vertex_Simple();

            (Vertex<Vector2>dest, double cost)[] result = new DijkstraRunner<Vector2>().Run(graph.GetVertex(1)).ToArray();

            Vertex<Vector2> vertex1 = graph.GetVertex(1);
            Vertex<Vector2> vertex2 = graph.GetVertex(2);
            Vertex<Vector2> vertex3 = graph.GetVertex(3);

            Assert.AreEqual(vertex1, result[0].Item1);
            Assert.AreEqual(vertex2, result[1].Item1);
        }

        [Test]
        public void Test_Iterator_Simple3VertexGraph()
        {
            Graph<Vector2> graph = GraphHelper.CreateGraph_3Vertex_Simple();

            Vertex<Vector2> vertex1 = graph.GetVertex(1);
            Vertex<Vector2> vertex2 = graph.GetVertex(2);
            Vertex<Vector2> vertex3 = graph.GetVertex(3);

            DijkstraIterator<Vector2> iterator = new DijkstraIterator<Vector2>(vertex1);

            Dictionary<Vertex<Vector2>, (Vertex<Vector2>, double)> results = new Dictionary<Vertex<Vector2>, (Vertex<Vector2>, double)>();

            while (iterator.MoveNext())
            {
                (Vertex<Vector2> vertex, (Vertex<Vector2>, double) previousDistance) = iterator.Current;

                results[vertex] = previousDistance;
            }

            Assert.AreEqual(null, results[vertex1].Item1);
            Assert.AreEqual(vertex1, results[vertex2].Item1);
            Assert.AreEqual(vertex2, results[vertex3].Item1);
        }
    }
}