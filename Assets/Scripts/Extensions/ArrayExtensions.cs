using Unity.VisualScripting;
using UnityEngine;

namespace Extensions
{
    public static class ArrayExtensions
    {
        internal static T[][] ToJaggedArray<T>(this T[,] twoDimensionalArray)
        {
            int rowsFirstIndex = twoDimensionalArray.GetLowerBound(0);
            int rowsLastIndex = twoDimensionalArray.GetUpperBound(0);
            int numberOfRows = rowsLastIndex + 1;

            int columnsFirstIndex = twoDimensionalArray.GetLowerBound(1);
            int columnsLastIndex = twoDimensionalArray.GetUpperBound(1);
            int numberOfColumns = columnsLastIndex + 1;

            T[][] jaggedArray = new T[numberOfRows][];
            for (int i = rowsFirstIndex; i <= rowsLastIndex; i++)
            {
                jaggedArray[i] = new T[numberOfColumns];

                for (int j = columnsFirstIndex; j <= columnsLastIndex; j++)
                {
                    jaggedArray[i][j] = twoDimensionalArray[i, j];
                }
            }
            return jaggedArray;
        }

        internal static void PrintJaggedArray<T>(this T[][] array)
        {
            
            for (int x = 0; x < array.Length; x++)
            {
                string strMark = x.ToString();
                string row = "Row number " + strMark + ": ";
                for (int i = 0; i < array[x].Length; i++)
                {
                    string str = array[x][i].ToString();
                    row = row + str + " ";
                }
                Debug.Log(row);
            }
        }
        
        internal static void PrintJaggedArray<T>(this T[][] array, string mark)
        {
            
            for (int x = 0; x < array.Length; x++)
            {
                string strMark = x.ToString();
                string row = mark + strMark +":  ";
                for (int i = 0; i < array[x].Length; i++)
                {
                    string str = array[x][i].ToString();
                    row = row + str + " ";
                }
                Debug.Log(row);
            }
        }

        internal static void PrintOneDimensionArray<T>(this T[] array, string mark)
        {
            string row = "";
            for (int x = 0; x < array.Length; x++)
            {
                string strValue = array[x].ToString();
                row = row + mark + strValue + " ";
            }
            Debug.Log(row);
        }

        internal static T[][] TransposeMatrix<T>(this T[][] matrix)
        {
            int rowCount = matrix.Length;
            int columnCount = matrix[0].Length;
            T[][] transposed = new T[columnCount][];
            if (rowCount == columnCount)
            {
                transposed = (T[][])matrix.Clone();
                for (int i = 1; i < rowCount; i++)
                {
                    for (int j = 0; j < i; j++)
                    {
                        T temp = transposed[i][j];
                        transposed[i][j] = transposed[j][i];
                        transposed[j][i] = temp;
                    }
                }
            }
            else
            {
                for (int column = 0; column < columnCount; column++)
                {
                    transposed[column] = new T[rowCount];
                    for (int row = 0; row < rowCount; row++)
                    {
                        transposed[column][row] = matrix[row][column];
                    }
                }
            }
            return transposed;
        }
    }
}