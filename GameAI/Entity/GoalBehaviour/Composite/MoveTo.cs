using System.Collections.Generic;
using System.Linq;
using GameAI.Entity.GoalBehaviour.Rendering;
using GameAI.Entity.Navigation;
using Graph;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class MoveTo<TOwner> : GoalComposite<TOwner> where TOwner : MovingEntity
    {
        public readonly IEnumerable<Vector2> Path;

        public MoveTo(TOwner owner, Vector2 target, PathFinder pathFinder) : base(owner)
        {
            this.Path = pathFinder.FindPath(this.Owner.Position, target, out IEnumerable<(Vertex<Vector2> @from, Vertex<Vector2> to)> consideredVertices);

            this.Renderer = new PathRenderer(this.Path, consideredVertices.Select(item => (item.from.Value, item.to.Value)));

            AddSubgoal(new FollowPath<TOwner>(owner, this.Path));
        }
    }
}