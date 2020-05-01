using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity.Components
{
    public interface IGraphicsComponent
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}