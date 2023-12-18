using System.Collections.Generic;
using System.Linq;
using Infrastructure;
using Infrastructure.Bootstrap.Tasks;
using UnityEngine;

namespace Extensions
{
    public static class Extensions
    {
        #region Расширения для возврата уникальных значений или массива

        public static int[] ReturnUniqueArrayNumbers(this int[] result, int[] array)
        {
            List<int> values = new List<int>();
            FullUniqueNumbers(array, values);
            result = ConvertListToArray(values);
            return result;
        }

        public static int[] ConvertListToArray(List<int> values)
        {
            int[] tempData = new int[values.Count];
            for (int i = 0; i < values.Count; i++)
            {
                tempData[i] = values[i];
            }
            return tempData;
        }

        public static int ReturnSizeUniqueNumbers(this int result, int[] array)
        {
            List<int> values = new List<int>();

            FullUniqueNumbers(array, values);

            result = values.Count;

            return result;
        }

        //TODO - для МГУА
        public static void PrintCountWithMark(this int count, string mark)
        {
            string row = "";
            for (int i = 0; i < count; i++)
            {
                string str = i.ToString();
                row = row + mark + str + ": ";
            }
            Debug.Log(row);
        }

        public static void PrintWithTitle(this float[] array, string title)
        {
            if (array != null)
            {
                string row = title + ": ";
                foreach (var value in array)
                {
                    string str = value.ToString();
                    row = row + str + ":   ";
                }
                Debug.Log(row);
            }
        }

        public static void PrintRangedLine(this float[] array, string label, int correctValue = 1)
        {
            var printedCount = 0;
            var averageList = array.ToList();
            string row = "Итог: ";
            while (printedCount != array.Length)
            {
                var minValue = averageList.Min();
                var index = averageList.IndexOf(minValue);
                string str = (index + correctValue).ToString();
                row = row + label + str + ": ";
                averageList[index] = float.MaxValue;
                printedCount++;
            }
            Debug.Log(row);
        }

        private static void FullUniqueNumbers(int[] numbers, List<int> values)
        {
            //TODO - ещё пример, где LINQ упростил решение задачи
            int[] distinct = numbers.Distinct().ToArray();
            for (int i = 0; i < distinct.Length; i++)
            {
                values.Add(distinct[i]);
            }
        }
        #endregion

        #region Расширения для типа AdjacenyListStruct

        //Расширение для класса массива
        public static int[] ConvertToArray(this AdjacenyListStruct[] result, AdjacenyListStruct[] graphData)
        {
            int[] allIntData;
            int arraySize = CalculateSizeForArray(graphData);
            allIntData = new int[arraySize];
            allIntData = FillDataArray(graphData, arraySize);
            return allIntData;
        }

        public static int GetShiftValue(this GraphData data, int[] array)
        {
            int minValue = array.Min();
            int shift = 0 + array.Min();
            return shift;
        }

        private static int[] FillDataArray(AdjacenyListStruct[] graphData, int arraySize)
        {
            int[] tempArrayData = new int[arraySize];
            int indexValue = 0;
            int count = graphData.Length;
            for (int i = 0; i < count; i++)
            {
                tempArrayData[indexValue] = graphData[i].graph;
                indexValue++;
                for (int j = 0; j < graphData[i].neighbours.Length; j++)
                {
                    tempArrayData[indexValue] = graphData[i].neighbours[j];
                    indexValue++;
                }
            }
            return tempArrayData;
        }

        private static int CalculateSizeForArray(AdjacenyListStruct[] graphData)
        {
            int sum = 0;
            int allDataCount = graphData.Length;
            for (int i = 0; i < allDataCount; i++)
            {
                sum += 1;
                for (int j = 0; j < graphData[i].neighbours.Length; j++)
                {
                    sum += 1;
                }
            }
            return sum;
        }

        #endregion
        
        
    }
}