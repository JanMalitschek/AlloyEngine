using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using AlloyEditorInterface;

namespace AlloyEngineEditor
{
    public class EditorCallback : ICallbackService
    {
        void ICallbackService.SendCallbackMessage(string message)
        {
            Alloy.Logging.LogInfo(this, message);
        }
    }
}
