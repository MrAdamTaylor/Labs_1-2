using System;
using System.Collections.Generic;
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
        }
    }

    public interface ILabExecuter : IService
    {
        public void Execute();
    }

    interface IMgyaExecuter : ILabExecuter
    {
        
    }

    interface IGraphExecuter : ILabExecuter
    {
        
    }


    public class MgyaExecuter : IMgyaExecuter
    {
        public void Execute()
        {
            throw new NotImplementedException();
        }
    }

    public class GraphExecuter : IGraphExecuter
    {
        public void Execute()
        {
            throw new NotImplementedException();
        }
    }

    interface ILabHandlersConteiner : IService
    {
        public ILabExecuter DataCheck<T>(T getData) where T : LBData;
    }
    
    public class LabHandlersConteiner : ILabHandlersConteiner
    {
        public ILabExecuter DataCheck<T>(T getData) where T : LBData
        {
            var handler = LBDataDictionaryAdapter<T>.Conteiner.GetHandlerByData(getData);
            return handler;
            //throw new Exception("Класс для подбора реализаций не реализован");
        }
        
    }

    public class LBDataDictionaryAdapter<T> where T : LBData
    {
        private static LBDataDictionaryAdapter<T> _conteiner;
        public static LBDataDictionaryAdapter<T> Conteiner
        {
            get
            {
                if (_conteiner == null)
                {
                    _conteiner = new LBDataDictionaryAdapter<T>();
                }
                return _conteiner;
            }
        }

        private List<ILabExecuter> _labsInterfaces = new List<ILabExecuter>();

        public ILabExecuter GetHandlerByData(T data) 
        {
            var index = OrderType(data);
            
            switch (index)
            {
                case 0:  
                    return LBDataDictionary.Conteiner.GetHandlerByData(data);
                case 1: 
                    return LBDataDictionary.Conteiner.GetHandlerByData(data);;
                default:
                    throw new Exception("Добавть новый индекс для данных, " +
                                        "так как интеграция словаря с OCP принципом реализована не достаточно хорошо!");
            }
        }

        private int OrderType(T data)
        {
            if (data.GetType() == typeof(MgyaData))
            {
                return 0;
            }
            else if(data.GetType() == typeof(GraphData))
            {
                return 1;
            }
            else
            {
                throw new Exception("Добавьте ещё один индекс");
            }
        }
    }
}