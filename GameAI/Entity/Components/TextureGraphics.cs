using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity.Components
{
    public class TextureGraphics : IGraphicsComponent
    {
        public BaseGameEntity Owner { get; }
        public Texture2D Texture;
        public Rectangle SourceRectangle;
        public Vector2 RotationOrigin;
        public float RotationOffset { get; set; } = 0f;

        public Color Colour { get; set; } = Color.White;

        public TextureGraphics(BaseGameEntity owner, Texture2D texture)
        {
            this.Owner = owner;
            this.Texture = texture;
            this.SourceRectangle = texture.Bounds;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.RotationOrigin = new Vector2(this.SourceRectangle.Width / 2f, this.SourceRectangle.Height / 2f);
            float rotation = Owner.Rotation + this.RotationOffset;

            spriteBatch.Draw(this.Texture, Owner.Position, this.SourceRectangle, this.Colour, rotation, this.RotationOrigin, 1, SpriteEffects.None, 1);
        }
    }
}