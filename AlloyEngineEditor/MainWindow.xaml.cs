using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AlloyEditorInterface;
using System.ServiceModel;

namespace AlloyEngineEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var callback = new EditorCallback();
            var context = new InstanceContext(callback);
            var pipeFactory = new DuplexChannelFactory<IEditorService>(context, new NetNamedPipeBinding(), new EndpointAddress("net.pipe://localhost/AlloyEditor"));
            var service = pipeFactory.CreateChannel();
            service.Connect();
            service.ShowMessage("Alloy Editor Client Connected!");
            
        }
    }
}
