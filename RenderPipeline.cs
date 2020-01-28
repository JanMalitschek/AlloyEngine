using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alloy.Components;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Alloy
{
    public static class RenderPipeline
    {
        public static Camera activeCamera;

        public static Matrix4 ViewMat { get; private set; }
        public static Matrix4 ProjMat { get; private set; }

        public static float aspectRatio = 1.0f;

        public static void Render()
        {
            Transform viewTransform = new Transform();
            if (activeCamera != null)
                viewTransform = activeCamera.transform;
            var T = Matrix4.CreateTranslation(viewTransform.position);
            var eulerAngles = viewTransform.rotation.ToAxisAngle();
            var Rx = Matrix4.CreateRotationX(eulerAngles.X);
            var Ry = Matrix4.CreateRotationX(eulerAngles.Y);
            var Rz = Matrix4.CreateRotationX(eulerAngles.Z);
            var R = Rz * Ry * Rx;
            var S = Matrix4.CreateScale(viewTransform.scale);
            ViewMat = T * R * S;
            ProjMat = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(activeCamera != null ? activeCamera.fov : 60.0f),
                aspectRatio,
                activeCamera != null ? activeCamera.nearPlane : 0.01f,
                activeCamera != null ? activeCamera.farPlane : 500.0f);
        }
    }
}
