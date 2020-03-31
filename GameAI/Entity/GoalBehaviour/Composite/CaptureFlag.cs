using GameAI.Entity.GoalBehaviour.Atomic;
using GameAI.GoalBehaviour;
using GameAI.GoalBehaviour.Composite;
using GameAI.Navigation;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class CaptureFlag : GoalComposite<Vehicle>
    {
        public CaptureFlag(Vehicle owner, Flag flag, PathFinder pathFinder) : base(owner)
        {
            this.AddSubgoal(new MoveTo<Vehicle>(owner, flag.Position, pathFinder));
            this.AddSubgoal(new TakeFlag(owner, flag));
            this.AddSubgoal(new MoveTo<Vehicle>(owner, owner.Team.Flag.Position, pathFinder));
        }
    }
}