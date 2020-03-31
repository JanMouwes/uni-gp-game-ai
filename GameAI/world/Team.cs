using System;
using System.Collections;
using System.Collections.Generic;
using GameAI.entity;
using Microsoft.Xna.Framework;

namespace GameAI.world
{
    public class Team
    {
        public readonly LinkedList<Vehicle> Vehicles;

        public IEnumerable<Vector2> SpawnPoints => this.spawnPoints;

        private readonly IList<Vector2> spawnPoints;

        public readonly Color Colour;

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