using System.Collections.Generic;
using GameAI.Entity;
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
                vehicle.Position,
                vehicle.Position + viewBox / 2f, // Halfway
                vehicle.Position + viewBox,      // At the end
                vehicle.Position + viewBox * 2   // Square
            };

            foreach (Vector2 checkpoint in checkpoints) { spriteBatch.DrawPoint(checkpoint, Color.Black, 5f); }

            CircleF notAllowedZone = new CircleF(rock.Position, rock.Scale + vehicle.Scale * 2);
            spriteBatch.DrawCircle(notAllowedZone, 360, Color.Orange);

            Vector2 dist = new Vector2(rock.Position.X - vehicle.Position.X, rock.Position.X - vehicle.Position.Y);
            Vector2 perpendicular = new Vector2(-dist.Y, dist.X);

            Vector2 realDist = vehicle.Position  + dist;
            Vector2 realHaaks = rock.Position    + perpendicular;
            Vector2 realMinHaaks = rock.Position - perpendicular;

            spriteBatch.DrawLine(vehicle.Position, realDist, Color.Purple);
            spriteBatch.DrawLine(rock.Position, realHaaks, Color.Aqua);
            spriteBatch.DrawLine(rock.Position, realMinHaaks, Color.Green);

            Vector2 vehicleVelocityPos = vehicle.Position + vehicle.Velocity;

            float haaksDistPlus = Vector2.DistanceSquared(realHaaks, vehicleVelocityPos);
            float haaksDistMin = Vector2.DistanceSquared(realMinHaaks, vehicleVelocityPos);

            Vector2 target = haaksDistPlus > haaksDistMin ? realMinHaaks : realHaaks;

            spriteBatch.DrawLine(vehicle.Position, target, Color.DarkRed);
            spriteBatch.DrawLine(vehicleVelocityPos, target, Color.Chocolate);
        }
    }
}