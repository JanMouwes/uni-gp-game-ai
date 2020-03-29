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
    public class ArriveBehaviour : SteeringBehaviour
    {
        private readonly float decelerateDistance;
        public Vector2 Target { get; set; }

        public ArriveBehaviour(MovingEntity entity, Vector2 target, float decelerateDistance) : base(entity)
        {
            this.Target = target;
            this.decelerateDistance = decelerateDistance;
        }

        public override Vector2 Calculate()
        {
            return SteeringBehaviours.Arrive(Target, Entity, decelerateDistance);
        }
    }
}