using Microsoft.Xna.Framework;

namespace GameAI.Entity.Steering.Complex
{
    public class SteeringUnion : SteeringBehaviour
    {
        private readonly SteeringBehaviour behaviourA;
        private readonly SteeringBehaviour behaviourB;

        public float WeightA { get; set; } = 1;
        public float WeightB { get; set; } = 1;

        public SteeringUnion(MovingEntity owner, SteeringBehaviour behaviourA, SteeringBehaviour behaviourB) : base(owner)
        {
            this.behaviourA = behaviourA;
            this.behaviourB = behaviourB;
        }


        public override Vector2 Calculate()
        {
            return this.behaviourA.Calculate() * this.WeightA + this.behaviourB.Calculate() * this.WeightB;
        }
    }
}