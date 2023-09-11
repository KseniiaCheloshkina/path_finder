using CompareSearchPath.Algorithms;
using CompareSearchPath.Models;

namespace CompareSearchPath.Tests;

public class AlgorithmAStarBoundaryValueTests
{
    private GeneralDijkstra _dijkstra;
    private string _buffer;

    private void Setup(Node start, Node end)
    {
        _buffer = string.Empty;
        var map = new bool[,]
        {
            { false, false, false },
            { true, true, false },
            { false, false, true }
        };
        _dijkstra = new GeneralDijkstra(map, start, end)
        {
            Action = txt => { _buffer += txt; }
        };
        _dijkstra.AlgorithmAStar();
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_To_Up_Range()
    {
        var start = new Node(0, 1);
        var end = new Node(1, 1);
        
        var expect = "Количество итераций: 2\n" +
                     "Число ячеек: 2\n" +
                     "Вес пути: 10\n" +
                     "  0 1 2 \n" +
                     "0 0 s 0 \n" +
                     "1 []e 0 \n" +
                     "2 0 0 []\n" +
                     "s - start\n" +
                     "e - end\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_To_Left_Range()
    {
        var start = new Node(1, 0);
        var end = new Node(1, 1);
        
        var expect = "Количество итераций: 2\n" +
                     "Число ячеек: 2\n" +
                     "Вес пути: 10\n" +
                     "  0 1 2 \n" +
                     "0 0 0 0 \n" +
                     "1 s e 0 \n" +
                     "2 0 0 []\n" +
                     "s - start\n" +
                     "e - end\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_To_Down_Range()
    {
        var start = new Node(2, 1);
        var end = new Node(1, 1);
        
        var expect = "Количество итераций: 2\n" +
                     "Число ячеек: 2\n" +
                     "Вес пути: 10\n" +
                     "  0 1 2 \n" +
                     "0 0 0 0 \n" +
                     "1 []e 0 \n" +
                     "2 0 s []\n" +
                     "s - start\n" +
                     "e - end\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_To_Right_Range()
    {
        var start = new Node(1, 2);
        var end = new Node(1, 1);
        
        var expect = "Количество итераций: 2\n" +
                     "Число ячеек: 2\n" +
                     "Вес пути: 10\n" +
                     "  0 1 2 \n" +
                     "0 0 0 0 \n" +
                     "1 []e s \n" +
                     "2 0 0 []\n" +
                     "s - start\n" +
                     "e - end\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_To_Up_Range()
    {
        var start = new Node(1, 1);
        var end = new Node(0, 1);
        
        var expect = "Количество итераций: 2\n" +
                     "Число ячеек: 2\n" +
                     "Вес пути: 10\n" +
                     "  0 1 2 \n" +
                     "0 0 e 0 \n" +
                     "1 []s 0 \n" +
                     "2 0 0 []\n" +
                     "s - start\n" +
                     "e - end\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_To_Left_Range()
    {
        var start = new Node(1, 1);
        var end = new Node(1, 0);
        
        var expect = "Количество итераций: 2\n" +
                     "Число ячеек: 2\n" +
                     "Вес пути: 10\n" +
                     "  0 1 2 \n" +
                     "0 0 0 0 \n" +
                     "1 e s 0 \n" +
                     "2 0 0 []\n" +
                     "s - start\n" +
                     "e - end\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_To_Down_Range()
    {
        var start = new Node(1, 1);
        var end = new Node(2, 1);
        
        var expect = "Количество итераций: 2\n" +
                     "Число ячеек: 2\n" +
                     "Вес пути: 10\n" +
                     "  0 1 2 \n" +
                     "0 0 0 0 \n" +
                     "1 []s 0 \n" +
                     "2 0 e []\n" +
                     "s - start\n" +
                     "e - end\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_To_Right_Range()
    {
        var start = new Node(1, 1);
        var end = new Node(1, 2);
        
        var expect = "Количество итераций: 2\n" +
                     "Число ячеек: 2\n" +
                     "Вес пути: 10\n" +
                     "  0 1 2 \n" +
                     "0 0 0 0 \n" +
                     "1 []s e \n" +
                     "2 0 0 []\n" +
                     "s - start\n" +
                     "e - end\n";
        Setup(start, end);
        
        Assert.AreEqual(expect, _buffer);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_Out_Up_Range()
    {
        var start = new Node(-1, 1);
        var end = new Node(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Стартовая точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_Out_Left_Range()
    {
        var start = new Node(1, -1);
        var end = new Node(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Стартовая точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_Out_Down_Range()
    {
        var start = new Node(3, 1);
        var end = new Node(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Стартовая точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_Out_Right_Range()
    {
        var start = new Node(1, 3);
        var end = new Node(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Стартовая точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_Out_Up_Range()
    {
        var start = new Node(1, 1);
        var end = new Node(-1, 1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Конечная точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_Out_Left_Range()
    {
        var start = new Node(1, 1);
        var end = new Node(1, -1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Конечная точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_Out_Down_Range()
    {
        var start = new Node(1, 1);
        var end = new Node(3, 1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Конечная точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_Out_Right_Range()
    {
        var start = new Node(1, 1);
        var end = new Node(1, 3);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Конечная точка лежит вне диапазонах карты", exception.Message);
    }
}