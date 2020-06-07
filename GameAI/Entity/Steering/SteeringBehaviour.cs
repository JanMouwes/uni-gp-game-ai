using Microsoft.Xna.Framework;

namespace GameAI.Entity.Steering
{
    public abstract class SteeringBehaviour
    {
        public MovingEntity Entity { get; set; }
        public abstract Vector2 Calculate();
        public float Strength { get; set; } = 1f;

        public SteeringBehaviour(MovingEntity entity)
        {
            this.Entity = entity;
        }
    }
}