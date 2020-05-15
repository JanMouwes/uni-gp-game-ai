using GameAI.Entity.Steering;
using GameAI.Entity.Steering.Complex;
using GameAI.Entity.Steering.Simple;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.Entity
{
    public abstract class MovingEntity : BaseGameEntity
    {
        private readonly WallAvoidance wallAvoidance;

        private readonly ObstacleAvoidance obstacleAvoidance;

        public Vector2 Velocity { get; set; }

        public Vector2 Orientation { get; set; }

        public override float Rotation => this.Orientation.ToAngle();

        public float Mass { get; set; }

        public float MaxSpeed { get; set; }

        public float MinSpeed { get; set; }

        //TODO add maxForce
        public SteeringBehaviour Steering { get; set; } = DefaultSteeringBehaviour.Instance;

        public MovingEntity(World world)
        {
            this.Mass = 30;
            this.MaxSpeed = .01f;
            this.MinSpeed = 1.0f;
            this.Velocity = new Vector2();
            this.Orientation = new Vector2(1, 0);

            this.wallAvoidance = new WallAvoidance(this, world, 15f, 1.5f);
            this.obstacleAvoidance = new ObstacleAvoidance(this, world);
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;

            if (this.Steering != null)
            {
                Vector2 obstacleCalc = this.obstacleAvoidance.Calculate();
                Vector2 wallCalc = this.wallAvoidance.Calculate();

                bool shouldAvoidObstacles = obstacleCalc.LengthSquared() > 0;
                bool shouldAvoidWalls = wallCalc.LengthSquared() > 0;

                // Prioritise avoiding walls and obstacles
                Vector2 steeringForce = shouldAvoidWalls || shouldAvoidObstacles ? wallCalc + obstacleCalc : this.Steering.Calculate();
                Vector2 acceleration = steeringForce / this.Mass;

                this.Velocity += acceleration * elapsedSeconds;
            }

            this.Velocity *= .95f;

            this.Velocity = this.Velocity.Truncate(this.MaxSpeed);

            this.Position += (this.Velocity * this.MinSpeed) * elapsedSeconds;

            if (this.Velocity != Vector2.Zero) { this.Orientation = this.Velocity.NormalizedCopy(); }
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawLine(this.Position, this.Position + this.Velocity, Color.Yellow);
            spriteBatch.DrawLine(this.Position, this.Position + this.Steering.Calculate(), Color.Green);
        }

        public override string ToString()
        {
            return $"{this.Position} with velocity {this.Velocity}";
        }

        // public string DebugInfo()
        // {
        //     
        // }
    }
}