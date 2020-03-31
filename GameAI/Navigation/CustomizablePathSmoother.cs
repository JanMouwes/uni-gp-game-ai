using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace GameAI.Navigation
{
    class CustomizablePathSmoother : IPathSmoother
    {
        private readonly int nodesAhead;
        public CustomizablePathSmoother(int nodesAhead = 2)
        {
            this.nodesAhead = nodesAhead;
        }

        public IEnumerable<Vector2> SmoothPath(LinkedList<Vector2> path)
        {
            // Select the first item of the path and yield return it
            LinkedListNode<Vector2> node = path.First;
            yield return node.Value;

            // Loop through the path
            for (int i = 0; i < path.Count(); i++)
            {
                // Setting the distanceOrignal and add the first node to the calculation
                LinkedListNode<Vector2> add = node;
                float distOriginal = 0;

                // Loop through the amount of times is allowed
                for (int j = 0; j < nodesAhead; j++)
                {
                    add = add.Next;

                    if (add == null) break;
                    if (add != node) distOriginal += Vector2.DistanceSquared(node.Value, add.Value);
                }

                // Small check to see if add was changed or is not null
                if (add != node && add != null)
                {
                    // Checking if the distance from the source to the second next node is faster by approaching it directly or stayed the same
                    // if that is the case, yield return the new add value
                    float distNew = Vector2.DistanceSquared(node.Value, add.Value);
                    if (distNew <= distOriginal)
                    {
                        node = add;
                        yield return add.Value;
                    }
                }
            }
        }
    }
}
