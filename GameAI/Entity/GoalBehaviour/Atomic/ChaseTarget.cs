using GameAI.GoalBehaviour;
using GameAI.Steering.Simple;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Atomic
{
    public class ChaseTarget : Goal<Vehicle>
    {
        private readonly Vehicle target;
        private readonly float nearRange;

        public ChaseTarget(Vehicle owner, Vehicle target, float nearRange) : base(owner)
        {
            this.target = target;
            this.nearRange = nearRange;
        }

        public override void Activate()
        {
            this.Owner.Steering = new PursueBehaviour(this.Owner, this.target);

            base.Activate();
        }

        public override void Process(GameTime gameTime)
        {
            if (Vector2.DistanceSquared(this.Owner.Position, this.target.Position) < this.nearRange * this.nearRange) { this.Status = GoalStatus.Completed; }
        }
    }
}