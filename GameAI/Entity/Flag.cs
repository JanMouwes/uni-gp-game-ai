using System;
using GameAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.Entity
{
    public class Flag : BaseGameEntity
    {
        public event Action<Flag> Captured;

        public float CapturedRange { get; set; } = 5f;

        public Ship Carrier { get; set; }

        public readonly Team Team;

        public Flag(Team team, float scale = 1) : base(scale)
        {
            this.Team = team;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Carrier != null)
            {
                this.Position = this.Carrier.Position;

                Vector2 otherTeamsBase = this.Carrier.Team.Base;

                if (Vector2.DistanceSquared(this.Position, otherTeamsBase) < this.CapturedRange * this.CapturedRange)
                {
                    Captured?.Invoke(this);
                    this.Carrier = null;
                }
            }
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawRectangle(this.Position, new Size2(this.Scale, this.Scale), this.Team.Colour);
        }
    }
}