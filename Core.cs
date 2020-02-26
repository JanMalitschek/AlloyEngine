using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Alloy.Assets;

namespace Alloy
{
    class Core : GameWindow
    {
        Model model;
        Entity cam;
        Alloy.Components.Camera c;
        Shader s;
        Texture t;

        public Core(uint width, uint height, GraphicsMode graphicsMode, string title) : base((int)width, (int)height, graphicsMode, title)
        {
            RenderPipeline.aspectRatio = width / height;
        }
        
        //Init
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Logging.LogInfo(this, "Initializing");
            AssetDatabase.Init();

            model = new Model("Assets/Models/Monument_01.dae");
            s = new Shader("Assets/Shaders/Unlit.glsl");
            t = new Texture("Assets/Textures/Bricks_02.png", Texture.Filter.Nearest);
            t.WriteMetaData();
            cam = new Entity();
            cam.Init();
            cam.transform.position = new Vector3(0.0f, -5.0f, -10.0f);
            c = cam.AddComponent<Alloy.Components.Camera>();
            c.fov = 100.0f;
            c.SetMain();

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Enable(EnableCap.DepthTest);
        }

        //Loop
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            Time.UpdateTime(e.Time);
        }

        //Render Loop
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            s.Use();
            model.meshes[0].subMeshes[0].vao.Bind();
            RenderPipeline.Render();
            var rot = Matrix4.CreateRotationY((float)MathHelper.DegreesToRadians(Time.TimeSinceStartup * 30.0f));
            GL.UniformMatrix4(s.GetUniformLocation("model"), false, ref rot);
            GL.UniformMatrix4(s.GetUniformLocation("view"), false, ref RenderPipeline.ViewMat);
            GL.UniformMatrix4(s.GetUniformLocation("proj"), false, ref RenderPipeline.ProjMat);
            t.Bind(0);
            GL.Uniform1(s.GetUniformLocation("tex"), 0);
            GL.DrawElements(BeginMode.Triangles, model.meshes[0].subMeshes[0].indices.Count, DrawElementsType.UnsignedInt, 0);

            SwapBuffers();
        }

        //Resized
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }
}
