using System.Linq;
using GameAI.Entity.GoalBehaviour.Atomic;
using GameAI.world;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class DefendCapturers : GoalComposite<Ship>
    {
        private readonly World world;

        private Ship currentCapturer;
        private Ship currentEnemy;

        public DefendCapturers(Ship owner, World world) : base(owner)
        {
            this.world = world;
        }

        public Ship FindValidEnemy()
        {
            return this.world.Entities.OfType<Ship>()
                       .Where(vehicle => vehicle.Team != this.Owner.Team && IsEnemyValid(vehicle))
                       .OrderBy(enemy => Vector2.DistanceSquared(enemy.Position, this.currentCapturer.Position))
                       .FirstOrDefault();
        }

        public Ship FindValidCapturer()
        {
            return this.Owner.Team.Vehicles
                       .Where(vehicle => vehicle != this.Owner && IsCapturerValid(vehicle))
                       .OrderBy(vehicle => Vector2.DistanceSquared(this.Owner.Position, vehicle.Position))
                       .FirstOrDefault();
        }

        public static bool IsCapturerValid(Ship capturer)
        {
            return capturer?.Brain.CurrentGoal is CaptureFlag;
        }

        public bool IsEnemyValid(Ship enemy)
        {
            const float panicDistance = 50f;

            return enemy != null && Vector2.DistanceSquared(enemy.Position, this.currentCapturer.Position) < panicDistance * panicDistance;
        }

        public override void Process(GameTime gameTime)
        {
            if (!IsCapturerValid(this.currentCapturer))
            {
                this.currentCapturer = FindValidCapturer();

                if (this.currentCapturer == null)
                {
                    this.Status = GoalStatus.Failed;

                    return;
                }
            }

            if (!IsEnemyValid(this.currentEnemy))
            {
                this.currentEnemy = FindValidEnemy();

                if (this.currentEnemy != null)
                {
                    ClearGoals();
                    AddSubgoal(new HuntEnemy(this.Owner, this.currentEnemy));
                }
                else if (this.GoalQueue.Count == 0) { AddSubgoal(new ChaseTarget(this.Owner, this.currentCapturer, 10f)); }
            }

            base.Process(gameTime);
        }
    }
}