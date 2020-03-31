using System;
using System.Collections.Generic;
using GameAI.entity;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.Steering
{
    public static class SteeringBehaviours
    {
        public static Vector2 Pursue(MovingEntity target, MovingEntity owner) => Seek(target.Position + target.Velocity, owner);

        public static Vector2 Flee(Vector2 target, MovingEntity owner)
        {
            const float panicDistance = 32f;
            float distance = Vector2.Distance(target, owner.Position);

            if (panicDistance > distance) { return Vector2.Zero; }

            Vector2 desiredVelocity = (owner.Position - target).NormalizedCopy() * owner.MaxSpeed;

            return desiredVelocity - owner.Velocity;
        }

        public static Vector2 Seek(Vector2 target, MovingEntity owner)
        {
            if (target == owner.Position) { return Vector2.Zero; }

            Vector2 desiredVelocity = (target - owner.Position).NormalizedCopy() * owner.MaxSpeed;

            return desiredVelocity - owner.Velocity;
        }

        public static Vector2 Arrive(Vector2 target, MovingEntity vehicle, float decelerateDistance)
        {
            //    Distance between target and location in vector
            Vector2 difference = target - vehicle.Position;

            // If not close enough to decelerate, don't decelerate 
            if (difference.LengthSquared() > decelerateDistance * decelerateDistance) { return Seek(target, vehicle); }

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

        public static Vector2 WallAvoidance(MovingEntity entity, World world, float panicDistance)
        {
            (float distToLeft, float distToTop) = entity.Position + entity.Velocity;

            float distToBottom = world.Height - distToTop;
            float distToRight = world.Width   - distToLeft;

            bool isNearLeft = panicDistance   > distToLeft;
            bool isNearRight = panicDistance  > distToRight;
            bool isNearTop = panicDistance    > distToTop;
            bool isNearBottom = panicDistance > distToBottom;

            if (!isNearLeft && !isNearRight && !isNearTop && !isNearBottom) { return Vector2.Zero; }

            Vector2 baseSteering = entity.Steering.Calculate();

            if (isNearLeft)
            {
                baseSteering.X = (panicDistance * 2) - distToLeft; 
            }
            else if (isNearRight)
            {
                baseSteering.X = -(panicDistance * 2) + distToRight; 
            }

            if (isNearTop)
            {
                baseSteering.Y = (panicDistance * 2) - distToTop;
            }
            else if (isNearBottom)
            {
                baseSteering.Y = -(panicDistance * 2) + distToBottom; 
            }

            return baseSteering;
        }

        public static Vector2 LeaderFollowing(MovingEntity target, MovingEntity owner, Vector2 offset)
        {
            //    Distance between target and location in vector
            Vector2 difference = target.Position - owner.Position - offset;
            float distance = difference.Length();

            if (distance <= 0) return target.Velocity;

            float desiredSpeed = distance;
            Vector2 desiredVelocity = difference * desiredSpeed / distance;

            return desiredVelocity - owner.Velocity;
        }

        public static Vector2 Separation(MovingEntity owner, IEnumerable<MovingEntity> neighbors)
        {
            Vector2 steeringForce = Vector2.Zero;

            foreach (MovingEntity neighbor in neighbors)
            {
                Vector2 toAgent = owner.Position - neighbor.Position;
                steeringForce += toAgent;
            }

            return steeringForce;
        }

        public static Vector2 Alignment(MovingEntity owner, IEnumerable<MovingEntity> neighbors)
        {
            Vector2 averageHeading = Vector2.Zero;
            int neighborCount = 0;

            foreach (MovingEntity neighbor in neighbors)
            {
                if (neighbor != owner)
                {
                    averageHeading += neighbor.Orientation;

                    ++neighborCount;
                }
            }

            if (neighborCount > 0)
            {
                averageHeading.Normalize();
                //averageHeading -= owner.Orientation;
            }

            return averageHeading;
        }

        public static Vector2 Cohesion(MovingEntity owner, IEnumerable<MovingEntity> neighbors)
        {
            Vector2 centerOfMass = Vector2.Zero,
                    steeringForce = Vector2.Zero;
            int neighborCount = 0;

            foreach (MovingEntity neighbor in neighbors)
            {
                if (neighbor != owner)
                {
                    centerOfMass += neighbor.Position;

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