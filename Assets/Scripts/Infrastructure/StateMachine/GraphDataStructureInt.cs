using System.Collections.Generic;

namespace Infrastructure.StateMachine
{
    public class GraphDataStructureInt
    {
        public int GraphSize;
        public List<int>[] GraphConections;
        public int shiftValue;

        public GraphDataStructureInt(int dataSize)
        {
            GraphConections = new List<int>[dataSize];
        }

        public void Add(int graphIndex, int graphValue)
        {
            int newIndex = graphIndex - shiftValue;
            GraphConections[newIndex].Add(graphValue);
        }

        public void InitEdge(int index)
        {
            GraphConections[index] = new List<int>();
        }
    }
}