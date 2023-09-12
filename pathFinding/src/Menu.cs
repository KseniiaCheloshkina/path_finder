using Newtonsoft.Json;
using Spectre.Console;

namespace pathFinding.src;

public static class Menu
{
    private static Algoritms algoritm = new Algoritms();

    public static void MainMenu()
    {
        var highlightStyle = new Style().Foreground(Color.Purple);
        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an operation?")
                .PageSize(5)
                .HighlightStyle(highlightStyle)
                .AddChoices(new[] {
                    "Set data",
                    "Find a solution",
                    "Help",
                    "Load testing",
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
            case "Load testing":
                RunLoadingTests();
                MainMenu();
                break;
        }
    }

    // define input grid and condiotions
    public static void SetData()
    {
        var startCell = new Cell();
        var endCell = new Cell();
        var grid = new bool[,] { };
        int width, height;
        int[,] walls;

        var highlightStyle = new Style().Foreground(Color.Purple);
        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an input type?")
                .PageSize(5)
                .HighlightStyle(highlightStyle)
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
                walls = FillGraph.DefineWalls(grid); // create walls
                Console.WriteLine("Insert coordinates of start position in format x y");
                FillGraph.InputPoint(out startCell, width, height);  // create start position
                Console.WriteLine("Insert coordinates of end position in format x y");
                FillGraph.InputPoint(out endCell, width, height); // create end position
                algoritm = new Algoritms(walls, grid, startCell, endCell);
                SetData();
                break;

            case "Read from file":
 
                string[] files = Directory.GetFiles(@"..\..\..\data\input_data\");
                string[] filesWithBack = new List<string>(files) { "Back" }.ToArray();
                var filename = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Choose an input type?")
                        .PageSize(10)
                        .HighlightStyle(highlightStyle)
                        .AddChoices(filesWithBack));
                if (filename != "Back")
                {
                    var model = JsonConvert.DeserializeObject<JsonModel>(File.ReadAllText(filename));

                    if (model != null)
                    {
                        var new_grid = new Grid(model);
                        algoritm = new Algoritms(new_grid, new Cell(model.start_node), new Cell(model.end_node));
                    }
                    Console.WriteLine($"File loaded {filename} \n");
                }
                MainMenu();
                break;

            case "Save file":
                if (algoritm.EmptyFlag)
                {
                    Console.WriteLine("No data found");
                    break;
                }
                Console.Write("Insert name of file with extention (1.json): ");
                var fName = Console.ReadLine();
                File.WriteAllText(@"..\..\..\data\input_data\" + fName, JsonConvert.SerializeObject(new
                {
                    algoritm.grid_size,
                    algoritm.walls,
                    algoritm.start_node,
                    algoritm.end_node
                }));
                Console.WriteLine("File is saved");
                MainMenu();
                break;

            case "Back to Main":
                MainMenu();
                break;
        }
    }

    // solution
    public static void FindSolution()
    {
        if (algoritm.EmptyFlag)
        {
            AnsiConsole.Markup("[maroon]You have not entered data![/]\n");
            MainMenu();
            return;
        }
        var highlightStyle = new Style().Foreground(Color.Purple);
        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose an algoritm?")
                .PageSize(5)
                .HighlightStyle(highlightStyle)
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

    // save into file
    private static void WriteInFile()
    {
        var path = string.Empty;
        var bufferString = string.Empty;
        // меняем вывод результата на экран на запись результата в файл
        algoritm.Action = txt => { bufferString += txt; };

        var highlightStyle = new Style().Foreground(Color.Purple);
        var operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Which solution to write down:")
                .PageSize(5)
                .HighlightStyle(highlightStyle)
                .AddChoices(new[] {
                    "Algo AStar",
                    "Algo Dijkstra",
                    "Back"
                }));

        switch (operation)
        {
            case "Algo AStar":
                // вводим имя файла
                FillGraph.DefineFileName(out path);
                algoritm.AlgoSearch(algoritm.Type["AStar"]);
                File.WriteAllText(path, bufferString);
                AnsiConsole.Markup("[darkgreen]File recorded[/]\n");
                break;
            case "Algo Dijkstra":
                FillGraph.DefineFileName(out path);
                algoritm.AlgoSearch(algoritm.Type["AStar"]);
                File.WriteAllText(path, bufferString);
                AnsiConsole.Markup("[darkgreen]The file was not written[/]\n");
                break;
        }
        // сбрасываем запись в файл на вывод на экран
        algoritm.ResetAction();
        MainMenu();
    }

    // load testing
    public static void RunLoadingTests()
    {
        //int width = 5;
        //int height = 6;
        //int walls_percent = 70;
        //string algo_name = "AStar";
        //bool[,] grid;
        //int[][] walls;
        //LoadTesting.GenerateGridByParams(out grid, out walls, width, height, walls_percent);
        //int time_in_ms = LoadTesting.GenerateSolution(grid, walls, algo_name);
        //Console.Write("Total execution time in ms: ");
        //Console.WriteLine(time_in_ms);
        // Dictionary<int, int> result = LoadTesting.ChangeWallsPercent("AStar");
        // Console.WriteLine(result);
    }
}