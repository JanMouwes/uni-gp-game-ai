using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameAI.Steering;
using GameAI.Steering.Complex;
using GameAI.Steering.Simple;
using GameAI.Entity;
using GameAI.Entity.GoalBehaviour.Composite;
using GameAI.Navigation;
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
        private readonly List<BaseGameEntity> entities = new List<BaseGameEntity>();
        public IEnumerable<BaseGameEntity> Entities => this.entities;

        private readonly PathFinder pathFinder;
        
        public int Width { get; set; }
        public int Height { get; set; }

        public World(int w, int h, PathFinder pathFinder)
        {
            Width = w;
            Height = h;
            this.pathFinder = pathFinder;
            this.Teams = new Dictionary<Color, Team>();
        }

        public void Populate(int vehicleCount)
        {
            // Add obstacles
            // Rock r = new Rock(this, new Vector2(300, 300), 150, Color.Black);
            // this.entities.Add(r);

            const int numberOfTeams = 2;
            Color[] teamColours =
            {
                Color.Blue,
                Color.Orange
            };
            Vector2[] teamSpawns =
            {
                new Vector2(50, 50),
                new Vector2(this.Width - 50, this.Height - 50)
            };

            for (int teamIndex = 0; teamIndex < numberOfTeams; teamIndex++)
            {
                Team team = new Team(teamColours[teamIndex]);

                team.AddSpawnPoint(teamSpawns[teamIndex]);
                Flag flag = new Flag(this, team, 5f)
                {
                    Position = teamSpawns[teamIndex]
                };
                this.entities.Add(flag);

                team.Flag = flag;

                this.Teams.Add(team.Colour, team);

                // Add Entities
                for (int vehicleIndex = 0; vehicleIndex < vehicleCount; vehicleIndex++)
                {
                    Vehicle vehicle = new Vehicle(this, team)
                    {
                        MaxSpeed = 100f,
                        Mass = 1
                    };
                    vehicle.Steering = new WanderBehaviour(vehicle, 20);

                    SpawnVehicle(vehicle);
                }
            }

            foreach (Team team in this.Teams.Values)
            {
                Team otherTeam = this.Teams.Values.First(item => item.Colour != team.Colour);
                
                foreach (Vehicle vehicle in team.Vehicles)
                {
                    vehicle.Brain.AddSubgoal(new CaptureFlag(vehicle, otherTeam.Flag, this.pathFinder));
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

                return (location - entity.Position).LengthSquared() < realRange * realRange;
            }

            return this.entities.Concat(this.entities).Where(IsNear);
        }

        public void SpawnVehicle(Vehicle vehicle, Vector2 position)
        {
            vehicle.Position = position;

            this.entities.Add(vehicle);
            vehicle.Team.Vehicles.AddLast(vehicle);
        }

        public void SpawnVehicle(Vehicle vehicle)
        {
            SpawnVehicle(vehicle, vehicle.Team.RandomSpawnPoint());
        }

        public void Update(GameTime gameTime)
        {
            foreach (BaseGameEntity me in this.entities) { me.Update(gameTime); }
        }

        public void Render(SpriteBatch spriteBatch)
        {
            entities.ForEach(e => e.Render(spriteBatch));
            this.entities.ForEach(o => o.Render(spriteBatch));
        }
    }
}