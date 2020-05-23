using System;
using System.Linq;
using Fuzzy;
using Fuzzy.Terms;
using GameAI.world;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.GoalBehaviour.Composite
{
    public class Think : GoalComposite<Vehicle>
    {
        private readonly World world;
        private readonly Random random;

        private readonly FuzzyModule fuzzyModule;

        private const string OWN_DISTANCE_VARIABLE_KEY = "own distance";
        private const string AVG_TEAMMATE_DISTANCE_VARIABLE_KEY = "average teammate distance";
        private const string OWN_STRATEGY_VARIABLE_KEY = "own strategy";

        public Think(Vehicle owner, World world) : base(owner)
        {
            this.world = world;
            this.random = new Random();

            this.fuzzyModule = new FuzzyModule();

            const float range = 600;
            const float nearPeak = .25f * range;
            const float mediumPeak = .5f * range;
            const float farPeak = .75f * range;

            Variable ownDistanceVariable = this.fuzzyModule.CreateVariable(OWN_DISTANCE_VARIABLE_KEY);
            SetProxy selfNear = ownDistanceVariable.AddLeftShoulder("near", 0, nearPeak, mediumPeak);
            SetProxy selfMedium = ownDistanceVariable.AddTriangleSet("medium", nearPeak, mediumPeak, farPeak);
            SetProxy selfFar = ownDistanceVariable.AddRightShoulder("far", mediumPeak, farPeak, range);

            Variable avgTeammatesDistanceVariable = this.fuzzyModule.CreateVariable(AVG_TEAMMATE_DISTANCE_VARIABLE_KEY);
            SetProxy teamNear = avgTeammatesDistanceVariable.AddLeftShoulder("near", 0, nearPeak, mediumPeak);
            SetProxy teamMedium = avgTeammatesDistanceVariable.AddTriangleSet("medium", nearPeak, mediumPeak, farPeak);
            SetProxy teamFar = avgTeammatesDistanceVariable.AddRightShoulder("far", mediumPeak, farPeak, range);

            Variable strategyVariable = this.fuzzyModule.CreateVariable(OWN_STRATEGY_VARIABLE_KEY);
            SetProxy defensive = strategyVariable.AddLeftShoulder("defensive", 0, .2, .5);
            SetProxy offensive = strategyVariable.AddRightShoulder("offensive", .2, .6, 1);

            /*
             * IF I am near AND teammates are near
             * THEN offensive
             * 
             * IF I am near AND teammates are medium distance or far
             * THEN defensive
             * 
             * IF I am medium distance AND teammates are (medium distance OR far)
             * THEN defensive
             *
             * IF teammates are near
             * THEN offensive
             * 
             * IF I am far 
             * THEN offensive
             */

            this.fuzzyModule.AddRule(new And(selfNear, teamNear), offensive);
            this.fuzzyModule.AddRule(new And(selfNear, teamMedium), defensive);
            this.fuzzyModule.AddRule(new And(selfMedium, new Or(teamMedium, teamFar)), defensive);
            this.fuzzyModule.AddRule(teamNear, offensive);
            this.fuzzyModule.AddRule(selfFar, offensive);
        }

        private bool HasCurrentGoal => this.GoalQueue.Count > 0;

        public override void Process(GameTime gameTime)
        {
            if (!this.HasCurrentGoal)
            {
                Goal<Vehicle> goal = FindNewGoal();
                AddSubgoal(goal);
            }

            base.Process(gameTime);
        }

        private double GetStrategy()
        {
            Vector2 flagPosition = this.Owner.Team.Flag.Position;

            double distanceToFlag = Vector2.Distance(this.Owner.Position, flagPosition);
            this.fuzzyModule.Fuzzify(OWN_DISTANCE_VARIABLE_KEY, distanceToFlag);

            float avgTeammateDistanceToFlag = this.Owner.Team.Vehicles.Where(veh => veh != this.Owner).Average(teammate => Vector2.Distance(teammate.Position, flagPosition));
            this.fuzzyModule.Fuzzify(AVG_TEAMMATE_DISTANCE_VARIABLE_KEY, avgTeammateDistanceToFlag);

            return this.fuzzyModule.Defuzzify(OWN_STRATEGY_VARIABLE_KEY, DefuzzifyMethods.Centroid);
        }

        private Goal<Vehicle> FindNewGoal()
        {
            if (this.Owner.Team.Flag.Carrier != null) { return new HuntEnemy(this.Owner, this.Owner.Team.Flag.Carrier, this.world); }

            Team otherTeam = this.world.Teams.Values.First(team => team.Colour != this.Owner.Team.Colour);

            bool shouldDefend = GetStrategy() < .4;

            if (shouldDefend) { return new DefendFlag(this.Owner, this.world, this.Owner.Team.Flag); }

            // Least likely to do CaptureFlag
            switch (this.random.Next(0, 11) % 3)
            {
                case 0:
                    return new CaptureFlag(this.Owner, otherTeam.Flag, this.world.PathFinder);
                case 1:
                    return new DefendCapturers(this.Owner, this.world);
                default:
                    return new AttackDefenders(this.Owner, this.world, otherTeam.Flag);
            }
        }
    }
}