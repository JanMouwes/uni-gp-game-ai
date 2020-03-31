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
            LinkedListNode<Vector2> node = path.First;
            yield return node.Value;
            for (int i = 0; i < path.Count(); i++)
            {
                LinkedListNode<Vector2> add = node;
                float distOriginal = 0;
                for (int j = 0; j < nodesAhead; j++)
                {
                    if (add == null) break;
                    try {
                        add = add.Next;
                    }
                    catch
                    {
                        break;
                    }

                    if (add != node && add != null) distOriginal += Vector2.DistanceSquared(node.Value, add.Value);
                }

                if (add != node && add != null)
                {
                    // Checking if the distance from the source to the second next node is faster by approaching it directly
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
