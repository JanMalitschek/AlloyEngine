using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using Alloy.Assets;
using Alloy.Components;
using Alloy.Utility;

namespace Alloy
{
    class Scene
    {
        private Tree<Entity> hierarchy;
        public Scene()
        {
            hierarchy = new Tree<Entity>();
        }
    }
}
