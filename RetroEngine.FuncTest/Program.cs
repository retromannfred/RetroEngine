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
            ChooseGame();

            // OPTION 2: Call directly your game to test
            // new TestPhysicsGame().Run();
        }

        static void ChooseGame()
        {
            Console.WriteLine();
            Console.WriteLine("      ****************************");
            Console.WriteLine("      *  ~ TEST GAME SELECTOR ~  *");
            Console.WriteLine("      ****************************");
            Console.WriteLine();

            Console.WriteLine("[ 1 ]: Test graphics engine along with ECS");
            Console.WriteLine("[ 2 ]: Test graphics engine performance");
            Console.WriteLine("[ 3 ]: Test sprite batcher without ECS");
            Console.WriteLine("[ 4 ]: Test collider system with masks.");

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
                case 3: new TestSpriteBatchWithoutECS().Run(); break;
                case 4: new TestCollisionsGame().Run(); break;
                default: return;
            }
        }
    }
}
