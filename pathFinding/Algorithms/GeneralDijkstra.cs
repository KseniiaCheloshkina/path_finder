using CompareSearchPath.Models;

namespace CompareSearchPath.Algorithms;

public class GeneralDijkstra
{
    private readonly bool[,] _map;

    private readonly Node _start;

    private readonly Node _end;

    private Node? _path;

    private int _step = 0;

    public Action<string>? Action;

    public Node? Path => _path;

    public bool[,] Map => _map;
    public int N => _map.GetLength(0);
    public int M => _map.GetLength(1);
    public Node Start => _start;
    public Node End => _end;

    public bool IsEmptyMap => _map.GetLength(0) == 0 || _map.GetLength(1) == 0;

    public GeneralDijkstra()
    {
        _map = new bool[,] { };
        _start = new Node();
        _end = new Node();
        _path = null;
        Action = Console.Write;
        _step = 0;
    }
    
    public GeneralDijkstra(bool[,] map, Node start, Node end)
    {
        if (map.GetLength(0) == 0 || map.GetLength(1) == 0)
            throw new IndexOutOfRangeException("Карта пуста");
        if(start.X < 0 || start.Y < 0 || start.X >= map.GetLength(0) || start.Y >= map.GetLength(1))
            throw new IndexOutOfRangeException("Стартовая точка лежит вне диапазонах карты");
        
        if(end.X < 0 || end.Y < 0 || end.X >= map.GetLength(0) || end.Y >= map.GetLength(1))
            throw new IndexOutOfRangeException("Конечная точка лежит вне диапазонах карты");

        if (start.X == end.X && start.Y == end.Y)
            throw new Exception("Стартовая и конечная точки совпадают");
        
        _map = new bool[map.GetLength(0), map.GetLength(1)];
        for (int i = 0; i < map.GetLength(0); i++)
        {
            for (int j = 0; j < map.GetLength(1); j++)
            {
                _map[i, j] = map[i, j];
            }
        }
        _start = start;
        _end = end;

        _step = 0;

        _map[start.X, start.Y] = false;
        _map[end.X, end.Y] = false;
        
        _path = null;
        Action = Console.Write;
    }

    public void ResetAction() => Action = Console.Write;
    
    public void AlgorithmAStar()
    {
        FindPath(SortByF);
        WriteSolving();
    }
    
    public void AlgorithmDijkstra()
    {
        FindPath(SortByG);
        WriteSolving();
    }
    
    // основной путь нахождения пути
    private void FindPath(Func<Node, int> selector)
    {
        _step = 0;
        var open = new List<Node> { _start };
        var close = new List<Node>();

        while (open.Count > 0)
        {
            _step++;
            var curNode = open.MinBy(selector);

            if (curNode.X == _end.X && curNode.Y == _end.Y)
            {
                _path = curNode;
                return;
            }

            open.Remove(curNode);
            close.Add(curNode);

            foreach (var neighbour in GetNeighbours(curNode))
            {
                if (close.Any(item => item.X == neighbour.X && item.Y == neighbour.Y))
                    continue;

                var openNode = open.FirstOrDefault(item => item.X == neighbour.X && item.Y == neighbour.Y);

                if (openNode == null)
                {
                    neighbour.GetH = (Math.Abs(_end.X - neighbour.X) + Math.Abs(_end.Y - neighbour.Y)) * 10;
                    open.Add(neighbour);
                }
                else
                {
                    if (neighbour.GetG < openNode.GetG)
                    {
                        openNode.Parent = curNode;
                        openNode.GetG = neighbour.GetG;
                    }
                }
            }
        }
        _path = null;
    }

    // вспомогательный метод для нахождения пути
    private List<Node> GetNeighbours(Node cur)
    {
        var rows = _map.GetLength(0);
        var cols = _map.GetLength(1);
        
        var nodes = new List<Node>
        {
            new Node(cur.X - 1, cur.Y - 1, cur),
            new Node(cur.X, cur.Y - 1, cur),
            new Node(cur.X + 1, cur.Y - 1, cur),
            new Node(cur.X - 1, cur.Y, cur),
            new Node(cur.X + 1, cur.Y, cur),
            new Node(cur.X - 1, cur.Y + 1, cur),
            new Node(cur.X, cur.Y + 1, cur),
            new Node(cur.X + 1, cur.Y + 1, cur)
        };

        for (var i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].X < 0 || nodes[i].X >= rows || nodes[i].Y < 0 || nodes[i].Y >= cols ||
                _map[nodes[i].X, nodes[i].Y])
                nodes.RemoveAt(i--);
        }

        return nodes;
    }

    private static int SortByF(Node node) => node.GetF;
    private static int SortByG(Node node) => node.GetG;

    private void WriteSolving()
    {
        if(Action == null) return;
        
        var listNode = new List<Node>();

        if (_path == null)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Action?.Invoke("Путь не найден!\n");
            Console.ResetColor();
        }
        else
        {
            Action?.Invoke($"Количество итераций: {_step}\n" + $"Число ячеек: {_path.CountParent + 1}\n");
            Action?.Invoke($"Вес пути: {_path.GetF}\n");

            _path = _path.Parent;

            while (_path.X != _start.X || _path.Y != _start.Y)
            {
                listNode.Add(_path);
                _path = _path.Parent;
            }
        }

        var maxLenCol = _map.GetLength(1).ToString().Length + 1;
        var maxLenRow = _map.GetLength(0).ToString().Length + 1;

        if (maxLenCol == 1)
            maxLenCol++;
        if (maxLenRow == 1)
            maxLenRow++;

        Action?.Invoke(" ".PadRight(maxLenRow));
        Console.ForegroundColor = ConsoleColor.Blue;
        for (int i = 0; i < _map.GetLength(1); i++)
        {
            Action?.Invoke($"{i}".PadRight(maxLenCol));
        }
        Console.ResetColor();
        Action?.Invoke("\n");
        for (int i = 0; i < _map.GetLength(0); i++)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Action?.Invoke($"{i}".PadRight(maxLenRow));
            for (int j = 0; j < _map.GetLength(1); j++)
            {
                Console.ResetColor();
                var cell = listNode.FirstOrDefault(x => x.X == i && x.Y == j);
                if (i == _start.X && j == _start.Y)
                {
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Action?.Invoke("s".PadRight(maxLenCol));
                    continue;
                }
                if (i == _end.X && j == _end.Y)
                {
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Action?.Invoke("e".PadRight(maxLenCol));
                    continue;
                }
                if (cell != null)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Action?.Invoke("*".PadRight(maxLenCol));
                    listNode.Remove(cell);
                    continue;
                }
                Action?.Invoke(_map[i, j] ? "[]".PadRight(maxLenCol) : "0".PadRight(maxLenCol));
            }
            Action?.Invoke("\n");
        }
        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Action?.Invoke("s - start\n");
        Console.ForegroundColor = ConsoleColor.DarkBlue;
        Action?.Invoke("e - end\n");
        Console.ResetColor();
    }

}