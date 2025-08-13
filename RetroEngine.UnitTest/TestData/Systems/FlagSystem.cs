using RetroEngine.Core;
using RetroEngine.Core.Elements;
using RetroEngine.UnitTest.TestData.Components;

namespace RetroEngine.UnitTest.TestData.Systems
{
    public class FlagSystem : RenderSystem
    {
        public FlagSystem()
            : base(Contract
                  .Include<TagComponent>()
                  .Include<FlagsComponent>())
        {

        }

        public override void Process(World world, GameTime time)
        {
            foreach (var entityId in GetEntities())
            {
                ref var tag = ref world.GetComponent<TagComponent>(entityId);
                ref var flag = ref world.GetComponent<FlagsComponent>(entityId);

                flag.FlagA = !flag.FlagA;
                flag.FlagB = flag.FlagA ? flag.FlagB : !flag.FlagB;
                flag.FlagC = flag.FlagB ? flag.FlagC : !flag.FlagC;
            }
        }
    }
}
