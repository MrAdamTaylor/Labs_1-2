using System.Collections.Generic;

namespace Infrastructure.StateMachine
{
    internal class CheckData
    {
        public List<float> YCentralization;
        public List<float> Rredundancy;
        public List<float> ESquareDeviant;
        public List<int> QCompactness;
        public List<float> QrRelativeCompactness;
        public List<int> dMaxDistance;
        public CheckData() 
        {
            // После - 0, Кольцевая - 1, Радиальная - 2, Древовидная - 3, Полный граф - 4
            YCentralization = new List<float>() { 0.571f, 0, 1f, 0.703f, 0 };
            Rredundancy = new List<float>() { 0, 0.125f, 0, 0.25f, 3.5f };
            ESquareDeviant = new List<float>() { 1.556f, 0, 43,556f, 15.556f, 0};
            QCompactness = new List<int>() { 240, 180, 128, 156, 72 };
            QrRelativeCompactness = new List<float>() { 2.333f, 1.5f, 0.778f, 0, 1.167f };
            dMaxDistance = new List<int>() { 8, 4, 2, 4, 2 };
        }
    }
}