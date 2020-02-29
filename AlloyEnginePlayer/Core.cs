﻿using System;
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
        Scene scene = new Scene("Assets/Scenes/Sample.alloy");

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

            //var entity = scene.AddEntity("Camera");
            //entity.Value.AddComponent<Alloy.Components.Camera>();
            //scene.AddEntity(entity, "Child Entity");
            //scene.SaveScene(scene.Path);

            scene.LoadScene(scene.Path);
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

            SwapBuffers();
        }

        //Resized
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }
}
