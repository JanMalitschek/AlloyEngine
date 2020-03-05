using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alloy.Components;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Alloy.Assets;

namespace Alloy
{
    public static class RenderPipeline
    {
        public static Camera activeCamera;

        public static Matrix4 ViewMat;
        public static Matrix4 ProjMat;

        public static float aspectRatio = 1.0f;

        private static List<Renderer> registeredRenderers = new List<Renderer>();

        //Post Processing
        private static RenderTexture grabPass1;
        private static RenderTexture grabPass2;
        private static bool currentGrabPass = false;
        private static Mesh ppQuad;
        public static List<Tuple<Material, bool>> ppEffects { get; private set; } = new List<Tuple<Material, bool>>();

        public enum PredefinedUniforms
        {
            a_mod,
            a_view,
            a_proj,
            a_screen
        }

        public static void Init(GameWindow window)
        {
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.Enable(EnableCap.DepthTest);

            grabPass1 = new RenderTexture(window.Width, window.Height, Texture.Filter.Nearest, Texture.Wrapping.Clamp);
            grabPass2 = new RenderTexture(window.Width, window.Height, Texture.Filter.Nearest, Texture.Wrapping.Clamp);

            ppQuad = new Mesh("PPQuad");
            ppQuad.vertices.Add(new Vertex() {
                position = new Vector3(-1.0f, -1.0f, 0.0f), uv = new Vector2(0.0f, 0.0f) });
            ppQuad.vertices.Add(new Vertex() { position = new Vector3(-1.0f, 1.0f, 0.0f), uv = new Vector2(0.0f, 1.0f) });
            ppQuad.vertices.Add(new Vertex() { position = new Vector3(1.0f, 1.0f, 0.0f), uv = new Vector2(1.0f, 1.0f) });
            ppQuad.vertices.Add(new Vertex() { position = new Vector3(1.0f, -1.0f, 0.0f), uv = new Vector2(1.0f, 0.0f) });
            ppQuad.subMeshes.Add(new SubMesh());
            var subMesh = ppQuad.subMeshes.Last();
            subMesh.indices.Add(0);
            subMesh.indices.Add(2);
            subMesh.indices.Add(1);
            subMesh.indices.Add(0);
            subMesh.indices.Add(3);
            subMesh.indices.Add(2);
            ppQuad.GenerateBuffers();
            ppQuad.subMeshes[0].GenerateBuffers(ppQuad);
        }

        public static void Render()
        {
            //Render Scene
            if (ppEffects.Count > 0)
                grabPass1.Bind();
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            UpdateMatrices();
            foreach (Renderer r in registeredRenderers)
                if (r.enabled)
                    r.Render();
            GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);

            //Do Post Processing
            currentGrabPass = false;
            for (int i = 0; i < ppEffects.Count; i++)
            {
                if (i < ppEffects.Count - 1)
                {
                    if (!currentGrabPass)
                        grabPass2.Bind();
                    else
                        grabPass1.Bind();
                }
                else
                    GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                PostProcess(i);
                if (i < ppEffects.Count - 1)
                    GL.Ext.BindFramebuffer(FramebufferTarget.FramebufferExt, 0);
                currentGrabPass = !currentGrabPass;
            }
        }

        private static void PostProcess(int effetcIdx)
        {
            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.DepthTest);
            int texIdx = 0;
            ppEffects[effetcIdx].Item1.Pass(texIdx);
            PassPredefinedUniforms(ppEffects[effetcIdx].Item1.PredefinedUniforms, null, ppEffects[effetcIdx].Item1.shader, ref texIdx);
            ppQuad.subMeshes[0].vao.Bind();
            GL.DrawElements(BeginMode.Triangles, 6, DrawElementsType.UnsignedInt, 0);
        }

        public static void AddPostProcessingEffect(Material m)
        {
            ppEffects.Add(new Tuple<Material, bool>(m, true));
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

        internal static void PassPredefinedUniforms(List<PredefinedUniforms> predefinedUniforms, Renderer renderer, Assets.Shader s, ref int texIdx)
        {
            foreach(var u in predefinedUniforms)
            {
                switch (u)
                {
                    case PredefinedUniforms.a_mod:
                        var mat = renderer != null ? renderer.transform.GetTransformationMatrix() : Matrix4.Identity;
                        GL.UniformMatrix4(s.GetUniformLocation("a_mod"), false, ref mat);
                        break;
                    case PredefinedUniforms.a_view:
                        GL.UniformMatrix4(s.GetUniformLocation("a_view"), false, ref ViewMat);
                        break;
                    case PredefinedUniforms.a_proj:
                        GL.UniformMatrix4(s.GetUniformLocation("a_proj"), false, ref ProjMat);
                        break;
                    case PredefinedUniforms.a_screen:
                        GL.ActiveTexture(TextureUnit.Texture0 + texIdx);
                        if(!currentGrabPass)
                            GL.BindTexture(TextureTarget.Texture2D, grabPass1.Handle);
                        else
                            GL.BindTexture(TextureTarget.Texture2D, grabPass2.Handle);
                        GL.Uniform1(s.GetUniformLocation("a_screen"), texIdx++);
                        break;
                }
            }
        }
    }
}
