using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.StateMachine
{
    public static class MgyaMethods
    {
        public static int[][] TransformMatrix(int[][] binaryRelation)
        {
            var matrix = new List<int[]>(binaryRelation.Length);

            for (var i = 0; i < binaryRelation.Length; i++)
            {
                matrix.Add(new int[binaryRelation[i].Length]);

                var j = 0;
                for (var grade = 1; grade <= binaryRelation[i].Max(); grade++)
                {
                    for (var k = 0; k < binaryRelation[i].Length; k++)
                    {
                        if (binaryRelation[i][k] == grade)
                        {
                            matrix[i][j] = (k + 1);
                            break;
                        }
                    }

                    j++;
                }
            }

            return matrix.ToArray();
        }

        public static float[] GetMedians(int[][] matrix)
        {
            int middleIndex = matrix[0].Length / 2;
            bool isEvenValue = (matrix[0].Length % 2 == 0);

            float[] medians = new float[matrix.Length];

            for (var i = 0; i < matrix.Length; i++)
            {
                medians[i] = (isEvenValue) ?
                    (((matrix[i][middleIndex - 1] + matrix[i][middleIndex])) / 2.0f) :
                    (matrix[i][middleIndex - 1]);
            }

            return medians;
        }
    }
}