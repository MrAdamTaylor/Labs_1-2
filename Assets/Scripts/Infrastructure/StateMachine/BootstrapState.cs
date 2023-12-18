using System;
using Infrastructure.Bootstrap;
using Infrastructure.Bootstrap.Tasks;
using Infrastructure.Services;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Infrastructure.StateMachine
{
    internal class BootstrapState : IState
    {
        private readonly ServiceAllocator _serviceAllocator;
        private readonly GameStateMachine _stateMachine;
        private readonly ITaskLoader _taskLoader;

        public BootstrapState(GameStateMachine gameStateMachine, ServiceAllocator serviceAllocator, BootstrapConfigs tasks)
        {
            _stateMachine = gameStateMachine;
            _serviceAllocator = serviceAllocator;
            _serviceAllocator.RegisterSingle<ITaskLoader>(new TaskLoader());
            _taskLoader = _serviceAllocator.Single<ITaskLoader>();
            
            if (tasks != null)
            {
                Debug.Log("Всё успешно загрузилось!");
                LoadTask(tasks);
            }

            ReadLabsData();
            FullDictionary<Task>(tasks.Tasks);
            _serviceAllocator.RegisterSingle<ILabHandlersConteiner>(new LabHandlersConteiner());
            _serviceAllocator.RegisterSingle<ILabsHandler>(new LabsIterator(_serviceAllocator.Single<ILabHandlersConteiner>()));
        }

        private void ReadLabsData()
        {
            for (int i = 0; i < DataConteiner.Conteiner.Data.Count; i++)
            {
                Debug.Log("Записанный тип - "+DataConteiner.Conteiner.Data[i].GetType());
            }

            //int[,] array = DataConteiner.Conteiner.Data[0].GetData<int[,]>();
        }

        private void LoadTask(BootstrapConfigs tasks)
        {
            DataConteiner.Conteiner.LBCount = tasks.Tasks.Length;
            for (int i = 0; i < tasks.Tasks.Length; i++)
            {
                tasks.Tasks[i].Run();
                Debug.Log("Тип класса "+ i +" будет " + tasks.Tasks[i].GetType());
                _taskLoader.Load(tasks.Tasks[i]);
            }
        }

        private void FullDictionary<T>(T[] Tasks) where T : Task
        {
            for (int j = 0; j < Tasks.Length; j++)
            {
                LBDataDictionary.Conteiner.AddDataType(
                    Tasks[j].GetData(), ExecuterMaster(DataConteiner.Conteiner.Data[j])
                    );
            }
        }

        private ILabExecuter ExecuterMaster(LBData dataTask)
        {
            if (dataTask.GetType() == typeof(MgyaData))
            {
                Debug.LogWarning("В словарь будет добавлена МГУА реализация");
                _serviceAllocator.RegisterSingle<IMgyaExecuter>(new MgyaExecuter());
                return _serviceAllocator.Single<IMgyaExecuter>();
            }
            else if(dataTask.GetType() == typeof(GraphData))
            {
                Debug.LogWarning("В словарь будет добавлена реализация с графами");
                _serviceAllocator.RegisterSingle<IGraphExecuter>(new GraphExecuter());
                return _serviceAllocator.Single<IGraphExecuter>();
            }
            else
            {
                throw new Exception("Необходимо будет добавить ещё один тип " +
                                    "для словаря (Данные - Реализация)");
            }
        }

        public void Enter()
        {
            _stateMachine.Enter<HandlerDataState>();
        }
        

        public void Exit()
        {
            
        }
    }

    public class TaskLoader : ITaskLoader
    {
        public void Load<T>(T data) where T : Task
        {
            var myData = data.GetData();
            DataConteiner.Conteiner.AddData(myData);
        }
    }

    public interface ITaskLoader : IService
    {
        public void Load<T>(T data) where T : Task;
    }
}