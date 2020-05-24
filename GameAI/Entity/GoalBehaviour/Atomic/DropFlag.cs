using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Atomic
{
    public class DropFlag : Goal<Ship>
    {
        private Flag flag;
        public DropFlag(Ship owner, Flag flag) : base(owner)
        {
            this.flag = flag;
        }

        public override void Process(GameTime gameTime)
        {
            if (this.flag.Carrier == null) { this.Status = GoalStatus.Failed; }

            this.flag.Carrier = null;

            this.Status = GoalStatus.Completed;
        }
    }
}