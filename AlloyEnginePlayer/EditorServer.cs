using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlloyEditorInterface;
using System.ServiceModel;

namespace Alloy
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class EditorServer : IEditorService
    {
        public static ICallbackService Callback { get; set; }

        void IEditorService.Connect()
        {
            Callback = OperationContext.Current.GetCallbackChannel<ICallbackService>();
        }

        void IEditorService.ShowMessage(string message)
        {
            Logging.LogInfo(this, message);
        }
    }
}
