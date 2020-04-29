using GameAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.Entity
{
    public class Flag : BaseGameEntity
    {
        public Vehicle Carrier { get; set; }

        public readonly Team Team;

        public Flag(Team team, float scale = 1) : base(scale)
        {
            this.Team = team;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Carrier != null) { this.Position = this.Carrier.Position; }
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.DrawRectangle(this.Position, new Size2(this.Scale, this.Scale), this.Team.Colour);
        }
    }
}