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

        public FlockingBehaviour(MovingEntity entity, World world, double radius) : base(entity)
        {
            this.World = world;
            this.radius = radius;
        }

        public override Vector2 Calculate()
        {
            bool IsNear(MovingEntity entity)
            {
                Vector2 to = entity.Pos - this.Entity.Pos;

                return to.LengthSquared() < this.radius * this.radius;
            }

            // Check whether there are any entities nearby
            IEnumerable<MovingEntity> neighbors = this.World.entities.Where(IsNear);

            Vector2 target = (SteeringBehaviours.Separation(this.Entity, neighbors) * 1) + 
                             (SteeringBehaviours.Alignment(this.Entity, neighbors) * 3) +
                             (SteeringBehaviours.Cohesion(this.Entity, neighbors) * 2);
            
            if (target.Equals(Vector2.Zero))
            {
                //  When there is no target, start wandering
                target = SteeringBehaviours.Wander(this.Entity, 10, 10);
            }

            return target;
        }
    }
}