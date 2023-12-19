using System.Collections.Generic;
using UnityEngine;

namespace Extensions
{
    public static class Indexator 
    {
        public static int ReturnIndex(this List<float> array, float value)
        {
            int ourIndex = -1;
            for (int i = 0; i < array.Count; i++)
            {
                if (Mathf.Approximately(array[i],value))
                {
                    ourIndex = i;
                }
            }

            return ourIndex;
        }

        public static int ReturnIndex(this List<int> array, int value)
        {
            int ourIndex = -1;
            for (int i = 0; i < array.Count; i++)
            {
                if (array[i] == value)
                {
                    ourIndex = i;
                }
            }

            return ourIndex;
        }
    }
}