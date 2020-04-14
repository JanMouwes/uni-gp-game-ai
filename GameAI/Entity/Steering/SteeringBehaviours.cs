using System;
using System.Collections.Generic;
using GameAI.Entity;
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


        public static Vector2 Wander(MovingEntity entity, float offset, float range, float distance = 80)
        {
            Random random = new Random();
            offset += random.Next(-100, 100) / 100f; // .Next() is exclusive
            offset = Math.Min(offset, range);
            offset = Math.Max(offset, -range);

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

            bool isNearWalls = isNearLeft || isNearRight || isNearTop || isNearBottom;

            if (!isNearWalls) { return Vector2.Zero; }

            Vector2 baseSteering = entity.Velocity;
            
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

        public static Vector2 Alignment(MovingEntity owner, IEnumerable<MovingEntity> neighbors)
        {
            Vector2 averageHeading = new Vector2();
            int size = 0;

            foreach (MovingEntity neighbor in neighbors)
            {
                if (neighbor != owner)
                {
                    averageHeading.X += neighbor.Velocity.X;
                    averageHeading.Y += neighbor.Velocity.Y;

                    size++;
                }
            }

            if (size > 0)
            {
                averageHeading.X /= size;
                averageHeading.Y /= size;
                if (averageHeading.X != 0 && averageHeading.Y != 0) averageHeading.Normalize();
            }

            return averageHeading;
        }

        public static Vector2 Cohesion(MovingEntity owner, IEnumerable<MovingEntity> neighbors)
        {
            Vector2 centerOfMass = new Vector2();
            int size = 0;

            foreach (MovingEntity neighbor in neighbors)
            {
                if (neighbor != owner)
                {
                    centerOfMass.X += neighbor.Position.X;
                    centerOfMass.Y += neighbor.Position.Y;

                    size++;
                }
            }

            if (size > 0)
            {
                centerOfMass.X /= size;
                centerOfMass.Y /= size;
                centerOfMass = new Vector2(centerOfMass.X - owner.Position.X, centerOfMass.Y - owner.Position.Y);
                if (centerOfMass.X != 0 && centerOfMass.Y != 0) centerOfMass.Normalize();
            }

            return centerOfMass;
        }

        public static Vector2 Separation(MovingEntity owner, IEnumerable<MovingEntity> neighbors)
        {
            Vector2 steeringForce = new Vector2();
            int size = 0;

            foreach (MovingEntity neighbor in neighbors)
            {
                if (neighbor != owner)
                {
                    steeringForce.X += neighbor.Position.X - owner.Position.X;
                    steeringForce.Y += neighbor.Position.Y - owner.Position.Y;
                    size++;
                }
            }

            if (size > 0)
            {
                steeringForce.X /= size;
                steeringForce.Y /= size;
                // * negative so the owner will steer away from the target
                steeringForce.X *= -1;
                steeringForce.Y *= -1;
                if (steeringForce.X != 0 && steeringForce.Y != 0) steeringForce.Normalize();
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