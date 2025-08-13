using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Core.Elements;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Components;

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
            : base(Contract.Include<Transform>().Include<SpriteRenderer>())
        {
            _graphicSettings = graphicSettings;
            _batches = new Dictionary<string, SpriteBatch>();
        }

        public override void Process(World world, GameTime time)
        {
            foreach (var transformMatrix in _graphicSettings.ClipSpaces)
            {
                SpriteBatch? lastBatch = null;

                foreach (var entity in GetEntities())
                {
                    ref var transform = ref world.GetComponent<Transform>(entity);
                    ref var renderer = ref world.GetComponent<SpriteRenderer>(entity);

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