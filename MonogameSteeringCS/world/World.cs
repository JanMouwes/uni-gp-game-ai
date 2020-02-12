using System;
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
        public int Width { get; set; }
        public int Height { get; set; }

        public World(int w, int h)
        {
            Width = w;
            Height = h;
            populate();
        }

        private void populate(int vehicleCount = 3)
        {
            Random random = new Random();

            for (int i = 0; i < vehicleCount; i++)
            {
                Vector2 position = new Vector2
                {
                    X = random.Next(0, Width),
                    Y = random.Next(0, Height)
                };

                Vehicle v = new Vehicle(position, this)
                {
                    VColor = Color.Blue, MaxSpeed = 24f, Mass = 1
                };
                v.Steering = new WanderBehaviour(v, -10, 10);

                entities.Add(v);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (MovingEntity me in entities)
            {
                // me.SB = new SeekBehaviour(me); // restore later
                me.Update(gameTime);
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            entities.ForEach(e => e.Render(spriteBatch));
        }
    }
}