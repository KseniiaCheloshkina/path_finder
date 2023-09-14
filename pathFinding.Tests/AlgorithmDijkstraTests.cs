﻿using pathFinding.src;

namespace PathFinding.Tests;

public class AlgorithmDijkstraTests
{
    private Algoritms _dijkstra;
    private string _buffer;
    
    private void Setup(Grid grid, Cell start, Cell end)
    {
        _buffer = string.Empty;
        _dijkstra = new Algoritms(grid, start, end)
        {
            Action = txt => { _buffer += txt; }
        };
        _dijkstra.AlgoSearch(_dijkstra.Type["Dijkstra"]);
    }

    // корреткные данные, путь найден
    [Test]
    public void Correct_Map_And_Start_And_End_Path_Found()
    {
            // { false, false, false },
            // { true, true, false },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(2, 0);
        var expect = "Количество итераций: 6\n" +
                     "Число ячеек: 5\n" +
                     "Вес пути: 48\n" +
                     "  0 1 2 \n" +
                     "0 s * 0 \n" +
                     "1 [][]* \n" +
                     "2 e * []\n" +
                     "s - start\n" +
                     "e - end\n";
        // act
        Setup(grid, start, end);
        
        // assert
        Assert.AreEqual(expect, _buffer);
    }
    
    // корреткные данные, путь не найден
    [Test]
    public void Correct_Map_And_Start_And_End_Path_Not_Found()
    {
            // { false, false, false },
            // { true, true, true },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{1,2},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(2, 0);
        var expect = "Путь не найден!\n" +
                     "  0 1 2 \n" +
                     "0 s 0 0 \n" +
                     "1 [][][]\n" +
                     "2 e 0 []\n" +
                     "s - start\n" +
                     "e - end\n";

        Setup(grid, start, end);
        
        Assert.AreEqual(expect, _buffer);
    }
    
    // корреткные данные, стартовая и целевая точки - соседи
    [Test]
    public void Correct_Map_And_Start_Neighbouring_End()
    {
            // { false, false, false },
            // { true, true, false },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(0, 1);
        
        Setup(grid, start, end);
    }
    
    // некорреткные данные, стартовая и целевая точки равны
    [Test]
    public void Correct_Map_And_Start_Equal_End()
    {
            // { false, false, false },
            // { true, true, false },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(0, 0);
        
        var exception = Assert.Throws<Exception>(() => new Algoritms(grid, start, end));
        Assert.AreEqual("Стартовая и конечная точки совпадают", exception.Message);
    }
    
    // некорреткные данные, карта пуста
    [Test]
    public void Map_Is_Empty()
    {
        var grid = new Grid();
        var start = new Cell(0, 0);
        var end = new Cell(2, 0);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => new Algoritms(grid, start, end));
        Assert.AreEqual("Карта пуста", exception.Message);
    }
    
    // некорреткные данные, стартовая точка вне карты
    [Test]
    public void Correct_Map_And_Incorrect_Start()
    {
            // { false, false, false },
            // { true, true, false },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 3);
        var end = new Cell(2, 0);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => new Algoritms(grid, start, end));
        Assert.AreEqual("Стартовая точка лежит вне диапазонах карты", exception.Message);
    }
    
    // некорреткные данные, конечная точка вне карты
    [Test]
    public void Correct_Map_And_Incorrect_End()
    {
            // { false, false, false },
            // { true, true, false },
            // { false, false, true }
        
        int[,] walls = {{0,1},{1,1},{2,2}};
        var grid = new Grid(3,3,walls);

        var start = new Cell(0, 0);
        var end = new Cell(2, 3);
        
        var exception = Assert.Throws<IndexOutOfRangeException>(() => new Algoritms(grid, start, end));
        Assert.AreEqual("Конечная точка лежит вне диапазонах карты", exception.Message);
    }
}