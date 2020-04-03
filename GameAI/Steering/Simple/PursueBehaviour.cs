using GameAI.entity;
using Microsoft.Xna.Framework;

namespace GameAI.Steering.Simple
{
    public class PursueBehaviour : SteeringBehaviour
    {
        public MovingEntity Target { get; set; }

        public PursueBehaviour(MovingEntity entity, MovingEntity target) : base(entity)
        {
            this.Target = target;
        }

        public override Vector2 Calculate()
        {
            return SteeringBehaviours.Pursue(this.Target, this.Entity);
        }
    }
}