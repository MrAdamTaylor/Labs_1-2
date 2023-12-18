using System;
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

        public static int[][] GetBinaryRelation(int[] expertGrad)
        {
            var size = expertGrad.Length;
            var matrix = new int[size][];
            for (var i = 0; i < size; i++)
            {
                matrix[i] = new int[size];
                var fixedElementIndex = Array.FindIndex(expertGrad, (int element) => { return (element == (i + 1)); });
                for (var j = 0; j < size; j++)
                {
                    if (i == j)
                    {
                        matrix[i][j] = 1;
                        continue;
                    }
                    var currentElementIndex = Array.FindIndex(expertGrad, (int element) => { return (element == (j + 1)); });
                    matrix[i][j] = (fixedElementIndex < currentElementIndex) ? 0 : 1;
                }
            }

            return matrix;
        }

        public static int GetDistanceBetweenBinaryRelation(int[][] A, int[][] B)
        {
            int distance = 0;
            var matrix = SubtractBinaryRelations(A, B);

            if (matrix == null) return (-1);
            
            for (var i = 0; i < matrix.Length; i++)
            {
                for (var j = 0; j < matrix.Length; j++)
                {
                    distance += matrix[i][j];
                }
            }

            return distance;
        }

        public static int[][] SubtractBinaryRelations(int[][] A, int[][] B)
        {
            if (A.Length != B.Length && A[0].Length != B[0].Length)
                return null;

            var result = new int[A.Length][];
            for (var i = 0; i < A.Length; i++)
            {
                result[i] = new int[A[i].Length];
                for (var j = 0; j < A[i].Length; j++)
                {
                    result[i][j] = Math.Abs(A[i][j] - B[i][j]);
                }
            }

            return result;
        }

        public static int[][] GetPreferenceMatrix(int[][] data)
        {
            var list = new List<int[]>();

            var n = data.Length;
            var m = data[0].Length;

            for (var i = 0; i < n; i++)
            {
                var line = new int[m];
                for (var j = 0; j < m; j++)
                {
                    line[j] = Array.FindIndex(data[i], (int value) => { return value == (j + 1); });
                }
                list.Add(line);
            }

            return list.ToArray();
        }

        public static int[][] GetLossMatrix(int[][] preferenceMatrix)
        {
            var list = new List<int[]>();

            var n = preferenceMatrix.Length;
            var m = preferenceMatrix[0].Length;

            var column = 0;

            for (var i = 0; i < m; i++)
            {
                var line = new int[m];
                for (var j = 0; j < m; j++)
                {
                    var sum = 0;
                    for (var k = 0; k < n; k++)
                    {
                        sum += Math.Abs(j - preferenceMatrix[k][column]);
                    }
                    line[j] = sum;
                }
                list.Add(line);

                column++;
            }

            return list.ToArray();
        }

        public static int[][] GetOptimizationMatrix(int[][] lossMatrix)
        {
            var n = lossMatrix.Length;
            
            var list = new List<int[]>();
            {
                var i = -1;
                while (++i < n)
                    list.Add(new int[lossMatrix[i].Length]);
            }

            var zeroMatrix = list.ToArray();

            var count = 0;
            while (count < n)
            {
                var point = GetMinimalElementOfMatrix(lossMatrix);
                zeroMatrix[point.x][point.y] = 1;
                count++;
            }

            return zeroMatrix;
        }
        
        class IndexPoint
        {
            public IndexPoint() { x = -1; y = -1; }
            public IndexPoint(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public const int MAX_VALUE = 65535;

            /// <summary>
            /// Строка таблицы
            /// </summary>
            public int x;
            /// <summary>
            /// Столбец таблицы
            /// </summary>
            public int y;
        }
        
        private static IndexPoint GetMinimalElementOfMatrix(int[][] matrix)
        {
            var min = matrix[0][0];
            var index = new IndexPoint(0, 0);

            for (var i = 0; i < matrix.Length; i++)
            {
                for (var j = 0; j < matrix[i].Length; j++)
                {
                    var element = matrix[i][j];

                    if (element < min)
                    {
                        index.x = i;
                        index.y = j;
                    }
                }
            }

            CheckSelectedElement(matrix, index);
            return index;
        }
            
        private static void CheckSelectedElement(int[][] matrix, IndexPoint point)
        {
            for (var i = 0; i < matrix.Length; i++)
                matrix[i][point.y] = IndexPoint.MAX_VALUE;

            for (var j = 0; j < matrix[0].Length; j++)
                matrix[point.x][j] = IndexPoint.MAX_VALUE;
        }   
            
    }
}