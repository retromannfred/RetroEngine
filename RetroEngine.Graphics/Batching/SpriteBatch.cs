using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RetroEngine.Graphics.Buffers;
using RetroEngine.Graphics.Settings;
using RetroEngine.Graphics.Shaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Graphics.Batching
{
    public class SpriteBatch
    {
        private const int FIXED_CAPACITY = 2048;

        private GraphicSettings _graphicSettings;

        private ShaderProgram _program;
        private VertexArray _vao;
        private ElementBuffer _indices;
        private Texture _texture;

        private VertexBuffer<Vector3> _positions;
        private VertexBuffer<Vector2> _texCoords;
        private VertexBuffer<Vector4> _colors;

        private List<VertexInfo> _items;

        public SpriteBatch(GraphicSettings graphicSettings, Texture texture)
        {
            _items = new();

            _program = new();
            _program.AddShader(new Shader(ShaderDefaults.DEFAULT_VERTEX_SHADER, ShaderType.VertexShader));
            _program.AddShader(new Shader(ShaderDefaults.DEFAULT_FRAGMENT_SHADER, ShaderType.FragmentShader));
            _program.Link();

            _graphicSettings = graphicSettings;
            _texture = texture;
            _vao = new();

            _positions = new();
            _positions.UpdateData(new Vector3[FIXED_CAPACITY * 4]);
            _texCoords = new();
            _texCoords.UpdateData(new Vector2[FIXED_CAPACITY * 4]);
            _colors = new();
            _colors.UpdateData(new Vector4[FIXED_CAPACITY * 4]);

            _indices = new();
            List<uint> itemIndices = new();
            for (uint i = 0; i < FIXED_CAPACITY * 4; i += 4) itemIndices.AddRange(new List<uint>() { i, i + 1, i + 2, i + 2, i + 3, i });
            _indices.UpdateData(itemIndices.ToArray());

            _vao.Link(0, _positions);
            _vao.Link(1, _texCoords);
            _vao.Link(2, _colors);
        }

        public void Begin(Matrix4 mvp)
        {
            _vao.Bind();
            _indices.Bind();
            _texture.Bind();
            _program.Bind();

            int modelViewProjection = GL.GetUniformLocation(1, "mvp");
            GL.UniformMatrix4(modelViewProjection, true, ref mvp);
        }

        public void Draw(Vector2 position,
            Vector2 sourceOffset,
            Vector2 sourceSize,
            Color4 color,
            float rotation,
            Vector2 scale,
            bool flipX,
            bool flipY,
            float layerDepth)
        {
            var TL = new VertexInfo()
            {
                Position = new Vector3(position.X - sourceSize.X * scale.X / 2, position.Y + sourceSize.Y * scale.Y / 2, layerDepth),
                TextureCoord = new Vector2(sourceOffset.X / _texture.Width, (_texture.Height - sourceOffset.Y) / _texture.Height),
                Color = (Vector4)color
            };
            var TR = new VertexInfo()
            {
                Position = new Vector3(position.X + sourceSize.X * scale.X / 2, position.Y + sourceSize.Y * scale.Y / 2, layerDepth),
                TextureCoord = new Vector2((sourceOffset.X + sourceSize.X) / _texture.Width, (_texture.Height - sourceOffset.Y) / _texture.Height),
                Color = (Vector4)color
            };
            var BR = new VertexInfo()
            {
                Position = new Vector3(position.X + sourceSize.X * scale.X / 2, position.Y - sourceSize.Y * scale.Y / 2, layerDepth),
                TextureCoord = new Vector2((sourceOffset.X + sourceSize.X) / _texture.Width, (_texture.Height - sourceOffset.Y - sourceSize.Y) / _texture.Height),
                Color = (Vector4)color
            };
            var BL = new VertexInfo()
            {
                Position = new Vector3(position.X - sourceSize.X * scale.X / 2, position.Y - sourceSize.Y * scale.Y / 2, layerDepth),
                TextureCoord = new Vector2(sourceOffset.X / _texture.Width, (_texture.Height - sourceOffset.Y - sourceSize.Y) / _texture.Height),
                Color = (Vector4)color
            };

            if( flipX )
            {
                var aux = TL.TextureCoord;
                TL.TextureCoord = TR.TextureCoord;
                TR.TextureCoord = aux;
                aux = BL.TextureCoord;
                BL.TextureCoord = BR.TextureCoord;
                BR.TextureCoord = aux;
            }
            if( flipY )
            {
                var aux = TL.TextureCoord;
                TL.TextureCoord = BL.TextureCoord;
                BL.TextureCoord = aux;
                aux = TR.TextureCoord;
                TR.TextureCoord = BR.TextureCoord;
                BR.TextureCoord = aux;
            }

            if (rotation != 0f)
            {
                TL.Position = Matrix3.CreateRotationZ(rotation) * TL.Position;
                TR.Position = Matrix3.CreateRotationZ(rotation) * TR.Position;
                BR.Position = Matrix3.CreateRotationZ(rotation) * BR.Position;
                BL.Position = Matrix3.CreateRotationZ(rotation) * BL.Position;
            }

            _items.Add(TL);
            _items.Add(TR);
            _items.Add(BR);
            _items.Add(BL);

            if (_items.Count >= FIXED_CAPACITY)
            {
                DrawBatch();
            }
        }

        private void DrawBatch()
        {
            if (_items.Count > 0)
            {
                _positions.UpdateData(0, _items.Select(v => v.Position).ToArray());
                _texCoords.UpdateData(0, _items.Select(v => v.TextureCoord).ToArray());
                _colors.UpdateData(0, _items.Select(v => v.Color).ToArray());

                GL.DrawElements(PrimitiveType.Triangles, _items.Count / 4 * 6, DrawElementsType.UnsignedInt, 0);

                _items.Clear();
            }
        }

        public void End()
        {
            DrawBatch();

            _vao.Unbind();
            _indices.Unbind();
            _texture.Unbind();
            _program.Unbind();
        }
    }
}
