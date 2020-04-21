using System;
using System.Linq;
using GameAI.world;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class Think : GoalComposite<Vehicle>
    {
        private readonly World world;
        private readonly Random random;

        public Think(Vehicle owner, World world) : base(owner)
        {
            this.world = world;
            this.random = new Random();
        }

        private bool HasCurrentGoal => this.GoalQueue.Count > 0;

        public override void Process(GameTime gameTime)
        {
            if (!HasCurrentGoal) { this.AddSubgoal(FindNewGoal()); }

            base.Process(gameTime);
        }

        private Goal<Vehicle> FindNewGoal()
        {
            if (this.Owner.Team.Flag.Carrier != null) { return new HuntEnemy(this.Owner, this.Owner.Team.Flag.Carrier, this.world); }

            Team otherTeam = this.world.Teams.Values.First(team => team.Colour != this.Owner.Team.Colour);

            switch (this.random.Next(0, 3))
            {
                case 0:
                    return new DefendFlag(this.Owner, this.world, this.Owner.Team.Flag);
                case 1:
                    return new AttackDefenders(this.Owner, this.world, otherTeam.Flag);
                default:
                    return new CaptureFlag(this.Owner, otherTeam.Flag, this.world.PathFinder);
            }
        }
    }
}