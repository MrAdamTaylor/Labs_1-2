using Infrastructure.Services;
using UnityEngine;
using Object = System.Object;

namespace Infrastructure.StateMachine
{
    internal class HandlerDataState : IState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly ILabsHandler _labsHandler;

        public HandlerDataState(GameStateMachine gameStateMachine)
        {
            _stateMachine = gameStateMachine;
            _labsHandler = ServiceAllocator.Container.Single<ILabsHandler>();
            Debug.Log("Переход осуществился!");
            for (int i = 0; i < DataConteiner.Conteiner.LBCount; i++)
            {
                Debug.Log("Здесь будет исполняться лабораторная "+(i+1));
                //var itType = DataConteiner.Conteiner.Data[i].GetData<LBData>();
                //Debug.Log(type.GetType());
                _labsHandler.Execute(DataConteiner.Conteiner.Data[i].GetData<LBData>());
            }
        }

        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }

    public interface ILabsHandler : IService
    {
        void Execute<T>(T getData) where T : LBData;
    }

    class LabsIterator : ILabsHandler
    {
        private ILabHandlersConteiner _labServiceConteiner;
        private ILabExecuter _concreateExecuter;

        public LabsIterator(ILabHandlersConteiner conteiner)
        {
            _labServiceConteiner = conteiner;
        }

        public void Execute<T>(T getData) where T : LBData
        {
            Debug.Log("Исполнитель реализованый с точки зрения OCP");
            _concreateExecuter = _labServiceConteiner.DataCheck(getData);
            Debug.LogWarning(_concreateExecuter.GetType() + " Тип исполнителя!");
            _concreateExecuter.LoadData(getData);
            _concreateExecuter.Execute();
        }
    }
}