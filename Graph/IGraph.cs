namespace Graph
{
    public interface IGraph<TValue>
    {
        Vertex<TValue> GetVertex(int id);
        void AddVertex(Vertex<TValue> vertex);
        void AddEdge(int source, int dest, double cost);
        void ClearAll();
    }
}