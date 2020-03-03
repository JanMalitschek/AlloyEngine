using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Alloy.Utility;

namespace Alloy.Assets
{
    public class Material : Asset
    {
        public Shader shader { get; private set; }
        public List<RenderPipeline.PredefinedUniforms> PredefinedUniforms
        {
            get
            {
                if (shader != null) return shader.predefinedUniforms;
                else return new List<RenderPipeline.PredefinedUniforms>();
            }
        }
        public List<Shader.Uniform> Uniforms { get; private set; }

        public Material(string path) : base(path)
        {
            shader = null;
            Uniforms = new List<Shader.Uniform>();
            XMLAbstraction xml = new XMLAbstraction("Material", path);
            var shaderNode = xml.GetNode("//Material/Shader");
            if (shaderNode != null)
                SetShader(AssetDatabase.GetAsset(Convert.ToInt32(shaderNode.InnerText)) as Shader);
            foreach(var n in xml.GetNodes("//Material/Uniforms/Uniform"))
            {
                Type t = Type.GetType(n.GetAttribute("type"));
                if (t.IsSubclassOf(typeof(Asset)))
                    SetUniform(n.GetAttribute("name"), AssetDatabase.GetAsset(TypeSerialization.DeserializePrimitiveType<int>(n.node)));
                else
                    SetUniform(n.GetAttribute("name"), TypeSerialization.DeserializeType(n.node, t));
            }
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
            if (shader == null)
                return;
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
            if (shader != null)
                requiredAtomicAssets.Add(shader.ID);
            foreach (Shader.Uniform u in Uniforms)
                if (u.type == typeof(Texture) && u.value != null)
                    requiredAtomicAssets.Add(((Texture)u.value).ID);
        }

        protected override void Save()
        {
            XMLAbstraction xml = new XMLAbstraction("Material");
            xml.AddNode("Shader", shader.ID.ToString());
            var uniformsNode = xml.AddNode("Uniforms");
            foreach (Shader.Uniform u in Uniforms)
            {
                if (u.value != null)
                {
                    var uniformNode = uniformsNode.AddNode("Uniform");
                    uniformNode.AddAttribute("name", u.name);
                    uniformNode.AddAttribute("type", u.type.FullName);
                    if (u.type.IsSubclassOf(typeof(Asset)))
                        TypeSerialization.SerializePrimitiveType((u.value as Asset).ID, uniformNode.node, xml.xml);
                    else
                        TypeSerialization.SerializeType(u.value, uniformNode.node, xml.xml);
                }
            }
            xml.Save(Path);
        }
    }
}
