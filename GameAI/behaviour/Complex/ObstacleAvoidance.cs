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
        private int range;
        private World w;

        public ObstacleAvoidance(MovingEntity entity, World w, int range = 100) : base(entity)
        {
            this.w = w;
            this.range = range;
        }

        public override Vector2 Calculate()
        {
            // The detection box is the current velocity divided by the max velocity of the entity
            // range is the maximum size of the box
            Vector2 box = Entity.Velocity / Entity.MaxSpeed * range;
            // Add the box in front of the entity
            Vector2 ahead = box + Entity.Pos;
            Vector2 ahead2 = (box / 2) + Entity.Pos;
            // These will be the x and y in the returning Velocity
            float x = 0;
            float y = 0;

            foreach (BaseGameEntity o in w.obstacles)
            {
                // Add a circle around the obstacle which can't be crossed
                CircleF c = new CircleF(o.Pos, o.Scale);
                if(c.Contains(ahead) || c.Contains(ahead2))
                {
                    if (Entity.Pos.X < o.Pos.X)
                    {
                        x = -ahead.X - Entity.Velocity.X;
                    }
                    if (Entity.Pos.X > o.Pos.X)
                    {
                        x = ahead.X - Entity.Velocity.X;
                    }
                    if (Entity.Pos.Y < o.Pos.Y)
                    {
                        y = -ahead.Y - Entity.Velocity.Y;
                    }
                    if (Entity.Pos.Y > o.Pos.Y)
                    {
                        y = ahead.Y - Entity.Velocity.Y;
                    }
                    return new Vector2(x, y);
                }
            }
            // Return identity vector if there will be no collision
            return new Vector2();
        }
    }
}
