using GameAI.GoalBehaviour;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Atomic
{
    public class TakeFlag : Goal<Vehicle>
    {
        private readonly Flag flag;

        public TakeFlag(Vehicle owner, Flag flag) : base(owner)
        {
            this.flag = flag;
        }

        public override void Process(GameTime gameTime)
        {
            if (this.flag.Carrier != null || Vector2.DistanceSquared(this.Owner.Position, this.flag.Position) > this.Owner.Scale * this.Owner.Scale) { this.Status = GoalStatus.Failed; }

            this.flag.Carrier = this.Owner;

            this.Status = GoalStatus.Completed;
        }
    }
}