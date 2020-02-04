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
    public class FleeBehaviour : SteeringBehaviour
    {
        public MovingEntity Target { get; set; }

        public FleeBehaviour(MovingEntity entity, MovingEntity target) : base(entity)
        {
            this.Target = target;
        }

        public override Vector2 Calculate()
        {
            return SteeringBehaviours.Flee(Target.Pos, Entity);
        }
    }
}