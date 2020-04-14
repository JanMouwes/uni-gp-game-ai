using System.Collections.Generic;
using System.Linq;
using GameAI.Entity;
using Microsoft.Xna.Framework;

namespace GameAI.Steering.Complex
{
    public class FlockingBehaviour : SteeringBehaviour
    {
        public World World;
        private readonly double separationRadius;
        private readonly double alignmentRadius;
        private readonly double cohesionRadius;

        public FlockingBehaviour(MovingEntity entity, World world, double radius) : base(entity)
        {
            this.World = world;
            this.separationRadius = radius;
            this.alignmentRadius = radius;
            this.cohesionRadius = radius;
        }

        public override Vector2 Calculate()
        {
            bool IsNearSeparation(MovingEntity entity)
            {
                Vector2 to = entity.Position - this.Entity.Position;

                return to.LengthSquared() < separationRadius * separationRadius;
            }

            bool IsNearAlignment(MovingEntity entity)
            {
                Vector2 to = entity.Position - this.Entity.Position;

                return to.LengthSquared() < alignmentRadius * alignmentRadius;
            }

            bool IsNearCohesion(MovingEntity entity)
            {
                Vector2 to = entity.Position - this.Entity.Position;

                return to.LengthSquared() < cohesionRadius * cohesionRadius;
            }

            Vector2 target = this.Entity.Velocity                                                                                      +
                             SteeringBehaviours.Separation(this.Entity, this.World.Entities.OfType<Vehicle>().Where(IsNearSeparation)) +
                             SteeringBehaviours.Alignment(this.Entity, this.World.Entities.OfType<Vehicle>().Where(IsNearAlignment))   +
                             SteeringBehaviours.Cohesion(this.Entity, this.World.Entities.OfType<Vehicle>().Where(IsNearCohesion));

            if (target.Equals(Vector2.Zero))
            {
                //  When there is no target, start wandering
                target = this.Entity.Velocity +
                         SteeringBehaviours.Wander(this.Entity, 0, 80);
            }

            return target;
        }
    }
}