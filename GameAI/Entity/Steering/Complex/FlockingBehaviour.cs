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
            bool IsNear(BaseGameEntity entity, float range) => Vector2.DistanceSquared(this.Entity.Position, entity.Position) < range * range;

            bool IsNearAlignment(MovingEntity entity) => IsNear(entity, (float) this.alignmentRadius);
            bool IsNearCohesion(MovingEntity entity) => IsNear(entity, (float) this.cohesionRadius + 20);
            bool IsNearSeparation(MovingEntity entity) => IsNear(entity, (float)this.separationRadius - 20);

            Vector2 A = SteeringBehaviours.Alignment(this.Entity, this.World.Entities.OfType<Vehicle>().Where(IsNearAlignment));
            Vector2 C = SteeringBehaviours.Cohesion(this.Entity, this.World.Entities.OfType<Vehicle>().Where(IsNearCohesion));
            Vector2 S = SteeringBehaviours.Separation(this.Entity, this.World.Entities.OfType<Vehicle>().Where(IsNearSeparation));

            Vector2 target = A + C + S;

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