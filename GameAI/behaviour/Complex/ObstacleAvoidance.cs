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
    class ObstacleAvoidance : SteeringBehaviour
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
            Vector2 ahead05 = (viewBox / 2) + Entity.Pos;
            Vector2 ahead = viewBox + Entity.Pos;
            Vector2 ahead2 = (viewBox * viewBox) + Entity.Pos;

            foreach (BaseGameEntity o in w.obstacles)
            {
                // Add a circle around the obstacle which can't be crossed
                CircleF c = new CircleF(o.Pos, o.Scale + Entity.Scale);
                if(c.Contains(Entity.Pos) || c.Contains(ahead) || c.Contains(ahead2) || c.Contains(ahead05))
                {
                    Vector2 dist = new Vector2(Entity.Pos.X - o.Pos.X, Entity.Pos.Y - o.Pos.Y);
                    Vector2 haaks = new Vector2(-dist.Y, dist.X);
                    Vector2 minHaaks = -haaks;

                    Vector2 haaksDistPlus = new Vector2(haaks.X = ahead.X, haaks.Y - ahead.Y);
                    Vector2 haaksDistMin = new Vector2(minHaaks.X - ahead.X, minHaaks.Y - ahead.Y);

                    if (haaksDistPlus.Length() > haaksDistMin.Length()) return Entity.Velocity - haaks;
                    
                    return Entity.Velocity + haaks;
                }
            }
            // Return identity vector if there will be no collision
            return new Vector2();
        }
    }
}
