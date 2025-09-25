using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // Приклад зубчастого масиву
        int[][] jaggedArray = new int[][]
        {
            new int[] { 0, -2, 3 },
            new int[] { 4, 5 },
            new int[] { -1, 6, 7, 8 }
        };

        // Знаходимо максимальну кількість стовпців
        int maxCols = 0;
        foreach (var row in jaggedArray)
            if (row.Length > maxCols) maxCols = row.Length;

        // Масив для перших додатних елементів у кожному стовпці
        List<int?> firstPositives = new List<int?>();

        for (int col = 0; col < maxCols; col++)
        {
            int? firstPositive = null;
            for (int row = 0; row < jaggedArray.Length; row++)
            {
                if (col < jaggedArray[row].Length && jaggedArray[row][col] > 0)
                {
                    firstPositive = jaggedArray[row][col];
                    break;
                }
            }
            firstPositives.Add(firstPositive);
        }

        // Вивід результату
        Console.WriteLine("Перші додатні елементи у кожному стовпці:");
        for (int i = 0; i < firstPositives.Count; i++)
        {
            if (firstPositives[i].HasValue)
                Console.Write(firstPositives[i] + " ");
            else
                Console.Write("None ");
        }
        Console.WriteLine();
    }
}