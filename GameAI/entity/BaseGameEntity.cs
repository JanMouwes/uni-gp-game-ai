using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using Color = Microsoft.Xna.Framework.Color;

namespace GameAI
{
    public abstract class BaseGameEntity
    {
        protected World world;

        public Vector2 Position { get; set; }
        public float Scale { get; set; }

        public BaseGameEntity(World world, float scale = 1)
        {
            this.world = world;
            this.Scale = scale;
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Render(SpriteBatch spriteBatch);
    }
}