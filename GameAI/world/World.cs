using System;
using System.Collections.Generic;
using System.Linq;
using GameAI.Steering.Simple;
using GameAI.Entity;
using GameAI.Entity.Components;
using GameAI.Entity.Navigation;
using GameAI.world;
using Graph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameAI
{
    public class World
    {
        public Graph<Vector2> NavigationGraph { get; set; }

        public readonly Dictionary<Color, Team> Teams;

        // Entities and obstacles should be one list while spatial partitioning is not implemented
        private readonly List<BaseGameEntity> entities = new List<BaseGameEntity>();
        public IEnumerable<BaseGameEntity> Entities => this.entities;

        public int Width { get; set; }
        public int Height { get; set; }
        public PathFinder PathFinder { get; set; }

        public World(int w, int h)
        {
            Width = w;
            Height = h;
            this.Teams = new Dictionary<Color, Team>();
        }

        public void Populate(int vehicleCount, Texture2D[] teamTextures, Texture2D rockTexture)
        {
            // Add obstacles
            // Rock r = new Rock(this, new Vector2(100, 100), 15, Color.Black);
            // r.Graphics = new TextureGraphics(r, rockTexture);
            // this.entities.Add(r);

            const int numberOfTeams = 2;
            Color[] teamColours =
            {
                Color.Blue,
                Color.Red
            };
            Vector2[][] teamSpawns =
            {
                new[]
                {
                    new Vector2(50, 50),
                    new Vector2(100, 50),
                    new Vector2(50, 100),
                },
                new[]
                {
                    new Vector2(this.Width - 50, this.Height  - 50),
                    new Vector2(this.Width - 100, this.Height - 50),
                    new Vector2(this.Width - 50, this.Height  - 100),
                }
            };

            Vector2[] teamFlagPosition =
            {
                new Vector2(75, 75),
                new Vector2(this.Width - 75, this.Height - 75)
            };

            for (int teamIndex = 0; teamIndex < numberOfTeams; teamIndex++)
            {
                Team team = new Team(teamColours[teamIndex]);
                Texture2D vehicleTexture = teamTextures[teamIndex];

                team.AddSpawnPoints(teamSpawns[teamIndex]);
                Flag flag = new Flag(this, team, 5f)
                {
                    Position = teamFlagPosition[teamIndex]
                };
                this.entities.Add(flag);

                team.Flag = flag;

                this.Teams.Add(team.Colour, team);

                Rectangle[] rectangles =
                {
                    new Rectangle(3, 698, 32, 32),
                    new Rectangle(3, 400, 32, 32)
                };

                // Add Entities
                for (int vehicleIndex = 0; vehicleIndex < vehicleCount; vehicleIndex++)
                {
                    Vehicle vehicle = new Vehicle(this, team)
                    {
                        MaxSpeed = 100f,
                        Mass = 1
                    };
                    vehicle.Graphics = new TextureGraphics(vehicle, vehicleTexture)
                    {
                        SourceRectangle = rectangles[teamIndex], RotationOffset = (float) Math.PI
                    };
                    vehicle.Steering = new WanderBehaviour(vehicle, 20);

                    SpawnVehicle(vehicle);
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

            vehicle.Death += OnVehicleDeath;
        }

        public void SpawnVehicle(Vehicle vehicle)
        {
            SpawnVehicle(vehicle, vehicle.Team.RandomSpawnPoint());
        }

        /// <summary>
        /// Removes vehicle from the world
        /// </summary>
        /// <param name="vehicle"></param>
        public void DespawnVehicle(Vehicle vehicle)
        {
            this.entities.Remove(vehicle);
            this.Teams[vehicle.Team.Colour].Vehicles.Remove(vehicle);

            vehicle.Death -= OnVehicleDeath;
        }

        public void OnVehicleDeath(object sender, Vehicle vehicle)
        {
            RespawnVehicle(vehicle);
        }

        public void RespawnVehicle(Vehicle vehicle)
        {
            vehicle.Position = vehicle.Team.RandomSpawnPoint();
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