using Microsoft.Xna.Framework;

namespace GameAI.Entity.Steering.Simple
{
    public class SeekBehaviour : SteeringBehaviour
    {
        public Vector2 Target { get; set; }

        public SeekBehaviour(MovingEntity entity, Vector2 target) : base(entity)
        {
            this.Target = target;
        }

        public override Vector2 Calculate()
        {
            return SteeringBehaviours.Seek(this.Target, this.Entity);
        }
    }
}