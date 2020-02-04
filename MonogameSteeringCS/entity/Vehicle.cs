using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.entity
{
    public class Vehicle : MovingEntity
    {
        public Color VColor { get; set; }

        public Vehicle(Vector2 pos, World w) : base(pos, w)
        {
            Velocity = new Vector2(0, 0);
            Scale = 5;

            VColor = Color.Black;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(this.Pos.X, this.Pos.Y);
            Vector2 radius = new Vector2(this.Scale);
            Vector2 velocity = new Vector2(this.Velocity.X, this.Velocity.Y);

            spriteBatch.DrawEllipse(position, radius, 360, VColor);

            spriteBatch.DrawLine(position, velocity * 2 + position, Color.Aqua);
        }
    }
}