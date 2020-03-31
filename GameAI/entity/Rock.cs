using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.entity
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
            spriteBatch.DrawCircle(this.Position, Scale, 360, Color);
        }
    }
}
