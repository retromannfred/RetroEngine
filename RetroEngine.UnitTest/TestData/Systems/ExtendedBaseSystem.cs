using RetroEngine.Core;
using RetroEngine.Core.Elements;
using RetroEngine.UnitTest.TestData.Components;

namespace RetroEngine.UnitTest.TestData.Systems
{
    public class ExtendedBaseSystem() : BaseSystem(Contract.Include<TagComponent>())
    {
        public override void Process(World world, GameTime time) { }
    }
}
