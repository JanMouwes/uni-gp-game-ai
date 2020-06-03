using System.Collections.Generic;
using System.Linq;
using GameAI.Entity.GoalBehaviour.Atomic;
using GameAI.world;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class PatrolAroundFlag : GoalComposite<Ship>
    {
        private readonly Flag flag;
        private readonly float nearRange;
        private readonly World world;

        public PatrolAroundFlag(Ship owner, Flag flag, float nearRange, World world) : base(owner)
        {
            this.flag = flag;
            this.nearRange = nearRange;
            this.world = world;
        }

        public IEnumerable<Vector2> GetPatrolPoints(Vector2 centre, float range)
        {
            (float x, float y) = centre;

            // North
            yield return new Vector2(x, y - range);

            // East
            yield return new Vector2(x + range, y);

            // South
            yield return new Vector2(x, y + range);

            // West
            yield return new Vector2(x - range, y);
        }

        public void AddNewGoals()
        {
            foreach (Vector2 patrolPoint in GetPatrolPoints(this.flag.Position, this.nearRange)) { AddSubgoal(new TraverseEdge<Ship>(this.Owner, patrolPoint)); }
        }

        public bool IsEnemyNearby()
        {
            bool IsShipEnemy(Ship ship)
            {
                return ship.Team != this.Owner.Team;
            }

            bool IsShipNearby(BaseGameEntity entity)
            {
                return Vector2.DistanceSquared(this.Owner.Position, entity.Position) < this.nearRange * this.nearRange;
            }

            return this.world.Entities
                       .OfType<Ship>()
                       .Any(ship => IsShipEnemy(ship) && IsShipNearby(ship));
        }

        public override void Process(GameTime gameTime)
        {
            if (IsEnemyNearby()) { ClearGoals(); }
            else if (this.GoalQueue.Count == 0) { AddNewGoals(); }

            base.Process(gameTime);
        }
    }
}