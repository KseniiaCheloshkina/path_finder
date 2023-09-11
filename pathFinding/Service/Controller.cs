using CompareSearchPath.Algorithms;
using CompareSearchPath.Common;
using CompareSearchPath.Models;
using Newtonsoft.Json;

namespace CompareSearchPath.Service;

public static class Controller
{
    private static GeneralDijkstra _dijkstra = new GeneralDijkstra();
    
    // метод ввода данных карты и стартовой и целевой точек
    public static void InputData()
    {
        var k = ".";

        var map = new bool[,] {};
        int n, m;
        var start = new Node();
        var end = new Node();

        while (k != "0")
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Выберите способ ввода данных:");
            Console.ResetColor();
            Console.WriteLine(" 1 - Ручной ввод");
            Console.WriteLine(" 2 - Чтение из файла");
            Console.WriteLine(" 3 - Сгенерировать карту");
            Console.WriteLine(" 4 - Сохранить карту");
            Console.WriteLine(" cls - Очистить консоль");
            Console.WriteLine(" 0 - Выход");
            k = Console.ReadLine()?.ToLower();
            
            switch (k)
            {
                case "1":
                    // сначала определеяем размерности матрицы
                    HelpInput.InputRangeMap(out n, out m);
                    map = new bool[n, m];
                    // выводим матрицу на консоль, чтобы пользователь мог взглянуть на нее
                    HelpInput.ShowMap(map);
                    // заполняем стены на карте
                    HelpInput.InputMap(map);
                    // выбор начальной и целевой точки
                    HelpInput.InputStartEnd(out  start, out end, n, m);
                    _dijkstra = new GeneralDijkstra(map, start, end);
                    break;
                case "2":
                    var codeOp = CodeOp.SuccessRead;
                    Console.Write("Введите название файла: ");
                    var fullPath = @"..\..\..\Data\JsonFiles\" + Console.ReadLine();

                    if (!File.Exists(fullPath))
                    {
                        Console.WriteLine("Файл не найден...");
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
                    var model = JsonConvert.DeserializeObject<JsonModel>(File.ReadAllText(fullPath), settings);
                    // если файл удалось прочитать
                    if (model != null)
                    {
                        _dijkstra = new GeneralDijkstra(model.Map, model.Start, model.End);
                    }
                    Console.ForegroundColor = codeOp == CodeOp.SuccessRead ? ConsoleColor.Green : ConsoleColor.DarkRed;
                    Console.WriteLine(codeOp.ToString());
                    Console.ResetColor();
                    break;
                case "3":
                    // определяем размерности матрицы
                    HelpInput.InputRangeMap(out n, out m);
                    // определяем процент заполнения стен
                    HelpInput.InputPercentWallMap(out var p);
                    var rnd = new Random();
                    var w = rnd.Next(0, n * m * p / 100);
                    map = new bool[n, m];
                    // случайно заполнение стенами карты
                    for (int i = 0; i < w; i++)
                    {
                        var _i = rnd.Next(0, n);
                        var _j = rnd.Next(0, m);
                        map[_i, _j] = true;
                    }
                    Console.WriteLine($"Размеры карты: {n} на {m}");
                    HelpInput.ShowMap(map);
                    HelpInput.InputStartEnd(out  start, out end, n, m);
                    _dijkstra = new GeneralDijkstra(map, start, end);
                    break;
                case "4":
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
                case "cls":
                    Console.Clear();
                    break;
                case "0":
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