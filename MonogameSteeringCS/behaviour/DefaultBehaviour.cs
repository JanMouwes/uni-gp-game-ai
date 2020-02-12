using GameAI.entity;
using Microsoft.Xna.Framework;

namespace GameAI.behaviour
{
    /// <summary>
    /// Null-object
    /// </summary>
    public class DefaultBehaviour : SteeringBehaviour
    {
        private DefaultBehaviour(MovingEntity entity) : base(entity) { }
        public override Vector2 Calculate() => Vector2.Zero;

        /// <summary>
        /// Instance for singleton-pattern.
        /// </summary>
        private static DefaultBehaviour instance;

        public static DefaultBehaviour Instance => instance ?? (instance = new DefaultBehaviour(null));
    }
}