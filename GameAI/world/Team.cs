using System.Collections;
using System.Collections.Generic;
using GameAI.entity;
using Microsoft.Xna.Framework;

namespace GameAI.world
{
    public class Team
    {
        public readonly LinkedList<Vehicle> Vehicles;

        public readonly Color Colour;


        public Team(Color colour) : this(colour, new Vehicle[0]) { }

        public Team(Color colour, IEnumerable<Vehicle> vehicles)
        {
            this.Vehicles = new LinkedList<Vehicle>(vehicles);
            this.Colour = colour;
        }
    }
}