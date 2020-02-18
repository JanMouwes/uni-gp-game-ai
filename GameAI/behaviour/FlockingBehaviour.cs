using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAI.entity;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.behaviour
{
    class FlockingBehaviour : SteeringBehaviour
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
                Vector2 to = entity.Pos - Entity.Pos;

                return to.LengthSquared() < radius * radius;
            }

            // Check whether there are any entities nearby
            IEnumerable<MovingEntity> neighbors = World.entities.Where(IsNear);

            Vector2 target = (SteeringBehaviours.Separation(Entity, neighbors) * 1) + 
                             (SteeringBehaviours.Alignment(Entity, neighbors) * 3) +
                             (SteeringBehaviours.Cohesion(Entity, neighbors) * 2);

            target.Normalize();

            if (target.X == 0 && target.Y == 0)
            {
                //  When there is no target, start wandering
                target = SteeringBehaviours.Wander(Entity, 10, 10);
            }

            return target;
        }
    }
}