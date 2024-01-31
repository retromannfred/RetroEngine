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

        private readonly VertexBuffer _positions;
        private readonly VertexBuffer _textureCoords;
        private readonly VertexBuffer _colors;

        private readonly List<SpriteBatchItem> _batchItems;

        /// <summary>
        /// Creates a new sprite batch.
        /// </summary>
        /// <param name="texture">Texture to be used to draw.</param>
        public SpriteBatch(Texture texture)
        {
            _texture = texture;
            _vao = new VertexArray();
            _batchItems = new List<SpriteBatchItem>();

            _positions = new VertexBuffer();
            _vao.Link(0, 3, _positions);
            _textureCoords = new VertexBuffer();
            _vao.Link(1, 2, _textureCoords);
            _colors = new VertexBuffer();
            _vao.Link(2, 3, _colors);

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
            _texture.Bind();
            _program.Bind();

            _batchItems.Clear();

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
        public void Draw(Vector3 position, Vector2 offset, Vector2 size, Vector4 color, float rotation, Vector2 scale)
        {
            var item = new SpriteBatchItem()
            {
                TopLeft = new VertexInfo()
                {
                    Position = new Vector3(position.X - size.X * scale.X / 2, position.Y + size.Y * scale.Y / 2, position.Z),
                    TextureCoord = new Vector2(offset.X / _texture.Width, (_texture.Height - offset.Y) / _texture.Height),
                    Color = color
                },
                TopRight = new VertexInfo()
                {
                    Position = new Vector3(position.X + size.X * scale.X / 2, position.Y + size.Y * scale.Y / 2, position.Z),
                    TextureCoord = new Vector2((offset.X + size.X) / _texture.Width, (_texture.Height - offset.Y) / _texture.Height),
                    Color = color
                },
                BottomRight = new VertexInfo()
                {
                    Position = new Vector3(position.X + size.X * scale.X / 2, position.Y - size.Y * scale.Y / 2, position.Z),
                    TextureCoord = new Vector2((offset.X + size.X) / _texture.Width, (_texture.Height - offset.Y - size.Y) / _texture.Height),
                    Color = color
                },
                BottomLeft = new VertexInfo()
                {
                    Position = new Vector3(position.X - size.X * scale.X / 2, position.Y - size.Y * scale.Y / 2, position.Z),
                    TextureCoord = new Vector2(offset.X / _texture.Width, (_texture.Height - offset.Y - size.Y) / _texture.Height),
                    Color = color
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

            _batchItems.Add(item);
        }

        /// <summary>
        /// Performs drawing of batched sprites.
        /// </summary>
        public void End()
        {
            var positions = new List<Vector3>();
            var textCoords = new List<Vector2>();
            var colors = new List<Vector4>();
            var indices = new List<uint>();

            foreach (var item in _batchItems)
            {
                uint count = (uint)positions.Count;

                positions.Add(item.TopLeft.Position);
                positions.Add(item.TopRight.Position);
                positions.Add(item.BottomRight.Position);
                positions.Add(item.BottomLeft.Position);

                textCoords.Add(item.TopLeft.TextureCoord);
                textCoords.Add(item.TopRight.TextureCoord);
                textCoords.Add(item.BottomRight.TextureCoord);
                textCoords.Add(item.BottomLeft.TextureCoord);

                colors.Add(item.TopLeft.Color);
                colors.Add(item.TopRight.Color);
                colors.Add(item.BottomRight.Color);
                colors.Add(item.BottomLeft.Color);

                indices.AddRange(new uint[6] {0 + count, 1 + count, 2 + count, 2 + count, 3 + count, 0 + count });
            }

            _positions.UpdateData(positions.ToArray());
            _textureCoords.UpdateData(textCoords.ToArray());
            _colors.UpdateData(colors.ToArray());
            _indices.UpdateData(indices.ToArray());

            GL.DrawElements(PrimitiveType.Triangles, indices.Count, DrawElementsType.UnsignedInt, 0);

            _vao.Unbind();
            _indices.Unbind();
            _texture.Unbind();
            _program.Unbind();
        }
    }
}
