using System.Linq;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    class GraphDataHandler : IGraphDataHandler
    {
        private GraphData _data;

        public GraphDataStructureInt GrapDataStructureInt { get; set; }

        private IStructGraphDataIntReader _graphDataReader;
        private IStructGraphDataIntWriter _graphDataWriter;
        private IStructTopologyHandler _structTopologyHandler;

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
            _structTopologyHandler = new StructTopologyHandler(GrapDataStructureInt, _data);
        }

        public void Execute()
        {
            _structTopologyHandler.CalculateAllMetrix();
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
}