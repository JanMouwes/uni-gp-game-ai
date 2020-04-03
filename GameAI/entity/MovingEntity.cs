using System;
using GameAI.Steering;
using GameAI.Steering.Complex;
using GameAI.Steering.Simple;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.entity
{
    public abstract class MovingEntity : BaseGameEntity
    {
        // for testing purposes

        private readonly WallAvoidance wallAvoidance;

        private readonly ObstacleAvoidance obstacleAvoidance;

        public Vector2 Velocity { get; set; }

        public Vector2 Orientation { get; set; } = new Vector2(1, 0);

        public float Mass { get; set; }

        public float MaxSpeed { get; set; }

        //TODO add maxForce
        public SteeringBehaviour Steering { get; set; } = DefaultSteeringBehaviour.Instance;

        public MovingEntity(Vector2 pos, World w) : base(pos, w)
        {
            Mass = 30;
            MaxSpeed = .01f;
            Velocity = new Vector2();
            this.wallAvoidance = new WallAvoidance(this, w, 5f, 1.5f);
            this.obstacleAvoidance = new ObstacleAvoidance(this, w);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (this.Steering != null)
            {
                Vector2 obstacleCalc = this.obstacleAvoidance.Calculate();
                bool shouldAvoidObstacles = obstacleCalc.LengthSquared() > 0;

                Vector2 wallCalc = this.wallAvoidance.Calculate();
                bool shouldAvoidWalls = wallCalc.LengthSquared() > 0;

                Vector2 steeringForce;

                if (shouldAvoidWalls) { steeringForce = wallCalc; }              // Avoid walls first
                else if (shouldAvoidObstacles) { steeringForce = obstacleCalc; } // No walls to avoid, avoid obstacles
                else { steeringForce = Steering.Calculate(); }                   // No walls or obstacles, do regular steering

                Vector2 acceleration = steeringForce / Mass;

                Velocity += acceleration * elapsedSeconds;
            }

            Velocity *= .95f;

            Velocity = Velocity.Truncate(MaxSpeed);

            Pos += Velocity * elapsedSeconds;

            if (Velocity != Vector2.Zero) { this.Orientation = Velocity.NormalizedCopy(); }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawLine(Pos, Pos + Velocity, Color.Yellow);
            spriteBatch.DrawLine(Pos, Pos + Steering.Calculate(), Color.Green);
        }

        public override string ToString()
        {
            return $"{this.Pos} with velocity {Velocity}";
        }

        // public string DebugInfo()
        // {
        //     
        // }
    }
}