using GameAI.Entity.GoalBehaviour.Atomic;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class PursueEnemy : GoalComposite<Ship>
    {
        private readonly float nearRange;

        public readonly Ship Enemy;

        private Vector2 currentTarget;

        public PursueEnemy(Ship owner, Ship enemy, float nearRange) : base(owner)
        {
            this.Enemy = enemy;
            this.nearRange = nearRange;
        }

        public override void Activate()
        {
            PathToEnemy();

            base.Activate();
        }

        private void PathToEnemy()
        {
            this.currentTarget = this.Enemy.Position;

            ClearGoals();

            AddSubgoal(new ChaseTarget(this.Owner, this.Enemy, this.nearRange));
        }

        private bool IsInRange(Vector2 position)
        {
            return Vector2.DistanceSquared(this.Owner.Position, position) < this.nearRange * this.nearRange;
        }

        public override void Process(GameTime gameTime)
        {
            if (IsInRange(this.Enemy.Position + this.Enemy.Velocity))
            {
                // Pursuing done, owner is close enough
                ClearGoals();
            }
            else if (!IsInRange(this.currentTarget) && this.GoalQueue.Count == 0)
            {
                // Current path's end is too far from enemy's position, recalculate
                PathToEnemy();
            }

            base.Process(gameTime);
        }
    }
}