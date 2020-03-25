using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;
using System.ComponentModel;
using Alloy.Assets;

namespace AlloyEditorInterface.Contracts
{
    [DataContract(Name = "TextureContract")]
    public class TextureContract
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Texture.Filter Filter { get; set; }
        [DataMember]
        public Texture.Wrapping Wrapping { get; set; }
    }

    [DataContract(Name = "ShaderContract")]
    public class ShaderContract
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public List<Tuple<string, string>> uniforms;
    }
}
