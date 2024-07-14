using OpenTK.Mathematics;
using RetroEngine.Core.Elements;
using RetroEngine.Core.Mapping;
using RetroEngine.Ecs.Components;
using RetroEngine.Graphics;
using RetroEngine.Graphics.Settings;

namespace RetroEngine.Ecs.Systems
{
    /// <summary>
    /// Renders clip spaces of each camera.
    /// </summary>
    public class CameraSystem : RenderSystem
    {
        private GraphicSettings _graphicSettings;

        /// <summary>
        /// Creates a new CameraSystem.
        /// </summary>
        /// <param name="graphicSettings">Graphic settings of the game.</param>
        public CameraSystem(GraphicSettings graphicSettings)
            : base(Aspect.All<Transform>().All<Camera>())
        {
            _graphicSettings = graphicSettings;
        }

        /// <summary>
        /// Renders entities filtered in this system.
        /// </summary>
        /// <param name="gameTime">Elapsed time of the game.</param>
        public override void Render(GameTime gameTime)
        {
            _graphicSettings.ClipSpaces.Clear();

            foreach (var entity in ActiveEntities)
            {
                ref var transform = ref World.GetComponent<Transform>(entity);
                ref var camera = ref World.GetComponent<Camera>(entity);

                var view = Matrix4.LookAt(
                    new Vector3(transform.Position),
                    new Vector3(transform.Position) + camera.GetFrontDirection(transform),
                    camera.GetUpDirection(transform));

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

                _graphicSettings.ClipSpaces.Add(view * projection);
            }
        }
    }
}
