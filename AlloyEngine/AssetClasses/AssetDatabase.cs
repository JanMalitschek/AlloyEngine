using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Alloy.Assets
{
    public class AssetDatabase
    {
        //Assets
        private static List<Asset> loadedAssets = new List<Asset>();
        private static List<Tuple<string, string>> atomicAssetsInProject = new List<Tuple<string, string>>();
        private static List<Tuple<string, string>> composedAssetsInProject = new List<Tuple<string, string>>();

        //ID Management
        private static int currentID = 0;
        private static List<int> deletedIDs = new List<int>();

        public static void Init()
        {
            Logging.LogInfo("Asset Database", "Loading Asset Database!");
            if (System.IO.File.Exists("AssetDatabase.xml"))
            {
                XmlDocument data = new XmlDocument();
                data.Load("AssetDatabase.xml");

                //Load IDs
                currentID = Convert.ToInt32(data.SelectSingleNode("//AssetDatabase/IDs/CurrentID").InnerText);
                XmlNodeList deletedIDNodes = data.SelectNodes("//AssetDatabase/IDs/DeletedIDs/ID");
                foreach (XmlNode n in deletedIDNodes)
                    deletedIDs.Add(Convert.ToInt32(n.InnerText));

                //Load Assets
                XmlNodeList assetNodes = data.SelectNodes("//AssetDatabase/Assets/*[local-name()='Asset']");
                foreach (XmlNode asset in assetNodes)
                {
                    string type = asset.Attributes["type"].Value;
                    if (IsTypeAtomic(type))
                        atomicAssetsInProject.Add(new Tuple<string, string>(asset.Attributes["path"].Value, type));
                    else
                        composedAssetsInProject.Add(new Tuple<string, string>(asset.Attributes["path"].Value, type));
                }
            }
            else
            {
                Logging.LogError("Asset Database", "Could not find AssetDatabase.xml!");
            }
        }

        private static string TypeFromExtension(string extension)
        {
            switch (extension)
            {
                case ".dae": return "Model";
                case ".glsl": return "Shader";
                case ".shader": return "Shader";
                case ".png": return "Texture2D";
                default: return "Unsupported";
            }
        }

        private static bool IsTypeAtomic(string type)
        {
            return type == "Model" || type == "Shader" || type == "Texture2D";
        }

        public static bool Import (string path)
        {
            string extension = System.IO.Path.GetExtension(path);
            string type = TypeFromExtension(extension);
            if (type != "Unsupported")
            {
                if (IsTypeAtomic(type))
                    atomicAssetsInProject.Add(new Tuple<string, string>(path, type));
                else
                    composedAssetsInProject.Add(new Tuple<string, string>(path, type));
                if(!Asset.HasMetaData(path))
                    Asset.WriteDefaultMetaData(GetNewID(), path);
                return true;
            }
            else
                return false;
        }

        public static int GetNewID()
        {
            if(deletedIDs.Count > 0)
            {
                int newID = deletedIDs.Last();
                deletedIDs.RemoveAt(deletedIDs.Count - 1);
                return newID;
            }
            else
                return currentID++;
        }

        public static void WriteDatabase()
        {
            XmlDocument data = new XmlDocument();
            XmlNode root = data.CreateElement("AssetDatabase");
            data.AppendChild(root);
            //IDs
            XmlNode ids = data.CreateElement("IDs");
            root.AppendChild(ids);
            XmlNode cID = data.CreateElement("CurrentID");
            cID.InnerText = currentID.ToString();
            ids.AppendChild(cID);
            XmlNode dIDs = data.CreateElement("DeletedIDs");
            ids.AppendChild(dIDs);
            foreach(int i in deletedIDs)
            {
                XmlNode dID = data.CreateElement("ID");
                dID.InnerText = i.ToString();
                dID.AppendChild(dID);
            }
            //Assets
            XmlNode assets = data.CreateElement("Assets");
            root.AppendChild(assets);
            foreach(var a in atomicAssetsInProject)
            {
                XmlNode asset = data.CreateElement("Asset");
                assets.AppendChild(asset);
                XmlAttribute path = data.CreateAttribute("path");
                path.Value = a.Item1;
                asset.Attributes.Append(path);
                XmlAttribute type = data.CreateAttribute("type");
                type.Value = a.Item2;
                asset.Attributes.Append(type);
            }
            data.Save("AssetDatabase.xml");
        }
    }
}
