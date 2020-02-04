using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAI.behaviour;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.entity
{
    public abstract class MovingEntity : BaseGameEntity
    {
        public Vector2 Velocity { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }

        public SteeringBehaviour SB { get; set; }

        public MovingEntity(Vector2 pos, World w) : base(pos, w)
        {
            Mass = 30;
            MaxSpeed = 150;
            Velocity = new Vector2();
        }

        public override void Update(float timeElapsed)
        {
            if (SB != null)
            {
                Vector2 SteeringForce = SB.Calculate();
                Vector2 Acceleration = SteeringForce / Mass;

                Velocity += Acceleration * timeElapsed;
            }

            Velocity.Truncate(MaxSpeed);

            Pos += Velocity * timeElapsed;

            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return String.Format("{0}", Velocity);
        }
    }
}