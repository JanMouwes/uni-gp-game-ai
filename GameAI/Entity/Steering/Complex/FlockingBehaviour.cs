using System.Collections.Generic;
using System.Linq;
using GameAI.Entity.Steering.Simple;
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

        private readonly WanderBehaviour innerWander;

        public FlockingBehaviour(MovingEntity entity, World world, float radius, float alignmentWeight = 6, float cohesionWeight = 16, float separationWeight = 16) : base(entity)
        {
            this.world = world;
            this.alignmentRadius = radius;
            this.cohesionRadius = radius;
            this.separationRadius = radius;

            this.alignmentWeight = alignmentWeight;
            this.cohesionWeight = cohesionWeight;
            this.separationWeight = separationWeight;

            this.innerWander = new WanderBehaviour(entity, radius);
        }

        public override Vector2 Calculate()
        {
            bool IsNear(BaseGameEntity entity, float range) => Vector2.DistanceSquared(this.Entity.Position, entity.Position) < range * range;

            bool InAlignmentRange(MovingEntity entity) => IsNear(entity, this.alignmentRadius);
            bool InCohesionRange(MovingEntity entity) => IsNear(entity, this.cohesionRadius);
            bool InSeparationRange(MovingEntity entity) => IsNear(entity, this.separationRadius);

            const int amountOfNeighbours = 10;
            IEnumerable<Bird> birds = this.world.Entities.OfType<Bird>()
                                          .OrderBy(bird => Vector2.DistanceSquared(bird.Position, this.Entity.Position))
                                          .Where(entity => entity != this.Entity)
                                          .Take(amountOfNeighbours)
                                          .ToArray();

            //  When no neighbours nearby, wander
            if (!birds.Any()) { return this.innerWander.Calculate(); }

            Vector2 alignment = SteeringBehaviours.Alignment(birds.Where(InAlignmentRange));
            Vector2 cohesion = SteeringBehaviours.Cohesion(this.Entity, birds.Where(InCohesionRange));
            Vector2 separation = SteeringBehaviours.Separation(this.Entity, birds.Where(InSeparationRange));

            Vector2 target = alignment * this.alignmentWeight +
                             cohesion * this.cohesionWeight +
                             separation * this.separationWeight;

            return target * this.Strength;
        }
    }
}