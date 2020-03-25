using System;
using System.Collections.Generic;
using System.Linq;
using GameAI.behaviour;
using GameAI.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI
{
    public class World
    {
        // Entities and obstacles should be one list while spatial partitioning is not implemented
        private List<MovingEntity> entities = new List<MovingEntity>();
        private LinkedList<BaseGameEntity> obstacles = new LinkedList<BaseGameEntity>();

        public int Width { get; set; }
        public int Height { get; set; }

        public World(int w, int h)
        {
            Width = w;
            Height = h;
            populate();
        }

        private void populate(int vehicleCount = 50)
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
                    Color = Color.Blue, MaxSpeed = 24f, Mass = 1
                };
                v.Steering = new WanderBehaviour(v, -10, 10);

                entities.Add(v);
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
                float realRange = range + entity.Scale;
                
                return (location - entity.Pos).LengthSquared() < realRange * realRange;
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
        }

        public void Render(SpriteBatch spriteBatch)
        {
            entities.ForEach(e => e.Render(spriteBatch));
        }
    }
}