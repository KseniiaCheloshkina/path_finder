using System.Diagnostics;
using Newtonsoft.Json;
namespace pathFinding.src;


public class LoadTesting
{
    public static void GenerateGridByParams(out bool[,] grid, out int[,] walls, int width, int height, int walls_percent)
    {
        grid = new bool[width, height];
        double n_walls = walls_percent * width * height / 100;
        int num_walls = Convert.ToInt32(n_walls);
        // generate coords and fill grid with walls
        var rnd = new Random();

        walls = new int[num_walls,2];
        for (int i = 0; i < num_walls; i++)
        {
            int x = rnd.Next(0, width);
            int y = rnd.Next(0, height);
            grid[x, y] = true;
            walls[i,0] = x;
            walls[i,1] = y;
        }
    }

    public static int GenerateSolution(bool[,] grid, int[,] walls, string algo_name)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        int[] start_pos = { 0, 0 };
        int[] end_pos = { width - 1, width - 1 };
        // run algo
        var algoritm = new Algoritms(walls, grid, new Cell(start_pos), new Cell(end_pos));
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        algoritm.AlgoSearch(algoritm.Type[algo_name]);
        stopwatch.Stop();
        TimeSpan stopwatchElapsed = stopwatch.Elapsed;
        int exec_time = Convert.ToInt32(stopwatchElapsed.TotalMilliseconds);
        return exec_time;
    }
    // варьируя процент стен считаем время 2х алгоритмов
    public static Dictionary<int, Dictionary<string, int>> ChangeWallsPercent()
    {
        int width = 100;
        int height = 100;
        bool[,] grid;
        int[,] walls;
        int[] start_pos = { 0, 0 };
        int[] end_pos = { width - 1, width - 1 };
        Dictionary<int, Dictionary<string,int>> record_time = new Dictionary<int, Dictionary<string, int>>();
        // percent numbers - untill 50%
        IEnumerable<int> all_wall_percents = Enumerable.Range(0, 50);
        foreach (int walls_percent in all_wall_percents)
        {
            // generate grid
            GenerateGridByParams(out grid, out walls, width, height, walls_percent);
            // save dataset
            var algoritm = new Algoritms(walls, grid, new Cell(start_pos), new Cell(end_pos));
            string fName = Convert.ToString(walls_percent);
            File.WriteAllText(Menu.FilePath("load_testing",$"changing_percent_{fName}.json"), JsonConvert.SerializeObject(new
            {
                algoritm.grid_size,
                algoritm.walls,
                algoritm.start_node,
                algoritm.end_node
            }));
            // run 2 algos
            int time_in_ms_astar = GenerateSolution(grid, walls, "AStar");
            int time_in_ms_dijkstra = GenerateSolution(grid, walls, "Dijkstra");
            Dictionary<string, int> cur_res = new Dictionary<string, int> {
                {"AStar", time_in_ms_astar },
                {"Dijkstra", time_in_ms_dijkstra}
             };
            record_time.Add(walls_percent, cur_res);   
            Console.WriteLine(walls_percent);
        }
        return record_time;
    }

    // варьируя размер матрицы считаем время 2х алгоритмов
    public static Dictionary<int, Dictionary<string, int>> ChangeGridSize()
    {
        int walls_percent = 20;
        bool[,] grid;
        int[,] walls;
        int[] start_pos = { 0, 0 };
        Dictionary<int, Dictionary<string, int>> record_time = new Dictionary<int, Dictionary<string, int>>();
        // width = height
        IEnumerable<int> all_widths = Enumerable.Range(1, 20);
        int multiplier = 10;
        foreach (int width in all_widths)
        {
            int height = width * multiplier;
            int[] end_pos = { width * multiplier - 1, width * multiplier - 1 };
            // generate grid
            GenerateGridByParams(out grid, out walls, width* multiplier, height, walls_percent);
            // save dataset
            var algoritm = new Algoritms(walls, grid, new Cell(start_pos), new Cell(end_pos));
            string fName = Convert.ToString(width * multiplier);
            File.WriteAllText(Menu.FilePath("load_testing",$"changing_width_{fName}.json"), JsonConvert.SerializeObject(new
            {
                algoritm.grid_size,
                algoritm.walls,
                algoritm.start_node,
                algoritm.end_node
            }));
            // run 2 algos
            int time_in_ms_astar = GenerateSolution(grid, walls, "AStar");
            int time_in_ms_dijkstra = GenerateSolution(grid, walls, "Dijkstra");
            Dictionary<string, int> cur_res = new Dictionary<string, int> {
                {"AStar", time_in_ms_astar },
                {"Dijkstra", time_in_ms_dijkstra}
             };
            record_time.Add(width * multiplier, cur_res);
            Console.WriteLine(width * multiplier);
        }
        return record_time;
    }

}
