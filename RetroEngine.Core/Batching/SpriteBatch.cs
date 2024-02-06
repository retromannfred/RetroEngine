using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RetroEngine.Core.Buffers;
using RetroEngine.Core.Shaders;
using System.Drawing;

namespace RetroEngine.Core.Batching
{
    /// <summary>
    /// Defines a spritebatch for drawing multiple sprites from a single texture.
    /// </summary>
    public class SpriteBatch
    {
        private readonly Texture _texture;
        private readonly ShaderProgram _program;

        private readonly VertexArray _vao;
        private readonly ElementBuffer _indices;

        private readonly VertexBuffer<Vector3> _positions;
        private readonly VertexBuffer<Vector2> _textureCoords;
        private readonly VertexBuffer<Vector4> _colors;

        private SpriteBatchItem[] _batchItems;
        private int _batchItemCount;

        /// <summary>
        /// Creates a new sprite batch.
        /// </summary>
        /// <param name="texture">Texture to be used to draw.</param>
        public SpriteBatch(Texture texture)
        {
            _texture = texture;
            _vao = new VertexArray();
            _batchItems = new SpriteBatchItem[256];

            _positions = new VertexBuffer<Vector3>();
            _vao.Link(0, _positions);
            _textureCoords = new VertexBuffer<Vector2>();
            _vao.Link(1, _textureCoords);
            _colors = new VertexBuffer<Vector4>();
            _vao.Link(2, _colors);

            _indices = new ElementBuffer();

            _program = new();
            _program.AddShader(new Shader(ShaderDefaults.DEFAULT_VERTEX_SHADER, ShaderType.VertexShader));
            _program.AddShader(new Shader(ShaderDefaults.DEFAULT_FRAGMENT_SHADER, ShaderType.FragmentShader));
            _program.Link();
        }

        /// <summary>
        /// Prepares elements in this sprite batch to begin drawing.
        /// </summary>
        /// <param name="projection">Projection matrix representing camera view</param>
        public void Begin(Matrix4 projection)
        {
            _vao.Bind();
            _indices.Bind();
            _texture.Bind();
            _program.Bind();

            Matrix4 model = Matrix4.Identity;
            Matrix4 view = Matrix4.Identity;

            int modelLocation = GL.GetUniformLocation(1, "model");
            int viewLocation = GL.GetUniformLocation(1, "view");
            int projectionLocation = GL.GetUniformLocation(1, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);
        }

        /// <summary>
        /// Sets a sprite to be drawn by this batch.
        /// </summary>
        /// <param name="position">Position of the sprite.</param>
        /// <param name="offset">Offset position of the texture section to be drawn.</param>
        /// <param name="size">Size of the texture section to be drawn.</param>
        /// <param name="color">Color of the sprite.</param>
        /// <param name="rotation">Rotation of the sprite.</param>
        /// <param name="scale">Scale of the sprite.</param>
        public void Draw(Vector2 position, Vector2 offset, Vector2 size, Color4 color, float rotation, Vector2 scale, float layerDepth)
        {
            if( _batchItemCount >= _batchItems.Length)
            {
                Array.Resize(ref _batchItems, _batchItemCount * 2);
            }

            var item = new SpriteBatchItem()
            {
                TopLeft = new VertexInfo()
                {
                    Position = new Vector3(position.X - size.X * scale.X / 2, position.Y + size.Y * scale.Y / 2, layerDepth),
                    TextureCoord = new Vector2(offset.X / _texture.Width, (_texture.Height - offset.Y) / _texture.Height),
                    Color = (Vector4) color
                },
                TopRight = new VertexInfo()
                {
                    Position = new Vector3(position.X + size.X * scale.X / 2, position.Y + size.Y * scale.Y / 2, layerDepth),
                    TextureCoord = new Vector2((offset.X + size.X) / _texture.Width, (_texture.Height - offset.Y) / _texture.Height),
                    Color = (Vector4) color
                },
                BottomRight = new VertexInfo()
                {
                    Position = new Vector3(position.X + size.X * scale.X / 2, position.Y - size.Y * scale.Y / 2, layerDepth),
                    TextureCoord = new Vector2((offset.X + size.X) / _texture.Width, (_texture.Height - offset.Y - size.Y) / _texture.Height),
                    Color = (Vector4) color
                },
                BottomLeft = new VertexInfo()
                {
                    Position = new Vector3(position.X - size.X * scale.X / 2, position.Y - size.Y * scale.Y / 2, layerDepth),
                    TextureCoord = new Vector2(offset.X / _texture.Width, (_texture.Height - offset.Y - size.Y) / _texture.Height),
                    Color = (Vector4) color
                }
            };

            if( rotation != 0f)
            {
                float sin = (float) MathHelper.Sin(rotation);
                float cos = (float) MathHelper.Cos(rotation);

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

            _batchItems[_batchItemCount++] = item;
        }

        /// <summary>
        /// Performs drawing of batched sprites.
        /// </summary>
        public void End()
        {
            var positions = new Vector3[_batchItemCount * 4];
            var textCoords = new Vector2[_batchItemCount * 4];
            var colors = new Vector4[_batchItemCount * 4];
            var indices = new uint[_batchItemCount * 6];

            for (int i = 0; i < _batchItemCount; i++)
            {
                positions[i * 4] =     _batchItems[i].TopLeft.Position;
                positions[i * 4 + 1] = _batchItems[i].TopRight.Position;
                positions[i * 4 + 2] = _batchItems[i].BottomRight.Position;
                positions[i * 4 + 3] = _batchItems[i].BottomLeft.Position;

                textCoords[i * 4] =     _batchItems[i].TopLeft.TextureCoord;
                textCoords[i * 4 + 1] = _batchItems[i].TopRight.TextureCoord;
                textCoords[i * 4 + 2] = _batchItems[i].BottomRight.TextureCoord;
                textCoords[i * 4 + 3] = _batchItems[i].BottomLeft.TextureCoord;

                colors[i * 4] =     _batchItems[i].TopLeft.Color;
                colors[i * 4 + 1] = _batchItems[i].TopRight.Color;
                colors[i * 4 + 2] = _batchItems[i].BottomRight.Color;
                colors[i * 4 + 3] = _batchItems[i].BottomLeft.Color;

                indices[i * 6] = (uint) i * 4;
                indices[i * 6 + 1] = (uint)i * 4 + 1;
                indices[i * 6 + 2] = (uint)i * 4 + 2;
                indices[i * 6 + 3] = (uint)i * 4 + 2;
                indices[i * 6 + 4] = (uint)i * 4 + 3;
                indices[i * 6 + 5] = (uint)i * 4;
            }

            _positions.UpdateData(positions.ToArray());
            _textureCoords.UpdateData(textCoords.ToArray());
            _colors.UpdateData(colors.ToArray());
            _indices.UpdateData(indices.ToArray());

            GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt, 0);

            _vao.Unbind();
            _indices.Unbind();
            _texture.Unbind();
            _program.Unbind();

            _batchItemCount = 0;
        }
    }
}
