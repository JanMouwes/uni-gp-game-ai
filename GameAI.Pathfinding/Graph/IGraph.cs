namespace GameAI.Pathfinding.Graph
{
    public interface IGraph
    {
        Vertex GetVertex(string name);
        void AddEdge(string source, string dest, double cost);
        void ClearAll();
    }
}