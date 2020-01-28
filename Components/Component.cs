using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloy.Components
{
    public abstract class Component
    {
        public bool enabled = true;
        public Transform transform;
        public virtual void OnInit() { }
        public virtual void OnUpdate() { }
    }
}
