using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace GameAI.Navigation
{
    public interface IPathSmoother
    {
        /// <summary>
        /// Smooths path, removes unnecessary vectors
        /// </summary>
        /// <param name="path">Path to smooth</param>
        /// <returns>Smoothed path</returns>
        IEnumerable<Vector2> SmoothPath(LinkedList<Vector2> path);
    }
}