using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity
{
    public class Rock : BaseGameEntity
    {
        public Color Color { get; set; }

        public Rock(World world, Vector2 position, float scale, Color color) : base(world, scale)
        {
            this.Position = position;
            this.Scale = scale;
            this.Color = color;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            this.Graphics.Draw(spriteBatch);
        }
    }
}
