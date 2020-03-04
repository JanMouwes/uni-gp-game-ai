using System.Collections.Generic;
using System.Linq;
using GameAI.entity;
using Microsoft.Xna.Framework;

namespace GameAI.behaviour.Complex
{
    public class FlockingBehaviour : SteeringBehaviour
    {
        public World World;
        private readonly double radius;
        public int separation;
        public int alignment;
        public int cohesion;

        public FlockingBehaviour(MovingEntity entity, World world, double radius, int separation, int alignment,
            int cohesion) : base(entity)
        {
            this.World = world;
            this.radius = radius;
            this.separation = separation;
            this.alignment = alignment;
            this.cohesion = cohesion;
        }

        public FlockingBehaviour(MovingEntity entity, World world, double radius) : this(entity, world, radius, 1, 2, 5)
        { }

        public override Vector2 Calculate()
        {
            bool IsNear(MovingEntity entity)
            {
                Vector2 to = entity.Pos - this.Entity.Pos;

                return to.LengthSquared() < this.radius * this.radius;
            }

            // Check whether there are any entities nearby
            IEnumerable<MovingEntity> neighbors = this.World.entities.Where(IsNear);

            Vector2 target = (SteeringBehaviours.Separation(this.Entity, neighbors) * separation) + 
                             (SteeringBehaviours.Alignment(this.Entity, neighbors) * alignment) +
                             (SteeringBehaviours.Cohesion(this.Entity, neighbors) * cohesion);
            
            if (target.Equals(Vector2.Zero))
            {
                //  When there is no target, start wandering
                target = SteeringBehaviours.Wander(this.Entity, 10, 10);
            }

            return target;
        }
    }
}