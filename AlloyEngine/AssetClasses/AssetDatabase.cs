using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Alloy.Utility;

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
                XMLAbstraction data = new XMLAbstraction("AssetDatabase", "AssetDatabase.xml");

                //Load IDs
                currentID = Convert.ToInt32(data.GetNode("//AssetDatabase/IDs/CurrentID").InnerText);
                var deletedIDNodes = data.GetNodes("//AssetDatabase/IDs/DeletedIDs/ID");
                foreach (var n in deletedIDNodes)
                    deletedIDs.Add(Convert.ToInt32(n.InnerText));

                //Load Assets
                var assetNodes = data.GetNodes("//AssetDatabase/Assets/*[local-name()='Asset']");
                foreach (var asset in assetNodes)
                {
                    string type = asset.GetAttribute("type");
                    if (IsTypeAtomic(type))
                        atomicAssetsInProject.Add(new Tuple<string, string, int>(asset.GetAttribute("path"), type, Convert.ToInt32(asset.GetAttribute("id"))));
                    else
                        composedAssetsInProject.Add(new Tuple<string, string, int>(asset.GetAttribute("path"), type, Convert.ToInt32(asset.GetAttribute("id"))));
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
                case ".png": return "Texture";
                case ".alloy": return "Scene";
                case ".mat": return "Material";
                default: return "Unsupported";
            }
        }

        private static bool IsTypeAtomic(string type)
        {
            return type == "Model" || type == "Shader" || type == "Texture";
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
            else if (a[0].Item2 == "Material")
                loadedAssets.Add(new Material(a[0].Item1));
        }
        public static void Load(params string[] paths)
        {
            foreach (string p in paths)
                Load(p);
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
            else if (a[0].Item2 == "Material")
                loadedAssets.Add(new Material(a[0].Item1));
            else
                return;
            loadedAssets.Last().ID = assetID;
        }
        public static void Load(params int[] assetIDs)
        {
            foreach (int i in assetIDs)
                Load(i);
        }

        public static Asset GetAsset(string path)
        {
            foreach (var a in loadedAssets)
                if (a.Path == path)
                    return a;
            return null;
        }
        public static T GetAsset<T>(string path) where T : Asset
        {
            foreach (var a in loadedAssets)
                if (a.Path == path && a is T)
                    return a as T;
            return null;
        }
        public static Asset GetAsset(int assetID)
        {
            foreach (var a in loadedAssets)
                if (a.ID == assetID)
                    return a;
            return null;
        }
        public static T GetAsset<T>(int assetID) where T : Asset
        {
            foreach (var a in loadedAssets)
                if (a.ID == assetID && a is T)
                    return a as T;
            return null;
        }

        public static int GetIDFromPath(string path)
        {
            foreach (var a in atomicAssetsInProject)
                if (a.Item1 == path)
                    return Convert.ToInt32(a.Item3);
            foreach (var a in composedAssetsInProject)
                if (a.Item1 == path)
                    return Convert.ToInt32(a.Item3);
            return -1;
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

        #region Asset Creation
        public static int CreateMaterial(string path)
        {
            System.IO.File.Create(path);
            XMLAbstraction mat = new XMLAbstraction("Material");
            mat.Save(path);
            if (!Import(path))
                return -1;
            Logging.LogInfo("Asset Database", "Created and imported new material!");
            WriteDatabase();
            return GetIDFromPath(path);
        }
        #endregion

        public static void WriteDatabase()
        {
            XMLAbstraction data = new XMLAbstraction("AssetDatabase");
            //IDs
            var idsNode = data.AddNode("IDs");
            idsNode.AddNode("CurrentID", currentID.ToString());
            var deletedIDsNode = idsNode.AddNode("DeletedIDs");
            foreach(int i in deletedIDs)
                deletedIDsNode.AddNode("ID", i.ToString());
            //Assets
            var assetsNode = data.AddNode("Assets");
            foreach(var a in atomicAssetsInProject)
            {
                var assetNode = assetsNode.AddNode("Asset");
                assetNode.AddAttribute("path", a.Item1);
                assetNode.AddAttribute("type", a.Item2);
                assetNode.AddAttribute("id", a.Item3);
            }
            foreach (var a in composedAssetsInProject)
            {
                var assetNode = assetsNode.AddNode("Asset");
                assetNode.AddAttribute("path", a.Item1);
                assetNode.AddAttribute("type", a.Item2);
                assetNode.AddAttribute("id", a.Item3);
            }
            data.Save("AssetDatabase.xml");
        }

        public static void LogAtomicAssets()
        {
            Logging.LogInfo("Asset Database", "Atomic Assets in project:");
            foreach (var a in atomicAssetsInProject)
                Logging.LogSimple($"\tPath: {a.Item1} Type: {a.Item2} ID: {a.Item3}");
        }
        public static void LogComposedAssets()
        {
            Logging.LogInfo("Asset Database", "Composed Assets in project:");
            foreach (var a in composedAssetsInProject)
                Logging.LogSimple($"\tPath: {a.Item1} Type: {a.Item2} ID: {a.Item3}");
        }
        public static void LogLoadedAssets()
        {
            Logging.LogInfo("Asset Database", "Loaded Assets:");
            foreach (var a in loadedAssets)
                Logging.LogSimple($"\tPath: {a.Path} Type: {a.GetType().Name} ID: {a.ID}");
        }
    }
}
