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

        public LeaderFollowingBehaviour(MovingEntity entity, MovingEntity target, Vector2 offset) : base(entity)
        {
            this.Target = target;
            this.Offset = offset;
        }

        public override Vector2 Calculate()
        {
            return SteeringBehaviours.LeaderFollowing(Target, Entity, Offset);
        }
    }
}
