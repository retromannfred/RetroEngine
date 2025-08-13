using OpenTK.Mathematics;
using RetroEngine.Core.Elements;

namespace RetroEngine.Ecs.Components
{
    /// <summary>
    /// Defines a camera view.
    /// </summary>
    public struct Camera : IComponent
    {
        /// <summary>
        /// Gets or sets the background used to clear the view of the camera.
        /// </summary>
        public Color4 Background { get; set; }

        /// <summary>
        /// Gets or sets the projection of the camera.
        /// </summary>
        public Projections Projection { get; set; }

        /// <summary>
        /// Gets or sets the field of view for the perspective view.
        /// </summary>
        /// <remarks>This property is used just for the perspective projection.</remarks>
        public float FieldOfView { get; set; }

        /// <summary>
        /// Gets or sets the nearest plane rendered by the camera.
        /// </summary>
        public float ClippingNear { get; set; }

        /// <summary>
        /// Gets or sets the farthest plane rendered by the camera.
        /// </summary>
        public float ClippingFar { get; set; }

        /// <summary>
        /// Creates a new camera component.
        /// </summary>
        public Camera()
        {
            Background = Color4.CornflowerBlue;
            Projection = Projections.Ortographic;
            FieldOfView = MathHelper.PiOver4;
            ClippingNear = .3f;
            ClippingFar = 1000f;
        }
    }

    /// <summary>
    /// Enumerates all projection types for the camera component.
    /// </summary>
    public enum Projections
    {
        Ortographic,
        Perspective
    }
}
