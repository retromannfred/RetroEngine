using RetroEngine.FuncTest.Games;

namespace RetroEngine.FuncTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var game = new TestMultipleBatchesGame();
            game.Run();
        }
    }
}