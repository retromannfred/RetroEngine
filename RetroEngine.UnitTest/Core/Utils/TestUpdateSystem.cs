using RetroEngine.Graphics;
using RetroEngine.Core.Elements;
using RetroEngine.Core.Managers;

namespace RetroEngine.UnitTest.Core.Utils
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
