using OpenTK.Mathematics;
using RetroEngine.Components;
using RetroEngine.Core;
using RetroEngine.Core.Batching;
using RetroEngine.Core.Settings;
using RetroEngine.ECS.Elements;
using RetroEngine.ECS.Managers;
using System.Drawing;
using System.Transactions;
using static System.Formats.Asn1.AsnWriter;

namespace RetroEngine.Systems
{
    public class SpriteRendererSystem : RenderSystem
    {
        private GraphicSettings _graphicSettings;
        private Dictionary<int, SpriteBatch> _batches;
        private bool _rendered = false;

        public SpriteRendererSystem(GraphicSettings graphicSettings)
            : base(Aspect.All<Transform>().All<SpriteRenderer>())
        {
            _graphicSettings = graphicSettings;
            _batches = new Dictionary<int, SpriteBatch>();
        }

        public override void Render(GameTime gameTime)
        {
            if (_rendered == false)
            {
                foreach (var entity in ActiveEntities)
                {
                    ref var transform = ref World.GetComponent<Transform>(entity);
                    ref var renderer = ref World.GetComponent<SpriteRenderer>(entity);

                    var batch = GetSpriteBatch(renderer.TextureId);
                    var spriteBatchItem = GetBatchItem(
                        new Vector2(renderer.Width, renderer.Height),
                        transform.Position,
                        Vector2.Zero,
                        new Vector2(renderer.Width, renderer.Height),
                        renderer.Color,
                        transform.Rotation,
                        Vector2.One,
                        renderer.LayerDepth);

                    batch.Update(entity, spriteBatchItem);
                }
            }

            foreach (var batch in _batches.Values)
            {
                batch.Draw(Matrix4.CreateTranslation(Vector3.UnitZ * -10), Matrix4.CreateOrthographic(_graphicSettings.Width, _graphicSettings.Height, 0.3f, 1000f));
            }
            //_rendered = true;
        }

        private SpriteBatch GetSpriteBatch(int textureId)
        {
            if (_batches.ContainsKey(textureId) == false)
            {
                var batch = new SpriteBatch(new Texture(textureId));
                _batches.TryAdd(textureId, batch);

                return batch;
            }

            return _batches[textureId];
        }

        private SpriteBatchItem GetBatchItem(Vector2 textureSize, Vector2 position, Vector2 offset, Vector2 size, Color4 color, float rotation, Vector2 scale, float layerDepth)
        {
            var item = new SpriteBatchItem()
            {
                TopLeft = new VertexInfo()
                {
                    Position = new Vector3(position.X - size.X * scale.X / 2, position.Y + size.Y * scale.Y / 2, layerDepth),
                    TextureCoord = new Vector2(offset.X / textureSize.X, (textureSize.Y - offset.Y) / textureSize.Y),
                    Color = (Vector4)color
                },
                TopRight = new VertexInfo()
                {
                    Position = new Vector3(position.X + size.X * scale.X / 2, position.Y + size.Y * scale.Y / 2, layerDepth),
                    TextureCoord = new Vector2((offset.X + size.X) / textureSize.X, (textureSize.Y - offset.Y) / textureSize.Y),
                    Color = (Vector4)color
                },
                BottomRight = new VertexInfo()
                {
                    Position = new Vector3(position.X + size.X * scale.X / 2, position.Y - size.Y * scale.Y / 2, layerDepth),
                    TextureCoord = new Vector2((offset.X + size.X) / textureSize.X, (textureSize.Y - offset.Y - size.Y) / textureSize.Y),
                    Color = (Vector4)color
                },
                BottomLeft = new VertexInfo()
                {
                    Position = new Vector3(position.X - size.X * scale.X / 2, position.Y - size.Y * scale.Y / 2, layerDepth),
                    TextureCoord = new Vector2(offset.X / textureSize.X, (textureSize.Y - offset.Y - size.Y) / textureSize.Y),
                    Color = (Vector4)color
                }
            };

            if (rotation != 0f)
            {
                float sin = (float)MathHelper.Sin(rotation);
                float cos = (float)MathHelper.Cos(rotation);

                float x, y;

                x = (item.TopLeft.Position.X - position.X) * cos - (item.TopLeft.Position.Y - position.Y) * sin + position.X;
                y = (item.TopLeft.Position.X - position.X) * sin + (item.TopLeft.Position.Y - position.Y) * cos + position.Y;
                item.TopLeft.Position.X = x;
                item.TopLeft.Position.Y = y;

                x = (item.TopRight.Position.X - position.X) * cos - (item.TopRight.Position.Y - position.Y) * sin + position.X;
                y = (item.TopRight.Position.X - position.X) * sin + (item.TopRight.Position.Y - position.Y) * cos + position.Y;
                item.TopRight.Position.X = x;
                item.TopRight.Position.Y = y;

                x = (item.BottomRight.Position.X - position.X) * cos - (item.BottomRight.Position.Y - position.Y) * sin + position.X;
                y = (item.BottomRight.Position.X - position.X) * sin + (item.BottomRight.Position.Y - position.Y) * cos + position.Y;
                item.BottomRight.Position.X = x;
                item.BottomRight.Position.Y = y;

                x = (item.BottomLeft.Position.X - position.X) * cos - (item.BottomLeft.Position.Y - position.Y) * sin + position.X;
                y = (item.BottomLeft.Position.X - position.X) * sin + (item.BottomLeft.Position.Y - position.Y) * cos + position.Y;
                item.BottomLeft.Position.X = x;
                item.BottomLeft.Position.Y = y;
            }

            return item;
        }
    }
}