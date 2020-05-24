using System.Linq;
using GameAI.Entity.GoalBehaviour.Atomic;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class DefendFlag : GoalComposite<Ship>
    {
        private readonly World world;
        private readonly Flag flag;

        private Ship currentEnemy;

        public DefendFlag(Ship owner, World world, Flag flag) : base(owner)
        {
            this.world = world;
            this.flag = flag;
        }

        public Ship FindValidEnemy()
        {
            return this.world.Entities.OfType<Ship>()
                       .Where(vehicle => vehicle.Team != this.Owner.Team && IsEnemyValid(vehicle))
                       .OrderBy(vehicle => Vector2.DistanceSquared(vehicle.Position, this.flag.Position))
                       .FirstOrDefault();
        }

        public bool IsEnemyValid(Ship enemy)
        {
            const float panicDistance = 150f;

            return enemy != null && Vector2.DistanceSquared(enemy.Position, this.flag.Position) < panicDistance * panicDistance;
        }

        public override void Process(GameTime gameTime)
        {
            if (!IsEnemyValid(this.currentEnemy))
            {
                this.currentEnemy = FindValidEnemy();

                if (this.currentEnemy != null)
                {
                    ClearGoals();
                    AddSubgoal(new PursueEnemy(this.Owner, this.currentEnemy, this.Owner.Scale * this.Owner.Scale, this.world.PathFinder));
                    AddSubgoal(new AttackEnemy(this.Owner, this.currentEnemy));
                }
                else if (this.GoalQueue.Count == 0) { AddSubgoal(new MoveTo<Ship>(this.Owner, this.flag.Position, this.world.PathFinder)); }
            }

            base.Process(gameTime);
            this.Status = GoalStatus.Active;
        }
    }
}