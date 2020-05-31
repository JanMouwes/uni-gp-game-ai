using Microsoft.Xna.Framework;

namespace GameAI.Entity.Steering.Complex
{
    public class SteeringUnion : SteeringBehaviour
    {
        private readonly SteeringBehaviour behaviourA;
        private readonly SteeringBehaviour behaviourB;

        public SteeringUnion(MovingEntity owner, SteeringBehaviour behaviourA, SteeringBehaviour behaviourB) : base(owner)
        {
            this.behaviourA = behaviourA;
            this.behaviourB = behaviourB;
        }


        public override Vector2 Calculate()
        {
            return this.behaviourA.Calculate() + this.behaviourB.Calculate();
        }
    }
}