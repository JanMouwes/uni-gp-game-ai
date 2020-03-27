namespace Graph
{
    public struct Edge<TValue>
    {
        public readonly Vertex<TValue> Dest;
        public readonly Vertex<TValue> Source;
        public readonly double Cost;

        public Edge(Vertex<TValue> source, Vertex<TValue> dest, double cost)
        {
            this.Source = source;
            this.Dest = dest;
            this.Cost = cost;
        }
    }
}