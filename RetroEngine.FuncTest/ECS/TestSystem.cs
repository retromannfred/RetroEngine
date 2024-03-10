using RetroEngine.Graphics;
using RetroEngine.Core;
using RetroEngine.Core.Elements;
using RetroEngine.Core.Mapping;

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
