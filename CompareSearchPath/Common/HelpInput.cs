using CompareSearchPath.Models;

namespace CompareSearchPath.Common;

public static class HelpInput
{
    // метод для ввода и проверки валидности файла
    public static void InputNameFile(out string path)
    {
        path = string.Empty;
        while (true)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Введите название файла:");
            Console.ResetColor();
            path = @"..\..\..\Data\TxtFiles\" + Console.ReadLine();
            var fileInfo = new FileInfo(path);
            if(fileInfo.Extension == ".txt")
                break;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Файл должен иметь расширение .txt");
            Console.ResetColor();
        }
    }
    
    // метод выводв матрицы на консоль
    public static void ShowMap(bool[,] map)
    {
        var maxLenCol = map.GetLength(1).ToString().Length + 1;
        var maxLenRow = map.GetLength(0).ToString().Length + 1;

        if (maxLenCol == 1)
            maxLenCol++;
        if (maxLenRow == 1)
            maxLenRow++;

        Console.Write(" ".PadRight(maxLenRow));
        Console.ForegroundColor = ConsoleColor.Blue;
        for (int i = 0; i < map.GetLength(1); i++)
        {
            Console.Write($"{i}".PadRight(maxLenCol));
        }
        Console.ResetColor();
        Console.WriteLine();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write($"{i}".PadRight(maxLenRow));
            Console.ResetColor();
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Console.Write(map[i, j] ? "[]".PadRight(maxLenCol) : "0".PadRight(maxLenCol));
            }
            Console.WriteLine();
        }
    }

    // метод для ввода стен на карте
    public static void InputMap(bool[,] map)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Введите кол-во стен карты:");
        Console.ResetColor();
        var n = -1;
        while (n < 0)
        {
            var tmp = Console.ReadLine();
            if (!int.TryParse(tmp, out n))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Некорректное значение. Повторите попытку:");
                Console.ResetColor();
                n = -1;
            }
            else if (n < 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Введено отрицательное число. Повторите попытку:");
                Console.ResetColor();
            }
        }

        for (int i = 0; i < n; i++)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Введите координаты {i + 1} стены через пробел");
            Console.ResetColor();
            InputPoint(out var node, map.GetLength(0), map.GetLength(1));
            map[node.X, node.Y] = true;
        }
    }

    // метод для ввода и проверки валидности размерности матрицы
    public static void InputRangeMap(out int n, out int m)
    {
        n = 0;
        m = 0;
        Console.WriteLine("Введите диапазон размера карты через пробел.");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Кол-во строк и кол-во столбцов карты соответственно: ");
        Console.ResetColor();
        // цикл выполняется пока введенные данные не будут удовлетворять верным данным
        while (n < 2 || m < 2)
        {
            var tmp = Console.ReadLine().Split();
            if (tmp.Length != 2 || !int.TryParse(tmp[0], out n) || !int.TryParse(tmp[1], out m))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Некорректное значение. Повторите попытку:");
                Console.ResetColor();
            }
            else if (n < 2 || m < 2)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Размеры должен быть минимум 2");
                Console.ResetColor();
            }
        }
    }

    // метод для ввода значения процента стен на карте
    public static void InputPercentWallMap(out int p)
    {
        p = -1;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Введите процент заполнения стен: ");
        Console.ResetColor();
        while (p < 0)
        {
            if (!int.TryParse(Console.ReadLine(), out p))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Некорректное значение. Повторите попытку:");
                Console.ResetColor();
                p = -1;
            }
            else if (p > 100)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Процент должен быть в диапазоне [0;100]. Повторите попытку:");
                Console.ResetColor();
                p = -1;
            }
        }
    }

    // метод для ввода стартовой и целевой точек
    public static void InputStartEnd(out Node start, out Node end, int n, int m)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Введите координаты стартовой точки через пробел:");
        Console.ResetColor();
        InputPoint(out start, n, m);
        
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Введите координаты целевой точки через пробел:");
        Console.ResetColor();
        InputPoint(out end, n, m);
    }

    // метод для ввода и проверки валидности точки
    private static void InputPoint(out Node node, int n = int.MaxValue, int m = int.MaxValue)
    {
        var loc = new int[] {-1, -1};
        while (loc[0] < 0 || loc[0] >= n || loc[1] < 0 || loc[1] >= m)
        {
            var tmp = Console.ReadLine().Split();
            if (tmp.Length != 2 || !int.TryParse(tmp[0], out loc[0]) || !int.TryParse(tmp[1], out loc[1]))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Некорректное значение. Повторите попытку:");
                Console.ResetColor();
                loc[0] = -1;
                loc[1] = -1;
            }
            else if (loc[0] < 0 || loc[0] >= n || loc[1] < 0 || loc[1] >= m)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Выход за пределы карты. Повторите попытку:");
                Console.ResetColor();
            }
        }
        node = new Node(loc[0], loc[1]);
    }
}