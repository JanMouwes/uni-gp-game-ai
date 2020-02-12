using System.Collections.Generic;
using GameAI.behaviour;
using GameAI.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI
{
    public class World
    {
        private List<MovingEntity> Entities = new List<MovingEntity>();
        public Vehicle Target { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public World(int w, int h)
        {
            Width = w;
            Height = h;
            Populate();
        }

        private void Populate()
        {
            Vehicle v = new Vehicle(new Vector2(10, 10), this) {VColor = Color.Blue, MaxSpeed = 64f, Mass = 1 };
            Vehicle w = new Vehicle(new Vector2(10, 20), this) {VColor = Color.Blue, MaxSpeed = 64f, Mass = 1 };
            Vehicle x = new Vehicle(new Vector2(20, 10), this) {VColor = Color.Blue, MaxSpeed = 64f, Mass = 1 };
            Vehicle y = new Vehicle(new Vector2(20, 20), this) {VColor = Color.Blue, MaxSpeed = 64f, Mass = 1 };
            Vehicle z = new Vehicle(new Vector2(20, 30), this) {VColor = Color.Blue, MaxSpeed = 64f, Mass = 1};

            Entities.Add(v);
            Entities.Add(w);
            Entities.Add(x);
            Entities.Add(y);
            Entities.Add(z);

            Target = new Vehicle(new Vector2(30, 30), this)
            {
                MaxSpeed = 12f,
                VColor = Color.Green,
                Mass = 10
            };

            Target.Steering = new FleeBehaviour(Target, z);

            v.Steering = new LeaderFollowingBehaviour(v, Target, new Vector2(10, 10));
            w.Steering = new LeaderFollowingBehaviour(w, v, new Vector2(10, 10));
            x.Steering = new LeaderFollowingBehaviour(x, w, new Vector2(10, 10));
            y.Steering = new LeaderFollowingBehaviour(y, x, new Vector2(10, 10));
            z.Steering = new LeaderFollowingBehaviour(z, y, new Vector2(10, 10));
        }

        public void Update(GameTime gameTime)
        {
            foreach (MovingEntity me in Entities)
            {
                // me.SB = new SeekBehaviour(me); // restore later
                me.Update(gameTime);
            }
            
            Target?.Update(gameTime);
        }

        public void Render(SpriteBatch spriteBatch)
        {
            Entities.ForEach(e => e.Render(spriteBatch));
            Target?.Render(spriteBatch);
        }
    }
}