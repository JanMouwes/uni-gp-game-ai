namespace GameAI.Pathfinding.Graph
{
    public class Edge
    {
        public Vertex dest;
        public double cost;

        public Edge(Vertex d, double c)
        {
            this.dest = d;
            this.cost = c;
        }
    }
}