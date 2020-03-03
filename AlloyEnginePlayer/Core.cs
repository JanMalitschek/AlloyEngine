using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using Alloy.Assets;
using Alloy.Components;

namespace Alloy
{
    class Core : GameWindow
    {
        Scene s;
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
            AssetDatabase.Load(0, 1, 2, 3, 4);
            s = AssetDatabase.GetAsset<Scene>(3);

            //var cam = s.AddEntity("Camera");
            //cam.Value.AddComponent<Camera>();
            //var monument = s.AddEntity("Monument");
            //var meshRenderer = monument.Value.AddComponent<ModelRenderer>();
            //meshRenderer.SetModel(AssetDatabase.GetAsset<Model>(0));
            //meshRenderer.SetMaterial(AssetDatabase.GetAsset<Material>(4), 0);
            //s.Apply();

            GL.Disable(EnableCap.CullFace);
        }

        //Loop
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Time.UpdateTime(e.Time);
        }

        //Render Loop
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.ClearColor(Color4.Black);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            s.Update();
            RenderPipeline.Render();

            SwapBuffers();
        }

        //Resized
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }
}
