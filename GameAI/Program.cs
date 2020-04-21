using System;

namespace GameAI
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Game1 game = new Game1())
                game.Run();

            // const int loops = 1000;
            // Graph<Vector2> graph = GraphGenerator.GenerateGraph((250, 250), (25, 25));
            // Random random = new Random();
            //
            // for (int i = 0; i < loops; i++)
            // {
            //     int fromId = random.Next(0, 625);
            //     int toId = random.Next(0, 625);
            //
            //     Vertex<Vector2> from = graph.GetVertex(fromId);
            //     Vertex<Vector2> to = graph.GetVertex(toId);
            //
            //     AStarRunner<Vector2> algoRunner = new AStarRunner<Vector2>(graph);
            //
            //     algoRunner.Run(from, to, Heuristics.Manhattan);
            //
            //     Console.WriteLine("ran loop " + i);
            // }
        }
    }
}