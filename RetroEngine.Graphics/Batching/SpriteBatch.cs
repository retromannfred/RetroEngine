using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RetroEngine.Graphics.Buffers;
using RetroEngine.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Graphics.Batching
{
    public class SpriteBatch
    {
        private const int FIXED_CAPACITY = 25000;

        private ShaderProgram _program;
        private Texture _texture;
        private VertexArray _vao;
        private ElementBuffer _indices;

        private VertexBuffer<Vector3> _positions;
        private VertexBuffer<Vector2> _texCoords;
        private VertexBuffer<Vector4> _colors;

        private Dictionary<long, int> _itemOffsets;

        public SpriteBatch(Texture texture)
        {
            _positions = new();
            _positions.UpdateData(new Vector3[FIXED_CAPACITY * 4]);
            _texCoords = new();
            _texCoords.UpdateData(new Vector2[FIXED_CAPACITY * 4]);
            _colors = new();
            _colors.UpdateData(new Vector4[FIXED_CAPACITY * 4]);

            _texture = texture;
            _indices = new();
            _indices.UpdateData(new uint[FIXED_CAPACITY * 6]);
            _vao = new VertexArray();
            _vao.Link(0, _positions);
            _vao.Link(1, _texCoords);
            _vao.Link(2, _colors);

            _program = new();
            _program.AddShader(new Shader(ShaderDefaults.DEFAULT_VERTEX_SHADER, ShaderType.VertexShader));
            _program.AddShader(new Shader(ShaderDefaults.DEFAULT_FRAGMENT_SHADER, ShaderType.FragmentShader));
            _program.Link();

            _itemOffsets = new();
        }

        public void Update(long itemId, SpriteBatchItem item)
        {
            if (_itemOffsets.ContainsKey(itemId) == false)
                _itemOffsets.Add(itemId, _itemOffsets.Count);

            int offset = _itemOffsets[itemId];
            uint uintOffset = (uint)offset * 4;

            _positions.UpdateData(offset * 4, new Vector3[4] { item.TopLeft.Position, item.TopRight.Position, item.BottomRight.Position, item.BottomLeft.Position });
            _texCoords.UpdateData(offset * 4, new Vector2[4] { item.TopLeft.TextureCoord, item.TopRight.TextureCoord, item.BottomRight.TextureCoord, item.BottomLeft.TextureCoord });
            _colors.UpdateData(offset * 4, new Vector4[4] { item.TopLeft.Color, item.TopRight.Color, item.BottomRight.Color, item.BottomLeft.Color });
            _indices.UpdateData(offset * 6, new uint[6] { uintOffset + 0, uintOffset + 1, uintOffset + 2, uintOffset + 2, uintOffset + 3, uintOffset + 0 });
        }

        public void Draw(Matrix4 view, Matrix4 projection)
        {
            _vao.Bind();
            _indices.Bind();
            _texture.Bind();
            _program.Bind();

            Matrix4 model = Matrix4.Identity;

            int modelLocation = GL.GetUniformLocation(1, "model");
            int viewLocation = GL.GetUniformLocation(1, "view");
            int projectionLocation = GL.GetUniformLocation(1, "projection");

            GL.UniformMatrix4(modelLocation, true, ref model);
            GL.UniformMatrix4(viewLocation, true, ref view);
            GL.UniformMatrix4(projectionLocation, true, ref projection);

            GL.DrawElements(PrimitiveType.Triangles, _itemOffsets.Count * 6, DrawElementsType.UnsignedInt, 0);

            _vao.Unbind();
            _indices.Unbind();
            _texture.Unbind();
            _program.Unbind();
        }
    }
}
