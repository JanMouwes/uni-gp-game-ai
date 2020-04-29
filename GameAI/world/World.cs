using System;
using System.Collections.Generic;
using System.Linq;
using GameAI.Entity;
using GameAI.Entity.Components;
using GameAI.Entity.Navigation;
using GameAI.Entity.Steering.Complex;
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

            return this.entities.Concat(this.entities.OfType<Vehicle>()).Where(IsNear);
        }

        public void SpawnGameEntity(BaseGameEntity entity, Vector2 position)
        {
            entity.Position = position;

            this.entities.Add(entity);
        }

        public void SpawnVehicle(Vehicle vehicle, Vector2 position)
        {
            SpawnGameEntity(vehicle, position);
            
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

        public void Render(SpriteBatch spriteBatch, GameTime gameTime)
        {
            this.entities.ForEach(o => o.Render(spriteBatch, gameTime));
        }
    }
}