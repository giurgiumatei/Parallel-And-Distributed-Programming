using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3.Domain
{
    public class MatrixCalculator
    {
        private static readonly object Lock = new();
        public static void ComputeTask(List<List<int>> matrix1, List<List<int>> matrix2, List<int> coordinate, int numberOfColumns ,List<List<int>> result)
        {
            lock (Lock)
            {
                int product = 0;
                int row = coordinate[0];
                int column = coordinate[1];

                for (int i = 0; i < numberOfColumns; i++)
                {
                    Interlocked.Add(ref product, matrix1[row][i] * matrix2[i][column]);
                }

                result[row][column] = product;
            }
        }

        public static  void Compute(List<List<int>> matrix1, List<List<int>> matrix2, List<List<int>> batch, int numberOfColumns, List<List<int>> result)
        {
            batch.ForEach(pair =>
            {
                lock (Lock)
                {
                    int product = 0;

                    int row = pair[0];
                    int column = pair[1];

                    for (int i = 0; i < numberOfColumns; i++)
                    {
                        Interlocked.Add(ref product, matrix1[row][i] * matrix2[i][column]);
                    }

                    result[row][column] = product;
                }
            });
        }

        public static List<List<int>> InitializeMatrix(int numberOfRows, int numberOfColumns)
        {
            var resultingMatrix = new List<List<int>>();

            for (int i = 0; i < numberOfRows; i++)
            {
                var row = Enumerable.Range(1, numberOfColumns).ToList();
                resultingMatrix.Add(row);
            }

            return resultingMatrix;
        }

        public static void PrintMatrix(List<List<int>> matrix)
        {
            matrix.ForEach(row =>
            {
                row.ForEach(element => Console.Write(element + " "));
                Console.Write("\n");
            });
        }

        public static List<List<int>> GetCoordinatesOfMatrix(int matrixRows, int matrixColumns)
        {
            List<List<int>> coordinates = new();
            
            for (int i = 0; i < matrixRows; i++)
            {
                for (int j = 0; j < matrixColumns; j++)
                {
                    coordinates.Add(new List<int>{i, j});
                }
            }
            return coordinates;
        }

    }
}
