using RetroEngine.Core;
using RetroEngine.ECS;
using RetroEngine.ECS.Elements;
using RetroEngine.ECS.Managers;

namespace RetroEngine.FuncTest.ECS
{
    internal class TestSystem : UpdateSystem
    {
        public TestSystem()
            : base(Aspect.All<TestComponent>())
        {
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var entityId in ActiveEntities)
            {
                var test = World.GetComponent<TestComponent>(entityId);
            }
        }
    }
}
