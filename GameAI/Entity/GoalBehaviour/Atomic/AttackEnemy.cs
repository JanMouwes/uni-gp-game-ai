using GameAI.GoalBehaviour;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Atomic
{
    public class AttackEnemy : Goal<Vehicle>
    {
        private readonly Vehicle enemy;

        public AttackEnemy(Vehicle owner, Vehicle enemy) : base(owner)
        {
            this.enemy = enemy;
        }

        public override void Process(GameTime gameTime)
        {
            float range = this.Owner.Scale * this.Owner.Scale;

            if (Vector2.DistanceSquared(this.enemy.Position, this.Owner.Position) > range * range) { this.Status = GoalStatus.Failed; }

            this.enemy.Kill();

            this.Status = GoalStatus.Completed;
        }
    }
}