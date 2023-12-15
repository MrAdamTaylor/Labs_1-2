using System;

namespace Infrastructure
{
    class MgyaData : LBData
    {
        public int[,] Value ;

        public override T GetData<T>()
        {
            return (T)(Object)this;
        }
    }
}