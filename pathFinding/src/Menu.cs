﻿using Newtonsoft.Json;
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
        int[][] walls;

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
                FillGraph.DefineWalls(out walls, grid); // create walls
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
                        var grid_matrix = new_grid.generate_grid();
                        algoritm = new Algoritms(model.walls, grid_matrix, new Cell(model.start_node), new Cell(model.end_node));
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

    // метод выполнения решения двумя алгоритмами
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

    // метод записи решения в файл
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
}