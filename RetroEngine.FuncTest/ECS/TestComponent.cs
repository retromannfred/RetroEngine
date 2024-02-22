using RetroEngine.Core.Elements;

namespace RetroEngine.FuncTest.ECS
{
    internal struct TestComponent : IComponent
    {
        private static int _maxId = 0;

        public int Id { get; set; }

        public TestComponent()
        {
            Id = ++_maxId;
        }
    }
}
