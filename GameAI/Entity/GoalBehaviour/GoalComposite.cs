using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity.GoalBehaviour
{
    public abstract class GoalComposite<TOwner> : Goal<TOwner>
    {
        protected readonly Queue<Goal<TOwner>> GoalQueue;

        public IEnumerable<Goal<TOwner>> Goals => this.GoalQueue;

        public Goal<TOwner> CurrentGoal => this.GoalQueue.Count > 0 ? this.GoalQueue.Peek() : null;

        protected GoalComposite(TOwner owner) : base(owner)
        {
            this.GoalQueue = new Queue<Goal<TOwner>>();
        }

        public override void Process(GameTime gameTime)
        {
            ProcessSubgoals(gameTime);
        }

        /// <summary>
        /// Removes all completed/failed goals at the head of the queue.
        /// Completes if there are no more goals, otherwise will process next goal.
        /// </summary>
        /// <param name="gameTime">Information about game's time</param>
        protected void ProcessSubgoals(GameTime gameTime)
        {
            while (this.GoalQueue.Count > 0 && (this.GoalQueue.Peek().IsCompleted() || this.GoalQueue.Peek().HasFailed()))
            {
                // Terminate all completed or failed goals
                this.GoalQueue.Dequeue().Terminate();
            }


            if (this.GoalQueue.Count == 0) { this.Status = GoalStatus.Completed; }
            else
            {
                Goal<TOwner> current = this.GoalQueue.Peek();

                if (current.IsInactive()) { current.Activate(); }

                current.Process(gameTime);

                if (current.Status == GoalStatus.Completed && this.GoalQueue.Count > 1) { this.Status = GoalStatus.Active; }
                else { this.Status = current.Status; }
            }
        }

        /// <summary>
        /// Add goal at the end of the goal-queue
        /// </summary>
        /// <param name="goal">Goal to add</param>
        public void AddSubgoal(Goal<TOwner> goal) => this.GoalQueue.Enqueue(goal);

        public override void Render(SpriteBatch spriteBatch)
        {
            if (this.GoalQueue.Count > 0) { this.GoalQueue.Peek().Render(spriteBatch); }
            
            Renderer.Render(spriteBatch);
        }

        /// <summary>
        /// Terminates all goals and clears the queue
        /// </summary>
        public void ClearGoals()
        {
            foreach (Goal<TOwner> goal in this.GoalQueue) { goal.Terminate(); }

            this.GoalQueue.Clear();
        }
    }
}