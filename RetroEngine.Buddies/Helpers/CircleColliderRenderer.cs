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
    /// Defines a render to draw colliders in circle form.
    /// </summary>
    public class CircleColliderRenderer
    {
        private readonly VertexArray _vao;
        private readonly VertexBuffer<Vector2> _vbo;
        private readonly ElementBuffer _ebo;
        private readonly ShaderProgram _shader;

        /// <summary>
        /// Creates a new <see cref="RectangleColliderRenderer"/>.
        /// </summary>
        public CircleColliderRenderer()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var vertexShader = new Shader(Shader.LoadShaderSource(assembly, assembly.GetName().Name + ".Shaders.circle.vert"), ShaderType.VertexShader);
            var fragmentShader = new Shader(Shader.LoadShaderSource(assembly, assembly.GetName().Name + ".Shaders.circle.frag"), ShaderType.FragmentShader);

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

            uint[] indices = [0, 1, 2, 2, 3, 0];
            _ebo.UpdateData(indices);

            _vao.Link(0, _vbo, 2);
        }

        /// <summary>
        /// Draws a circle collider.
        /// </summary>
        /// <param name="transform">Transform component associated to the collider.</param>
        /// <param name="collider">Collider component to draw.</param>
        /// <param name="view">Camera's view matrix.</param>
        /// <param name="projection">Camera's projection matrix.</param>
        /// <param name="color">Color of the collider lines.</param>
        /// <param name="screenSize">Size of the screen.</param>
        public void Draw(
            Transform transform,
            Collider2D collider,
            Matrix4 view,
            Matrix4 projection,
            Vector4 color,
            Vector2 screenSize)
        {
            _shader.Bind();
            _vao.Bind();
            _ebo.Bind();

            var model =
                Matrix4.CreateScale(transform.Scale.X, transform.Scale.Y, 1.0f) *
                Matrix4.CreateFromQuaternion(new Quaternion(transform.Rotation)) *
                Matrix4.CreateTranslation(transform.Position);

            var radius = collider.Radius;

            int locModel = GL.GetUniformLocation(_shader.Id, "u_model");
            int locView = GL.GetUniformLocation(_shader.Id, "u_view");
            int locProj = GL.GetUniformLocation(_shader.Id, "u_projection");
            int locColor = GL.GetUniformLocation(_shader.Id, "u_color");
            int locRadius = GL.GetUniformLocation(_shader.Id, "u_radius");

            GL.UniformMatrix4(locModel, false, ref model);
            GL.UniformMatrix4(locView, false, ref view);
            GL.UniformMatrix4(locProj, false, ref projection);
            GL.Uniform4(locColor, color);
            GL.Uniform1(locRadius, radius * radius);

            GL.DrawElements(PrimitiveType.Triangles, _ebo.Count, DrawElementsType.UnsignedInt, 0);
        }
    }
}
