using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity.Components
{
    public class AnimatedTextureGraphics : IGraphicsComponent
    {
        private int currentFrame = 0;
        private float nextUpdate = 0;

        public BaseGameEntity Owner { get; }
        public readonly Texture2D SpriteSheet;

        /// <summary>
        /// Wraps around if value >= Frames.Count
        /// </summary>
        public int CurrentFrame
        {
            get => this.currentFrame;
            set => this.currentFrame = value >= this.Frames.Count ? 0 : value;
        }

        /// <summary>
        /// Delay between frames in milliseconds
        /// </summary>
        public float FrameDelay { get; set; }

        public readonly IList<Rectangle> Frames;
        public Rectangle SourceRectangle => this.Frames[this.CurrentFrame];
        public Vector2 RotationOrigin;
        public float RotationOffset { get; set; }

        public Color Colour { get; set; } = Color.White;
        
        public AnimatedTextureGraphics(BaseGameEntity owner, Texture2D spriteSheet, IEnumerable<Rectangle> frames, float frameDelay = 250f)
        {
            this.Owner = owner;
            this.FrameDelay = frameDelay;
            this.SpriteSheet = spriteSheet;
            this.Frames = frames.ToList();
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.RotationOrigin = new Vector2(this.SourceRectangle.Width / 2f, this.SourceRectangle.Height / 2f);
            float rotation = this.Owner.Rotation + this.RotationOffset;

            spriteBatch.Draw(this.SpriteSheet, this.Owner.Position, this.SourceRectangle, this.Colour, rotation, this.RotationOrigin, 1, SpriteEffects.None, 1);
            
            if (this.nextUpdate <= gameTime.TotalGameTime.TotalMilliseconds)
            {
                this.nextUpdate = (float)gameTime.TotalGameTime.TotalMilliseconds + this.FrameDelay;


                this.CurrentFrame++;
            }
        }
    }
}