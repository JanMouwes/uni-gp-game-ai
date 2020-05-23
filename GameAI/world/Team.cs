using System;
using System.Collections.Generic;
using GameAI.Entity;
using Microsoft.Xna.Framework;

namespace GameAI.world
{
    public class Team
    {
        private readonly IList<Vector2> spawnPoints;

        public readonly LinkedList<Vehicle> Vehicles;
        public readonly Color Colour;

        public IEnumerable<Vector2> SpawnPoints => this.spawnPoints;

        public Flag Flag { get; set; }

        public Vector2 Base { get; set; }
        public int Points { get; set; }

        public Team(Color colour) : this(colour, new Vehicle[0]) { }

        public Team(Color colour, IEnumerable<Vehicle> vehicles)
        {
            this.spawnPoints = new List<Vector2>();
            this.Vehicles = new LinkedList<Vehicle>(vehicles);
            this.Colour = colour;
        }

        public void AddSpawnPoint(Vector2 spawnPoint)
        {
            this.spawnPoints.Add(spawnPoint);
        }

        public void RespawnFlag()
        {
            this.Flag.Position = this.Base;
            this.Flag.Carrier = null;
        }

        public void AddSpawnPoints(IEnumerable<Vector2> points)
        {
            foreach (Vector2 spawnPoint in points) { this.spawnPoints.Add(spawnPoint); }
        }

        public Vector2 RandomSpawnPoint()
        {
            if (this.spawnPoints.Count == 0) { throw new IndexOutOfRangeException(); }

            Random random = new Random();

            return this.spawnPoints[random.Next(0, this.spawnPoints.Count)];
        }
    }
}