using System;
using System.Collections.Generic;
using GameAI.entity;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.behaviour
{
    public static class SteeringBehaviours
    {
        public static Vector2 Pursue(MovingEntity target, MovingEntity owner) => Seek(target.Pos + target.Velocity, owner);

        public static Vector2 Flee(Vector2 target, MovingEntity owner)
        {
            const float panicDistance = 32f;
            float distance = Vector2.Distance(target, owner.Pos);

            if (panicDistance > distance) { return Vector2.Zero; }

            Vector2 desiredVelocity = (owner.Pos - target).NormalizedCopy() * owner.MaxSpeed;

            return desiredVelocity - owner.Velocity;
        }

        public static Vector2 Seek(Vector2 target, MovingEntity owner)
        {
            if (target == owner.Pos) { return Vector2.Zero; }

            Vector2 desiredVelocity = (target - owner.Pos).NormalizedCopy() * owner.MaxSpeed;

            return desiredVelocity - owner.Velocity;
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

        

        public static Vector2 Wander(MovingEntity entity, float distance, float offset)
        {
            Vector2 localTarget = new Vector2
            {
                X = entity.Orientation.Y,
                Y = -entity.Orientation.X
            };
            localTarget *= offset;

            return entity.Orientation * distance + localTarget;
        }

        public static Vector2 LeaderFollowing(MovingEntity target, MovingEntity owner, Vector2 offset)
        {
            //    Distance between target and location in vector
            Vector2 difference = target.Pos - owner.Pos - offset;
            float distance = difference.Length();

            if (distance <= 0) return target.Velocity;

            float desiredSpeed = distance;
            Vector2 desiredVelocity = difference * desiredSpeed / distance;

            return desiredVelocity - owner.Velocity;
        }

        public static Vector2 Separation(MovingEntity owner, IEnumerable<MovingEntity> neighbors)
        {
            Vector2 steeringForce = Vector2.Zero;

            foreach (var neighbor in neighbors)
            {
                if (neighbor != owner)
                {
                    Vector2 toAgent = owner.Pos - neighbor.Pos;

                    steeringForce += Vector2.Normalize(toAgent) / toAgent.Length();
                }
            }

            return steeringForce;
        }

        public static Vector2 Alignment(MovingEntity owner, IEnumerable<MovingEntity> neighbors)
        {
            Vector2 averageHeading = Vector2.Zero;
            int neighborCount = 0;

            foreach (var neighbor in neighbors)
            {
                if (neighbor != owner)
                {
                    averageHeading += neighbor.Orientation;

                    ++neighborCount;
                }
            }

            if (neighborCount > 0)
            {
                averageHeading /= neighborCount;
                averageHeading -= owner.Orientation;
            }

            return averageHeading;
        }

        public static Vector2 Cohesion(MovingEntity owner, IEnumerable<MovingEntity> neighbors)
        {
            Vector2 centerOfMass = Vector2.Zero, steeringForce = Vector2.Zero;
            int neighborCount = 0;

            foreach (var neighbor in neighbors)
            {
                if (neighbor != owner)
                {
                    centerOfMass += neighbor.Pos;

                    ++neighborCount;
                }
            }

            if (neighborCount > 0)
            {
                centerOfMass /= neighborCount;
                steeringForce = Seek(centerOfMass, owner);
            }

            return steeringForce;
        }
    }

    public enum DecelerationSpeed
    {
        Fast = 1,
        Medium = 2,
        Slow = 3
    }
}