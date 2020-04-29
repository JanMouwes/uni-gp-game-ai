using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameAI.Entity.Components;
using GameAI.Entity.GoalBehaviour.Composite;
using GameAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity
{
    class Bird : MovingEntity
    {
        public Color Color { get; set; }

        private CircleGraphics graphics;

        public Bird(World world) : base(world)
        {
            Velocity = new Vector2(0, 0);
            Scale = 2;

            this.Color = Color.Black;
            
            graphics = new CircleGraphics(this);
            graphics.Colour = Color;
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            graphics.Draw(spriteBatch);
        }
    }
}
