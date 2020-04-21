using Microsoft.Xna.Framework.Graphics;

namespace GameAI.Entity.GoalBehaviour.Rendering
{
    public class DefaultRenderer : IGoalRenderer
    {
        public void Render(SpriteBatch spriteBatch)
        {
            // Do nothing
        }

        private DefaultRenderer() { }

        public static DefaultRenderer Instance { get; } = new DefaultRenderer();
    }
}