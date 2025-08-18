using OpenTK.Compute.OpenCL;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Core.Elements;
using RetroEngine.Graphics;
using RetroEngine.Graphics.Batching;
using RetroEngine.Graphics.Components;
using RetroEngine.Graphics.Shaders;
using RetroEngine.Physics;
using RetroEngine.Physics.Components;
using RetroEngine.Physics.Enums;
using System;
using System.Reflection;


namespace RetroEngine.Buddies.System
{
    public class BuddyCollider2DSystem : RenderSystem
    {
        private const int FIXED_CAPACITY = 65536 * 4;

        private readonly GraphicSettings _graphicSettings;

        private readonly VertexArray _vao;
        private readonly VertexBuffer<float> _vbo;
        private readonly ShaderProgram _program;

        private readonly float[] _vertices;
        private int _verticesDrawn;

        public BuddyCollider2DSystem(GraphicSettings graphicSettings)
            : base(Contract
            .Include<Transform>()
            .Include<SpriteRenderer>()
            .Include<Collider2D>())
        {
            _graphicSettings = graphicSettings;

            _vao = new();
            _vbo = new(BufferUsageHint.StaticDraw);
            
            _vertices = new float[FIXED_CAPACITY];
            _verticesDrawn = 0;

            _vao.Bind();
            _vbo.Bind();

            _vbo.CreateData(_vertices);

            _vao.Link(0, _vbo, 2, 6, 0);
            _vao.Link(1, _vbo, 4, 6, 2);

            _vbo.Unbind();
            _vao.Unbind();

            var assembly = Assembly.GetExecutingAssembly();
            _program = new();
            _program.AddShader(new Shader(Shader.LoadShaderSource(assembly, assembly.GetName().Name + ".Shaders.vertexColliders.glsl"), ShaderType.VertexShader));
            _program.AddShader(new Shader(Shader.LoadShaderSource(assembly, assembly.GetName().Name + ".Shaders.fragmentColliders.glsl"), ShaderType.FragmentShader));
            _program.Link();
        }

        public override void Process(World world, GameTime time)
        {
            foreach (var clipSpace in _graphicSettings.ClipSpaces)
            {
                foreach (var entityA in GetEntities())
                {
                    ref var transformA = ref world.GetComponent<Transform>(entityA);
                    ref var colliderA = ref world.GetComponent<Collider2D>(entityA);
                    ref var rendererA = ref world.GetComponent<SpriteRenderer>(entityA);

                    foreach (var entityB in GetEntities())
                    {
                        if (entityA >= entityB)
                            continue;

                        ref var transformB = ref world.GetComponent<Transform>(entityB);
                        ref var colliderB = ref world.GetComponent<Collider2D>(entityB);
                        ref var rendererB = ref world.GetComponent<SpriteRenderer>(entityB);

                        if (Intersects(
                            transformA, colliderA,
                            transformB, colliderB,
                            out Vector2 direction, out float depth))
                        {

                        }
                    }
                }

                _vbo.UpdateData(0, _verticesDrawn * sizeof(float), _vertices);

                _program.Bind();
                _vao.Bind();

                int projLoc = GL.GetUniformLocation(_program.Id, "uProjection");
                int viewLoc = GL.GetUniformLocation(_program.Id, "uView");

                Matrix4 projection = clipSpace.Projection;
                Matrix4 view = clipSpace.View;

                GL.UniformMatrix4(projLoc, false, ref projection);
                GL.UniformMatrix4(viewLoc, false, ref view);

                int vertexStride = 6; // 2 pos + 4 color
                int vertsPerRect = 4; // 4 vértices por rectángulo
                int totalRects = _verticesDrawn / (vertsPerRect * vertexStride);

                for (int i = 0; i < totalRects; i++)
                {
                    GL.DrawArrays(PrimitiveType.LineLoop, i * vertsPerRect, vertsPerRect);
                }
            }
        }

        private bool Intersects(
            Transform transformA, Collider2D colliderA,
            Transform transformB, Collider2D colliderB,
            out Vector2 direction, out float depth)
        {
            direction = Vector2.Zero;
            depth = 0;

            if (colliderA.Shape == Shapes2D.Circle && colliderA.Shape == Shapes2D.Circle)
            {
                return CollisionMath.IntersectCircles(
                    transformA.Position.Xy + colliderA.Offset, colliderA.Radius,
                    transformB.Position.Xy + colliderB.Offset, colliderB.Radius,
                    out direction, out depth);
            }
            else if (colliderA.Shape == Shapes2D.Rectangle && colliderA.Shape == Shapes2D.Rectangle)
            {
                var verticesA = GetRectangleVertices(
                    transformA.Position.Xy + colliderA.Offset,
                    new Vector2(colliderA.Width, colliderA.Height),
                    transformA.Rotation.Z);

                var verticesB = GetRectangleVertices(
                    transformB.Position.Xy + colliderB.Offset,
                    new Vector2(colliderB.Width, colliderB.Height),
                    transformB.Rotation.Z);

                return CollisionMath.IntersectPolygons(verticesA, verticesB, out direction, out depth);
            }
            
            return false;
        }

        private static Vector2[] GetRectangleVertices(Vector2 position, Vector2 size, float rotation)
        {
            var vertices = new Vector2[4];
            var rotMatrix = Matrix2.CreateRotation(rotation);
            var halfSize = size / 2;

            vertices[0] = rotMatrix * new Vector2(-halfSize.X,  halfSize.Y) + position;
            vertices[1] = rotMatrix * new Vector2( halfSize.X,  halfSize.Y) + position;
            vertices[2] = rotMatrix * new Vector2( halfSize.X, -halfSize.Y) + position;
            vertices[3] = rotMatrix * new Vector2(-halfSize.X, -halfSize.Y) + position;

            return vertices;
        }
    }
}
