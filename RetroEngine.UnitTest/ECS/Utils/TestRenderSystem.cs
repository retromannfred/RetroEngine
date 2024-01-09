using RetroEngine.Core;
using RetroEngine.ECS.Elements;
using RetroEngine.ECS.Managers;

namespace RetroEngine.UnitTest.ECS.Utils
{
    internal class TestRenderSystem : RenderSystem
    {
        public TestRenderSystem()
            : base(Aspect.All<TestRenderComponent>()) { }

        public override void Render(GameTime gameTime)
        {
            var time = gameTime.ElapsedGameTime;

            foreach (var entity in ActiveEntities)
            {
                World.GetComponent<TestRenderComponent>(entity)
                    .Tag = $"Rendered in {time.Hours}:{time.Minutes}:{time.Seconds}";
            }
        }
    }
}
