using System;
using Extensions;
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

    public interface ILabExecuter : IService
    {
        public void Execute();
        void LoadData<T>(T getData) where T : LBData;
    }

    interface IMgyaExecuter : ILabExecuter
    {
        
    }

    interface IGraphExecuter : ILabExecuter
    {
        
    }


    public class MgyaExecuter : IMgyaExecuter
    {
        private IMgyaDataHandler _mgyaDataHandler;
        
        public void Execute()
        {
            AverageRanksMethod();
            MedianRanksMethod();
        }

        public void LoadData<T>(T getData) where T : LBData
        {
            if (getData.GetType() == typeof(MgyaData))
            {
                Debug.Log("Обработка данных МГУА возможна!");
                MgyaData data = getData as MgyaData;
                _mgyaDataHandler = new MgyaDataHandler(data);
            }
        }

        private void AverageRanksMethod()
        {
            Debug.Log("Метод средних арифметический рангов: ");
            _mgyaDataHandler.GradCount.PrintWithMark("a");
            _mgyaDataHandler.Data.PrintJaggedArray("Э");
            _mgyaDataHandler.SumValues.PrintWithTitle("Сум");
            _mgyaDataHandler.AverageValuer.PrintWithTitle("Сум");
            _mgyaDataHandler.AverageValuer.PrintRangedLine("Сум");
        }

        private void MedianRanksMethod()
        {
            Debug.Log("Метод медианных рангов: ");
            float[] medians = null;

            var medianMatrix = MgyaMethods.TransformMatrix(_mgyaDataHandler.Data);
            var projectCount = medianMatrix[0].Length;
            
            projectCount.PrintWithMark("a");
            medianMatrix.PrintJaggedArray();
            Debug.Log("Результат алгоритма: ");
            medianMatrix = medianMatrix.TransposeMatrix();
            Debug.Log("Транспланированная матрица");
            medianMatrix.PrintJaggedArray();
            for(var i = 0; i < medianMatrix.Length; i++)
                Array.Sort(medianMatrix[i]);

            medians = MgyaMethods.GetMedians(medianMatrix);
            Debug.Log("Результат алгоритма");
            medianMatrix = medianMatrix.TransposeMatrix();
            projectCount.PrintWithMark("a");
            medianMatrix.PrintJaggedArray();
            medians.PrintWithTitle("Медианны");
            medians.PrintRangedLine("a");

        }

        private void MedianKaemenyAvailableMethod()
        {
        }

        private void MedianKemenyAllRanksMethod()
        {
        }
    }

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