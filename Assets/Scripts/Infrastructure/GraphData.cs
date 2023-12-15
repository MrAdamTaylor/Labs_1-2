using System;

namespace Infrastructure
{
    class GraphData : LBData
    {
        public bool Undir;
        public int Size;
        public int[] GraphStructure;
        public GraphIntPairs[] Pairs;
        
        public GraphData(int count)
        {
            Pairs = new GraphIntPairs[count];
        }

        public override T GetData<T>() 
        {
            return (T)(Object)this;
        }

        public class GraphIntPairs
        {
            public int GraphPoint;
            public int[] NeighborPoints;
        }
    }

    
}