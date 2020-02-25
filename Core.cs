﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using Alloy.Assets;

namespace Alloy
{
    class Core : GameWindow
    {
        Model model;

        public Core(uint width, uint height, GraphicsMode graphicsMode, string title) : base((int)width, (int)height, graphicsMode, title)
        {
            RenderPipeline.aspectRatio = width / height;
        }
        
        //Init
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Logging.LogInfo(this, "Initializing");
            model = new Model("../../Assets/Models/CardboardBox_01.obj");
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

            SwapBuffers();
        }

        //Resized
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }
}