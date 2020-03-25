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
using System.Reflection;
using OpenTK.Input;
using Alloy.User;

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
            UserCodeDatabase.Init();
            RenderPipeline.Init(this);
            Input.Init(this);

            //AssetDatabase.Load(7);
            //Material m = AssetDatabase.GetAsset<Material>(7);
            //RenderPipeline.AddPostProcessingEffect(m);

            AssetDatabase.Load(3);
            s = AssetDatabase.GetAsset<Scene>(3);
            AssetDatabase.Load(2);
        }

        //Loop
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Time.UpdateTime(e.Time);
            Input.Update();
            ExternalCallQueue.Run();
        }

        //Render Loop
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            s.Update();
            RenderPipeline.Render();

            if (Input.GetMouseButtonDown(MouseButton.Left))
            {
                Logging.LogSimple(RenderPipeline.HoveredID);
            }

            SwapBuffers();
        }

        //Resized
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }
}
