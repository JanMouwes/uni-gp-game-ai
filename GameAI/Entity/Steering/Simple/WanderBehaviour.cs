using System;
using GameAI.Entity;
using Microsoft.Xna.Framework;

namespace GameAI.Steering.Simple
{
    public class WanderBehaviour : SteeringBehaviour
    {
        private readonly float min;
        private readonly float max;
        private float currentOffset;
        private readonly Random random;

        public WanderBehaviour(MovingEntity entity, float range) : this(entity, -range, range) { }

        public WanderBehaviour(MovingEntity entity, float min, float max) : base(entity)
        {
            this.min = min;
            this.max = max;
            this.random = new Random();
            this.currentOffset = 0;
        }

        public override Vector2 Calculate()
        {
            this.currentOffset += this.random.Next(-100, 100) / 100f; // .Next() is exclusive
            this.currentOffset = Math.Min(this.currentOffset, this.max);
            this.currentOffset = Math.Max(this.currentOffset, this.min);

            Vector2 target = SteeringBehaviours.Wander(this.Entity, 80, this.currentOffset);

            return target;
        }
    }
}