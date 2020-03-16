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
            var ahead = Entity.Pos + Entity.Velocity * 1;
            var ahead2 = Entity.Pos + Entity.Velocity * 1 / 2;

            foreach (var o in obstacles)
            {
                CircleF c = new CircleF(o.Pos, o.Scale);
                if (c.Contains(ahead2))
                {
                    return new Vector2(ahead.X - Entity.Velocity.X, ahead.Y - Entity.Velocity.Y);
                }
                if(c.Contains(ahead))
                {
                    return new Vector2(0, 0);
                }
            }
            return new Vector2(-10, -10);
        }
    }
}
