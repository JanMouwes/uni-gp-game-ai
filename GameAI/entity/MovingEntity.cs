using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAI.behaviour;
using GameAI.behaviour.Complex;
using GameAI.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.entity
{
    public abstract class MovingEntity : BaseGameEntity
    {
        // for testing purposes
        private WallAvoidance wallAvoidance;
        private ObstacleAvoidance obstacleAvoidance;
        
        public Vector2 Velocity { get; set; }

        public Vector2 Orientation { get; set; } = new Vector2(1, 0);

        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        //TODO add maxForce
        public SteeringBehaviour Steering { get; set; } = DefaultBehaviour.Instance;

        public MovingEntity(Vector2 pos, World w) : base(pos, w)
        {
            Mass = 30;
            MaxSpeed = .01f;
            Velocity = new Vector2();
            this.wallAvoidance = new WallAvoidance(this, w);
            this.obstacleAvoidance = new ObstacleAvoidance(this, w);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (this.Steering != null)
            {
                Vector2 obstacleCalc = this.obstacleAvoidance.Calculate();
                bool shouldAvoid = obstacleCalc.LengthSquared() > 0;

                Vector2 steeringForce = shouldAvoid ? obstacleCalc : this.Steering.Calculate() + this.wallAvoidance.Calculate();

                if (shouldAvoid)
                {
                    Console.WriteLine($"Obstacle avoidance:{obstacleCalc}");
                    Console.WriteLine($"Steering:{steeringForce}");
                }
                Vector2 acceleration = steeringForce / Mass;

                Velocity += acceleration * elapsedSeconds * 0.95f;
            }

            Velocity = Velocity.Truncate(MaxSpeed);

            Pos += Velocity * elapsedSeconds;

            if (Velocity != Vector2.Zero) { this.Orientation = Velocity.NormalizedCopy(); }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(Pos, Pos + Velocity, Color.Yellow);
            //spriteBatch.DrawLine(Pos, Pos + Steering.Calculate(), Color.Green);
        }

        public override string ToString()
        {
            return $"{this.Pos} with velocity {Velocity}";
        }
    }
}