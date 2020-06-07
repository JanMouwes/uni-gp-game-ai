using Microsoft.Xna.Framework;

namespace GameAI.Entity.Steering.Complex
{
    public class LeaderFollowingBehaviour : SteeringBehaviour
    {
        private MovingEntity Target { get; }
        public Vector2 Offset { get; set; }

        public LeaderFollowingBehaviour(MovingEntity entity, MovingEntity target, Vector2 offset) : base(entity)
        {
            this.Target = target;
            this.Offset = offset;
        }

        public override Vector2 Calculate()
        {
            return SteeringBehaviours.LeaderFollowing(this.Target, this.Entity, this.Offset);
        }
    }
}
