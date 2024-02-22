using OpenTK.Mathematics;
using RetroEngine.Graphics;
using RetroEngine.Core;
using RetroEngine.FuncTest.ECS;

namespace RetroEngine.FuncTest.Games
{
    internal class TestEcsGame : Game
    {
        private readonly World _world;

        public TestEcsGame() : base("Test game", 800, 600)
        {
            _world = new WorldBuilder()
                .AddSystem(new TestSystem())
                .Build();
        }

        protected override void Initialize()
        {

        }

        protected override void LoadContent()
        {
            var entity = _world.CreateEntity();
            entity.Attach(new TestComponent());
        }

        protected override void Update(GameTime gameTime)
        {
            _world.Update(gameTime);
        }

        protected override void Render(GameTime gameTime)
        {
            ClearScreen(Color4.CornflowerBlue);

            _world.Render(gameTime);
        }
    }
}
