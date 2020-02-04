using SteeringCS.entity;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SteeringCS
{
    class World
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
            Vehicle v = new Vehicle(new Vector2D(10, 10), this);
            v.VColor = Color.Blue;
            entities.Add(v);

            Target = new Vehicle(new Vector2D(100, 60), this)
            {
                VColor = Color.DarkRed,
                Pos = new Vector2D(100, 40)
            };
        }

        public void Update(float timeElapsed)
        {
            foreach (MovingEntity me in entities)
            {
                // me.SB = new SeekBehaviour(me); // restore later
                me.Update(timeElapsed);
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            entities.ForEach(e => e.Render(spriteBatch));
            Target?.Render(spriteBatch);
        }
    }
}