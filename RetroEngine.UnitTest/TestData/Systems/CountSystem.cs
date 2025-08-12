using RetroEngine.Core;
using RetroEngine.Core.Elements;
using RetroEngine.Core.Signing;
using RetroEngine.UnitTest.TestData.Components;

namespace RetroEngine.UnitTest.TestData.Systems
{
    public class CountSystem : UpdateSystem
    {
        public CountSystem()
            : base(Contract
                  .Include<TagComponent>()
                  .Include<CountComponent>())
        {

        }

        public override void Process(World world, float deltaTime)
        {
            foreach (var entityId in GetEntities())
            {
                ref var tag = ref world.GetComponent<TagComponent>(entityId);
                ref var count = ref world.GetComponent<CountComponent>(entityId);

                count.Count += (int)deltaTime;
            }
        }
    }
}
