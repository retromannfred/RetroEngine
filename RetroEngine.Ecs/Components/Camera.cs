using OpenTK.Mathematics;
using RetroEngine.Core.Elements;

namespace RetroEngine.Ecs.Components
{
    /// <summary>
    /// Defines a view camera for the cam.
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

        /// <summary>
        /// Gets the normalized front vector of this camera, representing where the camera is looking at.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <returns></returns>
        public Vector3 GetFrontDirection(Transform transform)
        {
            var x = (float)Math.Round(MathHelper.Cos(transform.Rotation.X), 6) * (float)Math.Round(MathHelper.Sin(transform.Rotation.Y), 6);
            var y = (float)Math.Round(MathHelper.Sin(transform.Rotation.X), 6);
            var z = (float)Math.Round(MathHelper.Cos(transform.Rotation.X), 6) * (float)Math.Round(MathHelper.Cos(transform.Rotation.Y), 6);

            return Vector3.Normalize(new Vector3(x, y, z));
        }

        /// <summary>
        /// Gets the normalized up vector of this camera, representing the roll / inclination of the camera.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <returns></returns>
        public Vector3 GetUpDirection(Transform transform)
        {
            return Vector3.UnitY;
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara forward.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void MoveForward(ref Transform transform, float module)
        {
            transform.Translate(GetFrontDirection(transform) * module);
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara backwards.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void MoveBackwards(ref Transform transform, float module)
        {
            transform.Translate(GetFrontDirection(transform) * -module);
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara up.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void MoveUp(ref Transform transform, float module)
        {
            transform.Translate(GetUpDirection(transform) * module);
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara down.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void MoveDown(ref Transform transform, float module)
        {
            transform.Translate(GetUpDirection(transform) * -module);
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara left.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void MoveLeft(ref Transform transform, float module)
        {
            transform.Translate(
                Vector3.Normalize(
                    Vector3.Cross(
                        GetFrontDirection(transform),
                        GetUpDirection(transform)))
                * -module);
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara right.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void MoveRight(ref Transform transform, float module)
        {
            transform.Translate(
                Vector3.Normalize(
                    Vector3.Cross(
                        GetFrontDirection(transform),
                        GetUpDirection(transform)))
                * module);
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look up.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void LookUp(ref Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitX * module);
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look down.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void LookDown(ref Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitX * -module);
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look left.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void LookLeft(ref Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitY * -module);
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look right.
        /// </summary>
        /// <param name="transform">Transform component of this camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <remarks>This modifies the transform component of this camera.</remarks>
        public void LookRight(ref Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitY * module);
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
