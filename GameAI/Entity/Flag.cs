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

        public Flag(World world, Team team, float scale = 1) : base(world, scale)
        {
            this.Team = team;
        }

        public override void Update(GameTime gameTime)
        {
            if (this.Carrier != null) { this.Position = Carrier.Position; }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(Position, new Size2(Scale, Scale), this.Team.Colour);
        }
    }
}