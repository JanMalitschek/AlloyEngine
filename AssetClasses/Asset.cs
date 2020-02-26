using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Alloy.Assets
{
    public abstract class Asset
    {
        public string Name { get; private set; }
        public string Path { get; private set; }

        private static int id = 0;
        public int ID { get; private set; }

        public Asset(string path)
        {
            Path = path;
            
        }

        public struct MetaDataEntry
        {
            public string key;
            public Type type;
            public object value;
            public MetaDataEntry(string key, object value)
            {
                this.key = key;
                this.type = value.GetType();
                this.value = value;
            }
        }
        public void WriteMetaData()
        {
            List<MetaDataEntry> metaData = new List<MetaDataEntry>();
            SaveMetaData(out metaData);
            if(metaData.Count > 0)
            {
                XmlDocument xml = new XmlDocument();
                XmlNode root = xml.CreateElement("Metadata");
                XmlAttribute idAtt = xml.CreateAttribute("ID");
                idAtt.Value = (id++).ToString();
                xml.AppendChild(root);
                foreach(var e in metaData)
                {
                    XmlNode entry = xml.CreateElement("Data");
                    XmlAttribute name = xml.CreateAttribute("Name");
                    name.Value = e.key;
                    entry.Attributes.Append(name);
                    XmlAttribute type = xml.CreateAttribute("Type");
                    type.Value = e.type.Name;
                    entry.Attributes.Append(type);
                    XmlAttribute value = xml.CreateAttribute("Value");
                    value.Value = e.value.ToString();
                    entry.Attributes.Append(value);
                    root.AppendChild(entry);
                }
                string fileName = System.IO.Path.GetFileNameWithoutExtension(Path);
                string path = System.IO.Path.GetDirectoryName(Path);
                Logging.LogInfo(this, $"Saving Metadata for {fileName}");
                xml.Save(path + "/" + fileName + ".meta");
            }
        }
        protected abstract void SaveMetaData(out List<MetaDataEntry> metaData);
    }
}