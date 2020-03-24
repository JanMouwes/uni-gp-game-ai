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
        private List<BaseGameEntity> obstacles;

        public ObstacleAvoidance(MovingEntity entity, List<BaseGameEntity> obstacles, int range) : base(entity)
        {
            this.obstacles = obstacles;
            this.range = range;
        }

        public override Vector2 Calculate()
        {
            var box = Entity.Velocity / Entity.MaxSpeed * 100;
            var ahead = box + Entity.Pos;
            float x = 0;
            float y = 0;
            //var ahead2 = Entity.Pos + Entity.Velocity * 1 / 2;


            foreach (var o in obstacles)
            {
                CircleF c = new CircleF(o.Pos, o.Scale);
                if(c.Contains(ahead))
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
            return new Vector2(-10, -10);
        }
    }
}
