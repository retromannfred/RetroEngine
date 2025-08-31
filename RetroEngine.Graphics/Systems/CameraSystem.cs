using RetroEngine.Core;

namespace RetroEngine.Graphics
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

                camera.UpdateCameraVectors(transform);
                _graphicSettings.ClipSpaces.Add(camera.GetClipSpace(transform, _graphicSettings.Width, _graphicSettings.Height));
            }
        }
    }
}
