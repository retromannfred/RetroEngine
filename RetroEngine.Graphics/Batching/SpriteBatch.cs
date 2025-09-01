using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Graphics.Shaders;
using System.Reflection;

namespace RetroEngine.Graphics.Batching
{
    /// <summary>
    /// Defines a sprite painter that buffers data to send it to the GPU.
    /// </summary>
    public class SpriteBatch
    {
        private const int FIXED_CAPACITY = 65536;

        private readonly Texture2D _texture;

        private readonly VertexArray _vao;
        private readonly ElementBuffer _ebo;
        private readonly ShaderProgram _program;

        private readonly VertexBuffer<float> _vboPositions;
        private readonly VertexBuffer<Matrix4> _vboModels;
        private readonly VertexBuffer<Vector4> _vboColors;
        private readonly VertexBuffer<Vector4> _vboTexCoords;

        private int _instanceCount;

        private readonly Matrix4[] _models;
        private readonly Vector4[] _colors;
        private readonly Vector4[] _texCoords;

        public string BatchKey => GetBatchKey(_texture);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        public SpriteBatch(Texture2D texture)
        {
            var layoutIndex = 0;

            _vao = new VertexArray();
            _ebo = new ElementBuffer();
            _texture = texture;

            _vboPositions = new VertexBuffer<float>(BufferUsageHint.StaticDraw);
            _vboPositions.CreateData(FixedSpriteBatchBufferData.PositionAndTextureCoords);
            _ebo.UpdateData(FixedSpriteBatchBufferData.ElementBufferIndices);

            _vao.Link(layoutIndex++, _vboPositions, 3, 5, 0);
            _vao.Link(layoutIndex++, _vboPositions, 2, 5, 3);

            _instanceCount = 0;
            _models = new Matrix4[FIXED_CAPACITY];
            _colors = new Vector4[FIXED_CAPACITY];
            _texCoords = new Vector4[FIXED_CAPACITY];

            _vboModels = new VertexBuffer<Matrix4>(BufferUsageHint.DynamicDraw);
            _vboModels.CreateData(_models);

            _vao.LinkDivided(layoutIndex++, _vboModels, 4, 16, 0, 1);
            _vao.LinkDivided(layoutIndex++, _vboModels, 4, 16, 4, 1);
            _vao.LinkDivided(layoutIndex++, _vboModels, 4, 16, 8, 1);
            _vao.LinkDivided(layoutIndex++, _vboModels, 4, 16, 12, 1);

            _vboColors = new VertexBuffer<Vector4>(BufferUsageHint.DynamicDraw);
            _vboColors.CreateData(_colors);

            _vao.LinkDivided(layoutIndex++, _vboColors, 4, 4, 0, 1);

            _vboTexCoords = new VertexBuffer<Vector4>(BufferUsageHint.DynamicDraw);
            _vboTexCoords.CreateData(_texCoords);

            _vao.LinkDivided(layoutIndex++, _vboTexCoords, 4, 4, 0, 1);

            VertexArray.Unbind();

            _program = new();
            _program.AddShader(new Shader(Shader.LoadShaderSource(Assembly.GetExecutingAssembly(), "RetroEngine.Graphics.Shaders.Defaults.sprite_batch.vert"), ShaderType.VertexShader));
            _program.AddShader(new Shader(Shader.LoadShaderSource(Assembly.GetExecutingAssembly(), "RetroEngine.Graphics.Shaders.Defaults.sprite_batch.frag"), ShaderType.FragmentShader));
            _program.Link();
        }

        internal static string GetBatchKey(Texture2D texture)
        {
            return $"T={texture.Id}";
        }

        public void Begin(Matrix4 view, Matrix4 projection)
        {
            _vao.Bind();
            _ebo.Bind();
            _texture.Bind();
            _program.Bind();

            GL.UniformMatrix4(GL.GetUniformLocation(_program.Id, "u_projection"), false, ref projection);
            GL.UniformMatrix4(GL.GetUniformLocation(_program.Id, "u_view"), false, ref view);
        }

        public void UpdateSpriteData(Transform transform, SpriteRenderer renderer)
        {
            _models[_instanceCount] =
                Matrix4.CreateScale(transform.Scale)
                *
                Matrix4.CreateFromQuaternion(new Quaternion(transform.Rotation))
                *
                Matrix4.CreateTranslation(transform.Position);

            _colors[_instanceCount] = (Vector4)renderer.Color;

            _texCoords[_instanceCount] = new Vector4(0, 0, 1, 1);

            ++_instanceCount;

            if (_instanceCount >= FIXED_CAPACITY)
            {
                DrawBatch();
            }
        }

        public void DrawBatch()
        {
            _vboModels.UpdateData(0, _instanceCount, _models);
            _vboColors.UpdateData(0, _instanceCount, _colors);
            _vboTexCoords.UpdateData(0, _instanceCount, _texCoords);

            GL.DrawElementsInstanced(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, 0, _instanceCount);

            _instanceCount = 0;
        }

        public void End()
        {
            if (_instanceCount > 0)
            {
                DrawBatch();
            }

            VertexArray.Unbind();
            ElementBuffer.Unbind();
            Texture2D.Unbind();
            ShaderProgram.Unbind();
        }
    }
}
