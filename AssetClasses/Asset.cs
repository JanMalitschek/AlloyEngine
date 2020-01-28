using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alloy.Assets
{
    public abstract class Asset
    {
        public string Name { get; private set; }
        public string Path { get; private set; }

        public Asset(string path)
        {
            Path = path;
        }
    }
}
