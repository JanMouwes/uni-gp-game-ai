using GameAI.Entity.GoalBehaviour.Rendering;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity.GoalBehaviour
{
    public abstract class Goal<TOwner>
    {
        public IGoalRenderer Renderer { get; set; } = DefaultRenderer.Instance;

        protected readonly TOwner Owner;
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

        public virtual void Render(SpriteBatch spriteBatch) => Renderer.Render(spriteBatch);

        public override string ToString()
        {
            return this.GetType().Name;
        }
    }
}