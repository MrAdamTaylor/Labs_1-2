using System;
using System.Collections.Generic;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    class StructTopologyHandler : IStructTopologyHandler
    {
        private GraphDataStructureInt _structData;
        private GraphData _data;
        private CheckData _checkData;
        private int[,] _adjacencyMatrix;
        private int[,] _distanceMatrix;
        private IBfs _bfs;
        
        public StructTopologyHandler(GraphDataStructureInt structStructData, GraphData graphData)
        {
            _structData = structStructData;
            _data = graphData;
            _checkData = new CheckData();
            _adjacencyMatrix = new int[_data.Size, _data.Size];
            _distanceMatrix = new int[_data.Size, _data.Size];
            CalculateAdjacencyMatrix();
            _adjacencyMatrix.Print2DMatrix("Adjancey Matrix:");
            _bfs = new BfsShortestPath(_structData, graphData);
        }

        private float CalculateStandartDeviation(int sumDegree, int m, int graphSize)
        {
            int mQuad = m.IntPowExtension();
            float value = (float)(4 * mQuad) / graphSize;
            Debug.Log("Value: "+value);
            float standartDeviant = sumDegree - value;
            Debug.Log("Среднее квадратичное отклонение: "+standartDeviant);
            return standartDeviant;
        }

        private float EstimateStructuralRedundancy(int connectivity, int size)
        {
            float r = (float)connectivity / ((float)size-1f) - 1f;
            Debug.Log("Структурная избыточность: "+r);
            if (r > 0)
            {
                Debug.Log("Структура избыточна");
            }
            else
            {
                Debug.Log("Структура не избыточна!");
            }

            return r;
        }

        private int EstimateConnectivity(int[] sums, int dataSize)
        {
            int intermediateM = sums.GetSum();
            int m = intermediateM / 2;
            Debug.Log("Структурная связность: "+m);
            if (m >= (dataSize - 1))
            {
                Debug.Log("Структура связана!");
            }
            else
            {
                Debug.Log("Структура не связанна");
            }

            return m;
        }
        

        public void CalculateAdjacencyMatrix()
        {
            for (int i = 0; i < _data.Size; i++)
            {
                for (int j = 0; j < _data.Size; j++)
                {
                    if (_structData.GraphConections[i] != null)
                    {
                        int value = j + _data.ShiftCount;
                        if (CheckMatch(_structData.GraphConections[i], value))
                        {
                            _adjacencyMatrix[i, j] = 1;
                        }
                        else
                        {
                            _adjacencyMatrix[i, j] = 0;
                        }
                    }
                }
            }
        }

        private bool CheckMatch(List<int> structDataGraphConection, int value)
        {
            for (int i = 0; i < structDataGraphConection.Count; i++)
            {
                if (structDataGraphConection[i] == value)
                {
                    return true;
                }
            }

            return false;
        }

        public void CalculateAllMetrix()
        {
            int[] sums = _adjacencyMatrix.CalculateSums();
            sums.PrintWithTitle("AdjM Sum");
            int connectivity = EstimateConnectivity(sums,_data.Size);
            float redundancyed = EstimateStructuralRedundancy(connectivity, _data.Size);
            int[] sumsDegree = sums.GetDegreeValue();
            sumsDegree.PrintWithTitle("Degrees:");
            int sumDegree = sumsDegree.GetSum();
            Debug.Log("Sum degree: "+sumDegree);
            float deviant = CalculateStandartDeviation(sumDegree, connectivity, _data.Size);
            int[] graphStruct = _data.GraphStructure;
            graphStruct.PrintWithTitle("Структура графа");
            Array.Sort(graphStruct);
            graphStruct.PrintWithTitle("Структура графа после сортировки");
            CreatePathMatrix(graphStruct, _bfs, _distanceMatrix);
            _distanceMatrix.Print2DMatrix("Path Matrix:");
            int[] pathSums = _distanceMatrix.CalculateSums();
            int Qcom = pathSums.GetSum();
            float QRel = CalculateQRelative(pathSums, _data.Size);
            Debug.Log("Структурная компактность: "+QRel);
            Debug.Log("Оценка степени централизации:");
            int minP = pathSums.Min();
            int maxP = _distanceMatrix.Cast<int>().Max();
            int pSumQ = pathSums.GetSum();
            float twoZMax = ((float)pSumQ / minP);
            Debug.Log("2zMax:"+twoZMax);
            float centralization = CalculateCentralization(twoZMax, (float)_data.Size);
            Debug.Log("Коэффициент централизации: "+centralization);
            EstimateCentralization(centralization);
            ProximityAssessment(centralization, redundancyed, deviant, Qcom, QRel, maxP, _checkData);

        }

        private void ProximityAssessment(float centralization, float redundancyed, float deviant, int qcom, float qRel, int maxP, CheckData checkData)
        {
            #region Получение ближайших значений и индексов

            float closestCentr = checkData.YCentralization.ClosestFromList(centralization);
            Debug.Log("Ближайшее значение централизации: " + closestCentr);
            int centralizationIndex = checkData.YCentralization.ReturnIndex(closestCentr);

            float closestRedun = checkData.Rredundancy.ClosestFromList(redundancyed);
            Debug.Log("Ближайшее значение избыточности: " + closestRedun);
            int redundancyedIndex = checkData.Rredundancy.ReturnIndex(closestRedun);

            float closestDeviant = checkData.ESquareDeviant.ClosestFromList(deviant);
            Debug.Log("Ближайшее значение квадратного отклонения: " + closestDeviant);
            int deviantIndex = checkData.Rredundancy.ReturnIndex(closestRedun);

            int closestQcom = checkData.QCompactness.ClosestFromList(qcom);
            Debug.Log("Ближайшее значение компактности: " + closestQcom);
            int compathIndex = checkData.QCompactness.ReturnIndex(closestQcom);
                
            float closestQrel = checkData.QrRelativeCompactness.ClosestFromList(qRel);
            Debug.Log("Ближайшее значение относительной компактности: " + closestQrel);
            int qrelIndex = checkData.QrRelativeCompactness.ReturnIndex(closestQrel);
                
            int closestDist = checkData.dMaxDistance.ClosestFromList(maxP);
            Debug.Log("Ближайшее значение диаметра: " + closestDist);
            int dIndex = checkData.dMaxDistance.ReturnIndex(closestDist);

            #endregion
        }

        private void EstimateCentralization(float centralization)
        {
            if (centralization >= 0 && centralization <= 1)
            {
                Debug.Log("Структура достаточно централизованная: ");
            }
            else if(centralization < 0)
            {
                Debug.Log("Структура не центрлизованна");
            }
            else if(centralization > 1)
            {
                Debug.Log("Структура черезчур централизованна");
            }
        }

        private float CalculateCentralization(float twoZMax, float dataSize)
        {
            float firstKoef = (dataSize - 1f) / (dataSize - 2f);
            float secondKoef = (twoZMax - dataSize)/(twoZMax/2);
            float final = firstKoef * secondKoef;
            return final;
        }

        private float CalculateQRelative(int[] pathSums, int dataSize)
        {
            float final = 0;
            float qMin = dataSize * (dataSize - 1);
            float qFull = pathSums.GetSum();
            Debug.Log("QMin: "+qMin);
            float intermediate = qFull / qMin;
            final = intermediate - 1f;
            return final;
        }

        private void CreatePathMatrix(int[] graphStruct, IBfs bfs, int[,] distanceMatrix)
        {
            Debug.Log("Веселье начинается");
            for (int i = 0; i < graphStruct.Length; i++)
            {
                for (int j = 0; j < graphStruct.Length; j++)
                {
                    int path = bfs.ReturnShortestPath(graphStruct[i], graphStruct[j]);
                    distanceMatrix[i, j] = path;
                }
            }
        }
    }
}