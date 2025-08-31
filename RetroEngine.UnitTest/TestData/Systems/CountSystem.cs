using RetroEngine.Core;
using RetroEngine.UnitTest.TestData.Components;

namespace RetroEngine.UnitTest.TestData.Systems
{
    public class CountSystem() : UpdateSystem(Contract
                  .Include<TagComponent>()
                  .Include<CountComponent>())
    {
        public override void Process(World world, GameTime time)
        {
            foreach (var entityId in GetEntities())
            {
                ref var tag = ref world.GetComponent<TagComponent>(entityId);
                ref var count = ref world.GetComponent<CountComponent>(entityId);

                count.Count += (int)time.Delta;
            }
        }
    }
}
