using System;
using System.Collections.Generic;
using System.Linq;
using GameAI.behaviour;
using GameAI.behaviour.Complex;
using GameAI.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI
{
    public class World
    {
        // Entities and obstacles should be one list while spatial partitioning is not implemented
        public List<MovingEntity> entities = new List<MovingEntity>();
        private List<BaseGameEntity> obstacles = new List<BaseGameEntity>();

        public int Width { get; set; }
        public int Height { get; set; }

        public World(int w, int h)
        {
            Width = w;
            Height = h;
            populate();
        }

        private void populate(int vehicleCount = 100)
        {
            Random random = new Random();

            // Add obstacles
            Rock r = new Rock(this, new Vector2(300, 300), 100, Color.Black);
            obstacles.Add(r);

            // Add Entities
            for (int i = 0; i < vehicleCount; i++)
            {
                Vector2 position = new Vector2
                {
                    X = random.Next(0, Width),
                    Y = random.Next(0, Height)
                };

                Vehicle v = new Vehicle(position, this)
                {
                    VColor = Color.Blue, MaxSpeed = 100f, Mass = 1
                };
                v.Steering = new ObstacleAvoidance(v, obstacles, 50);

                entities.Add(v);
            }
            foreach (var entity in entities)
            {
                entity.World = this;
            }
        }

        /// <summary>
        /// Exists to abstract away the finding of entities
        /// </summary>
        /// <param name="location"></param>
        /// <param name="range"></param>
        /// <returns></returns>
        public IEnumerable<BaseGameEntity> FindEntitiesNear(Vector2 location, float range)
        {
            bool IsNear(BaseGameEntity entity)
            {
                return (location - entity.Pos).LengthSquared() < range * range;
            }

            return this.entities.Concat(this.obstacles).Where(IsNear);
        }

        public void Update(GameTime gameTime)
        {
            foreach (MovingEntity me in entities)
            {
                // me.SB = new SeekBehaviour(me); // restore later
                me.Update(gameTime);
            }

            foreach (BaseGameEntity me in obstacles)
            {
                me.Update(gameTime);
            }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            entities.ForEach(e => e.Render(spriteBatch));
            obstacles.ForEach(o => o.Render(spriteBatch));
        }
    }
}