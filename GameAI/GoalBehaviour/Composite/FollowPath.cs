using System.Collections.Generic;
using GameAI.entity;
using GameAI.GoalBehaviour.Atomic;
using Microsoft.Xna.Framework;

namespace GameAI.GoalBehaviour.Composite
{
    public class FollowPath<TOwner> : GoalComposite<TOwner> where TOwner : MovingEntity
    {
        public FollowPath(TOwner owner, IEnumerable<Vector2> path) : base(owner)
        {
            foreach (Vector2 target in path) { AddWaypoint(target); }
        }

        public void AddWaypoint(Vector2 waypoint)
        {
            this.AddSubgoal(new TraverseEdge<TOwner>(this.Owner, waypoint));
        }
    }
}