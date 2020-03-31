using System;
using System.Collections.Generic;
using System.Linq;
using GameAI.Steering;
using GameAI.Steering.Complex;
using GameAI.Steering.Simple;
using GameAI.entity;
using GameAI.world;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI
{
    public class World
    {
        public readonly Dictionary<Color, Team> Teams;

        // Entities and obstacles should be one list while spatial partitioning is not implemented
        public List<MovingEntity> entities = new List<MovingEntity>();
        public List<BaseGameEntity> obstacles = new List<BaseGameEntity>();

        public int Width { get; set; }
        public int Height { get; set; }

        public World(int w, int h)
        {
            Width = w;
            Height = h;
            this.Teams = new Dictionary<Color, Team>();
        }

        public void Populate(int vehicleCount)
        {
            Random random = new Random();

            // Add obstacles
            Rock r = new Rock(this, new Vector2(300, 300), 150, Color.Black);
            obstacles.Add(r);

            const int numberOfTeams = 2;
            Color[] teamColours =
            {
                Color.Blue,
                Color.Orange
            };

            for (int teamIndex = 0; teamIndex < numberOfTeams; teamIndex++)
            {
                Team team = new Team(teamColours[teamIndex]);

                this.Teams.Add(team.Colour, team);

                // Add Entities
                for (int vehicleIndex = 0; vehicleIndex < vehicleCount; vehicleIndex++)
                {
                    Vector2 position = new Vector2
                    {
                        X = random.Next(0, Width),
                        Y = random.Next(0, Height)
                    };

                    Vehicle vehicle = new Vehicle(position, this, team)
                    {
                        MaxSpeed = 100f, Mass = 1
                    };
                    vehicle.Steering = new WanderBehaviour(vehicle, 20);

                    SpawnVehicle(vehicle, position);
                }
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

        public void SpawnVehicle(Vehicle vehicle, Vector2 position)
        {
            vehicle.Pos = position;

            this.entities.Add(vehicle);
            vehicle.Team.Vehicles.AddLast(vehicle);
        }

        public void Update(GameTime gameTime)
        {
            foreach (MovingEntity me in entities)
            {
                // me.SB = new SeekBehaviour(me); // restore later
                me.Update(gameTime);
            }

            foreach (BaseGameEntity me in obstacles) { me.Update(gameTime); }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            entities.ForEach(e => e.Render(spriteBatch));
            obstacles.ForEach(o => o.Render(spriteBatch));
        }
    }
}