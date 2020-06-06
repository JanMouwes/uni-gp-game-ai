using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameAI.Entity.Navigation
{
    public class DefaultPathSmoother : IPathSmoother
    {
        /// <summary>
        /// Does not smooth paths
        /// </summary>
        /// <inheritdoc cref="IPathSmoother#SmoothPath"/>
        public IEnumerable<Vector2> SmoothPath(IEnumerable<Vector2> path) => path;
    }
}