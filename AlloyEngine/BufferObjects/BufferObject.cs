using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloy.BufferObjects
{
    abstract class BufferObject
    {
        public int buffer { get; protected set; }
        public abstract void Bind();
        public abstract void Unbind();
    }
}
