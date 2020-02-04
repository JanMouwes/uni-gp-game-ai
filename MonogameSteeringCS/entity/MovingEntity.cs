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

        public SteeringBehaviour Steering { get; set; }

        public MovingEntity(Vector2 pos, World w) : base(pos, w)
        {
            Mass = 30;
            MaxSpeed = 150;
            Velocity = new Vector2();
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
            
            if (this.Steering != null)
            {
                Vector2 steeringForce = this.Steering.Calculate();
                Vector2 acceleration = steeringForce / Mass;

                Velocity += acceleration * elapsedSeconds * 0.95f;
            }

            Velocity.Truncate(MaxSpeed);

            Pos += Velocity * elapsedSeconds;

            Console.WriteLine(ToString());
        }

        public override string ToString()
        {
            return String.Format("{0}", Velocity);
        }
    }
}