using System;
using GameAI.Entity.GoalBehaviour.Composite;
using GameAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.Entity
{
    public class Vehicle : MovingEntity
    {
        public Color Color { get; set; }

        public readonly Think<Vehicle> Brain;

        public Team Team;

        public event EventHandler<Vehicle> Death;

        public Vehicle(World world, Team team) : base(world)
        {
            this.Team = team;

            Velocity = new Vector2(0, 0);
            Scale = 5;

            this.Color = team.Colour;

            this.Brain = new Think<Vehicle>(this);
            this.Brain.Activate();
        }

        public override void Update(GameTime gameTime)
        {
            this.Brain.Process(gameTime);

            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            this.Graphics.Draw(spriteBatch);
        }

        public void Kill()
        {
            this.Death?.Invoke(this, this);
            this.Brain.Terminate();
        }
    }
}