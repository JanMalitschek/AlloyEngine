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
            User.UserCodeDatabase.LoadAssembly("Assets/Scripts/Turntable.dll");
            User.UserCodeDatabase.LoadAssembly("Assets/Scripts/CameraController.dll");
            Input.Init(this);

            AssetDatabase.Load(3);
            s = AssetDatabase.GetAsset<Scene>(3);
            AssetDatabase.Load(2);

            GL.Disable(EnableCap.CullFace);
            GL.Enable(EnableCap.DepthTest);
        }

        //Loop
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            Time.UpdateTime(e.Time);
            Input.Update();
            if (Input.GetKey(Key.Space))
                Logging.LogSimple("Down");
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
