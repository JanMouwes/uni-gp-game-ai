using System;
using GameAI.Entity.GoalBehaviour.Composite;
using GameAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity
{
    public class Ship : MovingEntity
    {
        public event EventHandler<Ship> Death;

        public Color Color { get; set; }

        public readonly Think Brain;

        public readonly Team Team;

        public Ship(World world, Team team) : base(world)
        {
            this.Team = team;

            this.Velocity = new Vector2(0, 0);
            this.Scale = 5;

            this.Color = team.Colour;

            this.Brain = new Think(this, world);
            this.Brain.Activate();
        }

        public override void Update(GameTime gameTime)
        {
            this.Brain.Process(gameTime);

            base.Update(gameTime);
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.Graphics.Draw(spriteBatch, gameTime);
        }

        public void Kill()
        {
            Death?.Invoke(this, this);
            this.Brain.Terminate();
        }
    }
}