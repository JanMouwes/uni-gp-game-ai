using GameAI.Entity.GoalBehaviour.Atomic;
using GameAI.Entity.Navigation;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class CaptureFlag : GoalComposite<Vehicle>
    {
        private readonly Flag enemyFlag;

        public CaptureFlag(Vehicle owner, Flag enemyFlag, PathFinder pathFinder) : base(owner)
        {
            this.enemyFlag = enemyFlag;
            this.AddSubgoal(new MoveTo<Vehicle>(owner, enemyFlag.Position, pathFinder));
            this.AddSubgoal(new TakeFlag(owner, enemyFlag));
            this.AddSubgoal(new MoveTo<Vehicle>(owner, owner.Team.Flag.Position, pathFinder));
            this.AddSubgoal(new DropFlag(owner, enemyFlag));
        }

        public override void Process(GameTime gameTime)
        {
            if (this.enemyFlag.Carrier != null && this.enemyFlag.Carrier.Team == this.Owner.Team)
            {
                Status = GoalStatus.Failed;

                return;
            }

            base.Process(gameTime);
        }
    }
}