using GameAI.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity
{
    public class Bird : MovingEntity
    {
        public Color Color { get; set; }

        public Bird(World world, IGraphicsComponent graphics = null) : base(world)
        {
            this.Velocity = new Vector2(0, 0);
            this.Scale = 2;

            this.Color = Color.Black;

            this.Graphics = graphics ?? new CircleGraphics(this) {Colour = this.Color};
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.Graphics.Draw(spriteBatch, gameTime);
        }
    }
}