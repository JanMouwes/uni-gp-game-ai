using GameAI.Entity;
using Microsoft.Xna.Framework;

namespace GameAI.Steering.Simple
{
    public class ArriveBehaviour : SteeringBehaviour
    {
        private readonly float decelerateDistance;
        public Vector2 Target { get; set; }

        public ArriveBehaviour(MovingEntity entity, Vector2 target, float decelerateDistance) : base(entity)
        {
            this.Target = target;
            this.decelerateDistance = decelerateDistance;
        }

        public override Vector2 Calculate()
        {
            return SteeringBehaviours.Arrive(this.Target, this.Entity, this.decelerateDistance);
        }
    }
}