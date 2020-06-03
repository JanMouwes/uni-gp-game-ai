using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Atomic
{
    public class AttackEnemy : Goal<Ship>
    {
        private readonly Ship enemy;

        public AttackEnemy(Ship owner, Ship enemy) : base(owner)
        {
            this.enemy = enemy;
        }

        private bool IsInRange(Vector2 target, float range)
        {
            return Vector2.DistanceSquared(target, this.Owner.Position) < range * range;
        }

        public override void Process(GameTime gameTime)
        {
            float range = this.Owner.Scale + this.Owner.Scale;

            if (IsInRange(this.enemy.Position, range))
            {
                this.enemy.Kill();

                this.Status = GoalStatus.Completed;
                
                return;
            }

            this.Status = GoalStatus.Failed;
        }
    }
}