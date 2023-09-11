using Newtonsoft.Json;
using Spectre.Console;

namespace pathFinding.src;

public static class Menu
{
    private static Algoritms _dijkstra = new Algoritms();

    public static void MainMenu()
    {
        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an operation?")
                .PageSize(5)
                .AddChoices(new[] {
                    "Set data",
                    "Find a solution",
                    "Help"
                }));

        switch (operation)
        {
            case "Set data":
                SetData();
                break;
            case "Find a solution":
                FindSolution();
                break;
            case "Help":
                Console.WriteLine(File.ReadAllText(@"..\..\..\Data\Readme.txt"));
                break;
        }
    }

    // метод ввода данных карты и стартовой и целевой точек
    public static void SetData()
    {
        var k = ".";

        var grid = new bool[,] { };
        int width, height;
        var start = new Cell();
        var end = new Cell();


        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an input type?")
                .PageSize(5)
                .AddChoices(new[] {
                            "Insert in stdin",
                            "Read from file",
                            "Save file",
                            "Exit"
                }));

        switch (operation)
        {
            case "Set data":
                SetData();
                break;
            case "Find a solution":
                FindSolution();
                break;
            case "Help":
                Console.WriteLine(File.ReadAllText(@"..\..\..\Data\Readme.txt"));
                break;
        }
        switch (k)
        {
            // input from stdin
            case "man":
                FillGraph.InputRangeMap(out width, out height, out grid); // get input size
                                                                          // TODO: add walls on the map in ShowMap
                FillGraph.InputMap(grid); // create walls
                Console.WriteLine("Insert coordinates of start position in format `x y`");
                FillGraph.InputPoint(out start, width, height);  // create start position
                Console.WriteLine("Insert coordinates of end position in format `x y`");
                FillGraph.InputPoint(out end, width, height); // create end position
                _dijkstra = new Algoritms(grid, start, end);
                break;
            case "json":
                Console.WriteLine("Enter file name");
                var fullPath = @"..\..\..\data\input_data\" + Console.ReadLine();

                if (!File.Exists(fullPath))
                {
                    Console.WriteLine("No such file");
                    break;
                }

                var model = JsonConvert.DeserializeObject<JsonStructure>(File.ReadAllText(fullPath));

                if (model != null)
                {
                    var new_grid = new Grid(model);
                    var grid_matrix = new_grid.generate_grid();
                    _dijkstra = new Algoritms(grid_matrix, new Cell(model.start_node), new Cell(model.end_node));
                }
                Console.WriteLine("!Read successfuly\n");
                break;
            case "3":
                // определяем размерности матрицы
                FillGraph.InputRangeMap(out width, out height, out grid);
                // определяем процент заполнения стен
                FillGraph.InputPercentWallMap(out var p);
                var rnd = new Random();
                var w = rnd.Next(0, width * height * p / 100);
                grid = new bool[width, height];
                // случайно заполнение стенами карты
                for (int i = 0; i < w; i++)
                {
                    var _i = rnd.Next(0, width);
                    var _j = rnd.Next(0, height);
                    grid[_i, _j] = true;
                }
                Console.WriteLine($"Размеры карты: {width} на {height}");
                FillGraph.DrawGrid(grid);
                // HelpInput.InputStartEnd(out  start, out end, width, height);
                //_dijkstra = new GeneralDijkstra(map, start, end);
                break;
            case "save":
                if (_dijkstra.EmptyFlag)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Сначала введите данные!");
                    Console.ResetColor();
                    break;
                }
                Console.Write("Введите название файла: ");
                var fName = Console.ReadLine();
                File.WriteAllText(@"..\..\..\Data\JsonFiles" + fName, JsonConvert.SerializeObject(new
                {
                    _dijkstra.width,
                    _dijkstra.height,
                    _dijkstra.GridMatrix,
                    _dijkstra.StartPos,
                    _dijkstra.EndPos
                }));
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Файл записан");
                Console.ResetColor();
                break;
            // case "cls":
            //    Console.Clear();
            //    break;
            case "exit":
                Console.Clear();
                break;
        }




    }

    // метод выполнения решения двумя алгоритмами
    public static void FindSolution()
    {
        if (_dijkstra.EmptyFlag)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Сначала введите данные!");
            Console.ResetColor();
            return;
        }

        var k = ".";

        while (k != "0")
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Выберите алгоритм решения:");
            Console.ResetColor();
            Console.WriteLine(" 1 - Алгоритим A*");
            Console.WriteLine(" 2 - Алгоритм Дейкстра");
            Console.WriteLine(" 3 - Записать решение в файл");
            Console.WriteLine(" cls - Очистка");
            Console.WriteLine(" 0 - Выход");
            k = Console.ReadLine()?.ToLower();

            switch (k)
            {
                case "1":
                    _dijkstra.AStarAlgo();
                    break;
                case "2":
                    _dijkstra.DijkstraAlgo();
                    break;
                case "3":
                    WriteInFile();
                    break;
                case "cls":
                    Console.Clear();
                    break;
            }
        }
    }

    // метод записи решения в файл
    private static void WriteInFile()
    {
        var k = ".";
        var path = string.Empty;
        var bufferString = string.Empty;
        // меняем вывод результата на экран на запись результата в файл
        _dijkstra.Action = txt => { bufferString += txt; };

        while (k != "0")
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Какое решение записать:");
            Console.ResetColor();
            Console.WriteLine(" 1 - Алгоритим A*");
            Console.WriteLine(" 2 - Алгоритм Дейкстра");
            Console.WriteLine(" cls - Очистка");
            Console.WriteLine(" 0 - Выход");
            k = Console.ReadLine()?.ToLower();

            switch (k)
            {
                case "1":
                    // вводим имя файла
                    FillGraph.InputNameFile(out path);
                    _dijkstra.AStarAlgo();
                    File.WriteAllText(path, bufferString);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Файл записан");
                    Console.ResetColor();
                    break;
                case "2":
                    FillGraph.InputNameFile(out path);
                    _dijkstra.DijkstraAlgo();
                    File.WriteAllText(path, bufferString);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Файл записан");
                    Console.ResetColor();
                    break;
                case "cls":
                    Console.Clear();
                    break;
            }
        }
        // сбрасываем запись в файл на вывод на экран
        _dijkstra.ResetAction();
    }
}