using GameAI.GoalBehaviour;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour
{
    public abstract class Goal<TOwner>
    {
        protected TOwner Owner;
        public GoalStatus Status { get; protected set; } = GoalStatus.Inactive;

        protected Goal(TOwner owner)
        {
            this.Owner = owner;
        }

        public virtual void Activate() => this.Status = GoalStatus.Active;
        public abstract void Process(GameTime gameTime);
        public virtual void Terminate() => this.Status = GoalStatus.Failed;

        public bool IsActive() => this.Status    == GoalStatus.Active;
        public bool IsInactive() => this.Status  == GoalStatus.Inactive;
        public bool IsCompleted() => this.Status == GoalStatus.Completed;
        public bool HasFailed() => this.Status   == GoalStatus.Failed;

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}