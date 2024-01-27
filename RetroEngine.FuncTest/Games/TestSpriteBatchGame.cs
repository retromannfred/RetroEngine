using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Batching;
using RetroEngine.ECS;
using RetroEngine.FuncTest.ECS;

namespace RetroEngine.FuncTest.Games
{
    internal class TestSpriteBatchGame : Game
    {
        private Texture _texture;
        private SpriteBatch _spriteBatch;

        public TestSpriteBatchGame() : base("Test game", 800, 600)
        {

        }

        protected override void Initialize()
        {
            
        }

        protected override void LoadContent()
        {
            _texture = new Texture("Sprites/person.png");

            var positions = new List<Vector3>()
            {
                new Vector3(-16.0f, 16.0f, -1.0f),
                new Vector3(16.0f, 16.0f, -1.0f),
                new Vector3(16.0f, -16.0f, -1.0f),
                new Vector3(-16.0f, -16.0f, -1.0f)
            };

            var texCoords = new List<Vector2>()
            {
                new Vector2(0.0f, 1.0f),
                new Vector2(1.0f, 1.0f),
                new Vector2(1.0f, 0.0f),
                new Vector2(0.0f, 0.0f)
            };

            var colors = new List<Vector3>()
            {
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(1.0f, 1.0f, 1.0f),
                new Vector3(1.0f, 1.0f, 1.0f)
            };

            var indices = new List<uint>()
            {
                0, 1, 3, 1, 2, 3
            };

            _spriteBatch = new SpriteBatch(
                _texture,
                positions,
                texCoords,
                colors,
                indices
            );
        }

        protected override void Update(GameTime gameTime)
        {
            
        }

        protected override void Render(GameTime gameTime)
        {
            ClearScreen(Color4.CornflowerBlue);

            _spriteBatch.Begin(Matrix4.CreateOrthographic(this.Width, this.Height, 0.01f, 3000f));
            _spriteBatch.Draw();
            _spriteBatch.End();
        }
    }
}
