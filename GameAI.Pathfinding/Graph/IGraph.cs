namespace GameAI.Pathfinding.Graph
{
    public interface IGraph
    {
        Vertex GetVertex(int id);
        void AddVertex(Vertex vertex);
        void AddEdge(int source, int dest, double cost);
        void ClearAll();
    }
}