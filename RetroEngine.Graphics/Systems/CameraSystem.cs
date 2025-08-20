using OpenTK.Mathematics;
using RetroEngine.Core;
using RetroEngine.Core.Components;
using RetroEngine.Core.Elements;
using RetroEngine.Graphics.Components;
using RetroEngine.Graphics.Enums;

namespace RetroEngine.Graphics.Systems
{
    /// <summary>
    /// Renders clip spaces of each camera.
    /// </summary>
    /// <param name="graphicSettings">Graphic settings of the game.</param>
    public class CameraSystem(GraphicSettings graphicSettings) : RenderSystem(Contract
                  .Include<Transform>()
                  .Include<Camera>())
    {
        private readonly GraphicSettings _graphicSettings = graphicSettings;

        /// <summary>
        /// Renders entities filtered in this system.
        /// </summary>
        /// <param name="time">Info about the gametime.</param>
        public override void Process(World world, GameTime time)
        {
            _graphicSettings.ClipSpaces.Clear();

            foreach (var entity in GetEntities())
            {
                ref var transform = ref world.GetComponent<Transform>(entity);
                ref var camera = ref world.GetComponent<Camera>(entity);

                UpdateCameraVectors(ref camera, transform);

                var view = Matrix4.LookAt(
                    new Vector3(transform.Position),
                    new Vector3(transform.Position) + camera.Front,
                    camera.Up);

                var projection = camera.Projection == Projections.Ortographic ?
                    Matrix4.CreateOrthographic(
                        _graphicSettings.Width / 50, 
                        _graphicSettings.Height / 50, 
                        camera.ClippingNear, 
                        camera.ClippingFar)
                    :
                    Matrix4.CreatePerspectiveFieldOfView(
                        camera.FieldOfView,
                        _graphicSettings.AspectRatio,
                        camera.ClippingNear,
                        camera.ClippingFar);

                _graphicSettings.ClipSpaces.Add(new() { View = view, Projection = projection });
            }
        }

        private static void UpdateCameraVectors(ref Camera camera, Transform transform)
        {
            var frontX = (float)Math.Round(MathHelper.Cos(transform.Rotation.X), 6) * (float)Math.Round(MathHelper.Sin(transform.Rotation.Y), 6);
            var frontY = (float)Math.Round(MathHelper.Sin(transform.Rotation.X), 6);
            var frontZ = (float)Math.Round(MathHelper.Cos(transform.Rotation.X), 6) * (float)Math.Round(MathHelper.Cos(transform.Rotation.Y), 6);

            camera.Front = Vector3.Normalize(new Vector3(frontX, frontY, frontZ));
            camera.Right = Vector3.Normalize(Vector3.Cross(camera.Front, Vector3.UnitY));
            camera.Up = Vector3.Normalize(Vector3.Cross(camera.Right, camera.Front));
        }
    }
}
