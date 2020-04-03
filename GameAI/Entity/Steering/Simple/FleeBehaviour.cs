using GameAI.Entity;
using Microsoft.Xna.Framework;

namespace GameAI.Steering.Simple
{
    public class FleeBehaviour : SteeringBehaviour
    {
        public MovingEntity Target { get; set; }

        public FleeBehaviour(MovingEntity entity, MovingEntity target) : base(entity)
        {
            this.Target = target;
        }

        public override Vector2 Calculate()
        {
            return SteeringBehaviours.Flee(this.Target.Position, this.Entity);
        }
    }
}