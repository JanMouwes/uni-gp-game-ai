using GameAI.Pathfinding.Graph;
using Microsoft.Xna.Framework;

namespace GameAI.Pathfinding.Algorithms.AStar
{
    public static class Heuristics
    {
        public static double None(Vertex from, Vertex to)
        {
            return 0;
        }

        public static double Manhattan(Vertex from, Vertex to)
        {
            return (to.Position.X - from.Position.X) + (to.Position.Y - from.Position.Y);
        }

        public static double Euclidean(Vertex from, Vertex to)
        {
            return Vector2.Distance(to.Position, from.Position);
        }
    }
}