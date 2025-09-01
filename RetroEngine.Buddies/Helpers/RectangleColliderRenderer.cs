using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Shaders;
using RetroEngine.Physics;
using System.Reflection;

namespace RetroEngine.Buddies.Helpers
{
    /// <summary>
    /// Defines a render to draw colliders in rectangle form.
    /// </summary>
    public class RectangleColliderRenderer
    {
        private readonly VertexArray _vao;
        private readonly VertexBuffer<Vector2> _vbo;
        private readonly ElementBuffer _ebo;
        private readonly ShaderProgram _shader;

        /// <summary>
        /// Creates a new <see cref="RectangleColliderRenderer"/>.
        /// </summary>
        public RectangleColliderRenderer()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var vertexShader = new Shader(Shader.LoadShaderSource(assembly, assembly.GetName().Name + ".Shaders.rectangle_render.vert"), ShaderType.VertexShader);
            var fragmentShader = new Shader(Shader.LoadShaderSource(assembly, assembly.GetName().Name + ".Shaders.pass_color.frag"), ShaderType.FragmentShader);

            _shader = new ShaderProgram();
            _shader.AddShader(vertexShader);
            _shader.AddShader(fragmentShader);
            _shader.Link();

            vertexShader.Delete();
            fragmentShader.Delete();

            _vao = new VertexArray();
            _vbo = new VertexBuffer<Vector2>(BufferUsageHint.DynamicDraw);
            _ebo = new ElementBuffer();

            Vector2[] vertices =
            [
                new(-0.5f, -0.5f),
                new( 0.5f, -0.5f),
                new( 0.5f,  0.5f),
                new(-0.5f,  0.5f)
            ];

            _vbo.CreateData(vertices);

            uint[] indices = [0, 1, 2, 3];
            _ebo.UpdateData(indices);

            _vao.Link(0, _vbo, 2);
        }

        /// <summary>
        /// Draws a rectangle collider.
        /// </summary>
        /// <param name="transform">Transform component associated to the collider.</param>
        /// <param name="collider">Collider component to draw.</param>
        /// <param name="view">Camera's view matrix.</param>
        /// <param name="projection">Camera's projection matrix.</param>
        /// <param name="color">Color of the collider lines.</param>
        public void Draw(Transform transform, Collider2D collider, Matrix4 view, Matrix4 projection, Vector4 color)
        {
            _shader.Bind();
            _vao.Bind();
            _ebo.Bind();

            var model =
                Matrix4.CreateScale(collider.Width * transform.Scale.X, collider.Height * transform.Scale.Y, 1.0f) *
                Matrix4.CreateRotationX(transform.Rotation.X) *
                Matrix4.CreateRotationY(transform.Rotation.Y) *
                Matrix4.CreateRotationZ(transform.Rotation.Z) *
                Matrix4.CreateTranslation(transform.Position);

            int locModel = GL.GetUniformLocation(_shader.Id, "u_model");
            int locView = GL.GetUniformLocation(_shader.Id, "u_view");
            int locProj = GL.GetUniformLocation(_shader.Id, "u_projection");
            int locColor = GL.GetUniformLocation(_shader.Id, "u_color");

            GL.UniformMatrix4(locModel, false, ref model);
            GL.UniformMatrix4(locView, false, ref view);
            GL.UniformMatrix4(locProj, false, ref projection);
            GL.Uniform4(locColor, color);

            GL.DrawElements(PrimitiveType.LineLoop, _ebo.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
}
