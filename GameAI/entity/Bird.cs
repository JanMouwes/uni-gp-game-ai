using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAI.Entity.GoalBehaviour.Composite;
using GameAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity
{
    class Bird : MovingEntity
    {
        public Color Color { get; set; }

        public Bird(World world) : base(world)
        {
            Velocity = new Vector2(0, 0);
            Scale = 5;

            this.Color = Color.Black;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            this.Graphics.Draw(spriteBatch);
        }
    }
}
