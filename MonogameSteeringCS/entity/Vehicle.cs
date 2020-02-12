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
            spriteBatch.DrawCircle(Pos, Scale, 360, VColor);
            
            // Vector2 offsetY = this.Pos + new Vector2(0, this.Scale * -0.5f);
            // Vector2 offsetX = new Vector2(this.Scale * .5f, 0);
            //
            // Vector2 leftCorner = offsetY     - offsetX;
            // Vector2 rightCorner = offsetY    + offsetX;
            // Vector2 forwardCorner = this.Pos + new Vector2(0, this.Scale);
            //
            // spriteBatch.DrawLine(this.Pos, leftCorner, VColor);
            // spriteBatch.DrawLine(this.Pos, rightCorner, VColor);
            // spriteBatch.DrawLine(forwardCorner, leftCorner, VColor);
            // spriteBatch.DrawLine(forwardCorner, rightCorner, VColor);

            base.Render(spriteBatch);
        }
    }
}