using System.Linq;
using GameAI.world;
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
            if (this.flag.Carrier != null)
            {
                ClearGoals();
                this.Status = GoalStatus.Failed;

                return;
            }

            if (!IsEnemyValid(this.currentEnemy))
            {
                this.currentEnemy = FindValidEnemy();

                if (this.currentEnemy != null)
                {
                    ClearGoals();
                    AddSubgoal(new HuntEnemy(this.Owner, this.currentEnemy));
                }
                else if (this.GoalQueue.Count == 0) { AddSubgoal(new PatrolAroundFlag(this.Owner, this.flag, this.Owner.Scale * 8, this.world)); }
            }

            base.Process(gameTime);
            this.Status = GoalStatus.Active;
        }
    }
}