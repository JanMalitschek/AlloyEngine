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
        public string Name
        {
            get
            {
                return System.IO.Path.GetFileNameWithoutExtension(Path);
            }
        }
        public string Path { get; private set; }
        public string MetaDataPath
        {
            get
            {
                return System.IO.Path.GetDirectoryName(Path) + "/" + Name + ".meta";
            }
        }

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
        public static bool HasMetaData(string path)
        {
            return System.IO.File.Exists(System.IO.Path.GetDirectoryName(path) + "/" + System.IO.Path.GetFileNameWithoutExtension(path) + ".meta");
        }
        public static void WriteDefaultMetaData(int id, string path)
        {
            XmlDocument xml = new XmlDocument();
            XmlNode root = xml.CreateElement("Metadata");
            xml.AppendChild(root);
            XmlAttribute idAtt = xml.CreateAttribute("ID");
            idAtt.Value = id.ToString();
            root.Attributes.Append(idAtt);
            string name = System.IO.Path.GetFileNameWithoutExtension(path);
            Logging.LogInfo("Asset", $"Saving Metadata for {name}");
            xml.Save(System.IO.Path.GetDirectoryName(path) + "/" + name + ".meta");
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
                idAtt.Value = ID.ToString();
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
                
                Logging.LogInfo(this, $"Saving Metadata for {Name}");
                xml.Save(MetaDataPath);
            }
        }
        protected abstract void SaveMetaData(out List<MetaDataEntry> metaData);
    }
}