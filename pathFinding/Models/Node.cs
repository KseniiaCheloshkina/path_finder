namespace CompareSearchPath.Models;

public class Node
{
    private int _cntParent;
    public int X { get; set; }
    
    public int Y { get; set; }
    
    // Расстояние от старта до точки
    public int GetG {get; set; }
    
    // Примерное расстояние от точки до конечной точки
    public int GetH { get; set; }

    // Вес точки
    public int GetF => GetG + GetH;

    public int CountParent => _cntParent;

    public Node Parent;

    public Node() { }

    public Node(int[] position)
    {
        X = position[0];
        Y = position[1];

        GetG = GetH = _cntParent = 0;

        Parent = null;
    }
    
    public Node(int x, int y)
    {
        X = x;
        Y = y;

        GetG = GetH = _cntParent = 0;

        Parent = null;
    }
    
    public Node(int x, int y, Node parent)
    {
        X = x;
        Y = y;

        if (x == parent.X || y == parent.Y)
            GetG = parent.GetG + 10;
        else
            GetG = parent.GetG + 14;

        Parent = parent;
        _cntParent = Parent._cntParent + 1;
    }

    /*public static bool operator ==(Node node1, Node node2)
    {
        return node1.X == node2.X && node1.Y == node2.Y;
    }

    public static bool operator !=(Node node1, Node node2)
    {
        return !(node1 == node2);
    }*/
}