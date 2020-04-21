using System.Collections.Generic;
using System.Linq;
using GameAI.Navigation;
using GameAI.Pathfinding.AStar;
using Graph;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.Navigation
{
    public class PathFinder
    {
        private Graph<Vector2> graph;
        private IPathSmoother pathSmoother;

        public PathFinder(Graph<Vector2> graph, IPathSmoother pathSmoother)
        {
            this.graph = graph;
            this.pathSmoother = pathSmoother;
        }

        public PathFinder(Graph<Vector2> graph) : this(graph, new DefaultPathSmoother()) { }

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

        public IEnumerable<Vector2> FindPath(Vector2 source, Vector2 destination, out IEnumerable<(Vertex<Vector2> from, Vertex<Vector2> to)> consideredVertices)
        {
            Vertex<Vector2> sourceVertex = GetNearestVertex(source);
            Vertex<Vector2> destVertex = GetNearestVertex(destination);

            AStarRunner<Vector2> runner = new AStarRunner<Vector2>(this.graph);
            IEnumerable<Vertex<Vector2>> result = runner.Run(sourceVertex, destVertex, Heuristics.Manhattan);
            consideredVertices = runner.ConsideredEdges;

            LinkedList<Vector2> path = new LinkedList<Vector2>(result.Select(item => item.Value));

            path.AddFirst(source);
            path.AddLast(destination);

            return this.pathSmoother.SmoothPath(path);
        }
    }
}