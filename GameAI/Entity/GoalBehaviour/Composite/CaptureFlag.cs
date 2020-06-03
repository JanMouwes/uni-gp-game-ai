using GameAI.Entity.GoalBehaviour.Atomic;
using GameAI.Entity.Navigation;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class CaptureFlag : GoalComposite<Ship>
    {
        private readonly Flag enemyFlag;

        public CaptureFlag(Ship owner, Flag enemyFlag, PathFinder pathFinder) : base(owner)
        {
            this.enemyFlag = enemyFlag;
            AddSubgoal(new MoveTo<Ship>(owner, enemyFlag.Position, pathFinder));
            AddSubgoal(new TakeFlag(owner, enemyFlag));
            AddSubgoal(new MoveTo<Ship>(owner, owner.Team.Base, pathFinder));
            AddSubgoal(new DropFlag(owner, enemyFlag));
        }

        public override void Process(GameTime gameTime)
        {
            Ship carrier = this.enemyFlag.Carrier;

            if (carrier != null &&
                carrier != this.Owner)
            {
                this.Status = GoalStatus.Failed;

                return;
            }

            base.Process(gameTime);
        }
    }
}