using OpenTK.Mathematics;

namespace RetroEngine.Core
{
    /// <summary>
    /// Defines a clip space created by a camera's view and projection.
    /// </summary>
    public struct ClipSpace
    {
        /// <summary>
        /// Gets or sets the camera view matrix.
        /// </summary>
        public Matrix4 View { get; set; }

        /// <summary>
        /// Gets or sets the camera projection matrix.
        /// </summary>
        public Matrix4 Projection { get; set; }
    }
}
