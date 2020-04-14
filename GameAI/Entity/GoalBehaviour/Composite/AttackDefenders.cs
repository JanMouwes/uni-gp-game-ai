using System.Linq;
using GameAI.Entity.GoalBehaviour.Atomic;
using GameAI.Entity.Navigation;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class AttackDefenders : GoalComposite<Vehicle>
    {
        private readonly World world;
        private readonly Flag enemyFlag;
        private Vehicle currentEnemy;

        public AttackDefenders(Vehicle owner, World world, Flag enemyFlag) : base(owner)
        {
            this.world = world;
            this.enemyFlag = enemyFlag;
        }

        public Vehicle FindValidEnemy()
        {
            return this.world.Entities.OfType<Vehicle>()
                       .Where(vehicle => vehicle.Team != this.Owner.Team)
                       .OrderBy(vehicle => Vector2.DistanceSquared(vehicle.Position, this.Owner.Position))
                       .FirstOrDefault();
        }

        private static bool IsEnemyValid(Vehicle vehicle)
        {
            return vehicle != null;
        }

        public override void Process(GameTime gameTime)
        {
            if (!IsEnemyValid(this.currentEnemy) && this.GoalQueue.Count == 0)
            {
                this.currentEnemy = FindValidEnemy();

                if (this.currentEnemy != null)
                {
                    ClearGoals();
                    AddSubgoal(new PursueEnemy(this.Owner, this.currentEnemy, this.Owner.Scale * this.Owner.Scale, this.world.PathFinder));
                    AddSubgoal(new AttackEnemy(this.Owner, this.currentEnemy));
                }
                else if (this.GoalQueue.Count == 0) { AddSubgoal(new MoveTo<Vehicle>(this.Owner, this.enemyFlag.Position, this.world.PathFinder)); }
            }

            base.Process(gameTime);
            this.Status = GoalStatus.Active;


            base.Process(gameTime);
        }
    }
}