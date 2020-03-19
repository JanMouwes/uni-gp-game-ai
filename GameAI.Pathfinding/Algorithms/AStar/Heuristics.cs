using Graph;
using Microsoft.Xna.Framework;

namespace GameAI.Pathfinding.Algorithms.AStar
{
    public static class Heuristics
    {
        public static double None(Vertex<Vector2> from, Vertex<Vector2> to)
        {
            return 0;
        }

        public static double Manhattan(Vertex<Vector2> from, Vertex<Vector2> to)
        {
            return (to.Value.X - from.Value.X) + (to.Value.Y - from.Value.Y);
        }

        public static double Euclidean(Vertex<Vector2> from, Vertex<Vector2> to)
        {
            return Vector2.Distance(to.Value, from.Value);
        }
    }
}