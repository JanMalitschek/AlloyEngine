using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            XmlDocument xml = new XmlDocument();
            XmlNode root = xml.CreateElement("Material");
            xml.AppendChild(root);
            XmlNode shaderNode = xml.CreateElement("Shader");
            root.AppendChild(shaderNode);
            shaderNode.InnerText = shader.ID.ToString();
            XmlNode uniformsNode = xml.CreateElement("Uniforms");
            root.AppendChild(uniformsNode);
            foreach (Shader.Uniform u in Uniforms)
            {
                XmlNode uniformNode = xml.CreateElement("Uniform");
                XmlAttribute nameAtt = xml.CreateAttribute("name");
                nameAtt.Value = u.name;
                uniformNode.Attributes.Append(nameAtt);
                XmlAttribute typeAtt = xml.CreateAttribute("type");
                typeAtt.Value = u.type.FullName;
                uniformNode.Attributes.Append(nameAtt);
                XmlAttribute valueAtt = xml.CreateAttribute("value");
                if (u.type.IsSubclassOf(typeof(Asset)))
                    valueAtt.Value = (u.value as Asset).ID.ToString();
                else
                    valueAtt.Value = u.value.ToString();
                uniformNode.Attributes.Append(valueAtt);
            }
            xml.Save(Path);
        }
    }
}
