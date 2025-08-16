using OpenTK.Mathematics;
using RetroEngine.Core.Components;
using RetroEngine.Graphics.Enums;

namespace RetroEngine.Graphics.Components
{
    /// <summary>
    /// Defines a camera view component.
    /// </summary>
    public struct Camera()
    {
        #region Internal camera vectors

        /// <summary>
        /// Gets the normalized front vector, pointing in the direction the camera is looking.
        /// </summary>
        internal Vector3 Front = -Vector3.UnitZ;

        /// <summary>
        /// Gets the normalized right vector, pointing to the camera's right.
        /// </summary>
        internal Vector3 Right = Vector3.UnitX;

        /// <summary>
        /// Gets the normalized up vector, indicating the camera's upward direction int he image plane.
        /// </summary>
        internal Vector3 Up = Vector3.UnitY;

        #endregion

        #region Component properties

        /// <summary>
        /// Gets or sets the background used to clear the view of the camera.
        /// </summary>
        public Color4 Background { get; set; } = Color4.CornflowerBlue;

        /// <summary>
        /// Gets or sets the projection of the camera.
        /// </summary>
        public Projections Projection { get; set; } = Projections.Ortographic;

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

        #endregion

        #region Control functions

        /// <summary>
        /// Performs a translate on a camera transform component to move the camara forward.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <returns>New transform component with the translation applied.</returns>
        public readonly Transform MoveForward(Transform transform, float module)
        {
            transform.Translate(Front * module);
            return transform;
        }

        /// <summary>
        /// Performs a translate on a camera transform component to move the camara backwards.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <returns>New transform component with the translation applied.</returns>
        public readonly Transform MoveBackwards(Transform transform, float module)
        {
            transform.Translate(Front * -module);
            return transform;
        }

        /// <summary>
        /// Performs a translate on a camera transform component to move the camara up.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <returns>New transform component with the translation applied.</returns>
        public readonly Transform MoveUp(Transform transform, float module)
        {
            transform.Translate(Vector3.UnitY * module);
            return transform;
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara down.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <returns>New transform component with the translation applied.</returns>
        public readonly Transform MoveDown(Transform transform, float module)
        {
            transform.Translate(Vector3.UnitY * -module);
            return transform;
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara left.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <returns>New transform component with the translation applied.</returns>
        public readonly Transform MoveLeft(Transform transform, float module)
        {
            transform.Translate(Right * -module);
            return transform;
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara right.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <returns>New transform component with the translation applied.</returns>
        public readonly Transform MoveRight(Transform transform, float module)
        {
            transform.Translate(Right * module);
            return transform;
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look up.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <returns>New transform component with the rotation applied.</returns>
        public readonly Transform LookUp(Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitX * module);
            return transform;
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look down.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <returns>New transform component with the rotation applied.</returns>
        public readonly Transform LookDown(Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitX * -module);
            return transform;
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look left.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <returns>New transform component with the rotation applied.</returns>
        public readonly Transform LookLeft(Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitY * -module);
            return transform;
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look right.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <returns>New transform component with the rotation applied.</returns>
        public readonly Transform LookRight(Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitY * module);
            return transform;
        }

        #endregion
    }
}
