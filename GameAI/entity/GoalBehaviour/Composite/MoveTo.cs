using System.Collections.Generic;
using System.Threading;
using GameAI.entity;
using GameAI.Navigation;
using Microsoft.Xna.Framework;

namespace GameAI.GoalBehaviour.Composite
{
    public class MoveTo<TOwner> : GoalComposite<TOwner> where TOwner : MovingEntity
    {
        public readonly IEnumerable<Vector2> Path;

        public MoveTo(TOwner owner, Vector2 target, PathFinder pathFinder) : base(owner)
        {
            this.Path = pathFinder.FindPath(this.Owner.Pos, target);
            this.AddSubgoal(new FollowPath<TOwner>(owner, this.Path));
        }
    }
}