using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.Entity.Components
{
    public class CircleGraphics : IGraphicsComponent
    {
        public BaseGameEntity Owner { get; }
        public Color Colour { get; set; } = Color.White;

        public CircleGraphics(BaseGameEntity owner)
        {
            this.Owner = owner;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(this.Owner.Position, this.Owner.Scale, 360, this.Colour);
        }
    }
}