using OpenTK.Mathematics;
using RetroEngine.Ecs.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetroEngine.Ecs.Helpers
{
    public static class CameraHelper
    {
        /// <summary>
        /// Gets the normalized front vector of a camera, representing where the camera is looking at.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <returns></returns>
        public static Vector3 GetFrontDirection(Transform transform)
        {
            var x = (float)Math.Round(MathHelper.Cos(transform.Rotation.X), 6) * (float)Math.Round(MathHelper.Sin(transform.Rotation.Y), 6);
            var y = (float)Math.Round(MathHelper.Sin(transform.Rotation.X), 6);
            var z = (float)Math.Round(MathHelper.Cos(transform.Rotation.X), 6) * (float)Math.Round(MathHelper.Cos(transform.Rotation.Y), 6);

            return Vector3.Normalize(new Vector3(x, y, z));
        }

        /// <summary>
        /// Gets the normalized up vector of a camera, representing the roll / inclination of the camera.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <returns></returns>
        public static Vector3 GetUpDirection(Transform transform)
        {
            return Vector3.UnitY;
        }

        /// <summary>
        /// Performs a translate on a camera transform component to move the camara forward.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void MoveForward(ref Transform transform, float module)
        {
            transform.Translate(GetFrontDirection(transform) * module);
        }

        /// <summary>
        /// Performs a translate on a camera transform component to move the camara backwards.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void MoveBackwards(ref Transform transform, float module)
        {
            transform.Translate(GetFrontDirection(transform) * -module);
        }

        /// <summary>
        /// Performs a translate on a camera transform component to move the camara up.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void MoveUp(ref Transform transform, float module)
        {
            transform.Translate(GetUpDirection(transform) * module);
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara down.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void MoveDown(ref Transform transform, float module)
        {
            transform.Translate(GetUpDirection(transform) * -module);
        }

        /// <summary>
        /// Performs a translate on the camera transform component to move the camara left.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void MoveLeft(ref Transform transform, float module)
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
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to move.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void MoveRight(ref Transform transform, float module)
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
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void LookUp(ref Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitX * module);
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look down.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void LookDown(ref Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitX * -module);
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look left.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void LookLeft(ref Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitY * -module);
        }

        /// <summary>
        /// Performs a rotation on the camera transform component to make the camera look right.
        /// </summary>
        /// <param name="transform">Transform component of the camera.</param>
        /// <param name="module">Units to rotate.</param>
        /// <remarks>This modifies the transform component of the camera.</remarks>
        public static void LookRight(ref Transform transform, float module)
        {
            transform.Rotate(Vector3.UnitY * module);
        }
    }
}
