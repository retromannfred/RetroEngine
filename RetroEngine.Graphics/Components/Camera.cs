using OpenTK.Mathematics;
using RetroEngine.Core;

namespace RetroEngine.Graphics
{
    /// <summary>
    /// Defines a camera view component.
    /// </summary>
    public struct Camera()
    {
        /// <summary>
        /// Gets the direction the camera is pointing at.
        /// </summary>
        public Vector3 Front { get; internal set; } = -Vector3.UnitZ;

        /// <summary>
        /// Gets the camera is pointing to the right.
        /// </summary>
        public Vector3 Right { get; internal set; } = Vector3.UnitX;

        /// <summary>
        /// Gets the upward direction from the camera's perspective.
        /// </summary>
        public Vector3 Up { get; internal set; } = Vector3.UnitY;

        /// <summary>
        /// Gets or sets the background used to clear the view of the camera.
        /// </summary>
        public Color4 Background { get; set; } = Color4.CornflowerBlue;

        /// <summary>
        /// Gets or sets the projection of the camera.
        /// </summary>
        public ProjectionType Projection { get; set; } = ProjectionType.Orthographic;

        /// <summary>
        /// Gets or sets the field of view for the perspective view.
        /// </summary>
        /// <remarks>This property is used just for the perspective projection.</remarks>
        public float FieldOfView { get; set; } = MathHelper.PiOver4;

        /// <summary>
        /// Gets or sets the nearest plane rendered by the camera.
        /// </summary>
        public float ClippingNear { get; set; } = .3f;

        /// <summary>
        /// Gets or sets the farthest plane rendered by the camera.
        /// </summary>
        public float ClippingFar { get; set; } = 1000f;
    }
}
