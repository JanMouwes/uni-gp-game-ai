using Microsoft.Xna.Framework;

namespace GameAI.GoalBehaviour
{
    public abstract class Goal<TOwner>
    {
        protected TOwner Owner;
        protected GoalStatus Status = GoalStatus.Inactive;

        protected Goal(TOwner owner)
        {
            this.Owner = owner;
        }

        public virtual void Activate() => this.Status = GoalStatus.Active;
        public abstract GoalStatus Process(GameTime gameTime);
        public virtual void Terminate() => this.Status = GoalStatus.Completed;

        public bool IsActive() => this.Status    == GoalStatus.Active;
        public bool IsInactive() => this.Status  == GoalStatus.Inactive;
        public bool IsCompleted() => this.Status == GoalStatus.Completed;
        public bool HasFailed() => this.Status   == GoalStatus.Failed;
    }
}