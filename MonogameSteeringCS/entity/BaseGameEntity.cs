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

        public BaseGameEntity(Vector2 pos, World w)
        {
            this.Pos = pos;
            this.World = w;
        }

        public abstract void Update(GameTime gameTime);

        public virtual void Render(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(this.Pos.X, this.Pos.Y);

            //Brushes.Blue, new Rectangle((int) Pos.X,(int) Pos.Y, 10, 10)
            spriteBatch.DrawCircle(position, 10f, 360, Color.Blue);
        }
    }
}