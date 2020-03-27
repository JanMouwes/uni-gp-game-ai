using System.Collections.Generic;
using System.Linq;
using Graph;
using GameAI.Pathfinding.AStar;
using GameAI.Pathfinding.Dijkstra;
using Microsoft.Xna.Framework;

namespace GameAI.Navigation
{
    public class NavigationGraph
    {
        private Graph<Vector2> graph;
        private IPathSmoother pathSmoother;

        public NavigationGraph(Graph<Vector2> graph, IPathSmoother pathSmoother)
        {
            this.graph = graph;
            this.pathSmoother = pathSmoother;
        }

        public NavigationGraph(Graph<Vector2> graph) : this(graph, new DefaultPathSmoother()) { }

        public Vertex<Vector2> GetNearestVertex(Vector2 source)
        {
            Vertex<Vector2> nearest = null;
            float nearestDistanceSquared = float.MaxValue;

            foreach (Vertex<Vector2> graphVertex in this.graph.Vertices)
            {
                float currentDistance = Vector2.DistanceSquared(graphVertex.Value, source);

                if (currentDistance < nearestDistanceSquared)
                {
                    nearest = graphVertex;
                    nearestDistanceSquared = currentDistance;
                }
            }

            return nearest;
        }

        public IEnumerable<Vector2> FindPath(Vector2 source, Vector2 destination)
        {
            Vertex<Vector2> sourceVertex = GetNearestVertex(source);
            Vertex<Vector2> destVertex = GetNearestVertex(destination);

            DijkstraRunner<Vector2>.DijkstraResult result = new AStarRunner<Vector2>().Run(sourceVertex, destVertex, Heuristics.Euclidean);

            IEnumerable<Vector2> path = result.Results.Keys.Select(item => item.Value);

            return this.pathSmoother.SmoothPath(path);
        }
    }
}