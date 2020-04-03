using System.Collections.Generic;
using GameAI.entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace GameAI.Util
{
    public class DebugRendering
    {
        public static void DrawObstacleAvoidance(SpriteBatch spriteBatch, Vehicle vehicle, Rock rock)
        {
            // The detection box is the current velocity divided by the max velocity of the entity
            // range is the maximum size of the box
            Vector2 viewBox = vehicle.Velocity / vehicle.MaxSpeed * 100;
            // Add the box in front of the entity
            IEnumerable<Vector2> checkpoints = new[]
            {
                vehicle.Pos,
                vehicle.Pos + viewBox / 2f, // Halfway
                vehicle.Pos + viewBox,      // At the end
                vehicle.Pos + viewBox * 2   // Square
            };

            foreach (Vector2 checkpoint in checkpoints) { spriteBatch.DrawPoint(checkpoint, Color.Black, 5f); }

            CircleF notAllowedZone = new CircleF(rock.Pos, rock.Scale + vehicle.Scale * 2);
            spriteBatch.DrawCircle(notAllowedZone, 360, Color.Orange);

            Vector2 dist = new Vector2(rock.Pos.X - vehicle.Pos.X, rock.Pos.X - vehicle.Pos.Y);
            Vector2 perpendicular = new Vector2(-dist.Y, dist.X);

            Vector2 realDist = vehicle.Pos  + dist;
            Vector2 realHaaks = rock.Pos    + perpendicular;
            Vector2 realMinHaaks = rock.Pos - perpendicular;

            spriteBatch.DrawLine(vehicle.Pos, realDist, Color.Purple);
            spriteBatch.DrawLine(rock.Pos, realHaaks, Color.Aqua);
            spriteBatch.DrawLine(rock.Pos, realMinHaaks, Color.Green);

            Vector2 vehicleVelocityPos = vehicle.Pos + vehicle.Velocity;

            float haaksDistPlus = Vector2.DistanceSquared(realHaaks, vehicleVelocityPos);
            float haaksDistMin = Vector2.DistanceSquared(realMinHaaks, vehicleVelocityPos);

            Vector2 target = haaksDistPlus > haaksDistMin ? realMinHaaks : realHaaks;

            spriteBatch.DrawLine(vehicle.Pos, target, Color.DarkRed);
            spriteBatch.DrawLine(vehicleVelocityPos, target, Color.Chocolate);
        }
    }
}