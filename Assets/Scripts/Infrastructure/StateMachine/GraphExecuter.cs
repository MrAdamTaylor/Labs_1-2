using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class GraphExecuter : IGraphExecuter
    {
        private IGraphDataHandler _graphDataHandler;

        public void Execute()
        {
            _graphDataHandler.Test();
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

    internal interface IGraphDataHandler
    {
        public GraphDataStructureInt GrapDataStructureInt { get; set; }
        void Test();
        void ConvertDataToStruct();
    }

    //TODO - переработал по принципу GRASP, теперь инициализация ветки вызывается здесь. По заветам Игоря!
    public class GraphDataStructureInt
    {
        public int GraphSize;
        public List<int>[] GraphConections;
        public int shiftValue;

        public GraphDataStructureInt(int dataSize)
        {
            GraphConections = new List<int>[dataSize];
        }

        public void Add(int graphIndex, int graphValue)
        {
            int newIndex = graphIndex - shiftValue;
            GraphConections[newIndex].Add(graphValue);
        }

        public void InitEdge(int index)
        {
            GraphConections[index] = new List<int>();
        }
    }

    class GraphDataHandler : IGraphDataHandler
    {
        private GraphData _data;

        public GraphDataStructureInt GrapDataStructureInt { get; set; }

        public void Test()
        {
            Debug.Log("Проверка работы метода!");
        }

        private IStructGraphDataIntReader _graphDataReader;
        private IStructGraphDataIntWriter _graphDataWriter;

        public GraphDataHandler(GraphData graphData)
        {
            _data = graphData;
            GrapDataStructureInt = new GraphDataStructureInt(graphData.Size);
            GrapDataStructureInt.GraphSize = graphData.Size;
            GrapDataStructureInt.shiftValue = graphData.ShiftCount;
            _graphDataReader = new StructGraphDataIntReader(GrapDataStructureInt);
            _graphDataWriter = new StructGraphDataIntWriter(GrapDataStructureInt);
        }

        public void ConvertDataToStruct()
        {
            bool flag = _data.Undir;
            for (int i = 0; i < _data.Pairs.Length; i++)
            {
                for (int j = 0; j < _data.Pairs[i].NeighborPoints.Length; j++)
                {
                    int graphPoint = _data.Pairs[i].GraphPoint;
                    int neighbor = _data.Pairs[i].NeighborPoints[j];
                    AddEdge(graphPoint, neighbor, flag);
                }
            }
            DeleteDuplicates();
            OutputFinalGraph();
        }

        public void DeleteDuplicates()
        {
            for (int i = 0; i < GrapDataStructureInt.GraphConections.Length; i++)
            {
                GrapDataStructureInt.GraphConections[i] = GrapDataStructureInt.GraphConections[i].Distinct().ToList();
            }
        }

        public void OutputFinalGraph()
        {
            for (int i = 0; i < GrapDataStructureInt.GraphConections.Length; i++)
            {
                for (int j = 0; j < GrapDataStructureInt.GraphConections[i].Count; j++)
                {
                    int graphIndex = i + _data.ShiftCount;
                    Debug.Log("Point "+ graphIndex +" with edge: " + GrapDataStructureInt.GraphConections[i][j]);
                }
            }
        }

        public void AddEdge(int i, int j, bool undir = true)
        {
            if (_data.Size != 0)
            {
                if (_graphDataReader.CheckPointByNull(i))
                {
                    if (_graphDataWriter.CheckClone(i, j))
                    {
                        Debug.Log("Нашёлся клон");
                    }
                    else
                    {
                        Debug.Log($"В ветку {i} добавлен сосед {j}");
                        _graphDataWriter.Add(i, j);
                    }
                }
                else
                {
                    _graphDataWriter.InitEdge(i);
                    Debug.Log($"После инициализации В ветку {i} добавлен сосед {j}");
                    _graphDataWriter.Add(i, j);
                }

                if (undir)
                {
                    if (_graphDataReader.CheckPointByNull(j))
                    {
                        Debug.Log($"В ветку {j} добавлен сосед {i}");
                        _graphDataWriter.Add(j, i);
                    }
                    else
                    {
                        _graphDataWriter.InitEdge(j);
                        Debug.Log($"В ветку {j} добавлен сосед {i}");
                        _graphDataWriter.Add(j, i);
                    }
                }
            }

        }
    }

    public interface IStructGraphDataIntWriter
    {
        void LoadData(GraphDataStructureInt structData);
        void Add(int graphValue1, int graphValue2);
        void InitEdge(int i);
        bool CheckClone(int index, int i1);
    }

    class StructGraphDataIntWriter : IStructGraphDataIntWriter
    {
        private GraphDataStructureInt _data;

        public StructGraphDataIntWriter(GraphDataStructureInt data)
        {
            _data = data;
        }

        //TODO - пусть останеться на случай, если кому - то надо будет реализовывать DI
        public void WriteGraphSize(int dataSize)
        {
            _data.GraphSize = dataSize;
            _data.GraphConections = new List<int>[dataSize];
            Debug.Log("Размер установлен: ");
        }

        public void LoadData(GraphDataStructureInt structData)
        {
            _data = structData;
        }

        public void Add(int graphValue1, int graphValue2)
        {
            _data.Add(graphValue1, graphValue2);
        }

        public void InitEdge(int index)
        {
            int newIndex = index - _data.shiftValue;
            _data.InitEdge(newIndex);
        }

        public bool CheckClone(int index, int value)
        {
            int newIndex = index - _data.shiftValue;
            for (int j = 0; j < _data.GraphConections[newIndex].Count; j++)
            {
                if (_data.GraphConections[newIndex][j] == value)
                    return true;
            }

            return false;
        }
    }

    public interface IStructGraphDataIntReader
    {
        void LoadData(GraphDataStructureInt data);
        bool CheckPointByNull(int index);
        List<int> GetNeighboursByIndex(int first);
    }

    class StructGraphDataIntReader : IStructGraphDataIntReader
    {
        private GraphDataStructureInt _data;

        public StructGraphDataIntReader(GraphDataStructureInt data)
        {
            _data = data;
        }

        public void LoadData(GraphDataStructureInt data)
        {
            _data = data;
        }

        public bool CheckPointByNull(int index)
        {
            int newIndex = index - _data.shiftValue;
            if (_data.GraphConections[newIndex] != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<int> GetNeighboursByIndex(int first)
        {
            return _data.GraphConections[first];
        }
    }
}