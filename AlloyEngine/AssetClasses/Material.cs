using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Alloy.Utility;

namespace Alloy.Assets
{
    class Material : Asset
    {
        private Shader shader;
        public List<Shader.Uniform> Uniforms { get; private set; }

        public Material(string path) : base(path)
        {
            shader = null;
            Uniforms = new List<Shader.Uniform>();
        }

        public void SetShader(Shader s)
        {
            shader = s;
            Uniforms = shader.GetUniforms();
        }

        public void SetUniform(string name, object value)
        {
            for (int i = 0; i < Uniforms.Count; i++)
                if (Uniforms[i].name == name && Uniforms[i].type == value.GetType())
                {
                    Shader.Uniform uniform = Uniforms[i];
                    uniform.value = value;
                    Uniforms[i] = uniform;
                    return;
                }
        }

        public void Pass()
        {
            shader.Use();
            foreach (Shader.Uniform u in Uniforms)
                u.Pass();
        }

        protected override void SaveMetaData(out List<MetaDataEntry> metaData)
        {
            metaData = new List<MetaDataEntry>();
        }
        protected override void SaveRequiredAssetIDs(out List<int> requiredAtomicAssets, out List<int> requiredComposedAssets)
        {
            requiredAtomicAssets = new List<int>();
            requiredComposedAssets = new List<int>();
            foreach (Shader.Uniform u in Uniforms)
                if (u.type == typeof(Texture))
                    requiredAtomicAssets.Add(((Texture)u.value).ID);
        }

        protected override void Save()
        {
            XMLAbstraction xml = new XMLAbstraction("Material");
            xml.AddNode("Shader", shader.ID.ToString());
            var uniformsNode = xml.AddNode("Uniforms");
            foreach (Shader.Uniform u in Uniforms)
            {
                var uniformNode = uniformsNode.AddNode("Uniform");
                uniformsNode.AddAttribute("name", u.name);
                uniformsNode.AddAttribute("type", u.type.FullName);
                uniformsNode.AddAttribute("value", u.type.IsSubclassOf(typeof(Asset)) ? (u.value as Asset).ID.ToString() : u.value.ToString());
            }
            xml.Save(Path);
        }
    }
}
