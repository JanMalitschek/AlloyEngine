using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using System.ServiceModel;
using AlloyEditorInterface;

namespace Alloy
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var host = new ServiceHost(typeof(EditorServer), new Uri("net.pipe://localhost"));
            host.AddServiceEndpoint(typeof(IEditorService), new NetNamedPipeBinding(), "AlloyEditor");
            host.Open();

            using(Core core = new Core(800, 600, GraphicsMode.Default, "Alloy Engine"))
            {
                core.Run();           
            }
        }
    }
}
