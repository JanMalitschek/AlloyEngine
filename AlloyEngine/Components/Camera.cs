using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace Alloy.Components
{
    public class Camera : Component
    {
        public float fov = 60.0f;
        public float nearPlane = 0.01f;
        public float farPlane = 500.0f;
        public Color4 clearColor = new Color4(0.5f, 0.5f, 0.5f, 1.0f);
        public int renderedLayers = 0xFFFF; //Renders all layers by default

        public override void OnInit()
        {
            SetMain();
            
        }

        public void SetMain()
        {
            RenderPipeline.activeCamera = this;
            GL.ClearColor(clearColor.R, clearColor.G, clearColor.B, clearColor.A);
        }
    }
}
