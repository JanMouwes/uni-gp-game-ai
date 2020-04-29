using Microsoft.Xna.Framework;

namespace GameAI.Entity.Steering.Simple
{
    /// <summary>
    /// Null-object
    /// </summary>
    public class DefaultSteeringBehaviour : SteeringBehaviour
    {
        private DefaultSteeringBehaviour(MovingEntity entity) : base(entity) { }
        public override Vector2 Calculate() => Vector2.Zero;

        /// <summary>
        /// Instance for singleton-pattern.
        /// </summary>
        private static DefaultSteeringBehaviour instance;

        public static DefaultSteeringBehaviour Instance => instance ?? (instance = new DefaultSteeringBehaviour(null));
    }
}