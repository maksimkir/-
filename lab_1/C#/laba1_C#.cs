using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine(" Write your radius R:");
        Console.Write("R = ");
        int R = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine(" Write your point coordinates:");
        Console.Write("x = ");
        int x = Convert.ToInt32(Console.ReadLine());
        Console.Write("y = ");
        int y = Convert.ToInt32(Console.ReadLine());

        // Локальна функція для перевірки
        static string Popav(int x, int y, int R)
        {
            // Перша область – півколо у першій чверті
            if (x >= 0 && y >= 0)
            {
                double d = x * x + y * y;
                if (Math.Abs(d - R * R) < 1e-9) return "At the boundary";
                if (d < R * R) return "Yes";
            }

            // Друга область – ліва частина кола з центром (0, -R)
            if (x <= 0)
            {
                double d2 = x * x + (y + R) * (y + R);
                if (Math.Abs(d2 - R * R) < 1e-9) return "At the boundary";
                if (d2 < R * R) return "Yes";
            }

            return "No";
        }

        string result = Popav(x, y, R);
        Console.WriteLine("Result: " + result);
    }
}
