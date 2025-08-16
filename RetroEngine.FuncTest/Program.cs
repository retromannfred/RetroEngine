using RetroEngine.Core;
using RetroEngine.FuncTest.Games;
using System.Reflection;

namespace RetroEngine.FuncTest
{
    internal class Program
    {
        static void Main()
        {
            // OPTION 1: Choose your game to test
            // ChooseGame();

            // OPTION 2: Call directly your game to test
            new TestGraphicPerformanceGame().Run();
        }

        static void ChooseGame()
        {
            Console.WriteLine();
            Console.WriteLine("      ****************************");
            Console.WriteLine("      *  ~ TEST GAME SELECTOR ~  *");
            Console.WriteLine("      ****************************");
            Console.WriteLine();

            Console.WriteLine("[ 1 ]: Test graphics game");

            Console.WriteLine();
            Console.WriteLine("[ 0 ]: Exit");
            Console.WriteLine();
            Console.WriteLine();

            Console.Write("Choose game to test: ");
            if (int.TryParse(Console.ReadLine(), out int option) == false)
                return;

            switch (option)
            {
                case 0: return;
                case 1: new TestGraphicEngineGame().Run(); break;
                case 2: new TestGraphicPerformanceGame().Run(); break;
                default: return;
            }
        }
    }
}
