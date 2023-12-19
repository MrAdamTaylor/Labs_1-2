namespace Infrastructure.StateMachine
{
    public interface IStructGraphDataIntWriter
    {
        void LoadData(GraphDataStructureInt structData);
        void Add(int graphValue1, int graphValue2);
        void InitEdge(int i);
        bool CheckClone(int index, int i1);
    }
}