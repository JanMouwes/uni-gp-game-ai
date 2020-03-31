using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameAI.GoalBehaviour
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

        public override GoalStatus Process(GameTime gameTime)
        {
            return ProcessSubgoals(gameTime);
        }

        protected GoalStatus ProcessSubgoals(GameTime gameTime)
        {
            while (this.GoalQueue.Count > 0 && (this.GoalQueue.Peek().IsCompleted() || this.GoalQueue.Peek().HasFailed())) { this.GoalQueue.Dequeue().Terminate(); }

            if (this.GoalQueue.Count <= 0) { return GoalStatus.Completed; }

            Goal<TOwner> current = this.GoalQueue.Peek();

            if (current.IsInactive()) { current.Activate(); }

            this.Status = current.Process(gameTime);

            if (this.Status == GoalStatus.Completed && this.GoalQueue.Count > 1) { this.Status = GoalStatus.Active; }

            return this.Status;
        }

        /// <summary>
        /// Add goal at the end of the goal-queue
        /// </summary>
        /// <param name="goal">Goal to add</param>
        public void AddSubgoal(Goal<TOwner> goal) => this.GoalQueue.Enqueue(goal);

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