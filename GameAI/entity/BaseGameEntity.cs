using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Shapes;
using Color = Microsoft.Xna.Framework.Color;

namespace GameAI
{
    public abstract class BaseGameEntity
    {
        public Vector2 Pos { get; set; }
        public float Scale { get; set; }
        public World World { get; set; }

        public BaseGameEntity(Vector2 pos, World w, float s)
        {
            this.Pos = pos;
            this.World = w;
            this.Scale = s;
        }

        public BaseGameEntity(Vector2 pos, World w) : this(pos, w, 1)
        {
        }

        public abstract void Update(GameTime gameTime);

        public abstract void Render(SpriteBatch spriteBatch);
    }
}