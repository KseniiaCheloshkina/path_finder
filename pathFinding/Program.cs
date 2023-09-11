using CompareSearchPath.Service;

namespace CompareSearchPath;

class Program
{
    static void Main(string[] args)
    {
        var k = ".";

        while (k != "0")
        {
            // Ask for the user's favorite fruit
//            var fruit = AnsiConsole.Prompt(
//                new SelectionPrompt<string>()
//                    .Title("What's your [green]favorite fruit[/]?")
//                    .PageSize(10)
//                    .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
//                    .AddChoices(new[] {
//                        "Apple", "Apricot", "Avocado",
//                        "Banana", "Blackcurrant", "Blueberry",
//                        "Cherry", "Cloudberry", "Cocunut",
//                    }));
//
//            // Echo the fruit back to the terminal
//            AnsiConsole.WriteLine($"I agree. {fruit} is tasty!");

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Выберите операцию:");
            Console.ResetColor();
            Console.WriteLine(" 1 - Ввод данных");
            Console.WriteLine(" 2 - Найти решение");
            Console.WriteLine(" 3 - Help");
            Console.WriteLine(" 9 - Очистить консоль");
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
                case "9":
                    Console.Clear();
                    break;
            }
        }
    }
}