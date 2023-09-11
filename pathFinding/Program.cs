using CompareSearchPath.Service;

namespace CompareSearchPath;

class Program
{
    static void Main(string[] args)
    {
        var k = ".";

        while (k != "0")
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Выберите операцию:");
            Console.ResetColor();
            Console.WriteLine(" 1 - Ввод данных");
            Console.WriteLine(" 2 - Найти решение");
            Console.WriteLine(" 3 - Файл Readme");
            Console.WriteLine(" cls - Очистить консоль");
            Console.WriteLine(" 0 - Выход из приложения");
            k = Console.ReadLine()?.ToLower();

            switch (k)
            {
                case "1":
                    Controller.InputData();
                    break;
                case "2":
                    Controller.ExecuteProgram();
                    break;
                case "3":
                    Console.WriteLine(File.ReadAllText(@"..\..\..\Data\Readme.txt"));
                    break;
                case "cls":
                    Console.Clear();
                    break;
            }
        }
    }
}