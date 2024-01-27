using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RetroEngine.Core.Buffers;
using RetroEngine.Core.Shaders;

namespace RetroEngine.Core.Batching
{
    /// <summary>
    /// Defines a spritebatch for drawing multiple sprites from a single texture.
    /// </summary>
    public class SpriteBatch
    {
        private const int DEFAULT_SIZE = 10;
        private int _count;

        private Texture _texture;
        private ShaderProgram _program;

        private VertexArray _vao;
        private ElementBuffer _indices;

        private VertexBuffer _positions;
        private VertexBuffer _textureCoords;
        private VertexBuffer _colors;

        /// <summary>
        /// Creates a new sprite batch.
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="positions"></param>
        /// <param name="textureCoords"></param>
        /// <param name="colors"></param>
        /// <param name="indices"></param>
        public SpriteBatch(
            Texture texture,
            List<Vector3> positions,
            List<Vector2> textureCoords,
            List<Vector3> colors,
            List<uint> indices)
        {
            _texture = texture;
            _count = positions.Count / 4;
            _vao = new VertexArray();

            _positions = new VertexBuffer(positions);
            _vao.Link(0, 3, _positions);
            _textureCoords = new VertexBuffer(textureCoords);
            _vao.Link(1, 2, _textureCoords);
            _colors = new VertexBuffer(colors);
            _vao.Link(2, 3, _colors);

            _indices = new ElementBuffer(indices);

            _program = new();
            _program.AddShader(new Shader(ShaderDefaults.DEFAULT_VERTEX_SHADER, ShaderType.VertexShader));
            _program.AddShader(new Shader(ShaderDefaults.DEFAULT_FRAGMENT_SHADER, ShaderType.FragmentShader));
            _program.Link();
        }

        /// <summary>
        /// Prepares elements in this sprite batch to begin drawing.
        /// </summary>
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
        /// Draws elements in this sprite batch.
        /// </summary>
        public void Draw()
        {
            GL.DrawElements(PrimitiveType.Triangles, _indices.Count, DrawElementsType.UnsignedInt, 0);
        }

        /// <summary>
        /// Unbinds all elements in this sprite batch to end drawing.
        /// </summary>
        public void End()
        {
            _vao.Unbind();
            _indices.Unbind();
            _texture.Unbind();
            _program.Unbind();
        }
    }
}
