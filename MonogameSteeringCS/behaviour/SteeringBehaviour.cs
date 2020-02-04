using SteeringCS.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SteeringCS
{
    abstract class SteeringBehaviour
    {
        public MovingEntity Entity { get; set; }
        public abstract Vector2 Calculate();

        public SteeringBehaviour(MovingEntity entity)
        {
            this.Entity = entity;
        }
    }
}