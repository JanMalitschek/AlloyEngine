using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Alloy
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            using(Core core = new Core(800, 600, GraphicsMode.Default, "Alloy Engine"))
            {
                core.Run();           
            }
        }
    }
}
