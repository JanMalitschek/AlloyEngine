using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlloyEditorInterface;
using System.ServiceModel;
using Alloy.Assets;
using AlloyEditorInterface.Contracts;

namespace Alloy
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    class EditorServer : IEditorService
    {
        public static ICallbackService Callback { get; set; }

        List<Tuple<string, string, int>> IEditorService.GetAssetDatabase()
        {
            List<Tuple<string, string, int>> db = new List<Tuple<string, string, int>>();
            db.AddRange(AssetDatabase.atomicAssetsInProject);
            db.AddRange(AssetDatabase.composedAssetsInProject);
            return db;
        }

        void IEditorService.Connect()
        {
            Callback = OperationContext.Current.GetCallbackChannel<ICallbackService>();
        }

        void IEditorService.ShowMessage(string message)
        {
            Logging.LogInfo(this, message);
        }

        #region Textures
        TextureContract IEditorService.GetTexture(int id)
        {
            AssetDatabase.Load(id);
            var tex = AssetDatabase.GetAsset<Texture2D>(id);
            return new TextureContract
            {
                Name = tex.Name,
                Filter = tex.filter,
                Wrapping = tex.wrapping
            };
        }

        public void ChangeTextureFilter(int id, Texture.Filter filter)
        {
            ExternalCallQueue.AddCall(new TextureFilterCall(id, filter));
        }

        public void ChangeTextureWrapping(int id, Texture.Wrapping wrapping)
        {
            ExternalCallQueue.AddCall(new TextureWrappingCall(id, wrapping));
        }
        #endregion
        #region Shaders
        ShaderContract IEditorService.GetShader(int id)
        {
            AssetDatabase.Load(id);
            var shader = AssetDatabase.GetAsset<Shader>(id);
            ShaderContract sc = new ShaderContract();
            sc.Name = shader.Name;
            sc.uniforms = new List<Tuple<string, string>>();
            foreach (var u in shader.uniforms)
                sc.uniforms.Add(new Tuple<string, string>(u.type.Name, u.name));
            return sc;
        }
        #endregion
    }
}
