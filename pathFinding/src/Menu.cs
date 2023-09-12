using Newtonsoft.Json;
using Spectre.Console;

namespace pathFinding.src;

public static class Menu
{
    private static Algoritms algoritm = new Algoritms();

    public static void MainMenu()
    {
        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an operation?")
                .PageSize(5)
                .AddChoices(new[] {
                    "Set data",
                    "Find a solution",
                    "Help",
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
                Console.WriteLine(File.ReadAllText(@"..\..\..\Data\Help.txt"));
                MainMenu();
                break;
        }
    }

    // метод ввода данных карты и стартовой и целевой точек
    public static void SetData()
    {
        var startCell = new Cell();
        var endCell = new Cell();
        var grid = new bool[,] { };
        int width, height;

        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an input type?")
                .PageSize(5)
                .AddChoices(new[] {
                        "Insert in stdin",
                        "Read from file",
                        "Save file",
                        "Back to Main"
                }));

        
        switch (operation)
        {
            // input from stdin
            case "Insert in stdin":
                FillGraph.GridCreation(out width, out height, out grid); // get input size
                                                                         // TODO: add walls on the map in ShowMap
                FillGraph.DefineWalls(grid); // create walls
                Console.WriteLine("Insert coordinates of start position in format `x y`");
                FillGraph.InputPoint(out startCell, width, height);  // create start position
                Console.WriteLine("Insert coordinates of end position in format `x y`");
                FillGraph.InputPoint(out endCell, width, height); // create end position
                algoritm = new Algoritms(grid, startCell, endCell);
                SetData();
                break;

            case "Read from file":
                string[] files = Directory.GetFiles(@"..\..\..\data\input_data\");
                string[] filesWithBack = new List<string>(files) { "Back" }.ToArray();
                var filename = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose an input type?")
                        .PageSize(10)
                        .AddChoices(filesWithBack));
                if (filename != "Back") 
                {
                    var model = JsonConvert.DeserializeObject<JsonModel>(File.ReadAllText(filename));

                    if (model != null)
                    {
                        var new_grid = new Grid(model);
                        var grid_matrix = new_grid.generate_grid();
                        algoritm = new Algoritms(grid_matrix, new Cell(model.start_node), new Cell(model.end_node));
                    }
                    Console.WriteLine($"File loaded {filename} \n");
                }
                MainMenu();
                break;

            case "Save file":
                if (algoritm.EmptyFlag)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("Сначала введите данные!");
                    Console.ResetColor();
                    break;
                }
                Console.Write("Введите название файла: ");
                var fName = Console.ReadLine();
                File.WriteAllText(@"..\..\..\Data\input_data\" + fName, JsonConvert.SerializeObject(new
                {
                    algoritm.width,
                    algoritm.height,
                    algoritm.GridMatrix,
                    algoritm.StartPos,
                    algoritm.EndPos
                }));
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Файл записан");
                Console.ResetColor();
                MainMenu();
                break;

            case "Back to Main":
                MainMenu();
                break;
        }
    }

    // метод выполнения решения двумя алгоритмами
    public static void FindSolution()
    {
        if (algoritm.EmptyFlag)
        {
            AnsiConsole.Markup("[maroon]Сначала введите данные![/]\n");
            MainMenu();
            return;
        }

        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an algoritm?")
                .PageSize(5)
                .AddChoices(new[] {
                    "Algo AStar",
                    "Algo Dijkstra",
                    "Save result to file",
                    "Back"
                }));

        switch (operation)
        {
            case "Algo AStar":
                algoritm.AlgoSearch(algoritm.Type["AStar"]);
                MainMenu();
                break;
            case "Algo Dijkstra":
                algoritm.AlgoSearch(algoritm.Type["Dijkstra"]);
                MainMenu();
                break;
            case "Save result to file":
                WriteInFile();
                break;
            case "Back":
                MainMenu();
                break;
        }
    }

    // метод записи решения в файл
    private static void WriteInFile()
    {
        var path = string.Empty;
        var bufferString = string.Empty;
        // меняем вывод результата на экран на запись результата в файл
        algoritm.Action = txt => { bufferString += txt; };

        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Какое решение записать:")
                .PageSize(5)
                .AddChoices(new[] {
                    "Algo AStar",
                    "Algo Dijkstra",
                    "Back"
                }));

        switch (operation)
        {
            case "Algo AStar":
                // вводим имя файла
                FillGraph.InputNameFile(out path);
                algoritm.AlgoSearch(algoritm.Type["AStar"]);
                File.WriteAllText(path, bufferString);
                AnsiConsole.Markup("[darkgreen]Файл записан[/]\n");
                break;
            case "Algo Dijkstra":
                FillGraph.InputNameFile(out path);
                algoritm.AlgoSearch(algoritm.Type["AStar"]);
                File.WriteAllText(path, bufferString);
                AnsiConsole.Markup("[darkgreen]Файл записан[/]\n");
                break;
        }
        // сбрасываем запись в файл на вывод на экран
        algoritm.ResetAction();
        MainMenu();
    }
}