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
    class Rock : BaseGameEntity
    {
        public Vector2 Pos { get; set; }
        public int Scale { get; set; }
        public Color Color { get; set; }
        public World World { get; set; }

        public Rock(World w, Vector2 pos, int scale, Color color) : base(pos, w, scale)
        {
            this.Pos = pos;
            this.Scale = scale;
            this.Color = color;
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawCircle(Pos, Scale, 360, Color);

        }
    }
}
