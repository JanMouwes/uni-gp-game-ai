using GameAI.entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.behaviour
{
    public class SeekBehaviour : SteeringBehaviour
    {
        public Vector2 Target { get; set; }

        public SeekBehaviour(MovingEntity entity, Vector2 target) : base(entity)
        {
            this.Target = target;
        }

        public override Vector2 Calculate()
        {
            return SteeringBehaviours.Seek(Target, Entity);
        }
    }
}