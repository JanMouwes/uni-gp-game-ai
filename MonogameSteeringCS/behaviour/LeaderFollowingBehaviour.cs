using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAI.entity;
using Microsoft.Xna.Framework;

namespace GameAI.behaviour
{
    class LeaderFollowingBehaviour : SteeringBehaviour
    {
        public MovingEntity Target { get; set; }
        public Vector2 Offset { get; set; }
        public Vector2 Old { get; set; }

        public LeaderFollowingBehaviour(MovingEntity entity, MovingEntity target, Vector2 offset) : base(entity)
        {
            this.Target = target;
            this.Offset = offset;
            this.Old = new Vector2(10, 10);
        }

        public override Vector2 Calculate()
        {
            Old = SteeringBehaviours.LeaderFollowing(Target, Entity, Offset, Old);
            return Old;
        }
    }
}
