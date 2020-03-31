using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameAI.Navigation
{
    public class DefaultPathSmoother : IPathSmoother
    {
        /// <summary>
        /// Does not smooth paths
        /// </summary>
        /// <inheritdoc cref="IPathSmoother#SmoothPath"/>
        public IEnumerable<Vector2> SmoothPath(LinkedList<Vector2> path)
        {
            // First remove points with the first node as base point
            LinkedListNode<Vector2> node = path.First;
            for (int i = 0; i < path.Count; i++)
            {
                // Checking if this is the last item in the linkedlist
                if (node.Next == null) break;
                LinkedListNode<Vector2> one = node.Next;
                // Checking to see if one is the last item in the linkedlist
                if (one.Next == null) break;
                LinkedListNode<Vector2> two = one.Next;

                // Checking if the distance from the source to the second next node is faster by approaching it directly
                float distOriginal = Vector2.DistanceSquared(node.Value, one.Value) + Vector2.DistanceSquared(one.Value, two.Value);
                float distNew = Vector2.DistanceSquared(node.Value, two.Value);
                if (distNew <= distOriginal) path.Remove((one));
                
                node = node.Next;
            }
            return path;
        }
    }
}