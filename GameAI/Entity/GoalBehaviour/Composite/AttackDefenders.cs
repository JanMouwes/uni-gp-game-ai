using System.Linq;
using GameAI.world;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class AttackDefenders : GoalComposite<Ship>
    {
        private readonly World world;
        private readonly Flag enemyFlag;
        private Ship currentEnemy;

        public AttackDefenders(Ship owner, World world, Flag enemyFlag) : base(owner)
        {
            this.world = world;
            this.enemyFlag = enemyFlag;
        }

        public Ship FindValidEnemy()
        {
            return this.world.Entities.OfType<Ship>()
                       .Where(vehicle => vehicle.Team != this.Owner.Team)
                       .OrderBy(vehicle => Vector2.DistanceSquared(vehicle.Position, this.Owner.Position))
                       .FirstOrDefault();
        }

        private static bool IsEnemyValid(Ship ship)
        {
            return ship != null;
        }

        public override void Process(GameTime gameTime)
        {
            if (!IsEnemyValid(this.currentEnemy) && this.GoalQueue.Count == 0)
            {
                this.currentEnemy = FindValidEnemy();

                if (this.currentEnemy != null)
                {
                    ClearGoals();
                    AddSubgoal(new HuntEnemy(this.Owner, this.currentEnemy));
                }
                else if (this.GoalQueue.Count == 0) { AddSubgoal(new MoveTo<Ship>(this.Owner, this.enemyFlag.Position, this.world.PathFinder)); }
            }

            base.Process(gameTime);
            this.Status = GoalStatus.Active;


            base.Process(gameTime);
        }
    }
}