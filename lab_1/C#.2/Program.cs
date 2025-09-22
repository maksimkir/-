using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введіть координати двох білих слонів (x1 y1 x2 y2):");
        int[] whiteBishops = ReadCoordinates(2);

        Console.WriteLine("Введіть координати двох чорних слонів (x3 y3 x4 y4):");
        int[] blackBishops = ReadCoordinates(2);

        // Перевірка на коректність введення
        if (!IsValid(whiteBishops, blackBishops))
        {
            Console.WriteLine("Некоректне введення: фігури знаходяться за межами поля або на одній клітинці.");
            return;
        }

        Console.WriteLine("Якою фігурою зробити перший хід?");
        Console.WriteLine("1 - Білий слон 1");
        Console.WriteLine("2 - Білий слон 2");
        Console.WriteLine("3 - Чорний слон 1");
        Console.WriteLine("4 - Чорний слон 2");
        int choice = int.Parse(Console.ReadLine());

        int[] allBishops = new int[8];
        Array.Copy(whiteBishops, 0, allBishops, 0, 4);
        Array.Copy(blackBishops, 0, allBishops, 4, 4);

        int idx = (choice - 1) * 2;
        int x = allBishops[idx];
        int y = allBishops[idx + 1];

        bool attack = false, defend = false;
        string result = "простий хід";

        // Визначаємо, кого атакує або захищає обраний слон
        for (int i = 0; i < 4; i++)
        {
            if (i == (choice - 1)) continue; // не перевіряємо саму фігуру
            int tx = allBishops[i * 2];
            int ty = allBishops[i * 2 + 1];
            if (Math.Abs(x - tx) == Math.Abs(y - ty))
            {
                // Якщо атакує фігуру іншого кольору
                if ((choice <= 2 && i >= 2) || (choice >= 3 && i < 2))
                {
                    attack = true;
                    result = $"здійснює напад на {(i < 2 ? "білого" : "чорного")} слона {i % 2 + 1}";
                    break;
                }
                // Якщо "захищає" свою фігуру (умовно)
                else if ((choice <= 2 && i < 2) || (choice >= 3 && i >= 2))
                {
                    defend = true;
                    result = $"захищає {(i < 2 ? "білого" : "чорного")} слона {i % 2 + 1}";
                }
            }
        }

        Console.WriteLine(result);
    }

    static int[] ReadCoordinates(int count)
    {
        int[] coords = new int[count * 2];
        string[] input = Console.ReadLine().Split();
        for (int i = 0; i < count * 2; i++)
            coords[i] = int.Parse(input[i]);
        return coords;
    }

    static bool IsValid(int[] w, int[] b)
    {
        int[][] all = { new int[] { w[0], w[1] }, new int[] { w[2], w[3] }, new int[] { b[0], b[1] }, new int[] { b[2], b[3] } };
        // Перевірка меж поля
        foreach (var pos in all)
            if (pos[0] < 1 || pos[0] > 8 || pos[1] < 1 || pos[1] > 8)
                return false;
        // Перевірка на співпадіння координат
        for (int i = 0; i < all.Length; i++)
            for (int j = i + 1; j < all.Length; j++)
                if (all[i][0] == all[j][0] && all[i][1] == all[j][1])
                    return false;
                    
        return true;
    }
}