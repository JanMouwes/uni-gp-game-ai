using System;
using GameAI.Entity;
using Microsoft.Xna.Framework;

namespace GameAI.Steering.Simple
{
    public class WanderBehaviour : SteeringBehaviour
    {
        private readonly float range;
        private float currentOffset;
        private readonly Random random;

        public WanderBehaviour(MovingEntity entity, float range) : base(entity)
        {
            this.range = range;
            this.random = new Random();
            this.currentOffset = 0;
        }

        public override Vector2 Calculate()
        {
            this.currentOffset += this.random.Next(-100, 100) / 100f; // .Next() is exclusive
            this.currentOffset = Math.Min(this.currentOffset, this.range);
            this.currentOffset = Math.Max(this.currentOffset, -this.range);

            Vector2 target = SteeringBehaviours.Wander(this.Entity, this.currentOffset, this.range);

            return target;
        }
    }
}