using System;

namespace Infrastructure
{
    public class LBData
    {
        public virtual T GetData<T>()
        {
            throw new Exception("Необходимо добавить переопределение(ovveride) для нового типа данных");
        }
    }
}