using Fuzzy.Sets;
using Fuzzy.Terms;
using NUnit.Framework;
using Rule = Fuzzy.Rules.Rule;

namespace Fuzzy.Tests
{
    [TestFixture]
    public class RuleTests
    {
        private FuzzyModule fuzzyModule;

        private Term rule_nearAndNear;
        private Term rule_nearAndMedium;
        private Term rule_mediumAndMediumOrFar;
        private Term rule_near;
        private Term rule_far;

        private Term defensive;
        private Term offensive;

        private const string OWN_DISTANCE_VARIABLE_KEY = "own distance";
        private const string AVG_TEAMMATE_DISTANCE_VARIABLE_KEY = "average teammate distance";
        private const string OWN_STRATEGY_VARIABLE_KEY = "own strategy";

        [SetUp]
        public void Setup()
        {
            this.fuzzyModule = new FuzzyModule();

            const float range = 1000; // max distance from one corner to another
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
            this.defensive = strategyVariable.AddLeftShoulder("defensive", 0, .4, .6);
            this.offensive = strategyVariable.AddRightShoulder("offensive", .4, .6, 1);

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

            this.rule_nearAndNear = new And(selfNear, teamNear);
            this.rule_nearAndMedium = new And(selfNear, teamMedium);
            this.rule_mediumAndMediumOrFar = new And(selfMedium, new Or(teamMedium, teamFar));
            this.rule_near = teamNear;
            this.rule_far = selfFar;

            this.fuzzyModule.AddRule(this.rule_nearAndNear, this.offensive);
            this.fuzzyModule.AddRule(this.rule_nearAndMedium, this.defensive);
            this.fuzzyModule.AddRule(this.rule_mediumAndMediumOrFar, this.defensive);
            this.fuzzyModule.AddRule(this.rule_near, this.offensive);
            this.fuzzyModule.AddRule(this.rule_far, this.offensive);
        }

        [Test]
        public void Test_WhenCalculateCalled_ShouldOrWithMembership()
        {
            // Arrange
            FuzzySet antecedentSet = new TriangleSet(.5, .3, .3);
            antecedentSet.DegreeOfMembership = .7;
            Term antecedent = new SetProxy(antecedentSet);

            FuzzySet consequenceSet = new TriangleSet(.5, .3, .3);
            consequenceSet.DegreeOfMembership = 0;
            Term consequence = new SetProxy(consequenceSet);

            Rule rule = new Rule(antecedent, consequence);

            // Act
            rule.Calculate();

            // Assert
            Assert.AreEqual(.7, antecedent.Membership, .00001);
            Assert.AreEqual(.7, consequence.Membership, .00001);
        }
    }
}