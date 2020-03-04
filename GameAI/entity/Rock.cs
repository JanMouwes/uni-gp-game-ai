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
        public float Scale { get; set; }
        public Color Color { get; set; }
        public World World { get; set; }

        public Rock(Vector2 pos, World w, Color color) : base(pos, w)
        {
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
