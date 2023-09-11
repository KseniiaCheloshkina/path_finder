using CompareSearchPath.Algorithms;
using CompareSearchPath.Common;
using CompareSearchPath.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CompareSearchPath.Service;

public static class Controller
{
    private static GeneralDijkstra _dijkstra = new GeneralDijkstra();
    
    // метод ввода данных карты и стартовой и целевой точек
    public static void InputData()
    {
        var k = ".";

        var grid = new bool[,] {};
        int width, height;
        var start = new Node();
        var end = new Node();

        while (k != "0")
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Choose input type:");
            Console.ResetColor();
            Console.WriteLine(" man - insert in stdin");
            Console.WriteLine(" json - read from file");
            // Console.WriteLine(" 3 - Сгенерировать карту");
            Console.WriteLine(" save - save input map");
            // Console.WriteLine(" cls - Очистить консоль");
            Console.WriteLine(" exit - exit the program");
            k = Console.ReadLine()?.ToLower();
            
            switch (k)
            {
                // input from stdin
                case "man":
                    HelpInput.InputRangeMap(out width, out height, out grid); // get input size
                    // TODO: add walls on the map in ShowMap
                    HelpInput.InputMap(grid); // create walls
                    Console.WriteLine("Insert coordinates of start position in format `x y`");
                    HelpInput.InputPoint(out start, width, height);  // create start position
                    Console.WriteLine("Insert coordinates of end position in format `x y`");
                    HelpInput.InputPoint(out end, width, height); // create end position
                    _dijkstra = new GeneralDijkstra(grid, start, end);
                    break;
                case "json":
                    var codeOp = CodeOp.SuccessRead;
                    Console.WriteLine("Enter file name");
                    var fullPath = @"..\..\..\data\input_data\" + Console.ReadLine();

                    if (!File.Exists(fullPath))
                    {
                        Console.WriteLine("No such file");
                        break;
                    }

                    // настройки для проверки возможности чтения файла

                    var settings = new JsonSerializerSettings
                    {
                        Error = (sender, e) =>
                        {
                            codeOp = CodeOp.FailedReadFile;
                            e.ErrorContext.Handled = true;
                        }
                    };
                    var model = JsonConvert.DeserializeObject<JsonStructure>(File.ReadAllText(fullPath), settings);
                    Console.WriteLine(model);
                    // MY
                    //var json_content= File.ReadAllText(fullPath);
                    //JObject resJson = JObject.Parse(json_content);
                    //Console.WriteLine(resJson);
                    //Console.WriteLine(resJson["grid_size"]);
                    //var grid_size = resJson["grid_size"].ToObject<List<int>>();
                    //Console.WriteLine(grid_size);
                    //JArray parsed_grid = JArray.Parse(grid_size);
                    //Console.WriteLine(parsed_grid);
                    //var walls = resJson["walls"];

                    // OLD
                    //if (model != null)
                    //{
                    //    _dijkstra = new GeneralDijkstra(model.Map, model.Start, model.End);
                    //}
                    //Console.ForegroundColor = codeOp == CodeOp.SuccessRead ? ConsoleColor.Green : ConsoleColor.DarkRed;
                    //Console.WriteLine(codeOp.ToString());
                    //Console.ResetColor();
                    break;
                case "3":
                    // определяем размерности матрицы
                    HelpInput.InputRangeMap(out width, out height, out grid);
                    // определяем процент заполнения стен
                    HelpInput.InputPercentWallMap(out var p);
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
                    HelpInput.DrawGrid(grid);
                    // HelpInput.InputStartEnd(out  start, out end, width, height);
                    //_dijkstra = new GeneralDijkstra(map, start, end);
                    break;
                case "save":
                    if (_dijkstra.IsEmptyMap)
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
                        _dijkstra.N,
                        _dijkstra.M,
                        _dijkstra.Map,
                        _dijkstra.Start,
                        _dijkstra.End
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
    }

    // метод выполнения решения двумя алгоритмами
    public static void ExecuteProgram()
    {                    
        if (_dijkstra.IsEmptyMap)
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
                    _dijkstra.AlgorithmAStar();
                    break;
                case "2":
                    _dijkstra.AlgorithmDijkstra();
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
                    HelpInput.InputNameFile(out path);
                    _dijkstra.AlgorithmAStar();
                    File.WriteAllText(path, bufferString);
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Файл записан");
                    Console.ResetColor();
                    break;
                case "2":
                    HelpInput.InputNameFile(out path);
                    _dijkstra.AlgorithmDijkstra();
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