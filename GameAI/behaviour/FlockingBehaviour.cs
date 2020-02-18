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
        private MovingEntity[] neighbors;
        public World World;
        private readonly double radius;

        public FlockingBehaviour(MovingEntity entity, World world, double radius) : base(entity)
        {
            neighbors = new MovingEntity[] { };
            this.World = world;
            this.radius = radius;
        }

        public override Vector2 Calculate()
        {
            bool IsNear(MovingEntity entity)
            {
                Vector2 to = entity.Pos - this.Entity.Pos;

                return to.LengthSquared() < radius * radius;
            }

            IEnumerable<MovingEntity> neighbours = this.World.entities.Where(IsNear);

            foreach (MovingEntity current in neighbours)
            {
                
            }

            Vector2 target = SteeringBehaviours.Cohesion(Entity, neighbors)  +
                             SteeringBehaviours.Alignment(Entity, neighbors) +
                             SteeringBehaviours.Cohesion(Entity, neighbors);

            return target;
        }
    }
}