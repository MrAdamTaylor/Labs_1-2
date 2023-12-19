using System.Collections.Generic;

namespace Infrastructure.StateMachine
{
    public interface IStructGraphDataIntReader
    {
        void LoadData(GraphDataStructureInt data);
        bool CheckPointByNull(int index);
        List<int> GetNeighboursByIndex(int first);
        int GetShift();
    }
}