using GameAI.entity;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.behaviour
{
    public static class SteeringBehaviours
    {
        public static Vector2 Pursue(MovingEntity target, MovingEntity movement) => Seek(target.Pos + target.Velocity, movement);

        public static Vector2 Flee(Vector2 target, MovingEntity vehicle)
        {
            const float panicDistance =  32f;
            float distance = Vector2.Distance(target, vehicle.Pos);

            if (panicDistance > distance) { return Vector2.Zero; }

            Vector2 desiredVelocity = (vehicle.Pos - target).NormalizedCopy() * vehicle.MaxSpeed;

            return desiredVelocity - vehicle.Velocity;
        }

        public static Vector2 Seek(Vector2 target, MovingEntity vehicle)
        {
            if (target == vehicle.Pos) { return Vector2.Zero; }

            Vector2 desiredVelocity = (target - vehicle.Pos).NormalizedCopy() * vehicle.MaxSpeed;

            return desiredVelocity - vehicle.Velocity;
        }

        public static Vector2 Arrive(Vector2 target, MovingEntity vehicle)
        {
            //    Distance between target and location in vector
            Vector2 difference = target - vehicle.Pos;

            float distance = difference.Length();

            if (distance <= 0) { return Vector2.Zero; }

            float desiredSpeed = distance;

            Vector2 desiredVelocity = difference * desiredSpeed / distance;

            return desiredVelocity - vehicle.Velocity;
        }
    }

    public enum DecelerationSpeed
    {
        Fast = 1,
        Medium = 2,
        Slow = 3
    }
}