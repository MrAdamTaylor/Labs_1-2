using System;
using System.Linq;
using Extensions;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public class MgyaExecuter : IMgyaExecuter
    {
        private IMgyaDataHandler _mgyaDataHandler;
        
        public void Execute()
        {
            AverageRanksMethod();
            MedianRanksMethod();
            MedianKaemenyAvailableMethod();
            MedianKemenyAllRanksMethod();
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
            _mgyaDataHandler.GradCount.PrintCountWithMark("a");
            _mgyaDataHandler.Data.PrintJaggedArray("Э");
            _mgyaDataHandler.SumValues.PrintWithTitle("Сум");
            _mgyaDataHandler.AverageValuer.PrintWithTitle("Сум");
            _mgyaDataHandler.AverageValuer.PrintRangedLine("Итог");
        }

        private void MedianRanksMethod()
        {
            Debug.Log("Метод медианных рангов: ");
            float[] medians = null;

            var medianMatrix = MgyaMethods.TransformMatrix(_mgyaDataHandler.Data);
            var projectCount = medianMatrix[0].Length;
            
            projectCount.PrintCountWithMark("a");
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
            projectCount.PrintCountWithMark("a");
            medianMatrix.PrintJaggedArray();
            medians.PrintWithTitle("Медианны");
            medians.PrintRangedLine("a");

        }

        private void MedianKaemenyAvailableMethod()
        {
            Debug.Log("Медиана Камени именной ранжировки: ");
            var matrixSize = _mgyaDataHandler.Data.Length;
            var distantionMatrix = new int[matrixSize][];
            
            Debug.Log("Матрица бинарных отношений");
            var binaryRelation = new int[matrixSize][][];
            
            for (var i = 0; i < matrixSize; i++)
            {
                Debug.Log("Бинарная матрица №"+i);
                binaryRelation[i] = MgyaMethods.GetBinaryRelation(_mgyaDataHandler.Data[i]);

                #region Регион отладки

                binaryRelation[i].PrintJaggedArray("Строка бинароной матрицы");

                #endregion
            }

            for (int i = 0; i < matrixSize; i++)
            {
                distantionMatrix[i] = new int[matrixSize];
                for (int j = 0; j < matrixSize; j++)
                {
                    var A = binaryRelation[i];
                    var B = binaryRelation[j];
                    distantionMatrix[i][j] = MgyaMethods.GetDistanceBetweenBinaryRelation(A,B);
                    Debug.Log("Дистанция бинарного отношения между " +
                              "бинарной матрицей " + i + " и бинарной матрицей "+ j+": "+distantionMatrix[i][j]);
                }
            }
            
            Debug.Log(" Матрица попарных расстояний: ");
            matrixSize.PrintCountWithMark("Э");
            distantionMatrix.PrintJaggedArray("Э");

            float[] sum = new float[matrixSize];
            for (var i = 0; i < sum.Length; i++)
            {
                for (var j = 0; j < sum.Length; j++)
                {
                    sum[i] += distantionMatrix[i][j];
                }
            }
            sum.PrintWithTitle("Сум");
            var currentExpertIndex = Array.FindIndex(sum, (float value) 
                => { return (value == sum.Min()); });
            Debug.Log("Текущий индекс эксперта!" + currentExpertIndex);
            _mgyaDataHandler.Data[currentExpertIndex].PrintOneDimensionArray("a");
        }

        private void MedianKemenyAllRanksMethod()
        {
            Debug.Log("Медиана камени - метод всех Ранжировок");
            Debug.Log("Векторы предпочтений: ");
            int[][] preferenceMatrix = MgyaMethods.GetPreferenceMatrix(_mgyaDataHandler.Data);
            Debug.Log("Матрица предпочтений");
            preferenceMatrix.First().Length.PrintCountWithMark("a");
            preferenceMatrix.PrintJaggedArray("Строка предпочтений ");
            Debug.Log("Матрица потерь");
            int[][] lossMatrix = MgyaMethods.GetLossMatrix(preferenceMatrix);
            lossMatrix.First().Length.PrintCountWithMark("#");
            lossMatrix.PrintJaggedArray("Строка потерь a");
            Debug.Log("Матрица значений");
            int[][] finalMatrix = MgyaMethods.GetOptimizationMatrix(lossMatrix);
            finalMatrix.First().Length.PrintCountWithMark("op#a");
            finalMatrix.PrintJaggedArray("Строка оптимизированной матрицы ");
            string Finalrow = "Итог: ";
            for (var i = 0; i < finalMatrix.Length; i++)
            {
                string row = "";
                for (int j = 0; j < finalMatrix[i].Length; j++)
                {
                    if (finalMatrix[j][i] == 1)
                    {
                        string str = (j + 1).ToString();
                        row = row + "a" + str;
                    }
                }

                Finalrow = Finalrow + row + " ";
            }
            Debug.Log(Finalrow);
        }
    }
}