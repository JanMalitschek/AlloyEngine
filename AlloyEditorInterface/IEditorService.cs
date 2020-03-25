using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Alloy.Assets;
using AlloyEditorInterface.Contracts;

namespace AlloyEditorInterface
{
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(ICallbackService))]
    public interface IEditorService
    {
        [OperationContract(IsOneWay = true)]
        void Connect();
        [OperationContract(IsOneWay = true)]
        void ShowMessage(string message);
        //Asssets
        [OperationContract(IsOneWay = false)]
        List<Tuple<string, string, int>> GetAssetDatabase();
        //Texture
        [OperationContract(IsOneWay = false)]
        TextureContract GetTexture(int id);
        [OperationContract(IsOneWay = true)]
        void ChangeTextureFilter(int id, Texture.Filter filter);
        [OperationContract(IsOneWay = true)]
        void ChangeTextureWrapping(int id, Texture.Wrapping wrapping);
        //Shader
        [OperationContract(IsOneWay = false)]
        ShaderContract GetShader(int id);
    }
}
