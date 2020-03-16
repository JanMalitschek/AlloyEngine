using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AlloyEditorInterface
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallbackService))]
    public interface IEditorService
    {
        [OperationContract(IsOneWay = true)]
        void Connect();
        [OperationContract(IsOneWay = true)]
        void ShowMessage(string message);
    }
}
