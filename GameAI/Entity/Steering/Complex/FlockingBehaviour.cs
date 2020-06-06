using System.Collections.Generic;
using System.Linq;
using GameAI.world;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.Steering.Complex
{
    public class FlockingBehaviour : SteeringBehaviour
    {
        private readonly World world;

        private readonly float separationRadius;
        private readonly float alignmentRadius;
        private readonly float cohesionRadius;

        private readonly float alignmentWeight;
        private readonly float cohesionWeight;
        private readonly float separationWeight;
        
        private readonly float strength;

        public FlockingBehaviour(MovingEntity entity, World world, float radius, float strength, float alignmentWeight = 15, float cohesionWeight = 8, float separationWeight = 20) : base(entity)
        {
            this.world = world;
            this.alignmentRadius = radius;
            this.strength = strength;
            this.cohesionRadius = radius * .125f;
            this.separationRadius = radius * .125f;

            this.alignmentWeight = alignmentWeight;
            this.cohesionWeight = cohesionWeight;
            this.separationWeight = separationWeight;
        }

        public override Vector2 Calculate()
        {
            bool IsNear(BaseGameEntity entity, float range) => Vector2.DistanceSquared(this.Entity.Position, entity.Position) < range * range;

            bool InAlignmentRange(MovingEntity entity) => IsNear(entity, this.alignmentRadius);
            bool InCohesionRange(MovingEntity entity) => IsNear(entity, this.cohesionRadius);
            bool InSeparationRange(MovingEntity entity) => IsNear(entity, this.separationRadius);

            const int amountOfNeighbours = 5;
            IEnumerable<Bird> birds = this.world.Entities.OfType<Bird>()
                                          .OrderBy(bird => Vector2.DistanceSquared(bird.Position, this.Entity.Position))
                                          .Take(amountOfNeighbours)
                                          .ToArray();

            Vector2 alignment = SteeringBehaviours.Alignment(this.Entity, birds.Where(InAlignmentRange));
            Vector2 cohesion = SteeringBehaviours.Cohesion(this.Entity, birds.Where(InCohesionRange));
            Vector2 separation = SteeringBehaviours.Separation(this.Entity, birds.Where(InSeparationRange));

            Vector2 target = alignment * this.alignmentWeight +
                             cohesion * this.cohesionWeight +
                             separation * this.separationWeight;

            if (target.Equals(Vector2.Zero))
            {
                //  When there is no target, start wandering
                target = this.Entity.Velocity + SteeringBehaviours.Wander(this.Entity, 20, 80);
            }

            return target * this.strength;
        }
    }
}