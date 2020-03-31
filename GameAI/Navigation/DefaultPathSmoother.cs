using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameAI.Navigation
{
    public class DefaultPathSmoother : IPathSmoother
    {
        /// <summary>
        /// Does not smooth paths
        /// </summary>
        /// <inheritdoc cref="IPathSmoother#SmoothPath"/>
        public IEnumerable<Vector2> SmoothPath(LinkedList<Vector2> path) => path;
    }
}