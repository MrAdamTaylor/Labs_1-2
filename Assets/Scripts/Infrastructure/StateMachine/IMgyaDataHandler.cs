using Unity.VisualScripting;
using UnityEngine;

namespace Infrastructure.StateMachine
{
    public interface IMgyaDataHandler
    {
        //void PrintData();
        int[][] Data { get; set; }

        int GradCount { get; set; }

        float[] AverageValuer { get; set; }

        float[] SumValues { get; set; }
    }

    class MgyaDataHandler : IMgyaDataHandler
    {
        private MgyaData _mgyaData;
        public int[][] Data { get; set; }
        public int GradCount { get; set; }
        public int ExpertCount { get; set; }
        public float[] AverageValuer { get; set; }
        public float[] SumValues { get; set; }


        public MgyaDataHandler(MgyaData mgyaData)
        {
            _mgyaData = mgyaData;
            Debug.Log("Обработчик данных готов к работе!");
            //Data = _mgyaData.GetMatrix();
            Data = _mgyaData.GetJaggedMatrix();
            GradCount = Data[0].Length;
            ExpertCount = Data.Length;
            SumValues = new float[GradCount];
            AverageValuer = new float[GradCount];
            CalculateMetrics();
        }

        private void CalculateMetrics()
        {
            for (var i = 0; i < GradCount; i++)
            {
                for (var j = 0; j < ExpertCount; j++)
                {
                    SumValues[i] += Data[j][i];
                }
                AverageValuer[i] = SumValues[i] / ExpertCount;
            }
        }
    }
}