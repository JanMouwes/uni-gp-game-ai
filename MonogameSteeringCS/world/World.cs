using System.Collections.Generic;
using GameAI.behaviour;
using GameAI.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI
{
    public class World
    {
        private List<MovingEntity> entities = new List<MovingEntity>();
        public Vehicle Target { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public World(int w, int h)
        {
            Width = w;
            Height = h;
            populate();
        }

        private void populate()
        {
            Vehicle v = new Vehicle(new Vector2(10, 10), this) {VColor = Color.Blue, MaxSpeed = 32f,};

            entities.Add(v);

            Target = new Vehicle(new Vector2(100, 60), this)
            {
                MaxSpeed = 48f,
                VColor = Color.DarkRed,
                Mass = 10
            };

            Target.Steering = new FleeBehaviour(Target, v);

            v.Steering = new SeekBehaviour(v, Target);
        }

        public void Update(GameTime gameTime)
        {
            foreach (MovingEntity me in entities)
            {
                // me.SB = new SeekBehaviour(me); // restore later
                me.Update(gameTime);
            }
            
            Target?.Update(gameTime);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            entities.ForEach(e => e.Render(spriteBatch));
            Target?.Render(spriteBatch);
        }
    }
}