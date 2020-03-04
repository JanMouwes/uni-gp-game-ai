using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.entity
{
    class Rock : BaseGameEntity
    {
        public Vector2 Pos { get; set; }
        public float Scale { get; set; }
        public World World { get; set; }

        public Rock(Vector2 pos, World w) : base(pos, w)
        { }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
