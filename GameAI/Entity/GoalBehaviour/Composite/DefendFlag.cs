using System.Linq;
using GameAI.Entity.GoalBehaviour.Atomic;
using GameAI.GoalBehaviour;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class DefendFlag : GoalComposite<Vehicle>
    {
        private readonly World world;
        private readonly Flag flag;

        private Vehicle currentEnemy;

        public DefendFlag(Vehicle owner, World world, Flag flag) : base(owner)
        {
            this.world = world;
            this.flag = flag;
        }

        public Vehicle FindValidEnemy()
        {
            return this.world.Entities.OfType<Vehicle>()
                       .Where(vehicle => vehicle.Team != this.Owner.Team && IsEnemyValid(vehicle))
                       .OrderBy(vehicle => Vector2.DistanceSquared(vehicle.Position, this.flag.Position))
                       .FirstOrDefault();
        }

        public bool IsEnemyValid(Vehicle enemy)
        {
            const float panicDistance = 150f;

            return enemy != null && Vector2.DistanceSquared(enemy.Position, this.flag.Position) < panicDistance * panicDistance;
        }

        public override void Process(GameTime gameTime)
        {
            if (!IsEnemyValid(this.currentEnemy) && this.GoalQueue.Count == 0)
            {
                this.currentEnemy = FindValidEnemy();

                if (this.currentEnemy != null)
                {
                    ClearGoals();
                    AddSubgoal(new PursueEnemy(this.Owner, this.currentEnemy, this.Owner.Scale * this.Owner.Scale, this.world));
                    AddSubgoal(new AttackEnemy(this.Owner, this.currentEnemy));
                }

                AddSubgoal(new MoveTo<Vehicle>(this.Owner, this.flag.Position, this.world.PathFinder));
            }

            base.Process(gameTime);
            this.Status = GoalStatus.Active;
        }
    }
}