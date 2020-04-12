using System.Collections.Generic;
using GameAI.Entity.GoalBehaviour.Rendering;
using GameAI.Entity.Navigation;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class MoveTo<TOwner> : GoalComposite<TOwner> where TOwner : MovingEntity
    {
        public readonly IEnumerable<Vector2> Path;

        public MoveTo(TOwner owner, Vector2 target, PathFinder pathFinder) : base(owner)
        {
            this.Path = pathFinder.FindPath(this.Owner.Position, target);

            this.Renderer = new PathRenderer(this.Path);
            
            this.AddSubgoal(new FollowPath<TOwner>(owner, this.Path));
        }
    }
}