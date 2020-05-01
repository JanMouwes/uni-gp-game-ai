using GameAI.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity
{
    public abstract class BaseGameEntity
    {
        public IGraphicsComponent Graphics { get; set; } = new DefaultGraphics();

        public Vector2 Position { get; set; }
        public float Scale { get; set; }
        public virtual float Rotation { get; } = 0f;

        public BaseGameEntity(float scale = 1)
        {
            this.Scale = scale;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Render(SpriteBatch spriteBatch, GameTime gameTime);
    }
}