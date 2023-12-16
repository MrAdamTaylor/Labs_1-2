using System;
using Extensions;

namespace Infrastructure
{
    class MgyaData : LBData
    {
        public int[,] Value;

        public override T GetData<T>()
        {
            return (T)(Object)this;
        }

        public int[,] GetMatrix()
        {
            return Value;
        }

        public int[][] GetJaggedMatrix()
        {
            var array = Value.ToJaggedArray();
            return array;
        }
    }
}