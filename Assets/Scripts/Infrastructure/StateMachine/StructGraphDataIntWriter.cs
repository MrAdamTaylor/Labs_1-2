using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.StateMachine
{
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
}