using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAI.entity;
using GameAI.Util;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.Steering.Complex
{
    public class ObstacleAvoidance : SteeringBehaviour
    {
        private World w;
        private int range;

        public ObstacleAvoidance(MovingEntity entity, World w, int range = 100) : base(entity)
        {
            this.w = w;
            this.range = range;
        }

        public override Vector2 Calculate()
        {
            // The detection box is the current velocity divided by the max velocity of the entity
            // range is the maximum size of the box
            Vector2 viewBox = Entity.Velocity / Entity.MaxSpeed * range;
            // Add the box in front of the entity
            IEnumerable<Vector2> checkpoints = new[]
            {
                Entity.Pos,
                this.Entity.Pos + viewBox / 2f, // Halfway
                this.Entity.Pos + viewBox,      // At the end
                this.Entity.Pos + viewBox * 2   // Double
            };

            foreach (BaseGameEntity o in w.obstacles)
            {
                // Add a circle around the obstacle which can't be crossed
                CircleF notAllowedZone = new CircleF(o.Pos, o.Scale);

                if (checkpoints.Any(checkpoint => notAllowedZone.Contains(checkpoint)))
                {
                    Vector2 dist = new Vector2(o.Pos.X - Entity.Pos.X, o.Pos.X - Entity.Pos.Y);
                    Vector2 perpendicular = Vector2Helper.PerpendicularRightAngleOf(dist);

                    Vector2 perpendicularPositivePos = o.Pos + perpendicular;
                    Vector2 perpendicularNegativePos = o.Pos - perpendicular;

                    float perpDistPositive = Vector2.DistanceSquared(Entity.Pos + Entity.Velocity, perpendicularPositivePos);
                    float perpDistNegative = Vector2.DistanceSquared(Entity.Pos + Entity.Velocity, perpendicularNegativePos);

                    Vector2 targetRelative = (perpDistPositive > perpDistNegative ? perpendicularNegativePos : perpendicularPositivePos) - Entity.Pos;

                    return Vector2Helper.PerpendicularRightAngleOf(targetRelative);
                }
            }

            // Return identity vector if there will be no collision
            return new Vector2();
        }
    }
}