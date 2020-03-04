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

        public static Matrix4 ViewMat;
        public static Matrix4 ProjMat;

        public static float aspectRatio = 1.0f;

        private static List<Renderer> registeredRenderers = new List<Renderer>();

        public enum PredefinedUniforms
        {
            a_mod,
            a_view,
            a_proj
        }

        public static void Render()
        {
            UpdateMatrices();
            foreach (Renderer r in registeredRenderers)
                if (r.enabled)
                    r.Render();
        }

        private static void UpdateMatrices()
        {
            Transform viewTransform = new Transform();
            if (activeCamera != null)
                viewTransform = activeCamera.transform;
            ViewMat = viewTransform.GetTransformationMatrix(true);
            ProjMat = Matrix4.CreatePerspectiveFieldOfView(
                MathHelper.DegreesToRadians(activeCamera != null ? activeCamera.fov : 60.0f),
                aspectRatio,
                activeCamera != null ? activeCamera.nearPlane : 0.01f,
                activeCamera != null ? activeCamera.farPlane : 500.0f);
        }

        public static void Register(Renderer renderer)
        {
            registeredRenderers.Add(renderer);
        }

        internal static void PassPredefinedUniforms(List<PredefinedUniforms> predefinedUniforms, Renderer renderer, Assets.Shader s)
        {
            foreach(var u in predefinedUniforms)
            {
                switch (u)
                {
                    case PredefinedUniforms.a_mod:
                        var mat = renderer.transform.GetTransformationMatrix();
                        GL.UniformMatrix4(s.GetUniformLocation("a_mod"), false, ref mat);
                        break;
                    case PredefinedUniforms.a_view:
                        GL.UniformMatrix4(s.GetUniformLocation("a_view"), false, ref ViewMat);
                        break;
                    case PredefinedUniforms.a_proj:
                        GL.UniformMatrix4(s.GetUniformLocation("a_proj"), false, ref ProjMat);
                        break;
                }
            }
        }
    }
}
