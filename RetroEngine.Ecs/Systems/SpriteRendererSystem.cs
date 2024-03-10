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
    public class SpriteRendererSystem : RenderSystem
    {
        private GraphicSettings _graphicSettings;
        private Dictionary<int, SpriteBatch> _batches;

        /// <summary>
        /// Creates a new sprite renderer system.
        /// </summary>
        /// <param name="graphicSettings">Graphic settings of the game.</param>
        public SpriteRendererSystem(GraphicSettings graphicSettings)
            : base(Aspect.All<Transform>().All<SpriteRenderer>())
        {
            _graphicSettings = graphicSettings;
            _batches = new Dictionary<int, SpriteBatch>();
        }

        /// <summary>
        /// Renders entities filtered in this system.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        public override void Render(GameTime gameTime)
        {
            //foreach (var entity in ActiveEntities)
            //{
            //    ref var transform = ref World.GetComponent<Transform>(entity);
            //    ref var renderer = ref World.GetComponent<SpriteRenderer>(entity);

            //    string batchIdentifier = $"T={renderer.Texture.Id}";
            //    SpriteBatch? batch = World.GetSpriteBatch(batchIdentifier);

            //    if( batch == null)
            //    {
            //        batch = new SpriteBatch(_graphicSettings, renderer.Texture);
            //        World.AddSpriteBatch(batchIdentifier, batch);
            //    }

            //    batch.Draw(
            //        transform.Position,
            //        Vector2.Zero,
            //        new Vector2(renderer.Texture.Width, renderer.Texture.Height),
            //        renderer.Color,
            //        transform.Rotation,
            //        transform.Scale,
            //        (renderer.Flip & Flip.X) == Flip.X,
            //        (renderer.Flip & Flip.Y) == Flip.Y,
            //        renderer.LayerDepth
            //    );

            //    //var batch = GetSpriteBatch(renderer.TextureId);
            //    //var spriteBatchItem = GetBatchItem(
            //    //    new Vector2(renderer.Width, renderer.Height),
            //    //    transform.Position,
            //    //    Vector2.Zero,
            //    //    new Vector2(renderer.Width, renderer.Height),
            //    //    renderer.Color,
            //    //    transform.Rotation,
            //    //    Vector2.One,
            //    //    renderer.LayerDepth);

            //    //batch.Update(entity, spriteBatchItem);
            //}

            //foreach (var batch in _batches.Values)
            //{
            //    batch.Draw(Matrix4.CreateTranslation(Vector3.UnitZ * -10), Matrix4.CreateOrthographic(_graphicSettings.Width, _graphicSettings.Height, 0.3f, 1000f));
            //}
        }

        /// <summary>
        /// Gets the sprite batch which renders a texture.
        /// </summary>
        /// <param name="textureId">ID of the texture.</param>
        /// <returns>Found batch if was already created, or a new batch for that texture.</returns>
        //private SpriteBatch GetSpriteBatch(int textureId)
        //{
        //    if (_batches.ContainsKey(textureId) == false)
        //    {
        //        var batch = new SpriteBatch(new Texture(textureId));
        //        _batches.TryAdd(textureId, batch);

        //        return batch;
        //    }

        //    return _batches[textureId];
        //}

        /// <summary>
        /// Converts transform + sprite renderer components into OpenGL vertices.
        /// </summary>
        /// <param name="textureSize">Size of the whole texture.</param>
        /// <param name="position">Position of the sprite.</param>
        /// <param name="offset">Offset position of the rectangle section in the texture to render.</param>
        /// <param name="size">Size of the rectangle section in the texture to render.</param>
        /// <param name="color">Color tincture of the sprite.</param>
        /// <param name="rotation">Rotation in radians of the sprite.</param>
        /// <param name="scale">Scale of the sprite.</param>
        /// <param name="layerDepth">Depth of the sprite (z-position).</param>
        /// <returns>SpriteBatchItem representing four vertices of the rectangle to render in the GPU.</returns>
        //private SpriteBatchItem GetBatchItem(Vector2 textureSize, Vector2 position, Vector2 offset, Vector2 size, Color4 color, float rotation, Vector2 scale, float layerDepth)
        //{
        //    var item = new SpriteBatchItem()
        //    {
        //        TopLeft = new VertexInfo()
        //        {
        //            Position = new Vector3(position.X - size.X * scale.X / 2, position.Y + size.Y * scale.Y / 2, layerDepth),
        //            TextureCoord = new Vector2(offset.X / textureSize.X, (textureSize.Y - offset.Y) / textureSize.Y),
        //            Color = (Vector4)color
        //        },
        //        TopRight = new VertexInfo()
        //        {
        //            Position = new Vector3(position.X + size.X * scale.X / 2, position.Y + size.Y * scale.Y / 2, layerDepth),
        //            TextureCoord = new Vector2((offset.X + size.X) / textureSize.X, (textureSize.Y - offset.Y) / textureSize.Y),
        //            Color = (Vector4)color
        //        },
        //        BottomRight = new VertexInfo()
        //        {
        //            Position = new Vector3(position.X + size.X * scale.X / 2, position.Y - size.Y * scale.Y / 2, layerDepth),
        //            TextureCoord = new Vector2((offset.X + size.X) / textureSize.X, (textureSize.Y - offset.Y - size.Y) / textureSize.Y),
        //            Color = (Vector4)color
        //        },
        //        BottomLeft = new VertexInfo()
        //        {
        //            Position = new Vector3(position.X - size.X * scale.X / 2, position.Y - size.Y * scale.Y / 2, layerDepth),
        //            TextureCoord = new Vector2(offset.X / textureSize.X, (textureSize.Y - offset.Y - size.Y) / textureSize.Y),
        //            Color = (Vector4)color
        //        }
        //    };

        //    if (rotation != 0f)
        //    {
        //        float sin = (float)MathHelper.Sin(rotation);
        //        float cos = (float)MathHelper.Cos(rotation);

        //        float x, y;

        //        x = (item.TopLeft.Position.X - position.X) * cos - (item.TopLeft.Position.Y - position.Y) * sin + position.X;
        //        y = (item.TopLeft.Position.X - position.X) * sin + (item.TopLeft.Position.Y - position.Y) * cos + position.Y;
        //        item.TopLeft.Position.X = x;
        //        item.TopLeft.Position.Y = y;

        //        x = (item.TopRight.Position.X - position.X) * cos - (item.TopRight.Position.Y - position.Y) * sin + position.X;
        //        y = (item.TopRight.Position.X - position.X) * sin + (item.TopRight.Position.Y - position.Y) * cos + position.Y;
        //        item.TopRight.Position.X = x;
        //        item.TopRight.Position.Y = y;

        //        x = (item.BottomRight.Position.X - position.X) * cos - (item.BottomRight.Position.Y - position.Y) * sin + position.X;
        //        y = (item.BottomRight.Position.X - position.X) * sin + (item.BottomRight.Position.Y - position.Y) * cos + position.Y;
        //        item.BottomRight.Position.X = x;
        //        item.BottomRight.Position.Y = y;

        //        x = (item.BottomLeft.Position.X - position.X) * cos - (item.BottomLeft.Position.Y - position.Y) * sin + position.X;
        //        y = (item.BottomLeft.Position.X - position.X) * sin + (item.BottomLeft.Position.Y - position.Y) * cos + position.Y;
        //        item.BottomLeft.Position.X = x;
        //        item.BottomLeft.Position.Y = y;
        //    }

        //    return item;
        //}
    }
}