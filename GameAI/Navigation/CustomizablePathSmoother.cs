using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.Navigation
{
    class CustomizablePathSmoother : IPathSmoother
    {
        private World world;
        private readonly int nodesAhead;

        public CustomizablePathSmoother(World world, int nodesAhead = 2)
        {
            this.world = world;
            this.nodesAhead = nodesAhead;
        }

        public IEnumerable<Vector2> SmoothPath(IEnumerable<Vector2> path)
        {
            LinkedList<Vector2> linkedPath = new LinkedList<Vector2>(path);
            // Select the first item of the path and yield return it
            LinkedListNode<Vector2> node = linkedPath.First;
            yield return node.Value;

            LinkedListNode<Vector2> lastNode = linkedPath.Last;
            bool collides = false;
            foreach (BaseGameEntity baseGameEntity in world.obstacles)
            {
                CircleF notAllowedZone = new CircleF(baseGameEntity.Pos.ToPoint(), baseGameEntity.Scale);
                if (notAllowedZone.Contains(lastNode.Value))
                {
                    collides = true;

                    break;
                }
            }

            if (collides) linkedPath.Remove(lastNode);

            // Loop through the path
            for (int i = 0; i < linkedPath.Count(); i++)
            {
                // Setting the distanceOrignal and add the first node to the calculation
                LinkedListNode<Vector2> add = node;
                float distOriginal = 0;

                // Loop through the amount of times is allowed
                for (int j = 0; j < nodesAhead; j++)
                {
                    if (add.Next == null) break;
                    add = add.Next;

                    if (add != node) distOriginal += Vector2.DistanceSquared(node.Value, add.Value);
                }

                // Small check to see if add was changed or is not null
                if (add != node)
                {
                    // Checking if the distance from the source to the second next node is faster by approaching it directly or stayed the same
                    // if that is the case, yield return the new add value
                    float distNew = Vector2.DistanceSquared(node.Value, add.Value);
                    bool collidesLine = false;
                    if (distNew <= distOriginal)
                    {
                        foreach (BaseGameEntity baseGameEntity in world.obstacles)
                        {
                            CircleF notAllowedZone =
                                new CircleF(baseGameEntity.Pos.ToPoint(), baseGameEntity.Scale);
                            Vector2 line = new Vector2(add.Value.X - node.Value.X, add.Value.Y - node.Value.Y);
                            IEnumerable<Vector2> checkpoints = new[]
                            {
                                line,
                                line / nodesAhead,
                            };
                            if (checkpoints.Any(checkpoint => notAllowedZone.Contains(checkpoint)))
                            {
                                collidesLine = true;

                                break;
                            }
                        }

                        if (!collidesLine)
                        {
                            node = add;
                            yield return add.Value;
                        }
                    }

                    if (distNew > distOriginal || collidesLine)
                    {
                        add = node;
                        yield return node.Value;

                        for (int j = 0; j < nodesAhead; j++)
                        {
                            if (add.Next == null) break;
                            add = add.Next;

                            if (add != node) yield return add.Value;
                        }
                    }
                }
            }
        }
    }
}
