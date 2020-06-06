using GameAI.Entity.Steering.Simple;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Atomic
{
    public class SeekTarget : Goal<Ship>
    {
        private readonly Ship target;
        private readonly float nearRange;
        private Vector2 currentTarget;

        public SeekTarget(Ship owner, Ship target, float nearRange) : base(owner)
        {
            this.target = target;
            this.nearRange = nearRange;
        }

        public void ReseekTarget()
        {
            this.currentTarget = this.target.Position;
            this.Owner.Steering = new SeekBehaviour(this.Owner, this.target.Position);
        }

        public override void Activate()
        {
            ReseekTarget();

            base.Activate();
        }

        public override void Process(GameTime gameTime)
        {
            if (Vector2.DistanceSquared(this.target.Position, this.currentTarget) > this.nearRange * this.nearRange)
            {
                ReseekTarget();
            }

            if (Vector2.DistanceSquared(this.Owner.Position, this.target.Position) < this.nearRange * this.nearRange)
            {
                this.Status = GoalStatus.Completed;
            }
        }
    }
}