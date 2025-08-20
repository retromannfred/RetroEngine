using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.FuncTest.Games
{
    /// <summary>
    /// Game to test the sprite batcher without the ECS engine to check performance on itself.
    /// </summary>
    internal class TestSpriteBatchWithoutECS : Game
    {
        private const int NUMBER_OF_SQUARES = 100000;
        private Texture2D _texture;
        private SpriteBatch? _batch;

        public TestSpriteBatchWithoutECS()
            : base("Test sprite batch", 800, 600)
        {
            Console.WriteLine();
            Console.WriteLine("Su should be seeing 3 planes on {X=0,Y=0,Z=0} and " + NUMBER_OF_SQUARES + " squares in an arbitrary area.");
            Console.WriteLine("There's no camera movement. Need to check just drawing performance of the batch.");
            Console.WriteLine();
        }

        protected override void LoadContent()
        {
            _texture = TextureFactory.CreateRectangle(1, 1, Color4.White);
            _batch = new SpriteBatch(_texture);
        }

        protected override void Render(GameTime time)
        {
            ClearScreen(Color4.CornflowerBlue);

            var view = Matrix4.CreateTranslation(Vector3.UnitZ * -10f);
            var projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 800f / 600f, .3f, 1000f);

            _batch!.Begin(view, projection);

            var rand = new Random(100);

            for (int i = 0; i < 100000; i++)
            {
                var transform = new Transform()
                {
                    Position = new Vector3(rand.Next(-10, 10), rand.Next(-10, 10), rand.Next(-30, -10))
                };
                var renderer = new SpriteRenderer(_texture)
                {
                    Color = new Color4((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256), 255),
                };
                _batch.UpdateSpriteData(transform, renderer);
            }

            _batch.DrawBatch();
            _batch.End();
        }

        protected override void Update(GameTime time)
        {
            
        }
    }
}
