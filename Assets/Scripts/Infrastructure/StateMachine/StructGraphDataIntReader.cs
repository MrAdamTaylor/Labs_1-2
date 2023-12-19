using System.Collections.Generic;

namespace Infrastructure.StateMachine
{
    class StructGraphDataIntReader : IStructGraphDataIntReader
    {
        private GraphDataStructureInt _data;

        public StructGraphDataIntReader(GraphDataStructureInt data)
        {
            _data = data;
        }

        public void LoadData(GraphDataStructureInt data)
        {
            _data = data;
        }

        public bool CheckPointByNull(int index)
        {
            int newIndex = index - _data.shiftValue;
            if (_data.GraphConections[newIndex] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<int> GetNeighboursByIndex(int first)
        {
            int newIndex = first - _data.shiftValue;
            return _data.GraphConections[newIndex];
        }

        public int GetShift()
        {
            return _data.shiftValue;
        }
    }
}