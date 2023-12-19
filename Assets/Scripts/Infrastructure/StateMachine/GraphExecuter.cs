using Unity.VisualScripting;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GraphExecuter : IGraphExecuter
    {
        private IGraphDataHandler _graphDataHandler;

        public void Execute()
        {
            _graphDataHandler.Execute();
        }

        public void LoadData<T>(T getData) where T : LBData
        {
            if (getData.GetType() == typeof(GraphData))
            {
                GraphData graphData = getData as GraphData;
                _graphDataHandler = new GraphDataHandler(graphData);
                Debug.Log("Обработка данных графов возможна!");
                _graphDataHandler.ConvertDataToStruct();
            }
        }
    }

    //TODO - переработал по принципу GRASP, теперь инициализация ветки вызывается здесь. По заветам Игоря!
}