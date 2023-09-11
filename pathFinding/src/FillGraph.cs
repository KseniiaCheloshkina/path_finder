// TODO: check whether it is needed
using System.Runtime.InteropServices;

namespace pathFinding.src;

public static class FillGraph
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
            if (fileInfo.Extension == ".txt")
                break;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Файл должен иметь расширение .txt");
            Console.ResetColor();
        }
    }

    // print grid
    public static void DrawGrid(bool[,] map)
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

    // Define walls on the map
    public static void InputMap(bool[,] map)
    {
        // Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Input number of walls");
        // Console.ResetColor();
        var num_walls = -999;
        var mess = "Insert positive number";
        while (num_walls < 0)
        {
            var tmp = Console.ReadLine();
            if (!int.TryParse(tmp, out num_walls))
            {
                // Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(mess);
                // Console.ResetColor();
                num_walls = -999;
            }
            else if (num_walls < 0)
            {
                //Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(mess);
                // Console.ResetColor();
            }

        }

        for (int i = 0; i < num_walls; i++)
        {
            // Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Insert coordinate of {i + 1} wall using the following format: `x y`");
            // Console.ResetColor();
            Console.WriteLine(map.GetLength(0));
            Console.WriteLine(map.GetLength(1));
            Console.WriteLine(map);
            InputPoint(out var node, map.GetLength(0), map.GetLength(1));
            map[node.X, node.Y] = true;
        }
    }

    // Manual map designing
    public static void InputRangeMap(out int width, out int height, out bool[,] map)
    {
        width = 0;
        height = 0;
        // Console.WriteLine("Введите диапазон размера карты через пробел.");
        // Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Insert number of rows and number of columns splitted by whitespace");
        Console.ResetColor();
        while (width < 2 || height < 2)
        {
            var tmp = Console.ReadLine().Split();
            if (tmp.Length != 2 || !int.TryParse(tmp[0], out width) || !int.TryParse(tmp[1], out height) || width < 2 || height < 2)
            {
                // Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("You must specify 2 numbers in format `x y`. Both `x` and `y` should be not less than 2");
                Console.ResetColor();
            }
        }

        // create grid
        // var map = new bool[,] { };
        map = new bool[width, height];
        DrawGrid(map);
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


    // Define wall coordinate
    public static void InputPoint(out Cell node, int width = int.MaxValue, int height = int.MaxValue)
    {
        // position of new wall
        var loc = new int[] { -1, -1 };
        while (loc[0] < 0 || loc[0] >= width || loc[1] < 0 || loc[1] >= height)
        {
            var input_str = Console.ReadLine().Split();
            if (input_str.Length != 2 || !int.TryParse(input_str[0], out loc[0]) || !int.TryParse(input_str[1], out loc[1]))
            {
                // Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("You should insert two numbers in format `x y`");
                // Console.ResetColor();
                loc[0] = -1;
                loc[1] = -1;
            }
            else if (loc[0] < 0 || loc[0] >= width || loc[1] < 0 || loc[1] >= height)
            {
                //Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"First coordinate should be less than {width}, second coordinate should be less than {height}");
                //Console.ResetColor();
            }
        }
        node = new Cell(loc[0], loc[1]);
    }
}