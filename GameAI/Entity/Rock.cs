using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity
{
    public class Rock : BaseGameEntity
    {
        public Color Color { get; set; }

        public Rock(Vector2 position, float scale, Color color) : base(scale)
        {
            this.Position = position;
            this.Scale = scale;
            this.Color = color;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.Graphics.Draw(spriteBatch, gameTime);
        }
    }
}