using System;

class Program
{
    static void Main()
    {
        int n = 6;
        int[,] A = new int[n, n];
        int[,] B = new int[n, n];
        int[] X = new int[n];
        Random rnd = new Random();

        // Генерація матриць A і B
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                A[i, j] = rnd.Next(-50, 50); // -50 ... +49
                B[i, j] = rnd.Next(-50, 50);
            }
        }

        // Вивід матриці A
        Console.WriteLine("Matrix A:");
        PrintMatrix(A);

        // Вивід матриці B
        Console.WriteLine("Matrix B:");
        PrintMatrix(B);

        // Формування вектора X
        for (int i = 0; i < n; i++)
        {
            bool allGreater = true;
            for (int j = 0; j < n; j++)
            {
                if (A[i, j] <= B[i, j])
                {
                    allGreater = false;
                    break;
                }
            }
            X[i] = allGreater ? 1 : 0;
        }

        // Вивід вектора X
        Console.WriteLine("Vector X:");
        Console.WriteLine(string.Join(" ", X));
    }

    static void PrintMatrix(int[,] matrix)
    {
        int n = matrix.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write($"{matrix[i, j],4}");
            }
            Console.WriteLine();
        }
    }
}