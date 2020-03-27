using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAI.entity;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.behaviour.Complex
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
                // Vector2 obstacleCentre = o.Pos;
                // Vector2 entityVelocityPos = Entity.Pos + Entity.Velocity;
                // bool isObstacleBehind = Vector2.DistanceSquared(Entity.Pos, obstacleCentre) < Vector2.DistanceSquared(entityVelocityPos, obstacleCentre);
                //
                // if (isObstacleBehind) { continue; }

                // Add a circle around the obstacle which can't be crossed
                CircleF notAllowedZone = new CircleF(o.Pos, o.Scale + Entity.Scale * 2);

                if (checkpoints.Any(checkpoint => notAllowedZone.Contains(checkpoint)))
                {
                    Vector2 dist = new Vector2(o.Pos.X - Entity.Pos.X, o.Pos.X - Entity.Pos.Y);
                    Vector2 perpendicular = new Vector2(-dist.Y, dist.X);

                    Vector2 perpendicularPos = o.Pos         + perpendicular;
                    Vector2 perpendicularNegativePos = o.Pos - perpendicular;

                    float haaksDistPlus = Vector2.DistanceSquared(perpendicularPos, Entity.Pos        + Entity.Velocity);
                    float haaksDistMin = Vector2.DistanceSquared(perpendicularNegativePos, Entity.Pos + Entity.Velocity);

                    return haaksDistPlus > haaksDistMin ? perpendicularNegativePos : perpendicularPos;
                }
            }

            // Return identity vector if there will be no collision
            return new Vector2();
        }
    }
}