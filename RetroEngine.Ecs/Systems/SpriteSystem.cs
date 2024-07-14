using OpenTK.Mathematics;
using RetroEngine.Core.Elements;
using RetroEngine.Core.Mapping;
using RetroEngine.Ecs.Components;
using RetroEngine.Graphics;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Settings;

namespace RetroEngine.Ecs.Systems
{
    /// <summary>
    /// Renders sprites into the game screen.
    /// </summary>
    public class SpriteSystem : RenderSystem
    {
        private GraphicSettings _graphicSettings;
        private Dictionary<string, SpriteBatch> _batches;

        /// <summary>
        /// Creates a new sprite renderer system.
        /// </summary>
        /// <param name="graphicSettings">Graphic settings of the game.</param>
        public SpriteSystem(GraphicSettings graphicSettings)
            : base(Aspect.All<Transform>().All<SpriteRenderer>())
        {
            _graphicSettings = graphicSettings;
            _batches = new Dictionary<string, SpriteBatch>();
        }

        /// <summary>
        /// Renders entities filtered in this system.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        public override void Render(GameTime gameTime)
        {
            foreach (var transformMatrix in _graphicSettings.ClipSpaces)
            {
                SpriteBatch? lastBatch = null;

                foreach (var entity in ActiveEntities)
                {
                    ref var transform = ref World.GetComponent<Transform>(entity);
                    ref var renderer = ref World.GetComponent<SpriteRenderer>(entity);

                    var batchKey = SpriteBatch.GetBatchKey(renderer.Texture);

                    if (lastBatch == null || lastBatch.BatchKey != batchKey)
                    {
                        if (_batches.ContainsKey(batchKey))
                        {
                            lastBatch = _batches[batchKey];
                        }
                        else
                        {
                            lastBatch = new SpriteBatch(_graphicSettings, renderer.Texture);
                            _batches.Add(batchKey, lastBatch);
                        }

                        lastBatch.Begin(transformMatrix);
                    }

                    lastBatch.Draw(
                        transform.Position,
                        Vector2.Zero,
                        new Vector2(renderer.Texture.Width, renderer.Texture.Height),
                        renderer.Color,
                        transform.Rotation,
                        transform.Scale,
                        (renderer.Flip | Flip.X) == Flip.X,
                        (renderer.Flip | Flip.Y) == Flip.Y
                    );
                }

                foreach (var batch in _batches.Values)
                {
                    batch.Begin(transformMatrix);
                    batch.End();
                }
            }
        }
    }
}