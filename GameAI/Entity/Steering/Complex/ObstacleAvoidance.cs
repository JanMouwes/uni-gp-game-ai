using System.Collections.Generic;
using System.Linq;
using GameAI.Util;
using GameAI.world;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.Entity.Steering.Complex
{
    public class ObstacleAvoidance : SteeringBehaviour
    {
        private readonly World world;
        private readonly float range;

        public ObstacleAvoidance(MovingEntity entity, World world, float range = 100) : base(entity)
        {
            this.world = world;
            this.range = range;
        }

        public override Vector2 Calculate()
        {
            // The detection box is the current velocity divided by the max velocity of the entity
            // range is the maximum size of the box
            Vector2 viewBox = this.Entity.Velocity / this.Entity.MaxSpeed * this.range;
            
            // Add the check points in front of the entity
            IEnumerable<Vector2> checkpoints = new[]
            {
                this.Entity.Position,
                this.Entity.Position + viewBox / 2f, // Halfway
                this.Entity.Position + viewBox,      // At the end
                this.Entity.Position + viewBox * 2   // Double
            };

            foreach (Rock o in this.world.Entities.OfType<Rock>())
            {
                // Add a circle around the obstacle which can't be crossed
                CircleF notAllowedZone = new CircleF(o.Position, o.Scale);

                if (checkpoints.Any(checkpoint => notAllowedZone.Contains(checkpoint)))
                {
                    Vector2 dist = new Vector2(o.Position.X - this.Entity.Position.X, o.Position.X - this.Entity.Position.Y);
                    Vector2 perpendicular = Vector2Helper.PerpendicularRightAngleOf(dist);

                    Vector2 perpendicularPositivePos = o.Position + perpendicular;
                    Vector2 perpendicularNegativePos = o.Position - perpendicular;

                    float perpDistPositive = Vector2.DistanceSquared(this.Entity.Position + this.Entity.Velocity, perpendicularPositivePos);
                    float perpDistNegative = Vector2.DistanceSquared(this.Entity.Position + this.Entity.Velocity, perpendicularNegativePos);

                    Vector2 targetRelative = (perpDistPositive > perpDistNegative ? perpendicularNegativePos : perpendicularPositivePos) - this.Entity.Position;

                    return Vector2Helper.PerpendicularRightAngleOf(targetRelative);
                }
            }

            // Return identity vector if there will be no collision
            return new Vector2();
        }
    }
}