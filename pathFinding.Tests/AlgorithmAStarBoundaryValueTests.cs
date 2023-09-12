using pathFinding.src;

namespace PathFinding.Tests;

public class AlgorithmAStarBoundaryValueTests
{
    private Algoritms algo;
    private string result;

    private void Setup(Cell start, Cell end)
    {
        result = string.Empty;
        int[,] walls =  
        {
            { 0, 1 },
            { 2, 4}
        };

        var grid = Grid.static_generate_grid(5,5,walls);
        algo = new Algoritms(walls, grid, start, end)
        {
            Action = txt => { result += txt; }
        };
        algo.AlgoSearch(algo.Type["AStar"]);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_To_Up_Range()
    {
        var start = new Cell(0, 1);
        var end = new Cell(1, 1);
        
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
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_To_Left_Range()
    {
        var start = new Cell(1, 0);
        var end = new Cell(1, 1);
        
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
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_To_Down_Range()
    {
        var start = new Cell(2, 1);
        var end = new Cell(1, 1);
        
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
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_To_Right_Range()
    {
        var start = new Cell(1, 2);
        var end = new Cell(1, 1);
        
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
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_To_Up_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(0, 1);
        
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
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_To_Left_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, 0);
        
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
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_To_Down_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(2, 1);
        
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
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_To_Right_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, 2);
        
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
        
        Assert.AreEqual(expect, result);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_Out_Up_Range()
    {
        var start = new Cell(-1, 1);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Стартовая точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_Out_Left_Range()
    {
        var start = new Cell(1, -1);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Стартовая точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_X_Of_Start_Node_Close_Out_Down_Range()
    {
        var start = new Cell(3, 1);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Стартовая точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_Start_Node_Close_Out_Right_Range()
    {
        var start = new Cell(1, 3);
        var end = new Cell(1, 2);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Стартовая точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_Out_Up_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(-1, 1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Конечная точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_Out_Left_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, -1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Конечная точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_X_Of_End_Node_Close_Out_Down_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(3, 1);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Конечная точка лежит вне диапазонах карты", exception.Message);
    }

    [Test]
    public void Check_AStar_With_Y_Of_End_Node_Close_Out_Right_Range()
    {
        var start = new Cell(1, 1);
        var end = new Cell(1, 3);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => Setup(start, end));
        Assert.AreEqual("Конечная точка лежит вне диапазонах карты", exception.Message);
    }
}