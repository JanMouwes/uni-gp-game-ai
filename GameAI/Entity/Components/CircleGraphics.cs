using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.Entity.Components
{
    public class CircleGraphics : IGraphicsComponent
    {
        public Vehicle Owner { get; }

        public CircleGraphics(Vehicle owner)
        {
            this.Owner = owner;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(this.Owner.Position, this.Owner.Scale, 360, this.Owner.Color);
        }
    }
}