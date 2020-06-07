using System;
using System.Collections.Generic;
using System.Linq;
using GameAI.world;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace GameAI.Entity.Steering
{
    public static class SteeringBehaviours
    {
        /// <summary>
        /// Calculates force required to go to target's next expected position
        /// </summary>
        /// <param name="target">Target to go after</param>
        /// <param name="owner">Entity that does the chasing</param>
        /// <returns>Desired next local force</returns>
        public static Vector2 Pursue(MovingEntity target, MovingEntity owner) => Seek(target.Position + target.Velocity, owner);

        /// <summary>
        /// Calculates force to flee from a specified location if owner is within a specified distance 
        /// </summary>
        /// <param name="target">Location to flee from</param>
        /// <param name="owner">Entity that does the fleeing</param>
        /// <param name="panicDistance">Distance at which to panic</param>
        /// <returns>Desired next local force</returns>
        public static Vector2 FleeWhenPanicked(Vector2 target, MovingEntity owner, float panicDistance = 32f)
        {
            float distance = Vector2.DistanceSquared(target, owner.Position);

            bool shouldFlee = distance < panicDistance * panicDistance;

            return shouldFlee ? Flee(target, owner) : Vector2.Zero;
        }

        /// <summary>
        /// Calculates force to flee from a specified location
        /// </summary>
        /// <param name="target">Location to flee from</param>
        /// <param name="owner">Entity that does the fleeing</param>
        /// <returns>Desired next local force</returns>
        public static Vector2 Flee(Vector2 target, MovingEntity owner) => -Seek(target, owner);

        /// <summary>
        /// Calculates force to head for a certain position at the owner's max speed
        /// </summary>
        /// <param name="target">Position to head from</param>
        /// <param name="owner">Entity that does the seeking</param>
        /// <returns>Desired next local force</returns>
        public static Vector2 Seek(Vector2 target, MovingEntity owner)
        {
            if (target.Equals(owner.Position)) { return Vector2.Zero; }

            Vector2 desiredVelocity = (target - owner.Position).NormalizedCopy() * owner.MaxSpeed;

            return desiredVelocity - owner.Velocity;
        }

        /// <summary>
        /// Calculates force required to arrive at a specific position.
        /// Seeks if outside of deceleration distance, otherwise will slow down.
        /// </summary>
        /// <param name="target">Target to arrive at towards</param>
        /// <param name="owner">Entity that does the arriving</param>
        /// <param name="decelerateDistance">Distance at which to start decelerating</param>
        /// <returns>Desired new steering force</returns>
        public static Vector2 Arrive(Vector2 target, MovingEntity owner, float decelerateDistance)
        {
            //    Distance between target and location in vector
            Vector2 difference = target - owner.Position;

            // If not close enough to decelerate, don't decelerate 
            if (difference.LengthSquared() > decelerateDistance * decelerateDistance) { return Seek(target, owner); }

            if (difference.LengthSquared() <= 0) { return Vector2.Zero; }

            return difference - owner.Velocity;
        }

        /// <summary>
        /// Calculates force to head in a direction on a virtual line,
        /// perpendicular to the owner's orientation in front of the entity.
        /// </summary>
        /// <param name="entity">Entity to do the wandering</param>
        /// <param name="offset">Location on the line. Below zero means left, above zero means right</param>
        /// <param name="distance">Distance between the entity and the centre point of the line.</param>
        /// <returns>Desired new steering force</returns>
        public static Vector2 Wander(MovingEntity entity, float offset, float distance = 120)
        {
            // Perpendicular to the owner's orientation
            Vector2 localTarget = new Vector2
            {
                X = entity.Orientation.Y,
                Y = -entity.Orientation.X
            };
            localTarget *= offset;

            return entity.Orientation * distance + localTarget;
        }

        /// <summary>
        /// Calculates force required to avoid world's walls 
        /// </summary>
        /// <param name="entity">Contains necessary positional data</param>
        /// <param name="world">World whose walls to avoid</param>
        /// <param name="panicDistance">Distance from which to avoid walls</param>
        /// <returns>Zero if outside of panic distance, otherwise the force to avoid walls</returns>
        public static Vector2 WallAvoidance(MovingEntity entity, World world, float panicDistance)
        {
            (float distToLeft, float distToTop) = entity.Position + entity.Velocity;

            float distToBottom = world.Height - distToTop;
            float distToRight = world.Width - distToLeft;

            bool isNearLeft = panicDistance > distToLeft;
            bool isNearRight = panicDistance > distToRight;
            bool isNearTop = panicDistance > distToTop;
            bool isNearBottom = panicDistance > distToBottom;

            bool isNearWalls = isNearLeft || isNearRight || isNearTop || isNearBottom;

            if (!isNearWalls) { return Vector2.Zero; }

            float CalculateForce(float distance)
            {
                distance = distance > 0 ? distance : 1;
                float panicDistancePercentage = distance / panicDistance;
                float modifier = 1 / panicDistancePercentage;

                return modifier * modifier * panicDistance * panicDistance;
            }

            Vector2 baseSteering = entity.Velocity;

            if (isNearLeft) { baseSteering.X += CalculateForce(distToLeft); }
            else if (isNearRight) { baseSteering.X += -CalculateForce(distToRight); }

            if (isNearTop) { baseSteering.Y += CalculateForce(distToTop); }
            else if (isNearBottom) { baseSteering.Y += -CalculateForce(distToBottom); }

            return baseSteering;
        }

        /// <summary>
        /// Calculates the force required to follow another entity at a specified offset.
        /// </summary>
        /// <param name="leader">Leader to follow</param>
        /// <param name="owner">Entity that does the following</param>
        /// <param name="offset">Offset relative to the leader at which to follow the leader</param>
        /// <returns>Desired new steering force</returns>
        public static Vector2 LeaderFollowing(MovingEntity leader, MovingEntity owner, Vector2 offset)
        {
            Vector2 absoluteOffset = leader.Orientation * offset;

            return Seek(leader.Position + absoluteOffset, owner) * 10;
        }

        /// <summary>
        /// Calculates steering force required to align with neighbours
        /// </summary>
        /// <param name="neighbours">Neighbours with whom to align</param>
        /// <returns>Desired new steering force</returns>
        public static Vector2 Alignment(IEnumerable<MovingEntity> neighbours)
        {
            IEnumerable<Vector2> velocities = neighbours.Select(neighbour => neighbour.Velocity);

            Vector2 averageVelocity = AverageVector(velocities);

            if (!averageVelocity.Equals(Vector2.Zero)) { averageVelocity.Normalize(); }

            return averageVelocity;
        }

        /// <summary>
        /// Calculates steering force required to draw closer to neighbours
        /// </summary>
        /// <param name="owner">Entity that should draw closer to neighbours</param>
        /// <param name="neighbours">Neighbours to draw closer to</param>
        /// <returns>Desired new steering force</returns>
        public static Vector2 Cohesion(MovingEntity owner, IEnumerable<MovingEntity> neighbours)
        {
            IEnumerable<Vector2> positions = neighbours.Select(neighbour => neighbour.Position);

            Vector2 averagePosition = AverageVector(positions);

            if (!averagePosition.Equals(Vector2.Zero)) { averagePosition -= owner.Position; }

            return averagePosition;
        }

        /// <summary>
        /// Calculates steering force required to push away from neighbours
        /// </summary>
        /// <param name="owner">Entity that should push away from neighbours</param>
        /// <param name="neighbours">Neighbours to push away from</param>
        /// <returns>Desired new steering force</returns>
        public static Vector2 Separation(MovingEntity owner, IEnumerable<MovingEntity> neighbours) => -Cohesion(owner, neighbours);

        /// <summary>
        /// Calculates average vector out of Enumerable of vectors
        /// </summary>
        /// <param name="vectors">Vectors from which to calculate average</param>
        /// <returns>Average vector</returns>
        public static Vector2 AverageVector(IEnumerable<Vector2> vectors)
        {
            Vector2 averageVelocity = new Vector2();

            int size = 0;

            foreach (Vector2 vector in vectors)
            {
                averageVelocity.X += vector.X;
                averageVelocity.Y += vector.Y;

                size++;
            }

            if (size <= 0) return Vector2.Zero;

            averageVelocity.X /= size;
            averageVelocity.Y /= size;

            return averageVelocity;
        }
    }
}