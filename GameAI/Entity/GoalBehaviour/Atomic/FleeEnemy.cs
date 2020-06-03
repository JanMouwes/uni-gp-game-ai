using GameAI.Entity.Steering.Simple;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Atomic
{
    public class FleeEnemy : Goal<Ship>
    {
        private readonly Ship enemy;
        private readonly float safeDistance;

        public FleeEnemy(Ship owner, Ship enemy, float safeDistance) : base(owner)
        {
            this.enemy = enemy;
            this.safeDistance = safeDistance;
        }

        public override void Activate()
        {
            this.Owner.Steering = new FleeBehaviour(this.Owner, this.enemy);
            base.Activate();
        }

        public override void Process(GameTime gameTime)
        {
            if (Vector2.DistanceSquared(this.Owner.Position, this.enemy.Position) > this.safeDistance * this.safeDistance) { this.Status = GoalStatus.Completed; }
        }
    }
}