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
        private static List<Tuple<string, string, int>> atomicAssetsInProject = new List<Tuple<string, string, int>>();
        private static List<Tuple<string, string, int>> composedAssetsInProject = new List<Tuple<string, string, int>>();

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
                        atomicAssetsInProject.Add(new Tuple<string, string, int>(asset.Attributes["path"].Value, type, Convert.ToInt32(asset.Attributes["id"].Value)));
                    else
                        composedAssetsInProject.Add(new Tuple<string, string, int>(asset.Attributes["path"].Value, type, Convert.ToInt32(asset.Attributes["id"].Value)));
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
                case ".alloy": return "Scene";
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
                int id = GetNewID();
                if (IsTypeAtomic(type))
                    atomicAssetsInProject.Add(new Tuple<string, string, int>(path, type, id));
                else
                    composedAssetsInProject.Add(new Tuple<string, string, int>(path, type, id));
                if(!Asset.HasMetaData(path))
                    Asset.WriteDefaultMetaData(id, path);
                return true;
            }
            else
                return false;
        }

        public static bool IsAssetLoaded(string path)
        {
            return loadedAssets.FindAll(x => x.Path == path).Count > 0;
        }
        public static bool IsAssetLoaded(int assetID)
        {
            return loadedAssets.FindAll(x => x.ID == assetID).Count > 0;
        }

        public static void Load(string path)
        {
            if (IsAssetLoaded(path))
                return;
            var a = atomicAssetsInProject.FindAll(x => x.Item1 == path);
            if(a.Count <= 0)
                a = composedAssetsInProject.FindAll(x => x.Item1 == path);
            if (a.Count <= 0)
            {
                Logging.LogWarning("Asset Database", $"The Asset {path} cannot be loaded because it has not been imported yet!");
                return;
            }
            if (a[0].Item2 == "Model")
                loadedAssets.Add(new Model(path));
            else if (a[0].Item2 == "Scene")
                loadedAssets.Add(new Scene(path));
            else if (a[0].Item2 == "Shader")
                loadedAssets.Add(new Shader(path));
            else if (a[0].Item2 == "Texture")
                loadedAssets.Add(new Texture(path));
        }
        public static void Load(int assetID)
        {
            if (IsAssetLoaded(assetID))
                return;
            var a = atomicAssetsInProject.FindAll(x => x.Item3 == assetID);
            if (a.Count <= 0)
                a = composedAssetsInProject.FindAll(x => x.Item3 == assetID);
            if (a.Count <= 0)
            {
                Logging.LogWarning("Asset Database", $"The Asset with ID {assetID} cannot be loaded because it has not been imported yet!");
                return;
            }
            if (a[0].Item2 == "Model")
                loadedAssets.Add(new Model(a[0].Item1));
            else if (a[0].Item2 == "Scene")
                loadedAssets.Add(new Scene(a[0].Item1));
            else if (a[0].Item2 == "Shader")
                loadedAssets.Add(new Shader(a[0].Item1));
            else if (a[0].Item2 == "Texture")
                loadedAssets.Add(new Texture(a[0].Item1));
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
                XmlAttribute id = data.CreateAttribute("id");
                id.Value = a.Item3.ToString();
                asset.Attributes.Append(id);
            }
            foreach (var a in composedAssetsInProject)
            {
                XmlNode asset = data.CreateElement("Asset");
                assets.AppendChild(asset);
                XmlAttribute path = data.CreateAttribute("path");
                path.Value = a.Item1;
                asset.Attributes.Append(path);
                XmlAttribute type = data.CreateAttribute("type");
                type.Value = a.Item2;
                asset.Attributes.Append(type);
                XmlAttribute id = data.CreateAttribute("id");
                id.Value = a.Item3.ToString();
                asset.Attributes.Append(id);
            }
            data.Save("AssetDatabase.xml");
        }
    }
}
