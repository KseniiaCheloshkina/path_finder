using System.Diagnostics;
using Newtonsoft.Json;
namespace pathFinding.src;


public class LoadTesting
{
    public static void GenerateGridByParams(out bool[,] grid,out int[][] walls, int width, int height, int walls_percent)
    {
        grid = new bool[width, height];
        double n_walls = walls_percent * width * height / 100;
        int num_walls = Convert.ToInt32(n_walls);
        // generate coords and fill grid with walls
        var rnd = new Random();
        walls = new int[num_walls][];
        for (int i = 0; i < num_walls; i++)
        {
            int x = rnd.Next(0, width);
            int y = rnd.Next(0, height);
            grid[x, y] = true;
            walls[i] = new int[2] { x, y };
        }
    }

    public static int GenerateSolution(bool[,] grid, int[][] walls, string algo_name)
    {
        int width = grid.GetLength(0);
        int height = grid.GetLength(1);
        int[] start_pos = { 0, 0 };
        int[] end_pos = { width - 1, width - 1 };
        // run algo, algo_name - AStar, Dijkstra
        var algoritm = new Algoritms(walls, grid, new Cell(start_pos), new Cell(end_pos));
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        algoritm.AlgoSearch(algoritm.Type[algo_name]);
        stopwatch.Stop();
        TimeSpan stopwatchElapsed = stopwatch.Elapsed;
        int exec_time = Convert.ToInt32(stopwatchElapsed.TotalMilliseconds);
        return exec_time;
    }

    public static Dictionary<int, int> ChangeWallsPercent(string algo_name)
    {
        int width = 200;
        int height = 200;
        int walls_percent = 5;
        bool[,] grid;
        int[][] walls;
        Dictionary<int, int> record_time;
        GenerateGridByParams(out grid, out walls, width, height, walls_percent);
        int time_in_ms = GenerateSolution(grid, walls, algo_name);
        record_time = new Dictionary<int, int>()
        {
            { walls_percent, time_in_ms}
        };
        File.WriteAllText(@"..\..\..\data\results\change_percent.json", JsonConvert.SerializeObject(record_time));
        return record_time;
    }

}
