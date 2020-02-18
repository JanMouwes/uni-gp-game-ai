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
            neighbors = new MovingEntity[] {};
            this.World = world;
            this.radius = radius;
        }

        public override Vector2 Calculate()
        {
            foreach (var current in World.entities)
            {
                //  removing neighbors
                neighbors = neighbors?.Where(val => val != current).ToArray();

                Vector2 to = Vector2.Zero;
                to = current.Pos - Entity.Pos;

                if (current != Entity && to.LengthSquared() < radius * radius)
                {
                    neighbors.Append(current);
                }
            }

            Vector2 target = SteeringBehaviours.Cohesion(Entity, neighbors) +
                             SteeringBehaviours.Alignment(Entity, neighbors) +
                             SteeringBehaviours.Cohesion(Entity, neighbors);
            return target;
        }
    }
}
