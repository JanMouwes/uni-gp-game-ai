using System;
using GameAI.entity;
using Microsoft.Xna.Framework;

namespace GameAI.behaviour.Complex
{
    public class WallAvoidance : SteeringBehaviour
    {
        private readonly World world;

        public WallAvoidance(MovingEntity entity, World world) : base(entity)
        {
            this.world = world;
        }

        public override Vector2 Calculate()
        {
            const float panicDistance = 20f;

            float distToTop = Entity.Pos.Y;
            float distToBottom = this.world.Height - distToTop;
            float distToLeft = Entity.Pos.X;
            float distToRight = this.world.Width - distToLeft;

            Vector2 localForce = new Vector2();

            if (distToTop < panicDistance) { localForce.Y += panicDistance - distToTop; }

            if (distToBottom < panicDistance) { localForce.Y -= panicDistance - distToBottom; }

            if (distToLeft < panicDistance) { localForce.X += panicDistance - distToLeft; }

            if (distToRight < panicDistance) { localForce.X -= panicDistance - distToRight; }

            return localForce;
        }
    }
}