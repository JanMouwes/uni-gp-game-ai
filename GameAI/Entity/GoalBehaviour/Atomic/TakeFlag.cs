using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Atomic
{
    public class TakeFlag : Goal<Ship>
    {
        private readonly Flag flag;

        public TakeFlag(Ship owner, Flag flag) : base(owner)
        {
            this.flag = flag;
        }

        public override void Process(GameTime gameTime)
        {
            if (this.flag.Carrier != null || Vector2.DistanceSquared(this.Owner.Position, this.flag.Position) > this.Owner.Scale * this.Owner.Scale) { this.Status = GoalStatus.Failed; }

            this.flag.Carrier = this.Owner;
            this.Owner.Death += DropFlag;

            this.Status = GoalStatus.Completed;
        }

        private void DropFlag(object sender, Ship ship)
        {
            if (this.flag.Carrier != ship) return;

            ship.Death -= DropFlag;
            this.flag.Carrier = null;
        }
    }
}