using GameAI.GoalBehaviour;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class PursueEnemy : GoalComposite<Vehicle>
    {
        private readonly float nearRange;
        private readonly World world;

        public readonly Vehicle Enemy;

        private Vector2 currentTarget;

        public PursueEnemy(Vehicle owner, Vehicle enemy, float nearRange, World world) : base(owner)
        {
            this.Enemy = enemy;
            this.nearRange = nearRange;
            this.world = world;
        }

        public override void Activate()
        {
            PathToEnemy();

            base.Activate();
        }

        private void PathToEnemy()
        {
            this.currentTarget = this.Enemy.Position;

            this.ClearGoals();

            AddSubgoal(new MoveTo<Vehicle>(this.Owner, this.currentTarget, this.world.PathFinder));
        }

        public override void Process(GameTime gameTime)
        {
            if (Vector2.DistanceSquared(this.Owner.Position, this.Enemy.Position) < this.nearRange * this.nearRange)
            {
                // Pursuing done, owner is close enough
                ClearGoals();
            }
            else if (Vector2.DistanceSquared(this.Enemy.Position, this.currentTarget) > this.nearRange * this.nearRange)
            {
                // Current path's end is too far from enemy's position, recalculate
                PathToEnemy();
            }

            base.Process(gameTime);
        }
    }
}