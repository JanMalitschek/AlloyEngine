using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace AlloyEditorInterface
{
    public interface ICallbackService
    {
        [OperationContract(IsOneWay = true)]
        void SendCallbackMessage(string message);
    }
}
