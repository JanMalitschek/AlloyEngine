using System;
using System.Collections.Generic;
using System.Text;

namespace Alloy.Components
{
    public abstract class Renderer : Component
    {
        public override void OnInit()
        {
            RenderPipeline.Register(this);
            OnEnterPipeline();
        }

        protected virtual void OnEnterPipeline() { }
        public virtual void Render() { }
        protected virtual void OnExitPipeline() { }
    }
}
