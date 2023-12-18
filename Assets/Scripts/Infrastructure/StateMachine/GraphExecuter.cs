using System;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GraphExecuter : IGraphExecuter
    {
        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void LoadData<T>(T getData) where T : LBData
        {
            if (getData.GetType() == typeof(GraphData))
            {
                Debug.Log("Обработка данных графов возможна!");
            }
        }
    }
}