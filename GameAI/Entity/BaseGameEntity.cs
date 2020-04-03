using GameAI.Entity.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity
{
    public abstract class BaseGameEntity
    {
        protected World World;

        public IGraphicsComponent Graphics { get; set; } = new DefaultGraphics();

        public Vector2 Position { get; set; }
        public float Scale { get; set; }

        public BaseGameEntity(World world, float scale = 1)
        {
            this.World = world;
            this.Scale = scale;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Render(SpriteBatch spriteBatch);
    }
}