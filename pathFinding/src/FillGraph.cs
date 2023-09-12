namespace pathFinding.src;

public static class FillGraph
{
    // Define walls on the map
    public static void DefineWalls(out int[][] walls, bool[,] map)
    {
        Console.WriteLine("Input number of walls");
        var num_walls = -999;
        var mess = "Insert positive number";
        while (num_walls < 0)
        {
            var tmp = Console.ReadLine();
            if (!int.TryParse(tmp, out num_walls))
            {
                Console.WriteLine(mess);
                num_walls = -999;
            }
            else if (num_walls < 0)
            {
                Console.WriteLine(mess);
            }

        }
        walls = new int[num_walls][];

        for (int i = 0; i < num_walls; i++)
        {
            Console.WriteLine($"Insert coordinate of {i + 1} wall using the following format: x y");
            InputPoint(out var node, map.GetLength(0), map.GetLength(1));
            map[node.X, node.Y] = true;
            walls[i] = new int[] { node.X, node.Y };
        }
    }

    // Manual map designing
    public static void GridCreation(out int width, out int height, out bool[,] map)
    {
        width = 0;
        height = 0;
        Console.WriteLine("Insert number of rows and number of columns splitted by whitespace");
        Console.ResetColor();
        while (width < 2 || height < 2)
        {
            var tmp = Console.ReadLine().Split();
            if (tmp.Length != 2 || !int.TryParse(tmp[0], out width) || !int.TryParse(tmp[1], out height) || width < 2 || height < 2)
            {
                Console.WriteLine("You must specify 2 numbers in format x y. Both `x` and `y` should be not less than 2");
            }
        }

        // create grid
        map = new bool[width, height];
        DrawGrid(map);
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
                Console.WriteLine("You should insert two numbers in format x y");
                loc[0] = -1;
                loc[1] = -1;
            }
            else if (loc[0] < 0 || loc[0] >= width || loc[1] < 0 || loc[1] >= height)
            {
                Console.WriteLine($"First coordinate should be less than {width}, second coordinate should be less than {height}");
            }
        }
        node = new Cell(loc[0], loc[1]);
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
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        for (int i = 0; i < map.GetLength(1); i++)
        {
            Console.Write($"{i}".PadRight(maxLenCol));
        }
        Console.ResetColor();
        Console.WriteLine();
        for (int i = 0; i < map.GetLength(0); i++)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"{i}".PadRight(maxLenRow));
            Console.ResetColor();
            for (int j = 0; j < map.GetLength(1); j++)
            {
                Console.Write(map[i, j] ? "X".PadRight(maxLenCol) : "0".PadRight(maxLenCol));
            }
            Console.WriteLine();
        }
    }


    // file save helper
    public static void DefineFileName(out string path)
    {
        path = string.Empty;
        while (true)
        {
            Console.WriteLine("Enter file name:");
            path = @"..\..\..\data\results\" + Console.ReadLine();
            var fileInfo = new FileInfo(path);
            if (fileInfo.Extension == ".txt")
                break;
            Console.WriteLine("The file must have the extension .txt");
        }
    }
}

