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

        public int ID { get; internal set; }

        public Asset(string path)
        {
            Path = path;
            if (HasMetaData(path))
                foreach (int i in LoadRequiredAssetIDs())
                    AssetDatabase.Load(i);
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
            if (path != "")
                return System.IO.File.Exists(System.IO.Path.GetDirectoryName(path) + "/" + System.IO.Path.GetFileNameWithoutExtension(path) + ".meta");
            else
                return false;
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

            XmlDocument xml = new XmlDocument();
            XmlNode root = xml.CreateElement("Metadata");
            XmlAttribute idAtt = xml.CreateAttribute("ID");
            idAtt.Value = ID.ToString();
            xml.AppendChild(root);

            //Required Assets
            XmlNode requiredAssets = xml.CreateElement("RequiredAssets");
            root.AppendChild(requiredAssets);
            List<int> requiredAtomics = new List<int>();
            List<int> requiredComposed = new List<int>();
            SaveRequiredAssetIDs(out requiredAtomics, out requiredComposed);
            foreach(int a in requiredAtomics)
            {
                XmlNode iDNode = xml.CreateElement("ID");
                iDNode.InnerText = a.ToString();
                requiredAssets.AppendChild(iDNode);
            }
            foreach (int c in requiredComposed)
            {
                XmlNode iDNode = xml.CreateElement("ID");
                iDNode.InnerText = c.ToString();
                requiredAssets.AppendChild(iDNode);
            }

            //Entries
            foreach (var e in metaData)
            {
                XmlNode entry = xml.CreateElement("Data");
                XmlAttribute name = xml.CreateAttribute("name");
                name.Value = e.key;
                entry.Attributes.Append(name);
                XmlAttribute type = xml.CreateAttribute("type");
                type.Value = e.type.FullName;
                entry.Attributes.Append(type);
                XmlAttribute value = xml.CreateAttribute("value");
                value.Value = e.value.ToString();
                entry.Attributes.Append(value);
                root.AppendChild(entry);
            }
                
            Logging.LogInfo(this, $"Saving Metadata for {Name}");
            xml.Save(MetaDataPath);
        }
        protected abstract void SaveMetaData(out List<MetaDataEntry> metaData);
        protected List<MetaDataEntry> LoadMetaData()
        {
            List<MetaDataEntry> metaData = new List<MetaDataEntry>();
            if (HasMetaData(Path))
            {
                XmlDocument xml = new XmlDocument();
                xml.Load(MetaDataPath);
                XmlNodeList entries = xml.SelectNodes("//Metadata/*[local-name()='Data']");
                foreach (XmlNode e in entries)
                {
                    string name = e.Attributes["name"].Value;
                    Type type = Type.GetType(e.Attributes["type"].Value);
                    object value = Convert.ChangeType(e.Attributes["value"].Value, type);
                    metaData.Add(new MetaDataEntry(name, value));
                }
            }
            return metaData;
        }
        protected virtual void SaveRequiredAssetIDs(out List<int> requiredAtomicAssets, out List<int> requiredComposedAssets)
        {
            requiredAtomicAssets = new List<int>();
            requiredComposedAssets = new List<int>();
        }
        protected List<int> LoadRequiredAssetIDs()
        {
            List<int> requiredAssets = new List<int>();
            XmlDocument xml = new XmlDocument();
            xml.Load(MetaDataPath);
            XmlNodeList requiredAssetIDsNodes = xml.SelectNodes("//Metadata/RequiredAssets/ID");
            foreach (XmlNode i in requiredAssetIDsNodes)
                requiredAssets.Add(Convert.ToInt32(i.InnerText));
            return requiredAssets;
        }

        protected virtual void Save() { }

        public void Apply()
        {
            WriteMetaData();
            Save();
        }
    }
}