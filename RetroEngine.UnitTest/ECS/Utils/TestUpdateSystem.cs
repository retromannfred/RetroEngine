using RetroEngine.Core;
using RetroEngine.ECS.Elements;
using RetroEngine.ECS.Managers;

namespace RetroEngine.UnitTest.ECS.Utils
{
    internal class TestUpdateSystem : UpdateSystem
    {
        public TestUpdateSystem()
            : base(Aspect.All<TestUpdateComponent>()) { }

        public override void Update(GameTime gameTime)
        {
            var time = gameTime.ElapsedGameTime;

            foreach (var entity in ActiveEntities)
            {
                ref var comp = ref World.GetComponent<TestUpdateComponent>(entity);
                comp.Tag = $"Updated in {time.Hours}:{time.Minutes}:{time.Seconds}";
            }
        }
    }
}
